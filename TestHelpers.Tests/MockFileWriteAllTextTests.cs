namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using NUnit.Framework;

    using Text;

    using XFS = MockUnixSupport;

    public class MockFileWriteAllTextTests {
        [Test]
        public void MockFile_WriteAllText_ShouldWriteTextFileToMemoryFileSystem()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "Hello there!";
            var fileSystem = new MockFileSystem();

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

        private IEnumerable<KeyValuePair<Encoding, byte[]>> GetEncodingsWithExpectedBytes()
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

                // Default encoding does not need a BOM
                { Encoding.Default, new byte [] { 72, 101, 108, 108, 111, 32, 116,
                    104, 101, 114, 101, 33, 32, 68, 122, 105, 101, 107, 105, 46 } },

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

        [TestCaseSource("GetEncodingsWithExpectedBytes")]
        public void MockFile_WriteAllText_Encoding_ShouldWriteTextFileToMemoryFileSystem(KeyValuePair<Encoding, byte[]> encodingsWithContents)
        {
            // Arrange
            const string FileContent = "Hello there! Dzięki.";
            string path = XFS.Path(@"c:\something\demo.txt");
            byte[] expectedBytes = encodingsWithContents.Value;
            Encoding encoding = encodingsWithContents.Key;
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.File.WriteAllText(path, FileContent, encoding);

            // Assert
            var actualBytes = fileSystem.GetFile(path).Contents;
            Assert.AreEqual(expectedBytes, actualBytes);
        }
    }
}