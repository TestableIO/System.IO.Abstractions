namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using System.Collections.Generic;

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

            var cut = new MockFileStream(fileSystem, filepath, MockFileStream.StreamType.WRITE);

            // Act
            cut.WriteByte(255);
            cut.Flush();

            // Assert
            CollectionAssert.AreEqual(new byte[]{255}, fileSystem.GetFile(filepath).Contents);
        }

        [Test]
        public void MockFileStream_Dispose_ShouldNotResurrectFile()
        {
            // path in this test case is a subject to Directory.GetParent(path) Linux issue
            // https://github.com/System-IO-Abstractions/System.IO.Abstractions/issues/395
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

            Assert.AreEqual(1, fileCount1, "File should have existed");
            Assert.AreEqual(0, fileCount2, "File should have been deleted");
            Assert.AreEqual(0, fileCount3, "Disposing stream should not have resurrected the file");
        }

        [Test]
        public void MockFileStream_Constructor_Reading_Nonexistent_File_Throws_Exception()
        {
            // Arrange
            var nonexistentFilePath = XFS.Path(@"c:\something\foo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            fileSystem.AddDirectory(XFS.Path(@"C:\something"));

            // Act
            Assert.Throws<FileNotFoundException>(() => new MockFileStream(fileSystem, nonexistentFilePath, MockFileStream.StreamType.READ));

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
            var stream = new MockFileStream(fileSystem, filePath, MockFileStream.StreamType.READ);

            Assert.IsFalse(stream.CanWrite);
            Assert.Throws<NotSupportedException>(() => stream.WriteByte(1));
        }
        
        [Test]
        [TestCase(FileShare.None, MockFileStream.StreamType.READ)]
        [TestCase(FileShare.None, MockFileStream.StreamType.WRITE)]
        [TestCase(FileShare.None, MockFileStream.StreamType.APPEND)]
        [TestCase(FileShare.None, MockFileStream.StreamType.TRUNCATE)]
        [TestCase(FileShare.Read, MockFileStream.StreamType.WRITE)]
        [TestCase(FileShare.Read, MockFileStream.StreamType.APPEND)]
        [TestCase(FileShare.Read, MockFileStream.StreamType.TRUNCATE)]
        [TestCase(FileShare.Write, MockFileStream.StreamType.READ)]
        public void MockFileStream_Constructor_Insufficient_FileShare_Throws_Exception(FileShare allowedFileShare, MockFileStream.StreamType streamType)
        {
            var filePath = @"C:\locked.txt";            
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filePath, new MockFileData("cannot access") { AllowedFileShare = allowedFileShare } }
            });
            
            Assert.Throws<IOException>(() => new MockFileStream(fileSystem, filePath, streamType));
        }
        
        [Test]
        [TestCase(FileShare.Read, MockFileStream.StreamType.READ)]
        [TestCase(FileShare.Read | FileShare.Write, MockFileStream.StreamType.READ)]
        [TestCase(FileShare.Read | FileShare.Write, MockFileStream.StreamType.APPEND)]
        [TestCase(FileShare.Read | FileShare.Write, MockFileStream.StreamType.TRUNCATE)]
        [TestCase(FileShare.ReadWrite, MockFileStream.StreamType.READ)]
        [TestCase(FileShare.ReadWrite, MockFileStream.StreamType.WRITE)]
        [TestCase(FileShare.ReadWrite, MockFileStream.StreamType.APPEND)]
        [TestCase(FileShare.ReadWrite, MockFileStream.StreamType.TRUNCATE)]
        public void MockFileStream_Constructor_Sufficient_FileShare_Does_Not_Throw_Exception(FileShare allowedFileShare, MockFileStream.StreamType streamType)
        {
            var filePath = @"C:\locked.txt";       
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filePath, new MockFileData("cannot access") { AllowedFileShare = allowedFileShare } }
            });
            
            Assert.DoesNotThrow(() => new MockFileStream(fileSystem, filePath, streamType));
        }

        [Test]
        public void MockFileStream_Close_MultipleCallsDontThrow()
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
        public void MockFileStream_Dispose_MultipleCallsDontThrow()
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
            var stream = fileSystem.FileInfo.FromFileName(path).OpenWrite();

            // Act
            stream.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0));
        }
    }
}
