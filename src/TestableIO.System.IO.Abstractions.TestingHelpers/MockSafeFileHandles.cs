#if FEATURE_RANDOM_ACCESS
using System.Collections.Generic;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions.TestingHelpers;

/// <summary>
/// Keeps track of the <see cref="SafeFileHandle"/>s that <see cref="MockFile.OpenHandle"/> hands out.
/// </summary>
/// <remarks>
/// A <see cref="SafeFileHandle"/> is sealed and wraps an operating system handle, so the mock cannot create one that
/// a real system call would accept. It hands out a handle with a synthetic value instead, and remembers which file
/// that value stands for. The handle refers to the <see cref="MockFileData"/> it was opened on, not to its path, so
/// it keeps working after the file is deleted, as it does on a real file system.
/// </remarks>
internal sealed class MockSafeFileHandles
{
    /// <summary>
    /// Synthetic values start far above any plausible file descriptor or handle, so a mock handle that reaches a
    /// real system call by accident fails as an invalid handle instead of addressing an unrelated file. They are
    /// unique across all instances, so a handle from one <see cref="MockFileSystem"/> is never mistaken for one
    /// from another.
    /// </summary>
    private static long nextHandleValue = 0x4000_0000L;

    private const FileOptions ValidFileOptions = FileOptions.WriteThrough |
                                                 FileOptions.Asynchronous |
                                                 FileOptions.RandomAccess |
                                                 FileOptions.DeleteOnClose |
                                                 FileOptions.SequentialScan |
                                                 FileOptions.Encrypted |
                                                 (FileOptions)0x20000000 | // NoBuffering
                                                 (FileOptions)0x02000000; // BackupOrRestore

    private readonly IMockFileDataAccessor mockFileDataAccessor;
    private readonly Dictionary<IntPtr, Entry> entries = new();
    private readonly object gate = new();

    /// <summary>
    /// Serialises reads and writes through handles: <c>RandomAccess</c> permits concurrent writes at distinct
    /// offsets, which would lose each other if two of them started from the same snapshot of the contents.
    /// </summary>
    public object IoGate { get; } = new();
    private volatile bool hasEntries;

    /// <summary>
    /// Whether any handle is registered, so the file system can skip the sweep without taking a lock.
    /// </summary>
    public bool HasEntries => hasEntries;
    private bool sweeping;

    public MockSafeFileHandles(IMockFileDataAccessor mockFileDataAccessor)
    {
        this.mockFileDataAccessor = mockFileDataAccessor;
    }

    /// <summary>
    /// Returns the registry of the <see cref="MockFileSystem"/> behind <paramref name="mockFileDataAccessor"/>.
    /// </summary>
    public static MockSafeFileHandles For(IMockFileDataAccessor mockFileDataAccessor)
        => (mockFileDataAccessor as MockFileSystem)?.SafeFileHandles
           ?? throw CommonExceptions.RandomAccessNotSupported();

    public SafeFileHandle Open(string path, FileMode mode, FileAccess access, FileShare share,
        FileOptions options, long preallocationSize)
    {
        ValidateArguments(path, mode, access, share, options, preallocationSize);
        mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, nameof(path));
        path = mockFileDataAccessor.PathVerifier.FixPath(path);

        MockFileData fileData;
        var created = false;
        if (mockFileDataAccessor.FileExists(path))
        {
            fileData = mockFileDataAccessor.GetFile(path);
            if (fileData.IsDirectory)
            {
                throw CommonExceptions.AccessDenied(path);
            }

            if (mode == FileMode.CreateNew)
            {
                throw CommonExceptions.FileAlreadyExists(path);
            }

            fileData.CheckFileAccess(path, access);
        }
        else
        {
            var directoryPath = mockFileDataAccessor.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryPath) && !mockFileDataAccessor.Directory.Exists(directoryPath))
            {
                throw CommonExceptions.CouldNotFindPartOfPath(path);
            }

            if (mode is FileMode.Open or FileMode.Truncate)
            {
                throw CommonExceptions.FileNotFound(path);
            }

            fileData = new MockFileData(new byte[] { });
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.CreationTime | TimeAdjustments.LastAccessTime);
            mockFileDataAccessor.AddFile(path, fileData);
            created = true;
        }

        var shareGuid = Guid.NewGuid();
        mockFileDataAccessor.FileHandles.AddHandle(path, shareGuid, access, share);

        if (mode is FileMode.Create or FileMode.Truncate && !created)
        {
            fileData.Contents = new byte[] { };
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
        }

        var value = new IntPtr(Interlocked.Increment(ref nextHandleValue));
        var handle = new SafeFileHandle(value, ownsHandle: false);
        lock (gate)
        {
            entries[value] = new Entry(value, new WeakReference<SafeFileHandle>(handle), path, fileData,
                access, options, shareGuid);
            hasEntries = true;
        }

        return handle;
    }

    /// <summary>
    /// Returns the open file behind <paramref name="handle"/>, and throws what the runtime throws for a handle it
    /// cannot use.
    /// </summary>
    public Entry Resolve(SafeFileHandle handle, string paramName = "handle")
    {
        if (handle is null)
        {
            throw new ArgumentNullException(paramName);
        }

        if (handle.IsInvalid)
        {
            throw CommonExceptions.InvalidHandle(paramName);
        }

        if (handle.IsClosed)
        {
            throw CommonExceptions.HandleIsClosed();
        }

        lock (gate)
        {
            if (entries.TryGetValue(handle.DangerousGetHandle(), out var entry))
            {
                return entry;
            }
        }

        throw CommonExceptions.HandleNotFromThisFileSystem(paramName);
    }

    /// <summary>
    /// <see cref="SafeFileHandle"/> is sealed, so the mock is not told when one is closed. The file system calls this
    /// before it answers whether a file exists or may be opened, which is where a closed handle becomes observable:
    /// its file share is released and, for <see cref="FileOptions.DeleteOnClose"/>, its file deleted.
    /// </summary>
    /// <remarks>
    /// A handle that is dropped without being disposed counts as closed once it has been garbage collected, as a
    /// real handle is closed by its finalizer.
    /// </remarks>
    public void ReleaseClosedHandles()
    {
        if (!hasEntries)
        {
            return;
        }

        List<Entry> released = null;
        lock (gate)
        {
            // Only this thread can be sweeping: the file system serialises the sweep. This is a reentrant call
            // from the sweep itself, which removes a file and asks whether it exists.
            if (sweeping)
            {
                return;
            }

            foreach (var item in entries)
            {
                if (!item.Value.Handle.TryGetTarget(out var handle) || handle.IsClosed)
                {
                    (released ??= new List<Entry>()).Add(item.Value);
                }
            }

            if (released == null)
            {
                return;
            }

            foreach (var entry in released)
            {
                entries.Remove(entry.Value);
            }

            hasEntries = entries.Count > 0;
            sweeping = true;
        }

        try
        {
            foreach (var entry in released)
            {
                mockFileDataAccessor.FileHandles.RemoveHandle(entry.Path, entry.ShareGuid);
                if (entry.Options.HasFlag(FileOptions.DeleteOnClose) &&
                    ReferenceEquals(mockFileDataAccessor.GetFile(entry.Path), entry.Data))
                {
                    mockFileDataAccessor.RemoveFile(entry.Path, verifyAccess: false);
                }
            }
        }
        finally
        {
            lock (gate)
            {
                sweeping = false;
            }
        }
    }

    /// <summary>
    /// Validates in the order of the runtime's <c>FileStreamHelpers.ValidateArguments</c>, so that a call with more
    /// than one invalid argument reports the same one.
    /// </summary>
    private static void ValidateArguments(string path, FileMode mode, FileAccess access, FileShare share,
        FileOptions options, long preallocationSize)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (path.Length == 0)
        {
            throw CommonExceptions.PathIsEmpty(nameof(path));
        }

        if (mode is < FileMode.CreateNew or > FileMode.Append)
        {
            throw CommonExceptions.EnumValueOutOfRange(nameof(mode));
        }

        if (access is < FileAccess.Read or > FileAccess.ReadWrite)
        {
            throw CommonExceptions.EnumValueOutOfRange(nameof(access));
        }

        var shareWithoutInheritable = share & ~FileShare.Inheritable;
        if (shareWithoutInheritable is < FileShare.None or > (FileShare.ReadWrite | FileShare.Delete))
        {
            throw CommonExceptions.EnumValueOutOfRange(nameof(share));
        }

        if ((options & ~ValidFileOptions) != 0)
        {
            throw CommonExceptions.EnumValueOutOfRange(nameof(options));
        }

        if (preallocationSize < 0)
        {
            throw CommonExceptions.NonNegativeNumberRequired(nameof(preallocationSize));
        }

        if (!access.HasFlag(FileAccess.Write) &&
            mode is FileMode.Truncate or FileMode.CreateNew or FileMode.Create or FileMode.Append)
        {
            throw CommonExceptions.InvalidAccessCombination(mode, access);
        }

        if (access.HasFlag(FileAccess.Read) && mode == FileMode.Append)
        {
            throw CommonExceptions.AppendAccessOnlyInWriteOnlyMode();
        }

        if (preallocationSize > 0)
        {
            if (!access.HasFlag(FileAccess.Write))
            {
                throw CommonExceptions.PreallocationRequiresWriteAccess(access);
            }

            if (mode is not (FileMode.Create or FileMode.CreateNew))
            {
                throw CommonExceptions.PreallocationRequiresNewFile(mode);
            }
        }
    }

    /// <summary>
    /// A file opened through <see cref="MockFile.OpenHandle"/>.
    /// </summary>
    internal sealed class Entry
    {
        public Entry(IntPtr value, WeakReference<SafeFileHandle> handle, string path, MockFileData data,
            FileAccess access, FileOptions options, Guid shareGuid)
        {
            Value = value;
            Handle = handle;
            Path = path;
            Data = data;
            Access = access;
            Options = options;
            ShareGuid = shareGuid;
        }

        public WeakReference<SafeFileHandle> Handle { get; }
        public string Path { get; }
        public MockFileData Data { get; }
        public FileAccess Access { get; }
        public FileOptions Options { get; }
        public Guid ShareGuid { get; }
        public IntPtr Value { get; }
    }
}
#endif
