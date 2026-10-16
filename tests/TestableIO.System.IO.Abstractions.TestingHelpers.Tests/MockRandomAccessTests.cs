#if FEATURE_RANDOM_ACCESS
namespace System.IO.Abstractions.TestingHelpers.Tests;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using NUnit.Framework;

using XFS = MockUnixSupport;

[TestFixture]
public class MockRandomAccessTests
{
    private static readonly string FilePath = XFS.Path(@"C:\temp\file.bin");

    private static (MockFileSystem FileSystem, SafeFileHandle Handle) Arrange(byte[] contents,
        FileAccess access = FileAccess.ReadWrite)
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData(contents));
        return (fileSystem, fileSystem.File.OpenHandle(FilePath, FileMode.Open, access, FileShare.ReadWrite));
    }

    [Test]
    public async Task Read_ShouldReadAtTheOffset()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3, 4 });
        using var open = handle;
        var buffer = new byte[2];

        var read = fileSystem.RandomAccess.Read(handle, buffer, 1);

        await That(read).IsEqualTo(2);
        await That(buffer).IsEqualTo(new byte[] { 2, 3 });
    }

    [Test]
    public async Task Read_PastTheEnd_ShouldReturnZero()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2 });
        using var open = handle;

        var read = fileSystem.RandomAccess.Read(handle, new byte[4], 10);

        await That(read).IsEqualTo(0);
    }

    [Test]
    public async Task Read_IntoSeveralBuffers_ShouldFillThemInOrder()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3, 4, 5 });
        using var open = handle;
        var first = new byte[2];
        var second = new byte[4];

        var read = fileSystem.RandomAccess.Read(handle, new List<Memory<byte>> { first, second }, 0);

        await That(read).IsEqualTo(5);
        await That(first).IsEqualTo(new byte[] { 1, 2 });
        await That(second).IsEqualTo(new byte[] { 3, 4, 5, 0 });
    }

    [Test]
    public async Task Write_ShouldWriteAtTheOffset()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3, 4 });
        using (handle)
        {
            fileSystem.RandomAccess.Write(handle, new byte[] { 9, 9 }, 1);
        }

        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(new byte[] { 1, 9, 9, 4 });
    }

    [Test]
    public async Task Write_BeyondTheEnd_ShouldFillTheGapWithZeros()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        using (handle)
        {
            fileSystem.RandomAccess.Write(handle, new byte[] { 9 }, 3);
        }

        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(new byte[] { 1, 0, 0, 9 });
    }

    [Test]
    public async Task Write_FromSeveralBuffers_ShouldWriteThemInOrder()
    {
        var (fileSystem, handle) = Arrange(new byte[] { });
        using (handle)
        {
            fileSystem.RandomAccess.Write(handle,
                new List<ReadOnlyMemory<byte>> { new byte[] { 1, 2 }, new byte[] { 3 } }, 0);
        }

        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(new byte[] { 1, 2, 3 });
    }

    [Test]
    public async Task Write_ShouldBeSeenByAStreamOpenOnTheSameFile()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 });
        using var open = handle;
        using var stream = fileSystem.FileStream.New(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        fileSystem.RandomAccess.Write(handle, new byte[] { 9 }, 0);
        var buffer = new byte[3];
        _ = stream.Read(buffer, 0, buffer.Length);

        await That(buffer).IsEqualTo(new byte[] { 9, 2, 3 });
    }

    [Test]
    public async Task Write_ShouldUpdateTheLastWriteTime()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        using var open = handle;
        var now = new DateTime(2030, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        fileSystem.MockTime(() => now);

        fileSystem.RandomAccess.Write(handle, new byte[] { 9 }, 0);

        await That(fileSystem.File.GetLastWriteTimeUtc(FilePath)).IsEqualTo(now);
    }

    [Test]
    public async Task Write_AtAnOffsetBeyondTheMaximumLength_ShouldThrowIOException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        using var open = handle;

        await That(() => fileSystem.RandomAccess.Write(handle, new byte[] { 9 }, long.MaxValue))
            .Throws<IOException>();
    }

    [Test]
    public async Task Write_ThroughAReadOnlyHandle_ShouldThrowUnauthorizedAccessException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 }, FileAccess.Read);
        using var open = handle;

        await That(() => fileSystem.RandomAccess.Write(handle, new byte[] { 9 }, 0))
            .Throws<UnauthorizedAccessException>();
    }

    [Test]
    public async Task Read_ThroughAWriteOnlyHandle_ShouldThrowUnauthorizedAccessException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 }, FileAccess.Write);
        using var open = handle;

        await That(() => fileSystem.RandomAccess.Read(handle, new byte[1], 0))
            .Throws<UnauthorizedAccessException>();
    }

    [Test]
    public async Task GetLength_ThroughAWriteOnlyHandle_ShouldReturnTheLength()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 }, FileAccess.Write);
        using var open = handle;

        await That(fileSystem.RandomAccess.GetLength(handle)).IsEqualTo(3)
            .Because("the length is metadata, not content");
    }

    [Test]
    public async Task Read_WithANegativeOffset_ShouldThrowArgumentOutOfRangeException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        using var open = handle;

        await That(() => fileSystem.RandomAccess.Read(handle, new byte[1], -1))
            .Throws<ArgumentOutOfRangeException>().WithParamName("fileOffset");
    }

    [Test]
    public async Task Read_WithANullHandle_ShouldThrowArgumentNullException()
    {
        var fileSystem = new MockFileSystem();

        await That(() => fileSystem.RandomAccess.Read(null!, new byte[1], 0))
            .Throws<ArgumentNullException>().WithParamName("handle");
    }

    [Test]
    public async Task Read_WithAClosedHandle_ShouldThrowObjectDisposedException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        handle.Dispose();

        await That(() => fileSystem.RandomAccess.Read(handle, new byte[1], 0))
            .Throws<ObjectDisposedException>();
    }

    [Test]
    public async Task Read_WithNullBuffers_ShouldThrowArgumentNullException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        using var open = handle;

        await That(() => fileSystem.RandomAccess.Read(handle, (IReadOnlyList<Memory<byte>>)null!, 0))
            .Throws<ArgumentNullException>().WithParamName("buffers");
    }

    [Test]
    public async Task ReadAsync_ShouldReadAtTheOffset()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 });
        using var open = handle;
        var buffer = new byte[2];

        var read = await fileSystem.RandomAccess.ReadAsync(handle, buffer, 1);

        await That(read).IsEqualTo(2);
        await That(buffer).IsEqualTo(new byte[] { 2, 3 });
    }

    [Test]
    public async Task WriteAsync_WithACancelledToken_ShouldNotWrite()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        using var open = handle;
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await That(() => fileSystem.RandomAccess.WriteAsync(handle, new byte[] { 9 }, 0,
            cancellation.Token).AsTask()).Throws<OperationCanceledException>();
        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(new byte[] { 1 });
    }

    [Test]
    public async Task WriteAsync_ShouldWriteAtTheOffset()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2 });
        using (handle)
        {
            await fileSystem.RandomAccess.WriteAsync(handle, new byte[] { 9 }, 1);
        }

        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(new byte[] { 1, 9 });
    }

    [Test]
    public async Task Handle_ShouldKeepWorkingAfterTheFileIsDeleted()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2 });
        using var open = handle;

        fileSystem.File.Delete(FilePath);
        var buffer = new byte[2];
        var read = fileSystem.RandomAccess.Read(handle, buffer, 0);

        await That(read).IsEqualTo(2);
        await That(buffer).IsEqualTo(new byte[] { 1, 2 });
    }

#if FEATURE_RANDOM_ACCESS_FLUSH_TO_DISK
    [TestCase(2, new byte[] { 1, 2 })]
    [TestCase(5, new byte[] { 1, 2, 3, 0, 0 })]
    public async Task SetLength_ShouldTruncateOrExtendTheFile(long length, byte[] expected)
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 });
        using (handle)
        {
            fileSystem.RandomAccess.SetLength(handle, length);
        }

        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(expected);
    }

    [Test]
    public async Task SetLength_ThroughAWriteOnlyHandle_ShouldResizeTheFile()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 }, FileAccess.Write);
        using (handle)
        {
            fileSystem.RandomAccess.SetLength(handle, 1);
        }

        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(new byte[] { 1 });
    }

    [Test]
    [WindowsOnly("Windows reports a resize through a read-only handle as access denied")]
    public async Task SetLength_ThroughAReadOnlyHandle_OnWindows_ShouldThrowUnauthorizedAccessException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 }, FileAccess.Read);
        using var open = handle;

        await That(() => fileSystem.RandomAccess.SetLength(handle, 0)).Throws<UnauthorizedAccessException>();
    }

    [Test]
    [UnixOnly("ftruncate reports a resize through a read-only handle as EINVAL")]
    public async Task SetLength_ThroughAReadOnlyHandle_OnUnix_ShouldThrowIOException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 }, FileAccess.Read);
        using var open = handle;

        await That(() => fileSystem.RandomAccess.SetLength(handle, 0)).Throws<IOException>()
            .Because("ftruncate on a descriptor opened for reading fails with EINVAL");
    }

    [Test]
    public async Task FlushToDisk_ShouldAcceptAnOpenHandle()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        using var open = handle;

        await That(() => fileSystem.RandomAccess.FlushToDisk(handle)).DoesNotThrow();
    }

    [Test]
    public async Task FlushToDisk_WithAClosedHandle_ShouldThrowObjectDisposedException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        handle.Dispose();

        await That(() => fileSystem.RandomAccess.FlushToDisk(handle)).Throws<ObjectDisposedException>();
    }
#endif

    [Test]
    public async Task FileStreamNew_WithAHandle_ShouldReadTheFileTheHandleHoldsOpen()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 });

        using var stream = fileSystem.FileStream.New(handle, FileAccess.Read);
        var buffer = new byte[3];
        var read = stream.Read(buffer, 0, buffer.Length);

        await That(read).IsEqualTo(3);
        await That(buffer).IsEqualTo(new byte[] { 1, 2, 3 });
    }

    [Test]
    public async Task FileStreamNew_WithAHandle_ShouldNotTakeAFileShareOfItsOwn()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData(new byte[] { 1 }));
        var handle = fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

        await That(() =>
        {
            using var stream = fileSystem.FileStream.New(handle, FileAccess.ReadWrite);
        }).DoesNotThrow().Because("a stream adopts the handle it is given");
    }

    [TestCase(FileMode.CreateNew)]
    [TestCase(FileMode.Create)]
    [TestCase(FileMode.Truncate)]
    public async Task FileStreamNew_WithAHandle_ShouldNotApplyTheHandlesModeAgain(FileMode mode)
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\temp"));
        if (mode == FileMode.Truncate)
        {
            fileSystem.AddFile(FilePath, new MockFileData("previous content"));
        }

        var handle = fileSystem.File.OpenHandle(FilePath, mode, FileAccess.ReadWrite, FileShare.ReadWrite);
        fileSystem.RandomAccess.Write(handle, new byte[] { 1, 2, 3 }, 0);

        using (var stream = fileSystem.FileStream.New(handle, FileAccess.ReadWrite))
        {
            var buffer = new byte[3];
            var read = stream.Read(buffer, 0, buffer.Length);
            await That(read).IsEqualTo(3);
        }

        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(new byte[] { 1, 2, 3 });
    }

    [Test]
    public async Task FileStreamNew_WithAHandle_WhenDisposed_ShouldCloseTheHandle()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData(new byte[] { 1 }));
        var handle = fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

        fileSystem.FileStream.New(handle, FileAccess.ReadWrite).Dispose();

        await That(handle.IsClosed).IsTrue().Because("a stream owns the handle it was constructed from");
        await That(() =>
        {
            using var stream = fileSystem.FileStream.New(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }).DoesNotThrow().Because("closing the handle released its file share");
    }

    [Test]
    public async Task FileStreamNew_WithAHandle_ShouldWriteToTheFileTheHandleHoldsOpen()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 });

        using (var stream = fileSystem.FileStream.New(handle, FileAccess.ReadWrite))
        {
            stream.Position = 1;
            stream.WriteByte(9);
        }

        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(new byte[] { 1, 9, 3 });
    }

    [Test]
    public async Task FileStreamNew_WithAHandleFromAnotherFileSystem_ShouldThrowArgumentException()
    {
        var (_, handle) = Arrange(new byte[] { 1 });
        using var open = handle;
        var other = new MockFileSystem();

        await That(() => other.FileStream.New(handle, FileAccess.Read)).Throws<ArgumentException>();
    }
    [Test]
    public async Task FileStreamNew_OnAReadOnlyHandle_ShouldNotWriteThroughIt()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 }, FileAccess.Read);
        using var stream = fileSystem.FileStream.New(handle, FileAccess.ReadWrite);

        await That(() => stream.WriteByte(9)).Throws<UnauthorizedAccessException>()
            .Because("the handle was opened for reading, whatever the stream asks for");
        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(new byte[] { 1, 2, 3 });
    }

    [Test]
    public async Task FileStreamNew_OnAWriteOnlyHandle_ShouldNotReadThroughIt()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 }, FileAccess.Write);
        using var stream = fileSystem.FileStream.New(handle, FileAccess.ReadWrite);

        await That(() => stream.ReadByte()).Throws<UnauthorizedAccessException>();
    }

    [Test]
    public async Task FileStreamNew_AfterTheHandleIsClosed_ShouldNotWrite()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 });
        var stream = fileSystem.FileStream.New(handle, FileAccess.ReadWrite);
        handle.Dispose();

        await That(() => stream.WriteByte(9)).Throws<ObjectDisposedException>();
    }

    [Test]
    public async Task FileStreamNew_WithANegativeBufferSize_ShouldThrowArgumentOutOfRangeException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        using var open = handle;

        await That(() => fileSystem.FileStream.New(handle, FileAccess.Read, -1))
            .Throws<ArgumentOutOfRangeException>().WithParamName("bufferSize");
    }

    [Test]
    public async Task FileStreamNew_AsynchronouslyOnASynchronousHandle_ShouldThrowArgumentException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        using var open = handle;

        await That(() => fileSystem.FileStream.New(handle, FileAccess.Read, 4096, isAsync: true))
            .Throws<ArgumentException>().WithParamName("handle");
    }

    [Test]
    public async Task FileStreamNew_OnAnAsynchronousHandle_ShouldBeAsynchronous()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(FilePath, new MockFileData(new byte[] { 1 }));
        var handle = fileSystem.File.OpenHandle(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read,
            FileOptions.Asynchronous);

        using var stream = fileSystem.FileStream.New(handle, FileAccess.Read);

        await That(stream.IsAsync).IsTrue();
    }

    [Test]
    public async Task ReadAsync_IntoSeveralBuffers_ShouldFillThemInOrder()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1, 2, 3 });
        using var open = handle;
        var first = new byte[1];
        var second = new byte[2];

        var read = await fileSystem.RandomAccess.ReadAsync(handle, new List<Memory<byte>> { first, second }, 0);

        await That(read).IsEqualTo(3);
        await That(first).IsEqualTo(new byte[] { 1 });
        await That(second).IsEqualTo(new byte[] { 2, 3 });
    }

    [Test]
    public async Task WriteAsync_FromSeveralBuffers_ShouldWriteThemInOrder()
    {
        var (fileSystem, handle) = Arrange(new byte[] { });
        using (handle)
        {
            await fileSystem.RandomAccess.WriteAsync(handle,
                new List<ReadOnlyMemory<byte>> { new byte[] { 1 }, new byte[] { 2, 3 } }, 0);
        }

        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(new byte[] { 1, 2, 3 });
    }

    [Test]
    public async Task Write_ConcurrentlyAtDistinctOffsets_ShouldKeepEveryWrite()
    {
        var (fileSystem, handle) = Arrange(new byte[64]);
        using (handle)
        {
            Parallel.For(0, 64, i => fileSystem.RandomAccess.Write(handle, new[] { (byte)(i + 1) }, i));
        }

        var expected = new byte[64];
        for (var i = 0; i < expected.Length; i++)
        {
            expected[i] = (byte)(i + 1);
        }

        await That(fileSystem.File.ReadAllBytes(FilePath)).IsEqualTo(expected)
            .Because("RandomAccess permits concurrent writes at distinct offsets");
    }
    [Test]
    public async Task FileStreamNew_FlushAfterTheHandleIsClosed_ShouldThrowObjectDisposedException()
    {
        var (fileSystem, handle) = Arrange(new byte[] { 1 });
        var stream = fileSystem.FileStream.New(handle, FileAccess.ReadWrite);
        handle.Dispose();

        await That(() => stream.Flush()).Throws<ObjectDisposedException>();
        await That(() => stream.Flush(flushToDisk: true)).Throws<ObjectDisposedException>();
        await That(() => stream.FlushAsync()).Throws<ObjectDisposedException>();
        await That(() => stream.Dispose()).DoesNotThrow()
            .Because("disposing a stream whose handle is already closed succeeds, as it does for FileStream");
    }

    [Test]
    public async Task OpenHandle_FromManyThreadsAtOnce_ShouldResolveEveryHandle()
    {
        for (var attempt = 0; attempt < 20; attempt++)
        {
            var fileSystem = new MockFileSystem();
            var paths = new string[16];
            for (var i = 0; i < paths.Length; i++)
            {
                paths[i] = XFS.Path($@"C:\temp\file{i}.bin");
                fileSystem.AddFile(paths[i], new MockFileData(new byte[] { (byte)i }));
            }

            var handles = new SafeFileHandle[paths.Length];
            Parallel.For(0, paths.Length, i => handles[i] = fileSystem.File.OpenHandle(paths[i]));

            for (var i = 0; i < handles.Length; i++)
            {
                using var open = handles[i];
                await That(fileSystem.RandomAccess.GetLength(open)).IsEqualTo(1)
                    .Because("every handle is registered with the one registry of its file system");
            }
        }
    }
}
#endif
