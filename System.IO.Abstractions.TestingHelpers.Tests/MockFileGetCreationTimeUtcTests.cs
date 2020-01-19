using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileGetCreationTimeUtcTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_GetCreationTimeUtc_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.GetCreationTimeUtc(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_GetCreationTimeUtc_ShouldReturnDefaultTimeIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualCreationTime = fileSystem.File.GetCreationTimeUtc(@"c:\does\not\exist.txt");

            // Assert
            Assert.AreEqual(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc), actualCreationTime);
        }
    }
}
