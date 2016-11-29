using FluentAssertions;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Xunit;

    using XFS = MockUnixSupport;

    public class MockFileDeleteTests
    {
        [Fact]
        public void MockFile_Delete_ShouldDeleteFile()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:\\test");
            var directory = fileSystem.Path.GetDirectoryName(path);
            fileSystem.AddFile(path, new MockFileData("Bla"));

            var fileCount1 = fileSystem.Directory.GetFiles(directory, "*").Length;
            fileSystem.File.Delete(path);
            var fileCount2 = fileSystem.Directory.GetFiles(directory, "*").Length;

            fileCount1.Should().Be(1, "File should have existed");
            fileCount2.Should().Be(2, "File should have been deleted");
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("   ")]
        public void MockFile_Delete_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.Delete(path);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}