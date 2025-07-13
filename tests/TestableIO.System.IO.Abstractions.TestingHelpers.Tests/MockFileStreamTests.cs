namespace System.IO.Abstractions.TestingHelpers.Tests;

using Collections.Generic;
using Linq;
using Threading.Tasks;

using NUnit.Framework;

using XFS = MockUnixSupport;

[TestFixture]
public class MockFileStreamTests
{
    [Test]
    public async Task MockFileStream_Flush_WritesByteToFile()
    {
        // Arrange
        var filepath = XFS.Path(@"C:\something\foo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        fileSystem.AddDirectory(XFS.Path(@"C:\something"));

        var cut = new MockFileStream(fileSystem, filepath, FileMode.Create);

        // Act
        cut.WriteByte(255);
        cut.Flush();

        // Assert
        await That(fileSystem.GetFile(filepath).Contents)
            .IsEqualTo(new byte[] { 255 });
    }

    [Test]
    public async Task MockFileStream_FlushAsync_WritesByteToFile()
    {
        // bug replication test for issue
        // https://github.com/TestableIO/System.IO.Abstractions/issues/959

        // Arrange
        var filepath = XFS.Path(@"C:\something\foo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        fileSystem.AddDirectory(XFS.Path(@"C:\something"));

        var cut = new MockFileStream(fileSystem, filepath, FileMode.Create);

        // Act
        await cut.WriteAsync(new byte[] { 255 }, 0, 1);
        await cut.FlushAsync();

        // Assert
        await That(fileSystem.GetFile(filepath).Contents)
            .IsEqualTo(new byte[] { 255 });
    }

    [Test]
    public async Task MockFileStream_Dispose_ShouldNotResurrectFile()
    {
        // path in this test case is a subject to Directory.GetParent(path) Linux issue
        // https://github.com/TestableIO/System.IO.Abstractions/issues/395
        var fileSystem = new MockFileSystem();
        var path = XFS.Path("C:\\some_folder\\test");
        var directory = fileSystem.Path.GetDirectoryName(path);
        fileSystem.AddFile(path, new MockFileData("Bla"));
        var stream = fileSystem.File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete);

        var fileCount1 = fileSystem.Directory.GetFiles(directory, "*").Length;
        fileSystem.File.Delete(path);
        var fileCount2 = fileSystem.Directory.GetFiles(directory, "*").Length;
        stream.Dispose();
        var fileCount3 = fileSystem.Directory.GetFiles(directory, "*").Length;

        await That(fileCount1).IsEqualTo(1).Because("File should have existed");
        await That(fileCount2).IsEqualTo(0).Because("File should have been deleted");
        await That(fileCount3).IsEqualTo(0).Because("Disposing stream should not have resurrected the file");
    }

    [Test]
    public async Task MockFileStream_Constructor_Reading_Nonexistent_File_Throws_Exception()
    {
        // Arrange
        var nonexistentFilePath = XFS.Path(@"c:\something\foo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        fileSystem.AddDirectory(XFS.Path(@"C:\something"));

        // Act
        await That(() => new MockFileStream(fileSystem, nonexistentFilePath, FileMode.Open)).Throws<FileNotFoundException>();

        // Assert - expect an exception
    }

    [Test]
    public async Task MockFileStream_SharedFileContents_ShouldBeVisible()
    {
        // Reproduce issue #1131: The mock FileStream class does not handle shared file contents correctly
        var fileSystem = new MockFileSystem();
        var filename = fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), fileSystem.Path.GetRandomFileName());
        
        try
        {
            using var file1 = fileSystem.FileStream.New(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var file2 = fileSystem.FileStream.New(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            
            var buffer = new byte[4];

            for (int ix = 0; ix < 3; ix++)
            {
                file1.Position = 0;
                file1.Write(BitConverter.GetBytes(ix), 0, 4);
                file1.Flush();

                file2.Position = 0;
                file2.Flush();
                var bytesRead = file2.Read(buffer, 0, buffer.Length);
                await That(bytesRead).IsEqualTo(4).Because("should read exactly 4 bytes");
                int readValue = BitConverter.ToInt32(buffer, 0);
                
                await That(readValue).IsEqualTo(ix)
                    .Because($"file2 should read the value {ix} that was written by file1, but got {readValue}");
            }
        }
        finally
        {
            fileSystem.File.Delete(filename);
        }
    }

    [Test]
    public async Task MockFileStream_SharedContent_SetLengthTruncation_ShouldBeVisible()
    {
        var fileSystem = new MockFileSystem();
        var filename = fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), fileSystem.Path.GetRandomFileName());
        
        try
        {
            using var file1 = fileSystem.FileStream.New(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var file2 = fileSystem.FileStream.New(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            
            // Write initial data
            file1.Write(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, 0, 8);
            file1.Flush();
            
            // Verify file2 can see the data
            file2.Position = 0;
            var buffer = new byte[8];
            var bytesRead = file2.Read(buffer, 0, buffer.Length);
            await That(bytesRead).IsEqualTo(8);
            
            // Truncate file via file1
            file1.SetLength(4);
            file1.Flush();
            
            // Verify file2 sees the truncation
            file2.Position = 0;
            buffer = new byte[8];
            bytesRead = file2.Read(buffer, 0, buffer.Length);
            await That(bytesRead).IsEqualTo(4)
                .Because("file2 should see truncated length");
            await That(buffer.Take(4).ToArray()).IsEquivalentTo(new byte[] { 1, 2, 3, 4 });
        }
        finally
        {
            fileSystem.File.Delete(filename);
        }
    }

    [Test]
    public async Task MockFileStream_SharedContent_PositionBeyondFileBounds_ShouldHandleGracefully()
    {
        var fileSystem = new MockFileSystem();
        var filename = fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), fileSystem.Path.GetRandomFileName());
        
        try
        {
            using var file1 = fileSystem.FileStream.New(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var file2 = fileSystem.FileStream.New(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            
            // Write some data and position file2 beyond it
            file1.Write(new byte[] { 1, 2, 3, 4 }, 0, 4);
            file1.Flush();
            
            file2.Position = 10; // Beyond file end
            
            // Truncate file via file1
            file1.SetLength(2);
            file1.Flush();
            
            // file2 position should be adjusted
            var buffer = new byte[4];
            var bytesRead = file2.Read(buffer, 0, buffer.Length);
            await That(bytesRead).IsEqualTo(0)
                .Because("reading beyond file end should return 0 bytes");
            await That(file2.Position).IsLessThanOrEqualTo(file2.Length)
                .Because("position should be adjusted to file bounds");
        }
        finally
        {
            fileSystem.File.Delete(filename);
        }
    }

    [Test]
    public async Task MockFileStream_SharedContent_ConcurrentWritesToDifferentPositions()
    {
        var fileSystem = new MockFileSystem();
        var filename = fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), fileSystem.Path.GetRandomFileName());
        
        try
        {
            using var file1 = fileSystem.FileStream.New(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var file2 = fileSystem.FileStream.New(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            
            // Pre-allocate space
            file1.SetLength(20);
            file1.Flush();
            
            // Write to different positions
            file1.Position = 0;
            file1.Write(new byte[] { 1, 1, 1, 1 }, 0, 4);
            file1.Flush();
            
            file2.Position = 10;
            file2.Write(new byte[] { 2, 2, 2, 2 }, 0, 4);
            file2.Flush();
            
            // Verify both writes are visible
            file1.Position = 10;
            var buffer1 = new byte[4];
            var bytesRead1 = file1.Read(buffer1, 0, buffer1.Length);
            await That(bytesRead1).IsEqualTo(4);
            await That(buffer1).IsEquivalentTo(new byte[] { 2, 2, 2, 2 });
            
            file2.Position = 0;
            var buffer2 = new byte[4];
            var bytesRead2 = file2.Read(buffer2, 0, buffer2.Length);
            await That(bytesRead2).IsEqualTo(4);
            await That(buffer2).IsEquivalentTo(new byte[] { 1, 1, 1, 1 });
        }
        finally
        {
            fileSystem.File.Delete(filename);
        }
    }

    [Test]
    public async Task MockFileStream_SharedContent_ReadOnlyStreamShouldRefresh()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("test.txt", new MockFileData("initial"));
        
        using var writeStream = fileSystem.FileStream.New("test.txt", FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
        using var readStream = fileSystem.FileStream.New("test.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        
        // Verify initial content
        var buffer = new byte[7];
        var bytesRead = readStream.Read(buffer, 0, buffer.Length);
        await That(bytesRead).IsEqualTo(7);
        await That(System.Text.Encoding.UTF8.GetString(buffer)).IsEqualTo("initial");
        
        // Write new content
        writeStream.Position = 0;
        var updatedBytes = "updated"u8.ToArray();
        writeStream.Write(updatedBytes, 0, updatedBytes.Length);
        writeStream.Flush();
        
        // Read-only stream should see updated content
        readStream.Position = 0;
        buffer = new byte[7];
        bytesRead = readStream.Read(buffer, 0, buffer.Length);
        await That(bytesRead).IsEqualTo(7);
        await That(System.Text.Encoding.UTF8.GetString(buffer)).IsEqualTo("updated");
    }

    [Test]
    public async Task MockFileStream_SharedContent_WriteOnlyStreamShouldNotUnnecessarilyRefresh()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("test.txt", new MockFileData("initial"));
        
        using var readStream = fileSystem.FileStream.New("test.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var writeStream = fileSystem.FileStream.New("test.txt", FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
        
        // Read initial content
        var buffer = new byte[7];
        var bytesRead = readStream.Read(buffer, 0, buffer.Length);
        await That(bytesRead).IsEqualTo(7);
        
        // Write to write-only stream
        writeStream.Position = 0;
        var changedBytes = "changed"u8.ToArray();
        writeStream.Write(changedBytes, 0, changedBytes.Length);
        writeStream.Flush();
        
        // Read stream should see the change
        readStream.Position = 0;
        buffer = new byte[7];
        bytesRead = readStream.Read(buffer, 0, buffer.Length);
        await That(bytesRead).IsEqualTo(7);
        await That(System.Text.Encoding.UTF8.GetString(buffer)).IsEqualTo("changed");
    }

    [Test]
    public async Task MockFileStream_SharedContent_PartialReadsAndWrites()
    {
        var fileSystem = new MockFileSystem();
        var filename = fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), fileSystem.Path.GetRandomFileName());
        
        try
        {
            using var file1 = fileSystem.FileStream.New(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var file2 = fileSystem.FileStream.New(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            
            // Write data in chunks
            file1.Write(new byte[] { 1, 2, 3, 4 }, 0, 4);
            file1.Write(new byte[] { 5, 6, 7, 8 }, 0, 4);
            file1.Flush();
            
            // Read data in different chunk sizes from file2
            var buffer = new byte[3];
            
            // First partial read
            var bytesRead1 = file2.Read(buffer, 0, buffer.Length);
            await That(bytesRead1).IsEqualTo(3);
            await That(buffer).IsEquivalentTo(new byte[] { 1, 2, 3 });
            
            // Second partial read
            var bytesRead2 = file2.Read(buffer, 0, buffer.Length);
            await That(bytesRead2).IsEqualTo(3);
            await That(buffer).IsEquivalentTo(new byte[] { 4, 5, 6 });
            
            // Final partial read
            buffer = new byte[5];
            var bytesRead3 = file2.Read(buffer, 0, buffer.Length);
            await That(bytesRead3).IsEqualTo(2);
            await That(buffer.Take(2).ToArray()).IsEquivalentTo(new byte[] { 7, 8 });
        }
        finally
        {
            fileSystem.File.Delete(filename);
        }
    }

    [Test]
    public async Task MockFileStream_SharedContent_FileLengthExtensionShouldBeVisible()
    {
        var fileSystem = new MockFileSystem();
        var filename = fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), fileSystem.Path.GetRandomFileName());
        
        try
        {
            using var file1 = fileSystem.FileStream.New(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var file2 = fileSystem.FileStream.New(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            
            // Write initial data
            file1.Write(new byte[] { 1, 2, 3, 4 }, 0, 4);
            file1.Flush();
            
            // Verify file2 sees initial length
            await That(file2.Length).IsEqualTo(4);
            
            // Extend file via file1
            file1.SetLength(10);
            file1.Position = 8;
            file1.Write(new byte[] { 9, 10 }, 0, 2);
            file1.Flush();
            
            // file2 should see extended file
            await That(file2.Length).IsEqualTo(10);
            
            file2.Position = 8;
            var buffer = new byte[2];
            var bytesRead = file2.Read(buffer, 0, buffer.Length);
            await That(bytesRead).IsEqualTo(2);
            await That(buffer).IsEquivalentTo(new byte[] { 9, 10 });
        }
        finally
        {
            fileSystem.File.Delete(filename);
        }
    }

    [Test]
    public async Task MockFileStream_SharedContent_DisposedStreamsShouldNotAffectVersioning()
    {
        var fileSystem = new MockFileSystem();
        var filename = fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), fileSystem.Path.GetRandomFileName());
        
        try
        {
            using var persistentStream = fileSystem.FileStream.New(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            
            // Create and dispose a stream that writes data
            using (var tempStream = fileSystem.FileStream.New(filename, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                tempStream.Write(new byte[] { 1, 2, 3, 4 }, 0, 4);
                tempStream.Flush();
            } // tempStream is disposed here
            
            // persistentStream should still see the data
            persistentStream.Position = 0;
            var buffer = new byte[4];
            var bytesRead = persistentStream.Read(buffer, 0, buffer.Length);
            await That(bytesRead).IsEqualTo(4);
            await That(buffer).IsEquivalentTo(new byte[] { 1, 2, 3, 4 });
        }
        finally
        {
            fileSystem.File.Delete(filename);
        }
    }

    [Test]
    public async Task MockFileStream_SharedContent_LargeFile_ShouldPerformCorrectly()
    {
        var fileSystem = new MockFileSystem();
        var filename = fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), fileSystem.Path.GetRandomFileName());
        
        try
        {
            using var file1 = fileSystem.FileStream.New(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var file2 = fileSystem.FileStream.New(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            
            // Test with a reasonably large amount of data to ensure synchronization works correctly
            var largeData = new byte[1024 * 100]; // 100KB
            for (int i = 0; i < largeData.Length; i++)
            {
                largeData[i] = (byte)(i % 256);
            }
            
            // Test multiple write/read cycles to ensure synchronization is maintained
            for (int cycle = 0; cycle < 3; cycle++)
            {
                // Modify some data for this cycle
                largeData[cycle] = (byte)(cycle + 100);
                
                file1.Position = 0;
                file1.Write(largeData, 0, largeData.Length);
                file1.Flush();
                
                // file2 should see the updated data
                file2.Position = 0;
                var readData = new byte[largeData.Length];
                var bytesRead = file2.Read(readData, 0, readData.Length);
                
                await That(bytesRead).IsEqualTo(largeData.Length);
                await That(readData).IsEquivalentTo(largeData);
                await That(readData[cycle]).IsEqualTo((byte)(cycle + 100))
                    .Because($"cycle {cycle} should see the updated data");
            }
        }
        finally
        {
            fileSystem.File.Delete(filename);
        }
    }


    [Test]
    public async Task MockFileStream_Constructor_ReadTypeNotWritable()
    {
        // Arrange
        var filePath = @"C:\test.txt";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filePath, new MockFileData("hi") }
        });

        // Act
        var stream = new MockFileStream(fileSystem, filePath, FileMode.Open, FileAccess.Read);

        await That(stream.CanWrite).IsFalse();
        await That(() => stream.WriteByte(1)).Throws<NotSupportedException>();
    }

    [Test]
    [TestCase(FileAccess.Write)]
    [TestCase(FileAccess.ReadWrite)]
    public async Task MockFileStream_Constructor_WriteAccessOnReadOnlyFile_Throws_Exception(
        FileAccess fileAccess)
    {
        // Arrange
        var filePath = @"C:\test.txt";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filePath, new MockFileData("hi") { Attributes = FileAttributes.ReadOnly } }
        });

        // Act
        await That(() => new MockFileStream(fileSystem, filePath, FileMode.Open, fileAccess)).Throws<UnauthorizedAccessException>();
    }

    [Test]
    public async Task MockFileStream_Constructor_ReadAccessOnReadOnlyFile_Does_Not_Throw_Exception()
    {
        // Arrange
        var filePath = @"C:\test.txt";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filePath, new MockFileData("hi") { Attributes = FileAttributes.ReadOnly } }
        });

        // Act
        await That(() => new MockFileStream(fileSystem, filePath, FileMode.Open, FileAccess.Read)).DoesNotThrow();
    }


    [Test]
    public async Task MockFileStream_Constructor_WriteAccessOnNonReadOnlyFile_Does_Not_Throw_Exception()
    {
        // Arrange
        var filePath = @"C:\test.txt";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filePath, new MockFileData("hi") { Attributes = FileAttributes.Normal } }
        });

        // Act
        await That(() => new MockFileStream(fileSystem, filePath, FileMode.Open, FileAccess.Write)).DoesNotThrow();
    }

    [Test]
    [TestCase(FileShare.None, FileAccess.Read)]
    [TestCase(FileShare.None, FileAccess.ReadWrite)]
    [TestCase(FileShare.None, FileAccess.Write)]
    [TestCase(FileShare.Read, FileAccess.Write)]
    [TestCase(FileShare.Read, FileAccess.ReadWrite)]
    [TestCase(FileShare.Write, FileAccess.Read)]
    public async Task MockFileStream_Constructor_Insufficient_FileShare_Throws_Exception(
        FileShare allowedFileShare,
        FileAccess fileAccess)
    {
        var filePath = @"C:\locked.txt";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filePath, new MockFileData("cannot access") { AllowedFileShare = allowedFileShare } }
        });

        await That(() => new MockFileStream(fileSystem, filePath, FileMode.Open, fileAccess)).Throws<IOException>();
    }

    [Test]
    [TestCase(FileShare.Read, FileAccess.Read)]
    [TestCase(FileShare.Read | FileShare.Write, FileAccess.Read)]
    [TestCase(FileShare.Read | FileShare.Write, FileAccess.ReadWrite)]
    [TestCase(FileShare.ReadWrite, FileAccess.Read)]
    [TestCase(FileShare.ReadWrite, FileAccess.ReadWrite)]
    [TestCase(FileShare.ReadWrite, FileAccess.Write)]
    public async Task MockFileStream_Constructor_Sufficient_FileShare_Does_Not_Throw_Exception(
        FileShare allowedFileShare,
        FileAccess fileAccess)
    {
        var filePath = @"C:\locked.txt";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filePath, new MockFileData("cannot access") { AllowedFileShare = allowedFileShare } }
        });

        await That(() => new MockFileStream(fileSystem, filePath, FileMode.Open, fileAccess)).DoesNotThrow();
    }

    [Test]
    public async Task MockFileStream_Close_MultipleCallsDoNotThrow()
    {
        var fileSystem = new MockFileSystem();
        var path = XFS.Path("C:\\test");
        fileSystem.AddFile(path, new MockFileData("Bla"));
        var stream = fileSystem.File.OpenRead(path);

        // Act
        stream.Close();

        // Assert
        await That(() => stream.Close()).DoesNotThrow();
    }

    [Test]
    public async Task MockFileStream_Dispose_MultipleCallsDoNotThrow()
    {
        var fileSystem = new MockFileSystem();
        var path = XFS.Path("C:\\test");
        fileSystem.AddFile(path, new MockFileData("Bla"));
        var stream = fileSystem.File.OpenRead(path);

        // Act
        stream.Dispose();

        // Assert
        await That(() => stream.Dispose()).DoesNotThrow();
    }

    [Test]
    public async Task MockFileStream_Dispose_OperationsAfterDisposeThrow()
    {
        var fileSystem = new MockFileSystem();
        var path = XFS.Path("C:\\test");
        fileSystem.AddFile(path, new MockFileData(new byte[0]));
        var stream = fileSystem.FileInfo.New(path).OpenWrite();

        // Act
        stream.Dispose();

        // Assert
        await That(() => stream.WriteByte(0)).Throws<ObjectDisposedException>();
    }

    [Test]
    public async Task MockFileStream_Flush_ShouldNotChangePosition()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path = XFS.Path("C:\\test");
        fileSystem.AddFile(path, new MockFileData(new byte[0]));

        using (var stream = fileSystem.FileInfo.New(path).OpenWrite())
        {
            // Act
            stream.Write(new byte[400], 0, 400);
            stream.Seek(200, SeekOrigin.Begin);
            stream.Flush();

            // Assert
            await That(stream.Position).IsEqualTo(200);
        }
    }

    [Test]
    public async Task MockFileStream_FlushBool_ShouldNotChangePosition([Values] bool flushToDisk)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path = XFS.Path("C:\\test");
        fileSystem.AddFile(path, new MockFileData(new byte[0]));

        using (var stream = fileSystem.FileInfo.New(path).OpenWrite())
        {
            // Act
            stream.Write(new byte[400], 0, 400);
            stream.Seek(200, SeekOrigin.Begin);
            stream.Flush(flushToDisk);

            // Assert
            await That(stream.Position).IsEqualTo(200);
        }
    }

    [Test]
    public async Task MockFileStream_Null_ShouldReturnSingletonObject()
    {
        var result1 = MockFileStream.Null;
        var result2 = MockFileStream.Null;

        await That(result1).IsSameAs(result2);
    }
        
#if FEATURE_ASYNC_FILE
    [Test]
    public async Task MockFileStream_DisposeAsync_ShouldNotThrow()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.File.WriteAllText("foo.txt", "");
        {
            await using var reportStream = fileSystem.File.OpenRead("foo.txt");
        }
    }
#endif

    [Test]
    public async Task MockFileStream_Null_ShouldHaveExpectedProperties()
    {
        var result = MockFileStream.Null;

        await That(result.Name).IsEqualTo(".");
        await That(result.Length).IsEqualTo(0);
        await That(result.IsAsync).IsTrue();
    }
        
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public async Task MockFileStream_WhenBufferSizeIsNotPositive_ShouldThrowArgumentNullException(int bufferSize)
    {
        var fileSystem = new MockFileSystem();
        fileSystem.File.WriteAllText("foo.txt", "");
        fileSystem.File.WriteAllText("bar.txt", "");
        using var source = fileSystem.FileInfo.New(@"foo.txt").OpenRead();
        using var destination = fileSystem.FileInfo.New(@"bar.txt").OpenWrite();

        async Task Act() =>
            await source.CopyToAsync(destination, bufferSize);
        await That(Act).Throws<ArgumentOutOfRangeException>();
    }
        
    [Test]
    public async Task MockFileStream_WhenDestinationIsClosed_ShouldThrowObjectDisposedException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.File.WriteAllText("foo.txt", "");
        using var source = fileSystem.FileInfo.New(@"foo.txt").OpenRead();
        using var destination = new MemoryStream();
        destination.Close();

        async Task Act() =>
            await source.CopyToAsync(destination);
        await That(Act).Throws<ObjectDisposedException>();
    }
        
    [Test]
    public async Task MockFileStream_WhenDestinationIsNull_ShouldThrowArgumentNullException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.File.WriteAllText("foo.txt", "");
        using var source = fileSystem.FileInfo.New(@"foo.txt").OpenRead();

        async Task Act() =>
            await source.CopyToAsync(null);
        await That(Act).Throws<ArgumentNullException>();
    }
        
    [Test]
    public async Task MockFileStream_WhenDestinationIsReadOnly_ShouldThrowNotSupportedException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.File.WriteAllText("foo.txt", "");
        fileSystem.File.WriteAllText("bar.txt", "");
        using var source = fileSystem.FileInfo.New(@"foo.txt").OpenRead();
        using var destination = fileSystem.FileInfo.New(@"bar.txt").OpenRead();

        async Task Act() =>
            await source.CopyToAsync(destination);
        await That(Act).Throws<NotSupportedException>();
    }

    [Test]
    public async Task MockFileStream_WhenSourceIsClosed_ShouldThrowObjectDisposedException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.File.WriteAllText("foo.txt", "");
        fileSystem.File.WriteAllText("bar.txt", "");
        using var source = fileSystem.FileInfo.New(@"foo.txt").OpenRead();
        using var destination = fileSystem.FileInfo.New(@"bar.txt").OpenWrite();
        source.Close();

        async Task Act() =>
            await source.CopyToAsync(destination);
        await That(Act).Throws<ObjectDisposedException>();
    }

    [Test]
    public async Task MockFileStream_WhenSourceIsWriteOnly_ShouldThrowNotSupportedException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.File.WriteAllText("foo.txt", "");
        fileSystem.File.WriteAllText("bar.txt", "");
        using var source = fileSystem.FileInfo.New(@"foo.txt").OpenWrite();
        using var destination = fileSystem.FileInfo.New(@"bar.txt").OpenWrite();

        async Task Act() =>
            await source.CopyToAsync(destination);
        await That(Act).Throws<NotSupportedException>();
    }
}