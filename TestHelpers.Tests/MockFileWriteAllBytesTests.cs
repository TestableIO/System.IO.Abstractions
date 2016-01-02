﻿using System.Collections.Generic;

using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileWriteAllBytesTests
    {
        [Fact]
        public void MockFile_WriteAllBytes_ShouldWriteDataToMemoryFileSystem()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };

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
            Assert.Throws<UnauthorizedAccessException>(action, "Access to the path '{0}' is denied.", path);
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
            Assert.That(exception.Message, Is.StringStarting("Path cannot be null."));
            Assert.That(exception.ParamName, Is.EqualTo("path"));
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
            Assert.That(exception.Message, Is.StringStarting("Value cannot be null."));
            Assert.That(exception.ParamName, Is.EqualTo("bytes"));
        }
    }
}
