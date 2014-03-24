using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileInfoTests
    {
        [Test]
        public void MockFileInfo_Exists_ShouldReturnTrueIfFileExistsInMemoryFileSystem()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b\c.txt", new MockFileData("Demo text content") },
            });
            var fileInfo = new MockFileInfo(fileSystem, @"c:\a.txt");

            // Act
            var result = fileInfo.Exists;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MockFileInfo_Exists_ShouldReturnFalseIfFileDoesNotExistInMemoryFileSystem()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b\c.txt", new MockFileData("Demo text content") },
            });
            var fileInfo = new MockFileInfo(fileSystem, @"c:\foo.txt");

            // Act
            var result = fileInfo.Exists;

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockFileInfo_Length_ShouldReturnLengthOfFileInMemoryFileSystem()
        {
            // Arrange
            const string fileContent = "Demo text content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData(fileContent) },
                { @"c:\a\b\c.txt", new MockFileData(fileContent) },
            });
            var fileInfo = new MockFileInfo(fileSystem, @"c:\a.txt");

            // Act
            var result = fileInfo.Length;

            // Assert
            Assert.AreEqual(fileContent.Length, result);
        }

        [Test]
        public void MockFileInfo_Length_ShouldThrowFileNotFoundExceptionIfFileDoesNotExistInMemoryFileSystem()
        {
            // Arrange
            const string fileContent = "Demo text content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData(fileContent) },
                { @"c:\a\b\c.txt", new MockFileData(fileContent) },
            });
            var fileInfo = new MockFileInfo(fileSystem, @"c:\foo.txt");

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var ex = Assert.Throws<FileNotFoundException>(() => fileInfo.Length.ToString(CultureInfo.InvariantCulture));
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.AreEqual(@"c:\foo.txt", ex.FileName);
        }

        [Test]
        public void MockFileInfo_CreationTimeUtc_ShouldReturnCreationTimeUtcOfFileInMemoryFileSystem()
        {
            // Arrange
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, @"c:\a.txt");

            // Act
            var result = fileInfo.CreationTimeUtc;

            // Assert
            Assert.AreEqual(creationTime.ToUniversalTime(), result);
        }

        [Test]
        public void MockFileInfo_LastAccessTimeUtc_ShouldReturnLastAccessTimeUtcOfFileInMemoryFileSystem()
        {
            // Arrange
            var lastAccessTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastAccessTime = lastAccessTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, @"c:\a.txt");

            // Act
            var result = fileInfo.LastAccessTimeUtc;

            // Assert
            Assert.AreEqual(lastAccessTime.ToUniversalTime(), result);
        }

        [Test]
        public void MockFileInfo_LastWriteTimeUtc_ShouldReturnLastWriteTimeUtcOfFileInMemoryFileSystem()
        {
            // Arrange
            var lastWriteTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastWriteTime = lastWriteTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, @"c:\a.txt");

            // Act
            var result = fileInfo.LastWriteTimeUtc;

            // Assert
            Assert.AreEqual(lastWriteTime.ToUniversalTime(), result);
        }
 
        [Test]
        public void MockFileInfo_GetExtension_ShouldReturnExtension()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var fileInfo = new MockFileInfo(fileSystem, @"c:\a.txt");

            // Act
            var result = fileInfo.Extension;

            // Assert
            Assert.AreEqual(".txt", result);
        }

        [Test]
        public void MockFileInfo_GetExtensionWithoutExtension_ShouldReturnEmptyString()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var fileInfo = new MockFileInfo(fileSystem, @"c:\a");

            // Act
            var result = fileInfo.Extension;

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void MockFileInfo_GetDirectoryName_ShouldReturnCompleteDirectoryPath()
        {
            // Arrange
            var fileInfo = new MockFileInfo(new MockFileSystem(), @"c:\temp\level1\level2\file.txt");

            // Act
            var result = fileInfo.DirectoryName;

            Assert.AreEqual(@"c:\temp\level1\level2", result);
        }

        [Test]
        public void MockFileInfo_GetDirectory_ShouldReturnDirectoryInfoWithCorrectPath()
        {
            // Arrange
            var fileInfo = new MockFileInfo(new MockFileSystem(), @"c:\temp\level1\level2\file.txt");

            // Act
            var result = fileInfo.Directory;

            Assert.AreEqual(@"c:\temp\level1\level2", result.FullName);
        }

        [Test]
        public void MockFileInfo_OpenRead_ShouldReturnByteContentOfFile()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(@"c:\temp\file.txt", new MockFileData(new byte[] { 1, 2 }));
            var fileInfo = fileSystem.FileInfo.FromFileName(@"c:\temp\file.txt");

            // Act
            byte[] result = new byte[2];
            using (var stream = fileInfo.OpenRead())
            {
                stream.Read(result, 0, 2);
            }

            Assert.AreEqual(new byte[] { 1, 2 }, result);
        }

        [Test]
        public void MockFileInfo_OpenText_ShouldReturnStringContentOfFile()
        {
          // Arrange
          var fileSystem = new MockFileSystem();
          fileSystem.AddFile(@"c:\temp\file.txt", new MockFileData(@"line 1\r\nline 2"));
          var fileInfo = fileSystem.FileInfo.FromFileName(@"c:\temp\file.txt");

          // Act
          string result;
          using (var streamReader = fileInfo.OpenText())
          {
            result = streamReader.ReadToEnd();
          }

          Assert.AreEqual(@"line 1\r\nline 2", result);
        }
    }
}