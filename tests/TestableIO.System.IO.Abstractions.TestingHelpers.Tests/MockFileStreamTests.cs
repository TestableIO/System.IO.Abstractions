namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using NUnit.Framework;

    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockFileStreamTests
    {
        [Test]
        public void MockFileStream_Flush_WritesByteToFile()
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
            Assert.That(fileSystem.GetFile(filepath).Contents,
                Is.EqualTo(new byte[] { 255 }));
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
            Assert.That(fileSystem.GetFile(filepath).Contents,
                Is.EqualTo(new byte[] { 255 }));
        }

        [Test]
        public void MockFileStream_Dispose_ShouldNotResurrectFile()
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

            Assert.That(fileCount1, Is.EqualTo(1), "File should have existed");
            Assert.That(fileCount2, Is.EqualTo(0), "File should have been deleted");
            Assert.That(fileCount3, Is.EqualTo(0), "Disposing stream should not have resurrected the file");
        }

        [Test]
        public void MockFileStream_Constructor_Reading_Nonexistent_File_Throws_Exception()
        {
            // Arrange
            var nonexistentFilePath = XFS.Path(@"c:\something\foo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            fileSystem.AddDirectory(XFS.Path(@"C:\something"));

            // Act
            Assert.Throws<FileNotFoundException>(() => new MockFileStream(fileSystem, nonexistentFilePath, FileMode.Open));

            // Assert - expect an exception
        }

        [Test]
        public void MockFileStream_Constructor_ReadTypeNotWritable()
        {
            // Arrange
            var filePath = @"C:\test.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filePath, new MockFileData("hi") }
            });

            // Act
            var stream = new MockFileStream(fileSystem, filePath, FileMode.Open, FileAccess.Read);

            Assert.That(stream.CanWrite, Is.False);
            Assert.Throws<NotSupportedException>(() => stream.WriteByte(1));
        }

        [Test]
        [TestCase(FileAccess.Write)]
        [TestCase(FileAccess.ReadWrite)]
        public void MockFileStream_Constructor_WriteAccessOnReadOnlyFile_Throws_Exception(
            FileAccess fileAccess)
        {
            // Arrange
            var filePath = @"C:\test.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filePath, new MockFileData("hi") { Attributes = FileAttributes.ReadOnly } }
            });

            // Act
            Assert.Throws<UnauthorizedAccessException>(() => new MockFileStream(fileSystem, filePath, FileMode.Open, fileAccess));
        }

        [Test]
        public void MockFileStream_Constructor_ReadAccessOnReadOnlyFile_Does_Not_Throw_Exception()
        {
            // Arrange
            var filePath = @"C:\test.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filePath, new MockFileData("hi") { Attributes = FileAttributes.ReadOnly } }
            });

            // Act
            Assert.DoesNotThrow(() => new MockFileStream(fileSystem, filePath, FileMode.Open, FileAccess.Read));
        }


        [Test]
        public void MockFileStream_Constructor_WriteAccessOnNonReadOnlyFile_Does_Not_Throw_Exception()
        {
            // Arrange
            var filePath = @"C:\test.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filePath, new MockFileData("hi") { Attributes = FileAttributes.Normal } }
            });

            // Act
            Assert.DoesNotThrow(() => new MockFileStream(fileSystem, filePath, FileMode.Open, FileAccess.Write));
        }

        [Test]
        [TestCase(FileShare.None, FileAccess.Read)]
        [TestCase(FileShare.None, FileAccess.ReadWrite)]
        [TestCase(FileShare.None, FileAccess.Write)]
        [TestCase(FileShare.Read, FileAccess.Write)]
        [TestCase(FileShare.Read, FileAccess.ReadWrite)]
        [TestCase(FileShare.Write, FileAccess.Read)]
        public void MockFileStream_Constructor_Insufficient_FileShare_Throws_Exception(
            FileShare allowedFileShare,
            FileAccess fileAccess)
        {
            var filePath = @"C:\locked.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filePath, new MockFileData("cannot access") { AllowedFileShare = allowedFileShare } }
            });

            Assert.Throws<IOException>(() => new MockFileStream(fileSystem, filePath, FileMode.Open, fileAccess));
        }

        [Test]
        [TestCase(FileShare.Read, FileAccess.Read)]
        [TestCase(FileShare.Read | FileShare.Write, FileAccess.Read)]
        [TestCase(FileShare.Read | FileShare.Write, FileAccess.ReadWrite)]
        [TestCase(FileShare.ReadWrite, FileAccess.Read)]
        [TestCase(FileShare.ReadWrite, FileAccess.ReadWrite)]
        [TestCase(FileShare.ReadWrite, FileAccess.Write)]
        public void MockFileStream_Constructor_Sufficient_FileShare_Does_Not_Throw_Exception(
            FileShare allowedFileShare,
            FileAccess fileAccess)
        {
            var filePath = @"C:\locked.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filePath, new MockFileData("cannot access") { AllowedFileShare = allowedFileShare } }
            });

            Assert.DoesNotThrow(() => new MockFileStream(fileSystem, filePath, FileMode.Open, fileAccess));
        }

        [Test]
        public void MockFileStream_Close_MultipleCallsDoNotThrow()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:\\test");
            fileSystem.AddFile(path, new MockFileData("Bla"));
            var stream = fileSystem.File.OpenRead(path);

            // Act
            stream.Close();

            // Assert
            Assert.DoesNotThrow(() => stream.Close());
        }

        [Test]
        public void MockFileStream_Dispose_MultipleCallsDoNotThrow()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:\\test");
            fileSystem.AddFile(path, new MockFileData("Bla"));
            var stream = fileSystem.File.OpenRead(path);

            // Act
            stream.Dispose();

            // Assert
            Assert.DoesNotThrow(() => stream.Dispose());
        }

        [Test]
        public void MockFileStream_Dispose_OperationsAfterDisposeThrow()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:\\test");
            fileSystem.AddFile(path, new MockFileData(new byte[0]));
            var stream = fileSystem.FileInfo.New(path).OpenWrite();

            // Act
            stream.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0));
        }

        [Test]
        public void MockFileStream_Flush_ShouldNotChangePosition()
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
                Assert.That(stream.Position, Is.EqualTo(200));
            }
        }

        [Test]
        public void MockFileStream_FlushBool_ShouldNotChangePosition([Values] bool flushToDisk)
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
                Assert.That(stream.Position, Is.EqualTo(200));
            }
        }

        [Test]
        public void MockFileStream_Null_ShouldReturnSingletonObject()
        {
            var result1 = MockFileStream.Null;
            var result2 = MockFileStream.Null;

            Assert.That(result1, Is.SameAs(result2));
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
        public void MockFileStream_Null_ShouldHaveExpectedProperties()
        {
            var result = MockFileStream.Null;

            Assert.That(result.Name, Is.EqualTo("."));
            Assert.That(result.Length, Is.Zero);
            Assert.That(result.IsAsync, Is.True);
        }
        
        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void MockFileStream_WhenBufferSizeIsNotPositive_ShouldThrowArgumentNullException(int bufferSize)
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("foo.txt", "");
            fileSystem.File.WriteAllText("bar.txt", "");
            using var source = fileSystem.FileInfo.New(@"foo.txt").OpenRead();
            using var destination = fileSystem.FileInfo.New(@"bar.txt").OpenWrite();
            
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => 
                await source.CopyToAsync(destination, bufferSize));
        }
        
        [Test]
        public void MockFileStream_WhenDestinationIsClosed_ShouldThrowObjectDisposedException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("foo.txt", "");
            using var source = fileSystem.FileInfo.New(@"foo.txt").OpenRead();
            using var destination = new MemoryStream();
            destination.Close();
            
            Assert.ThrowsAsync<ObjectDisposedException>(async () => 
                await source.CopyToAsync(destination));
        }
        
        [Test]
        public void MockFileStream_WhenDestinationIsNull_ShouldThrowArgumentNullException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("foo.txt", "");
            using var source = fileSystem.FileInfo.New(@"foo.txt").OpenRead();
            
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await source.CopyToAsync(null));
        }
        
        [Test]
        public void MockFileStream_WhenDestinationIsReadOnly_ShouldThrowNotSupportedException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("foo.txt", "");
            fileSystem.File.WriteAllText("bar.txt", "");
            using var source = fileSystem.FileInfo.New(@"foo.txt").OpenRead();
            using var destination = fileSystem.FileInfo.New(@"bar.txt").OpenRead();
            
            Assert.ThrowsAsync<NotSupportedException>(async () => 
                await source.CopyToAsync(destination));
        }

        [Test]
        public void MockFileStream_WhenSourceIsClosed_ShouldThrowObjectDisposedException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("foo.txt", "");
            fileSystem.File.WriteAllText("bar.txt", "");
            using var source = fileSystem.FileInfo.New(@"foo.txt").OpenRead();
            using var destination = fileSystem.FileInfo.New(@"bar.txt").OpenWrite();
            source.Close();
            
            Assert.ThrowsAsync<ObjectDisposedException>(async () => 
                await source.CopyToAsync(destination));
        }

        [Test]
        public void MockFileStream_WhenSourceIsWriteOnly_ShouldThrowNotSupportedException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("foo.txt", "");
            fileSystem.File.WriteAllText("bar.txt", "");
            using var source = fileSystem.FileInfo.New(@"foo.txt").OpenWrite();
            using var destination = fileSystem.FileInfo.New(@"bar.txt").OpenWrite();
            
            Assert.ThrowsAsync<NotSupportedException>(async () => 
                await source.CopyToAsync(destination));
        }
    }
}
