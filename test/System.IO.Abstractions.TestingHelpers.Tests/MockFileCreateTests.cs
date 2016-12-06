using FluentAssertions;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using Globalization;

    using Xunit;

    using Text;

    using XFS = MockUnixSupport;

    public class MockFileCreateTests
    {
        [Fact]
        public void Mockfile_Create_ShouldCreateNewStream()
        {
            string fullPath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var sut = new MockFile(fileSystem);

            Assert.False(fileSystem.FileExists(fullPath));

            sut.Create(fullPath).Dispose();

            Assert.True(fileSystem.FileExists(fullPath));
        }

        [Fact]
        public void Mockfile_Create_CanWriteToNewStream()
        {
            string fullPath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var data = new UTF8Encoding(false).GetBytes("Test string");

            var sut = new MockFile(fileSystem);
            using (var stream = sut.Create(fullPath))
            {
                stream.Write(data, 0, data.Length);
            }

            var mockFileData = fileSystem.GetFile(fullPath);
            var fileData = mockFileData.Contents;

            Assert.Equal(data, fileData);
        }

        [Fact]
        public void Mockfile_Create_OverwritesExistingFile()
        {
            string path = XFS.Path(@"c:\some\file.txt");
            var fileSystem = new MockFileSystem();

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

            Assert.Equal(expectedContents, actualContents);
        }

        [Fact]
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
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path), exception.Message);
        }

        [Fact]
        public void Mockfile_Create_ShouldThrowArgumentExceptionIfPathIsZeroLength()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.Create("");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData("\"")]
        [InlineData("<")]
        [InlineData(">")]
        [InlineData("|")]
        public void MockFile_Create_ShouldThrowArgumentNullExceptionIfPathIsNull1(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.Create(path);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("   ")]
        public void MockFile_Create_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.Create(path);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void MockFile_Create_ShouldThrowArgumentNullExceptionIfPathIsNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.Create(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            exception.Message.Should().StartWith("Path cannot be null.");
        }
    }
}