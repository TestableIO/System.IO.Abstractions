using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileGetLastWriteTimeUtcTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_GetLastWriteTimeUtc_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.GetLastWriteTimeUtc(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_GetLastWriteTimeUtc_ShouldReturnDefaultTimeIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc(@"c:\does\not\exist.txt");

            // Assert
            Assert.That(actualLastWriteTime, Is.EqualTo(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc)));
        }

        [Test]
        public void MockFile_GetLastWriteTimeUtc_ShouldBeSet()
        {
            var now = DateTime.Now.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => now);
            fileSystem.File.WriteAllText("foo.txt", "xyz");

            var result = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
            Assert.That(result, Is.EqualTo(now.ToUniversalTime()));
        }
    }
}