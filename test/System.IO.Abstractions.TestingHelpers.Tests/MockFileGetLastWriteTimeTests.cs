using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileGetLastWriteTimeTests
    {
        [Theory]
        [InlineData(" ")]
        [InlineData("   ")]
        public void MockFile_GetLastWriteTime_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.GetLastWriteTime(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("path", exception.ParamName);
        }

        [Fact]
        public void MockFile_GetLastWriteTime_ShouldReturnDefaultTimeIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualLastWriteTime = fileSystem.File.GetLastWriteTime(@"c:\does\not\exist.txt");

            // Assert
            Assert.Equal(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc).ToLocalTime(), actualLastWriteTime);
        }
    }
}
