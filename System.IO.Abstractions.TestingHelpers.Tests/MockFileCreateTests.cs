namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using Globalization;

    using NUnit.Framework;

    using Text;

    using XFS = MockUnixSupport;

    public class MockFileCreateTests
    {
        [Test]
        public void Mockfile_Create_ShouldCreateNewStream()
        {
            string fullPath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            var sut = new MockFile(fileSystem);

            Assert.That(fileSystem.FileExists(fullPath), Is.False);

            sut.Create(fullPath).Dispose();

            Assert.That(fileSystem.FileExists(fullPath), Is.True);
        }

        [Test]
        public void Mockfile_Create_CanWriteToNewStream()
        {
            string fullPath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));
            var data = new UTF8Encoding(false).GetBytes("Test string");

            var sut = new MockFile(fileSystem);
            using (var stream = sut.Create(fullPath))
            {
                stream.Write(data, 0, data.Length);
            }

            var mockFileData = fileSystem.GetFile(fullPath);
            var fileData = mockFileData.Contents;

            Assert.That(fileData, Is.EqualTo(data));
        }

        [Test]
        public void Mockfile_Create_OverwritesExistingFile()
        {
            string path = XFS.Path(@"c:\some\file.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\some"));

            var mockFile = new MockFile(fileSystem);

            // Create a file
            using (var stream = mockFile.Create(path))
            {
                var contents = new UTF8Encoding(false).GetBytes("Test 1");
                stream.Write(contents, 0, contents.Length);
            }

            // Create new file that should overwrite existing file
            var expectedContents = new UTF8Encoding(false).GetBytes("Test 2");
            using (var stream = mockFile.Create(path))
            {
                stream.Write(expectedContents, 0, expectedContents.Length);
            }

            var actualContents = fileSystem.GetFile(path).Contents;

            Assert.That(actualContents, Is.EqualTo(expectedContents));
        }

        [Test]
        public void Mockfile_Create_ShouldThrowUnauthorizedAccessExceptionIfPathIsReadOnly()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\read-only.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { { path, new MockFileData("Content") } });
            var mockFile = new MockFile(fileSystem);

            // Act
            mockFile.SetAttributes(path, FileAttributes.ReadOnly);

            // Assert
            var exception =  Assert.Throws<UnauthorizedAccessException>(() => mockFile.Create(path).Dispose());
            Assert.That(exception.Message, Is.EqualTo(string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path)));
        }

        [Test]
        public void Mockfile_Create_ShouldThrowArgumentExceptionIfPathIsZeroLength()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.Create("");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [TestCase("\"")]
        [TestCase("<")]
        [TestCase(">")]
        [TestCase("|")]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockFile_Create_ShouldThrowArgumentNullExceptionIfPathIsNull1(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.Create(path);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_Create_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.Create(path);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void MockFile_Create_ShouldThrowArgumentNullExceptionIfPathIsNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.Create(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.That(exception.Message, Does.StartWith("Path cannot be null."));
        }

        [Test]
        public void MockFile_Create_ShouldThrowDirectoryNotFoundExceptionIfCreatingAndParentPathDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var file = XFS.Path("C:\\path\\NotFound.ext");

            // Act
            TestDelegate action = () => fileSystem.File.Create(file);

            // Assert
            var exception = Assert.Throws<DirectoryNotFoundException>(action);
            Assert.That(exception.Message, Does.StartWith("Could not find a part of the path"));
        }

        [Test]
        public void MockFile_Create_TruncateShouldWriteNewContents()
        {
            // Arrange
            string testFileName = XFS.Path(@"c:\someFile.txt");
            var fileSystem = new MockFileSystem();
            
            using (var stream = fileSystem.FileStream.Create(testFileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("original_text");
                }
            }

            // Act
            using (var stream = fileSystem.FileStream.Create(testFileName, FileMode.Truncate, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("new_text");
                }
            }

            // Assert
            Assert.That(fileSystem.File.ReadAllText(testFileName), Is.EqualTo("new_text"));
        }

        [Test]
        public void MockFile_Create_TruncateShouldClearFileContentsOnOpen()
        {
            // Arrange
            string testFileName = XFS.Path(@"c:\someFile.txt");
            var fileSystem = new MockFileSystem();

            using (var stream = fileSystem.FileStream.Create(testFileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("original_text");
                }
            }

            // Act
            using (var stream = fileSystem.FileStream.Create(testFileName, FileMode.Truncate, FileAccess.Write))
            {
                // Opening the stream is enough to reset the contents
            }

            // Assert
            Assert.That(fileSystem.File.ReadAllText(testFileName), Is.EqualTo(string.Empty));
        }

        [Test]
        public void MockFile_Create_DeleteOnCloseOption_FileExistsWhileStreamIsOpen()
        {
            var root = XFS.Path(@"C:\");
            var filePath = XFS.Path(@"C:\test.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(root);

            using (fileSystem.File.Create(filePath, 4096, FileOptions.DeleteOnClose))
            {
                Assert.IsTrue(fileSystem.File.Exists(filePath));
            }
        }

        [Test]
        public void MockFile_Create_DeleteOnCloseOption_FileDeletedWhenStreamIsClosed()
        {
            var root = XFS.Path(@"C:\");
            var filePath = XFS.Path(@"C:\test.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(root);

            using (fileSystem.File.Create(filePath, 4096, FileOptions.DeleteOnClose))
            {
            }

            Assert.IsFalse(fileSystem.File.Exists(filePath));
        }

#if NET40
        [Test]
        public void MockFile_Create_EncryptedOption_FileNotYetEncryptedsWhenStreamIsOpen()
        {
            var root = XFS.Path(@"C:\");
            var filePath = XFS.Path(@"C:\test.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(root);

            using (var stream = fileSystem.File.Create(filePath, 4096, FileOptions.Encrypted))
            {
                var fileInfo = fileSystem.FileInfo.FromFileName(filePath);
                Assert.IsFalse(fileInfo.Attributes.HasFlag(FileAttributes.Encrypted));
            }
        }

        [Test]
        public void MockFile_Create_EncryptedOption_EncryptsFileWhenStreamIsClose()
        {
            var root = XFS.Path(@"C:\");
            var filePath = XFS.Path(@"C:\test.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(root);

            using (var stream = fileSystem.File.Create(filePath, 4096, FileOptions.Encrypted))
            {
            }

            var fileInfo = fileSystem.FileInfo.FromFileName(filePath);
            Assert.IsTrue(fileInfo.Attributes.HasFlag(FileAttributes.Encrypted));
        }
#endif

        [Test]
        public void MockFile_Create_ShouldWorkWithRelativePath()
        {
            var relativeFile = "file.txt";
            var fileSystem = new MockFileSystem();

            fileSystem.File.Create(relativeFile).Close();

            Assert.That(fileSystem.File.Exists(relativeFile));
        }
    }
}
