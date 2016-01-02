using System.Collections.Generic;
using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{

    public class MockFileSystemTests
    {
        [Fact]
        public void MockFileSystem_GetFile_ShouldReturnNullWhenFileIsNotRegistered()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", new MockFileData("Demo\r\ntext\ncontent\rvalue") },
                { @"c:\something\other.gif", new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            // Act
            var result = fileSystem.GetFile(@"c:\something\else.txt");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void MockFileSystem_GetFile_ShouldReturnFileRegisteredInConstructor()
        {
            // Arrange
            var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", file1 },
                { @"c:\something\other.gif", new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            // Act
            var result = fileSystem.GetFile(@"c:\something\demo.txt");

            // Assert
            Assert.Equal(file1, result);
        }

        [Fact]
        public void MockFileSystem_GetFile_ShouldReturnFileRegisteredInConstructorWhenPathsDifferByCase()
        {
            // Arrange
            var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", file1 },
                { @"c:\something\other.gif", new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            // Act
            var result = fileSystem.GetFile(@"c:\SomeThing\DEMO.txt");

            // Assert
            Assert.Equal(file1, result);
        }

        [Fact]
        public void MockFileSystem_AddFile_ShouldRepaceExistingFile()
        {
            const string path = @"c:\some\file.txt";
            const string existingContent = "Existing content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData(existingContent) }
            });
            Assert.Equal(fileSystem.GetFile(path).TextContents, existingContent);

            const string newContent = "New content";
            fileSystem.AddFile(path, new MockFileData(newContent));

            Assert.Equal(fileSystem.GetFile(path).TextContents, newContent);
        }

#if DNX451
        [Fact]
        public void Is_Serializable()
        {
            var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", file1 },
                { @"c:\something\other.gif", new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });
            var memoryStream = new MemoryStream();

            var serializer = new Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            serializer.Serialize(memoryStream, fileSystem);

            Assert.True(memoryStream.Length > 0, "Length didn't increase after serialization task.");
        }
#endif

        [Fact]
        public void MockFileSystem_AddDirectory_ShouldCreateDirectory()
        {
            // Arrange
            string baseDirectory = MockUnixSupport.Path(@"C:\Test");
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.AddDirectory(baseDirectory);

            // Assert
            Assert.True(fileSystem.Directory.Exists(baseDirectory));
        }

        [Fact]
        public void MockFileSystem_AddDirectory_ShouldThrowExceptionIfDirectoryIsReadOnly()
        {
            // Arrange
            const string baseDirectory = @"C:\Test";
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(baseDirectory, new MockFileData(string.Empty));
            fileSystem.File.SetAttributes(baseDirectory, FileAttributes.ReadOnly);

            // Act

            // Assert
            Assert.Throws<UnauthorizedAccessException>(() => fileSystem.AddDirectory(baseDirectory));
        }
    }
}