#if FEATURE_RANDOM_ACCESS
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions.TestingHelpers;

using XFS = MockUnixSupport;

/// <summary>
/// Reads and writes the files behind handles from <see cref="MockFile.OpenHandle"/>, at an offset and without a
/// file position, like <see cref="System.IO.RandomAccess"/>.
/// </summary>
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
public class MockRandomAccess : IRandomAccess
{
    /// <inheritdoc />
    public MockRandomAccess(IFileSystem fileSystem)
    {
        FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    /// <inheritdoc />
    public IFileSystem FileSystem { get; }

#if FEATURE_RANDOM_ACCESS_FLUSH_TO_DISK
    /// <inheritdoc />
    public void FlushToDisk(SafeFileHandle handle)
    {
        // There is no write-back cache to flush, but the handle is still validated as the runtime does.
        _ = Resolve(handle, fileOffset: 0);
    }
#endif

    /// <inheritdoc />
    public long GetLength(SafeFileHandle handle)
    {
        // The length is metadata, so it can be queried through a handle opened for writing only.
        var entry = Resolve(handle, fileOffset: 0);
        return entry.Data.Contents.Length;
    }

    /// <inheritdoc />
    public int Read(SafeFileHandle handle, Span<byte> buffer, long fileOffset)
    {
        var entry = Resolve(handle, fileOffset);
        return ReadAt(entry, buffer, fileOffset);
    }

    /// <inheritdoc />
    public long Read(SafeFileHandle handle, IReadOnlyList<Memory<byte>> buffers, long fileOffset)
    {
        var entry = Resolve(handle, fileOffset);
        ValidateBuffers(buffers);
        return ReadAt(entry, buffers, fileOffset);
    }

    /// <inheritdoc />
    public ValueTask<int> ReadAsync(SafeFileHandle handle, Memory<byte> buffer, long fileOffset,
        CancellationToken cancellationToken = default)
    {
        var entry = Resolve(handle, fileOffset);
        if (cancellationToken.IsCancellationRequested)
        {
            return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
        }

        return new ValueTask<int>(ReadAt(entry, buffer.Span, fileOffset));
    }

    /// <inheritdoc />
    public ValueTask<long> ReadAsync(SafeFileHandle handle, IReadOnlyList<Memory<byte>> buffers,
        long fileOffset, CancellationToken cancellationToken = default)
    {
        var entry = Resolve(handle, fileOffset);
        ValidateBuffers(buffers);
        if (cancellationToken.IsCancellationRequested)
        {
            return new ValueTask<long>(Task.FromCanceled<long>(cancellationToken));
        }

        return new ValueTask<long>(ReadAt(entry, buffers, fileOffset));
    }

#if FEATURE_RANDOM_ACCESS_FLUSH_TO_DISK
    /// <inheritdoc />
    public void SetLength(SafeFileHandle handle, long length)
    {
        var entry = Resolve(handle, fileOffset: 0);
        if (length < 0)
        {
            throw CommonExceptions.NonNegativeNumberRequired(nameof(length));
        }

        if (!entry.Access.HasFlag(FileAccess.Write))
        {
            // `ftruncate` on a descriptor opened for reading fails with `EINVAL`, where Windows denies access.
            throw XFS.IsUnixPlatform()
                ? CommonExceptions.InvalidArgument(entry.Path)
                : CommonExceptions.AccessDenied(entry.Path);
        }

        if (length > Array.MaxLength)
        {
            throw CommonExceptions.FileTooLarge(entry.Path);
        }

        lock (Registry.IoGate)
        {
            var contents = entry.Data.Contents;
            var resized = new byte[length];
            Array.Copy(contents, resized, Math.Min(contents.Length, length));
            Publish(entry, resized);
        }
    }
#endif

    /// <inheritdoc />
    public void Write(SafeFileHandle handle, ReadOnlySpan<byte> buffer, long fileOffset)
    {
        var entry = Resolve(handle, fileOffset);
        WriteAt(entry, buffer, fileOffset);
    }

    /// <inheritdoc />
    public void Write(SafeFileHandle handle, IReadOnlyList<ReadOnlyMemory<byte>> buffers,
        long fileOffset)
    {
        var entry = Resolve(handle, fileOffset);
        ValidateBuffers(buffers);
        WriteAt(entry, Gather(buffers), fileOffset);
    }

    /// <inheritdoc />
    public ValueTask WriteAsync(SafeFileHandle handle, ReadOnlyMemory<byte> buffer,
        long fileOffset, CancellationToken cancellationToken = default)
    {
        var entry = Resolve(handle, fileOffset);
        if (cancellationToken.IsCancellationRequested)
        {
            return new ValueTask(Task.FromCanceled(cancellationToken));
        }

        WriteAt(entry, buffer.Span, fileOffset);
        return default;
    }

    /// <inheritdoc />
    public ValueTask WriteAsync(SafeFileHandle handle, IReadOnlyList<ReadOnlyMemory<byte>> buffers,
        long fileOffset, CancellationToken cancellationToken = default)
    {
        var entry = Resolve(handle, fileOffset);
        ValidateBuffers(buffers);
        if (cancellationToken.IsCancellationRequested)
        {
            return new ValueTask(Task.FromCanceled(cancellationToken));
        }

        WriteAt(entry, Gather(buffers), fileOffset);
        return default;
    }

    private MockSafeFileHandles Registry
        => MockSafeFileHandles.For(FileSystem as IMockFileDataAccessor);

    /// <summary>
    /// Validates in the runtime's order: the handle, then the offset.
    /// </summary>
    private MockSafeFileHandles.Entry Resolve(SafeFileHandle handle, long fileOffset)
    {
        var entry = Registry.Resolve(handle);
        if (fileOffset < 0)
        {
            throw CommonExceptions.NonNegativeNumberRequired(nameof(fileOffset));
        }

        return entry;
    }

    private static void ValidateBuffers<T>(IReadOnlyList<T> buffers)
    {
        if (buffers == null)
        {
            throw new ArgumentNullException(nameof(buffers));
        }
    }

    private int ReadAt(MockSafeFileHandles.Entry entry, Span<byte> buffer, long fileOffset)
    {
        RequireRead(entry);
        lock (Registry.IoGate)
        {
            var contents = entry.Data.Contents;
            if (fileOffset >= contents.Length)
            {
                return 0;
            }

            var count = (int)Math.Min(buffer.Length, contents.Length - fileOffset);
            contents.AsSpan((int)fileOffset, count).CopyTo(buffer);
            Accessor.AdjustTimes(entry.Data, TimeAdjustments.LastAccessTime);
            return count;
        }
    }

    private long ReadAt(MockSafeFileHandles.Entry entry, IReadOnlyList<Memory<byte>> buffers, long fileOffset)
    {
        RequireRead(entry);
        lock (Registry.IoGate)
        {
            var contents = entry.Data.Contents;
            long copied = 0;
            foreach (var buffer in buffers)
            {
                var position = fileOffset + copied;
                if (position >= contents.Length)
                {
                    break;
                }

                var count = (int)Math.Min(buffer.Length, contents.Length - position);
                contents.AsSpan((int)position, count).CopyTo(buffer.Span);
                copied += count;
            }

            Accessor.AdjustTimes(entry.Data, TimeAdjustments.LastAccessTime);
            return copied;
        }
    }

    private void WriteAt(MockSafeFileHandles.Entry entry, ReadOnlySpan<byte> buffer, long fileOffset)
    {
        RequireWrite(entry);
        if (buffer.Length == 0)
        {
            return;
        }

        // The contents are a single array, so an offset beyond its maximum length cannot be written, much like one
        // beyond the maximum file size of a real file system.
        if (fileOffset > Array.MaxLength - buffer.Length)
        {
            throw CommonExceptions.FileTooLarge(entry.Path);
        }

        lock (Registry.IoGate)
        {
            var contents = entry.Data.Contents;
            var end = fileOffset + buffer.Length;
            var written = new byte[Math.Max(contents.Length, end)];
            Array.Copy(contents, written, contents.Length);
            buffer.CopyTo(written.AsSpan((int)fileOffset));
            Publish(entry, written);
        }
    }

    /// <summary>
    /// Replaces the contents instead of changing them in place, so a <see cref="MockFileStream"/> open on the same
    /// file notices the change.
    /// </summary>
    private void Publish(MockSafeFileHandles.Entry entry, byte[] contents)
    {
        entry.Data.Contents = contents;
        Accessor.AdjustTimes(entry.Data, TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
    }

    private static byte[] Gather(IReadOnlyList<ReadOnlyMemory<byte>> buffers)
    {
        var length = 0;
        foreach (var buffer in buffers)
        {
            length += buffer.Length;
        }

        var bytes = new byte[length];
        var position = 0;
        foreach (var buffer in buffers)
        {
            buffer.Span.CopyTo(bytes.AsSpan(position));
            position += buffer.Length;
        }

        return bytes;
    }

    private static void RequireRead(MockSafeFileHandles.Entry entry)
    {
        if (!entry.Access.HasFlag(FileAccess.Read))
        {
            throw CommonExceptions.AccessDenied(entry.Path);
        }
    }

    private static void RequireWrite(MockSafeFileHandles.Entry entry)
    {
        if (!entry.Access.HasFlag(FileAccess.Write))
        {
            throw CommonExceptions.AccessDenied(entry.Path);
        }
    }

    private IMockFileDataAccessor Accessor => (IMockFileDataAccessor)FileSystem;
}
#endif
