using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileGetCreationTimeTests
    {
        [Theory]
        [InlineData(" ")]
        [InlineData("   ")]
        public void MockFile_GetCreationTime_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.GetCreationTime(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("path", exception.ParamName);
        }

        [Fact]
        public void MockFile_GetCreationTime_ShouldReturnDefaultTimeIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualCreationTime = fileSystem.File.GetCreationTime(@"c:\does\not\exist.txt");

            // Assert
            Assert.Equal(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc).ToLocalTime(), actualCreationTime);
        }
    }
}
