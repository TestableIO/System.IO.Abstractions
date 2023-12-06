﻿using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileGetLastAccessTimeTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_GetLastAccessTime_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.GetLastAccessTime(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_GetLastAccessTime_ShouldReturnDefaultTimeIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualLastAccessTime = fileSystem.File.GetLastAccessTime(@"c:\does\not\exist.txt");

            // Assert
            Assert.That(actualLastAccessTime, Is.EqualTo(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc).ToLocalTime()));
        }

        [Test]
        public void MockFile_GetLastAccessTime_ShouldBeSet()
        {
            var now = DateTime.Now.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => now);
            fileSystem.File.WriteAllText("foo.txt", "xyz");

            var result = fileSystem.File.GetLastAccessTime("foo.txt");

            Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Local));
            Assert.That(result, Is.EqualTo(now.ToLocalTime()));
        }
    }
}
