namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using Globalization;


    using Text;
    using Xunit;
    using XFS = MockUnixSupport;

    public class MockFileAppendAllTextTests {

        [Fact]
        public void MockFile_AppendAllText_ShouldPersistNewText()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllText(path, "+ some text");

            // Assert
            Assert.Equal(
                "Demo text content+ some text",
                file.ReadAllText(path));
        }

        [Fact]
        public void MockFile_AppendAllText_ShouldPersistNewTextWithDifferentEncoding()
        {
            // Arrange
            const string Path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { Path, new MockFileData("AA", Encoding.UTF32) }
            });

            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllText(Path, "BB", Encoding.UTF8);

            // Assert
            CollectionAssert.Equal(
                new byte[] { 255, 254, 0, 0, 65, 0, 0, 0, 65, 0, 0, 0, 66, 66 },
                fileSystem.GetFile(Path).Contents);
        }

        [Fact]
        public void MockFile_AppendAllText_ShouldCreateIfNotExist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            fileSystem.File.AppendAllText(path, " some text");

            // Assert
            Assert.Equal(
                "Demo text content some text",
                fileSystem.File.ReadAllText(path));
        }

        [Fact]
        public void MockFile_AppendAllText_ShouldCreateIfNotExistWithBom()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            const string path = @"c:\something\demo3.txt";
            fileSystem.AddDirectory(@"c:\something\");

            // Act
            fileSystem.File.AppendAllText(path, "AA", Encoding.UTF32);

            // Assert
            CollectionAssert.Equal(
                new byte[] { 255, 254, 0, 0, 65, 0, 0, 0, 65, 0, 0, 0 },
                fileSystem.GetFile(path).Contents);
        }

        [Fact]
        public void MockFile_AppendAllText_ShouldFailIfNotExistButDirectoryAlsoNotExist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            path = XFS.Path(@"c:\something2\demo.txt");

            // Assert
            Exception ex;
            ex = Assert.Throws<DirectoryNotFoundException>(() => fileSystem.File.AppendAllText(path, "some text"));
            Assert.Equal(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path), ex.Message);

            ex = Assert.Throws<DirectoryNotFoundException>(() => fileSystem.File.AppendAllText(path, "some text", Encoding.Unicode));
            Assert.Equal(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path), ex.Message);
        }

        [Fact]
        public void MockFile_AppendAllText_ShouldPersistNewTextWithCustomEncoding()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllText(path, "+ some text", Encoding.BigEndianUnicode);

            // Assert
            var expected = new byte[]
            {
                68, 101, 109, 111, 32, 116, 101, 120, 116, 32, 99, 111, 110, 116,
                101, 110, 116, 0, 43, 0, 32, 0, 115, 0, 111, 0, 109, 0, 101,
                0, 32, 0, 116, 0, 101, 0, 120, 0, 116
            };

            if (XFS.IsUnixPlatform())
            {
                // Remove EOF on mono
                expected = new byte[]
                {
                    68, 101, 109, 111, 32, 116, 101, 120, 116, 32, 99, 111, 110, 116,
                    101, 110, 0, 43, 0, 32, 0, 115, 0, 111, 0, 109, 0, 101,
                    0, 32, 0, 116, 0, 101, 0, 120, 0, 116
                };
            }

            CollectionAssert.Equal(
                expected,
                file.ReadAllBytes(path));
        }
    }
}