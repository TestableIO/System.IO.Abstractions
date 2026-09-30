#if FEATURE_RANDOM_ACCESS
namespace System.IO.Abstractions.TestingHelpers.Tests;

using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using NUnit.Framework;

using XFS = MockUnixSupport;

[TestFixture]
public class MockFileOpenHandleTests
{
    private static readonly string FilePath = XFS.Path(@"C:\temp\file.bin");

    [Test]
    public async Task OpenHandle_ExistingFile_ShouldReturnAnOpenHandle()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));

        using var handle = fileSystem.File.OpenHandle(FilePath);

        await That(handle.IsInvalid).IsFalse();
        await That(handle.IsClosed).IsFalse();
    }

    [Test]
    public async Task OpenHandle_MissingFile_ShouldThrowFileNotFoundException()
    {
        var fileSystem = new MockFileSystem();

        await That(() => fileSystem.File.OpenHandle(FilePath)).Throws<FileNotFoundException>();
    }

    [Test]
    public async Task OpenHandle_MissingDirectory_ShouldThrowDirectoryNotFoundException()
    {
        var fileSystem = new MockFileSystem();

        await That(() => fileSystem.File.OpenHandle(XFS.Path(@"C:\missing\file.bin"), FileMode.Create,
            FileAccess.Write)).Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task OpenHandle_CreateNewOnExistingFile_ShouldThrowIOException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));

        await That(() => fileSystem.File.OpenHandle(FilePath, FileMode.CreateNew, FileAccess.Write))
            .Throws<IOException>();
    }

    [Test]
    public async Task OpenHandle_Create_ShouldCreateTheFile()
    {
        var fileSystem = new MockFileSystem();

        using var handle = fileSystem.File.OpenHandle(FilePath, FileMode.Create, FileAccess.Write);

        await That(fileSystem.File.Exists(FilePath)).IsTrue();
    }

    [TestCase(FileMode.Create)]
    [TestCase(FileMode.Truncate)]
    public async Task OpenHandle_CreateOrTruncateOnExistingFile_ShouldEmptyTheFile(FileMode mode)
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));

        using var handle = fileSystem.File.OpenHandle(FilePath, mode, FileAccess.Write);

        await That(fileSystem.RandomAccess.GetLength(handle)).IsEqualTo(0);
    }

    [Test]
    public async Task OpenHandle_OnADirectory_ShouldThrowUnauthorizedAccessException()
    {
        var fileSystem = new MockFileSystem();
        var directory = XFS.Path(@"C:\temp\directory");
        fileSystem.AddDirectory(directory);

        await That(() => fileSystem.File.OpenHandle(directory)).Throws<UnauthorizedAccessException>();
    }

    [Test]
    public async Task OpenHandle_ReadOnlyFileForWriting_ShouldThrowUnauthorizedAccessException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content") { Attributes = FileAttributes.ReadOnly });

        await That(() => fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.Write))
            .Throws<UnauthorizedAccessException>();
    }

    [Test]
    public async Task OpenHandle_NullPath_ShouldThrowArgumentNullException()
    {
        var fileSystem = new MockFileSystem();

        await That(() => fileSystem.File.OpenHandle(null!, FileMode.Create, FileAccess.Read))
            .Throws<ArgumentNullException>().WithParamName("path")
            .Because("the path is validated before the mode and access combination");
    }

    [Test]
    public async Task OpenHandle_EmptyPath_ShouldThrowArgumentException()
    {
        var fileSystem = new MockFileSystem();

        await That(() => fileSystem.File.OpenHandle("", FileMode.Create, FileAccess.Read))
            .Throws<ArgumentException>().WithParamName("path");
    }

    [Test]
    public async Task OpenHandle_InvalidMode_ShouldThrowArgumentOutOfRangeException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));

        await That(() => fileSystem.File.OpenHandle(FilePath, (FileMode)99))
            .Throws<ArgumentOutOfRangeException>().WithParamName("mode");
    }

    [Test]
    public async Task OpenHandle_InvalidShare_ShouldThrowArgumentOutOfRangeException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));

        await That(() => fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.Read, (FileShare)99))
            .Throws<ArgumentOutOfRangeException>().WithParamName("share");
    }

    [Test]
    public async Task OpenHandle_NegativePreallocationSize_ShouldThrowArgumentOutOfRangeException()
    {
        var fileSystem = new MockFileSystem();

        await That(() => fileSystem.File.OpenHandle(FilePath, FileMode.Create, FileAccess.Write,
                FileShare.None, FileOptions.None, -1))
            .Throws<ArgumentOutOfRangeException>().WithParamName("preallocationSize");
    }

    [Test]
    public async Task OpenHandle_PreallocationSizeOnExistingFile_ShouldThrowArgumentException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));

        await That(() => fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.Write,
                FileShare.None, FileOptions.None, 100))
            .Throws<ArgumentException>().WithParamName("mode")
            .Because("a preallocation size can only be requested for new files");
    }

    [TestCase(FileMode.Create)]
    [TestCase(FileMode.CreateNew)]
    [TestCase(FileMode.Truncate)]
    [TestCase(FileMode.Append)]
    public async Task OpenHandle_ReadAccessWithAModeThatWrites_ShouldThrowArgumentException(FileMode mode)
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));

        await That(() => fileSystem.File.OpenHandle(FilePath, mode, FileAccess.Read))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task OpenHandle_WithFileShareNone_ShouldBlockAStreamOnTheSameFile()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));

        using var handle = fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.Read, FileShare.None);

        await That(() => fileSystem.FileStream.New(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            .Throws<IOException>();
    }

    [Test]
    public async Task OpenHandle_WhenDisposed_ShouldReleaseItsFileShare()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));

        fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.Read, FileShare.None).Dispose();

        await That(() =>
        {
            using var stream = fileSystem.FileStream.New(FilePath, FileMode.Open, FileAccess.Read, FileShare.None);
        }).DoesNotThrow();
    }

    [Test]
    public async Task OpenHandle_WhenNeverDisposed_ShouldReleaseItsFileShareOnceCollected()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));

        OpenAndDrop(fileSystem, FilePath);
        GC.Collect();
        GC.WaitForPendingFinalizers();

        await That(() =>
            {
                using var stream = fileSystem.FileStream.New(FilePath, FileMode.Open, FileAccess.Read, FileShare.None);
            }).DoesNotThrow()
            .Because("a handle nothing refers to any more is closed, as a real one is by its finalizer");
    }

    [Test]
    public async Task OpenHandle_WithDeleteOnClose_ShouldDeleteTheFileWhenDisposed()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\temp"));

        using (fileSystem.File.OpenHandle(FilePath, FileMode.Create, FileAccess.Write, FileShare.None,
                   FileOptions.DeleteOnClose))
        {
            await That(fileSystem.File.Exists(FilePath)).IsTrue();
        }

        await That(fileSystem.File.Exists(FilePath)).IsFalse();
    }

    [Test]
    public async Task OpenHandle_WithDeleteOnClose_ShouldNotDeleteAFileThatReplacedIt()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\temp"));

        using (fileSystem.File.OpenHandle(FilePath, FileMode.Create, FileAccess.Write, FileShare.Delete,
                   FileOptions.DeleteOnClose))
        {
            fileSystem.File.Delete(FilePath);
            fileSystem.AddFile(FilePath, new MockFileData("a different file"));
        }

        await That(fileSystem.File.ReadAllText(FilePath)).IsEqualTo("a different file");
    }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
    [Test]
    public async Task HandleOverloads_ShouldReadAndWriteTheAttributesOfTheOpenFile()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));
        using var handle = fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.ReadWrite);

        fileSystem.File.SetAttributes(handle, FileAttributes.Hidden);

        await That(fileSystem.File.GetAttributes(handle)).IsEqualTo(FileAttributes.Hidden);
        await That(fileSystem.File.GetAttributes(FilePath)).IsEqualTo(FileAttributes.Hidden);
    }

    [Test]
    public async Task HandleOverloads_ShouldReadAndWriteTheTimesOfTheOpenFile()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));
        using var handle = fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.ReadWrite);
        var creation = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var access = new DateTime(2021, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var write = new DateTime(2022, 1, 2, 3, 4, 5, DateTimeKind.Utc);

        fileSystem.File.SetCreationTimeUtc(handle, creation);
        fileSystem.File.SetLastAccessTimeUtc(handle, access);
        fileSystem.File.SetLastWriteTimeUtc(handle, write);

        await That(fileSystem.File.GetCreationTimeUtc(handle)).IsEqualTo(creation);
        await That(fileSystem.File.GetLastAccessTimeUtc(handle)).IsEqualTo(access);
        await That(fileSystem.File.GetLastWriteTimeUtc(handle)).IsEqualTo(write);
        await That(fileSystem.File.GetCreationTime(handle)).IsEqualTo(creation.ToLocalTime());
        await That(fileSystem.File.GetLastAccessTime(handle)).IsEqualTo(access.ToLocalTime());
        await That(fileSystem.File.GetLastWriteTime(handle)).IsEqualTo(write.ToLocalTime());
        await That(fileSystem.File.GetLastWriteTimeUtc(FilePath)).IsEqualTo(write);
    }

    [Test]
    public async Task HandleOverloads_LocalTimeSetters_ShouldRoundTrip()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));
        using var handle = fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.ReadWrite);
        var time = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Local);

        fileSystem.File.SetCreationTime(handle, time);
        fileSystem.File.SetLastAccessTime(handle, time);
        fileSystem.File.SetLastWriteTime(handle, time);

        await That(fileSystem.File.GetCreationTime(handle)).IsEqualTo(time);
        await That(fileSystem.File.GetLastAccessTime(handle)).IsEqualTo(time);
        await That(fileSystem.File.GetLastWriteTime(handle)).IsEqualTo(time);
    }

#if FEATURE_UNIX_FILE_MODE
    [Test]
    public async Task HandleOverloads_ShouldReadAndWriteTheUnixFileModeOfTheOpenFile()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));
        using var handle = fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.ReadWrite);
        const UnixFileMode mode = UnixFileMode.UserRead | UnixFileMode.UserWrite;

#pragma warning disable CA1416 // The mock supports this on every platform.
        fileSystem.File.SetUnixFileMode(handle, mode);

        await That(fileSystem.File.GetUnixFileMode(handle)).IsEqualTo(mode);
#pragma warning restore CA1416
    }
#endif

    [Test]
    public async Task HandleOverloads_WithANullHandle_ShouldThrowArgumentNullException()
    {
        var fileSystem = new MockFileSystem();

        await That(() => fileSystem.File.GetAttributes((SafeFileHandle)null!))
            .Throws<ArgumentNullException>().WithParamName("fileHandle");
    }

    [Test]
    public async Task HandleOverloads_WithAClosedHandle_ShouldThrowObjectDisposedException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData("content"));
        var handle = fileSystem.File.OpenHandle(FilePath);
        handle.Dispose();

        await That(() => fileSystem.File.GetAttributes(handle)).Throws<ObjectDisposedException>();
    }

    [Test]
    public async Task HandleOverloads_WithAHandleFromAnotherFileSystem_ShouldThrowArgumentException()
    {
        var fileSystem = new MockFileSystem();
        var other = new MockFileSystem();
        other.AddFile(FilePath, new MockFileData("content"));
        using var handle = other.File.OpenHandle(FilePath);

        await That(() => fileSystem.File.GetAttributes(handle))
            .Throws<ArgumentException>().WithParamName("fileHandle");
    }

#endif

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void OpenAndDrop(MockFileSystem fileSystem, string path)
        => _ = fileSystem.File.OpenHandle(path, FileMode.Open, FileAccess.Read, FileShare.None);
}
#endif
