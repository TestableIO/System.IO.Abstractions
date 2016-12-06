using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileWriteAllBytesTests
    {
        [Fact]
        public void MockFile_WriteAllBytes_ShouldThrowDirectoryNotFoundExceptionIfPathDoesNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\file.txt");
            var fileContent = new byte[] { 1, 2, 3, 4 };

            // Act
            Action action = () => fileSystem.File.WriteAllBytes(path, fileContent);

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action);
        }

        [Fact]
        public void MockFile_WriteAllBytes_ShouldWriteDataToMemoryFileSystem()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };
            fileSystem.AddDirectory(@"c:\something");

            // Act
            fileSystem.File.WriteAllBytes(path, fileContent);

            // Assert
            Assert.Equal(
                fileContent,
                fileSystem.GetFile(path).Contents);
        }

        [Fact]
        public void MockFile_WriteAllBytes_ShouldThrowAnUnauthorizedAccessExceptionIfFileIsHidden()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { path, new MockFileData("this is hidden") },
                });
            fileSystem.File.SetAttributes(path, FileAttributes.Hidden);

            // Act
            Action action = () => fileSystem.File.WriteAllBytes(path, new byte[] { 123 });

            // Assert
            action.ShouldThrow<UnauthorizedAccessException>("Access to the path '{0}' is denied.", path);
        }

        [Fact]
        public void MockFile_WriteAllBytes_ShouldThrowAnArgumentExceptionIfContainsIllegalCharacters()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.WriteAllBytes("<<<", new byte[] { 123 });

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void MockFile_WriteAllBytes_ShouldThrowAnArgumentNullExceptionIfContainsIllegalCharacters()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.WriteAllBytes(null, new byte[] { 123 });

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            exception.Message.Should().StartWith("Path cannot be null.");
            Assert.Equal("path", exception.ParamName);
        }

        [Fact]
        public void MockFile_WriteAllBytes_ShouldThrowAnArgumentNullExceptionIfBytesAreNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\demo.txt");

            // Act
            Action action = () => fileSystem.File.WriteAllBytes(path, null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            exception.Message.Should().StartWith("Value cannot be null.");
            Assert.Equal("bytes", exception.ParamName);
        }
    }
}
