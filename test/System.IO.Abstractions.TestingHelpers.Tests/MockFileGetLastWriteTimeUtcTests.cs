using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileGetLastWriteTimeUtcTests
    {
        [Theory]
        [InlineData(" ")]
        [InlineData("   ")]
        public void MockFile_GetLastWriteTimeUtc_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.GetLastWriteTimeUtc(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("path", exception.ParamName);
        }

        [Fact]
        public void MockFile_GetLastWriteTimeUtc_ShouldReturnDefaultTimeIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc(@"c:\does\not\exist.txt");

            // Assert
            Assert.Equal(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc), actualLastWriteTime);
        }
    }
}