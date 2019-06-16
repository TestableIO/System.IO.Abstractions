namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using NUnit.Framework;

    using Text;

    using XFS = MockUnixSupport;

#if NETCOREAPP2_0
    using System.Threading.Tasks;
#endif

    public class MockFileWriteAllTextTests {
        [Test]
        public void MockFile_WriteAllText_ShouldWriteTextFileToMemoryFileSystem()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "Hello there!";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            // Act
            fileSystem.File.WriteAllText(path, fileContent);

            // Assert
            Assert.AreEqual(
                fileContent,
                fileSystem.GetFile(path).TextContents);
        }

        [Test]
        public void MockFile_WriteAllText_ShouldOverriteAnExistingFile()
        {
            // http://msdn.microsoft.com/en-us/library/ms143375.aspx

            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            // Act
            fileSystem.File.WriteAllText(path, "foo");
            fileSystem.File.WriteAllText(path, "bar");

            // Assert
            Assert.AreEqual("bar", fileSystem.GetFile(path).TextContents);
        }

        [Test]
        public void MockFile_WriteAllText_ShouldThrowAnUnauthorizedAccessExceptionIfFileIsHidden()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("this is hidden") },
            });
            fileSystem.File.SetAttributes(path, FileAttributes.Hidden);

            // Act
            TestDelegate action = () => fileSystem.File.WriteAllText(path, "hello world");

            // Assert
            Assert.Throws<UnauthorizedAccessException>(action, "Access to the path '{0}' is denied.", path);
        }

        [Test]
        public void MockFile_WriteAllText_ShouldThrowAnArgumentExceptionIfThePathIsEmpty()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.WriteAllText(string.Empty, "hello world");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void MockFile_WriteAllText_ShouldNotThrowAnArgumentNullExceptionIfTheContentIsNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string directoryPath = XFS.Path(@"c:\something");
            string filePath = XFS.Path(@"c:\something\demo.txt");
            fileSystem.AddDirectory(directoryPath);

            // Act
            fileSystem.File.WriteAllText(filePath, null);

            // Assert
            // no exception should be thrown, also the documentation says so
            var data = fileSystem.GetFile(filePath);
            Assert.That(data.Contents, Is.Empty);
        }

        [Test]
        public void MockFile_WriteAllText_ShouldThrowAnUnauthorizedAccessExceptionIfTheFileIsReadOnly()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string filePath = XFS.Path(@"c:\something\demo.txt");
            var mockFileData = new MockFileData(new byte[0]);
            mockFileData.Attributes = FileAttributes.ReadOnly;
            fileSystem.AddFile(filePath, mockFileData);

            // Act
            TestDelegate action = () => fileSystem.File.WriteAllText(filePath, null);

            // Assert
            Assert.Throws<UnauthorizedAccessException>(action);
        }

        [Test]
        public void MockFile_WriteAllText_ShouldThrowAnUnauthorizedAccessExceptionIfThePathIsOneDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string directoryPath = XFS.Path(@"c:\something");
            fileSystem.AddDirectory(directoryPath);

            // Act
            TestDelegate action = () => fileSystem.File.WriteAllText(directoryPath, null);

            // Assert
            Assert.Throws<UnauthorizedAccessException>(action);
        }

        [Test]
        public void MockFile_WriteAllText_ShouldThrowDirectoryNotFoundExceptionIfPathDoesNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\file.txt");

            // Act
            TestDelegate action = () => fileSystem.File.WriteAllText(path, string.Empty);

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action);
        }

        public static IEnumerable<KeyValuePair<Encoding, byte[]>> GetEncodingsWithExpectedBytes()
        {
            Encoding utf8WithoutBom = new UTF8Encoding(false, true);
            return new Dictionary<Encoding, byte[]>
            {
                // ASCII does not need a BOM
                { Encoding.ASCII, new byte[] { 72, 101, 108, 108, 111, 32, 116,
                    104, 101, 114, 101, 33, 32, 68, 122, 105, 63, 107, 105, 46 } },

                // BigEndianUnicode needs a BOM, the BOM is the first two bytes
                { Encoding.BigEndianUnicode, new byte [] { 254, 255, 0, 72, 0, 101,
                    0, 108, 0, 108, 0, 111, 0, 32, 0, 116, 0, 104, 0, 101, 0, 114,
                    0, 101, 0, 33, 0, 32, 0, 68, 0, 122, 0, 105, 1, 25, 0, 107, 0, 105, 0, 46 } },

#if NET40
                // Default encoding does not need a BOM
                { Encoding.Default, new byte [] { 72, 101, 108, 108, 111, 32, 116,
                    104, 101, 114, 101, 33, 32, 68, 122, 105, 101, 107, 105, 46 } },
#endif
                // UTF-32 needs a BOM, the BOM is the first four bytes
                { Encoding.UTF32, new byte [] {255, 254, 0, 0, 72, 0, 0, 0, 101,
                    0, 0, 0, 108, 0, 0, 0, 108, 0, 0, 0, 111, 0, 0, 0, 32, 0, 0,
                    0, 116, 0, 0, 0, 104, 0, 0, 0, 101, 0, 0, 0, 114, 0, 0, 0,
                    101, 0, 0, 0, 33, 0, 0, 0, 32, 0, 0, 0, 68, 0, 0, 0, 122, 0,
                    0, 0, 105, 0, 0, 0, 25, 1, 0, 0, 107, 0, 0, 0, 105, 0, 0, 0, 46, 0, 0, 0 } },

                // UTF-7 does not need a BOM
                { Encoding.UTF7, new byte [] {72, 101, 108, 108, 111, 32, 116,
                    104, 101, 114, 101, 43, 65, 67, 69, 45, 32, 68, 122, 105,
                    43, 65, 82, 107, 45, 107, 105, 46 } },

                // The default encoding does not need a BOM
                { utf8WithoutBom, new byte [] { 72, 101, 108, 108, 111, 32, 116,
                    104, 101, 114, 101, 33, 32, 68, 122, 105, 196, 153, 107, 105, 46 } },

                // Unicode needs a BOM, the BOM is the first two bytes
                { Encoding.Unicode, new byte [] { 255, 254, 72, 0, 101, 0, 108,
                    0, 108, 0, 111, 0, 32, 0, 116, 0, 104, 0, 101, 0, 114, 0,
                    101, 0, 33, 0, 32, 0, 68, 0, 122, 0, 105, 0, 25, 1, 107, 0,
                    105, 0, 46, 0 } }
            };
        }

        [TestCaseSource(typeof(MockFileWriteAllTextTests), "GetEncodingsWithExpectedBytes")]
        public void MockFile_WriteAllText_Encoding_ShouldWriteTextFileToMemoryFileSystem(KeyValuePair<Encoding, byte[]> encodingsWithContents)
        {
            // Arrange
            const string FileContent = "Hello there! Dzięki.";
            string path = XFS.Path(@"c:\something\demo.txt");
            byte[] expectedBytes = encodingsWithContents.Value;
            Encoding encoding = encodingsWithContents.Key;
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            // Act
            fileSystem.File.WriteAllText(path, FileContent, encoding);

            // Assert
            var actualBytes = fileSystem.GetFile(path).Contents;
            Assert.AreEqual(expectedBytes, actualBytes);
        }

        [Test]
        public void MockFile_WriteAllTextMultipleLines_ShouldWriteTextFileToMemoryFileSystem()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");

            var fileContent = new List<string> {"Hello there!", "Second line!"};
            var expected = "Hello there!" + Environment.NewLine + "Second line!" + Environment.NewLine;

            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            // Act
            fileSystem.File.WriteAllLines(path, fileContent);

            // Assert
            Assert.AreEqual(
                expected,
                fileSystem.GetFile(path).TextContents);
        }

#if NETCOREAPP2_0
        [Test]
        public async Task MockFile_WriteAllTextAsync_ShouldWriteTextFileToMemoryFileSystem()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "Hello there!";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            // Act
            await fileSystem.File.WriteAllTextAsync(path, fileContent);

            // Assert
            Assert.AreEqual(
                fileContent,
                fileSystem.GetFile(path).TextContents);
        }

        [Test]
        public async Task MockFile_WriteAllTextAsync_ShouldOverriteAnExistingFile()
        {
            // http://msdn.microsoft.com/en-us/library/ms143375.aspx

            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            // Act
            await fileSystem.File.WriteAllTextAsync(path, "foo");
            await fileSystem.File.WriteAllTextAsync(path, "bar");

            // Assert
            Assert.AreEqual("bar", fileSystem.GetFile(path).TextContents);
        }

        [Test]
        public void MockFile_WriteAllTextAsync_ShouldThrowAnUnauthorizedAccessExceptionIfFileIsHidden()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("this is hidden") },
            });
            fileSystem.File.SetAttributes(path, FileAttributes.Hidden);

            // Act
            AsyncTestDelegate action = () => fileSystem.File.WriteAllTextAsync(path, "hello world");

            // Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(action, "Access to the path '{0}' is denied.", path);
        }

        [Test]
        public void MockFile_WriteAllTextAsync_ShouldThrowAnArgumentExceptionIfThePathIsEmpty()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            AsyncTestDelegate action = () => fileSystem.File.WriteAllTextAsync(string.Empty, "hello world");

            // Assert
            Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Test]
        public async Task MockFile_WriteAllTextAsync_ShouldNotThrowAnArgumentNullExceptionIfTheContentIsNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string directoryPath = XFS.Path(@"c:\something");
            string filePath = XFS.Path(@"c:\something\demo.txt");
            fileSystem.AddDirectory(directoryPath);

            // Act
            await fileSystem.File.WriteAllTextAsync(filePath, null);

            // Assert
            // no exception should be thrown, also the documentation says so
            var data = fileSystem.GetFile(filePath);
            Assert.That(data.Contents, Is.Empty);
        }

        [Test]
        public void MockFile_WriteAllTextAsync_ShouldThrowAnUnauthorizedAccessExceptionIfTheFileIsReadOnly()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string filePath = XFS.Path(@"c:\something\demo.txt");
            var mockFileData = new MockFileData(new byte[0]);
            mockFileData.Attributes = FileAttributes.ReadOnly;
            fileSystem.AddFile(filePath, mockFileData);

            // Act
            AsyncTestDelegate action = () => fileSystem.File.WriteAllTextAsync(filePath, null);

            // Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(action);
        }

        [Test]
        public void MockFile_WriteAllTextAsync_ShouldThrowAnUnauthorizedAccessExceptionIfThePathIsOneDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string directoryPath = XFS.Path(@"c:\something");
            fileSystem.AddDirectory(directoryPath);

            // Act
            AsyncTestDelegate action = () => fileSystem.File.WriteAllTextAsync(directoryPath, null);

            // Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(action);
        }

        [Test]
        public void MockFile_WriteAllTextAsync_ShouldThrowDirectoryNotFoundExceptionIfPathDoesNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\file.txt");

            // Act
            AsyncTestDelegate action = () => fileSystem.File.WriteAllTextAsync(path, string.Empty);

            // Assert
            Assert.ThrowsAsync<DirectoryNotFoundException>(action);
        }

        [TestCaseSource(typeof(MockFileWriteAllTextTests), "GetEncodingsWithExpectedBytes")]
        public async Task MockFile_WriteAllTextAsync_Encoding_ShouldWriteTextFileToMemoryFileSystem(KeyValuePair<Encoding, byte[]> encodingsWithContents)
        {
            // Arrange
            const string FileContent = "Hello there! Dzięki.";
            string path = XFS.Path(@"c:\something\demo.txt");
            byte[] expectedBytes = encodingsWithContents.Value;
            Encoding encoding = encodingsWithContents.Key;
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            // Act
            await fileSystem.File.WriteAllTextAsync(path, FileContent, encoding);

            // Assert
            var actualBytes = fileSystem.GetFile(path).Contents;
            Assert.AreEqual(expectedBytes, actualBytes);
        }

        [Test]
        public async Task MockFile_WriteAllTextAsyncMultipleLines_ShouldWriteTextFileToMemoryFileSystem()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");

            var fileContent = new List<string> { "Hello there!", "Second line!" };
            var expected = "Hello there!" + Environment.NewLine + "Second line!" + Environment.NewLine;

            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            // Act
            await fileSystem.File.WriteAllLinesAsync(path, fileContent);

            // Assert
            Assert.AreEqual(
                expected,
                fileSystem.GetFile(path).TextContents);
        }
#endif
    }
}
