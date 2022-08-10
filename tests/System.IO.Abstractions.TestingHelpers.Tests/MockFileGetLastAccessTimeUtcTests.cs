using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileGetLastAccessTimeUtcTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_GetLastAccessTimeUtc_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.GetLastAccessTimeUtc(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_GetLastAccessTimeUtc_ShouldReturnDefaultTimeIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc(@"c:\does\not\exist.txt");

            // Assert
            Assert.AreEqual(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc), actualLastAccessTime);
        }

        [Test]
        public void MockFile_GetLastAccessTimeUtc_ShouldBeSet()
        {
            var now = DateTime.Now.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => now);
            fileSystem.File.WriteAllText("foo.txt", "xyz");

            var result = fileSystem.File.GetLastAccessTimeUtc("foo.txt");

            Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
            Assert.That(result, Is.EqualTo(now.ToUniversalTime()));
        }
    }
}
