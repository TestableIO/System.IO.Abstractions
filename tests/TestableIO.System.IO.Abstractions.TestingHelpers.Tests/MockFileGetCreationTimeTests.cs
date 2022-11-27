using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileGetCreationTimeTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_GetCreationTime_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.GetCreationTime(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_GetCreationTime_ShouldReturnDefaultTimeIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualCreationTime = fileSystem.File.GetCreationTime(@"c:\does\not\exist.txt");

            // Assert
            Assert.AreEqual(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc).ToLocalTime(), actualCreationTime);
        }

        [Test]
        public void MockFile_GetCreationTime_ShouldBeSet()
        {
            var now = DateTime.Now.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => now);
            fileSystem.File.WriteAllText("foo.txt", "xyz");

            var result = fileSystem.File.GetCreationTime("foo.txt");

            Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Local));
            Assert.That(result, Is.EqualTo(now.ToLocalTime()));
        }
    }
}
