using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests;

[TestFixture]
public class MockFileGetLastAccessTimeTests
{
    [TestCase(" ")]
    [TestCase("   ")]
    public async Task MockFile_GetLastAccessTime_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.GetLastAccessTime(path);

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.ParamName).IsEqualTo("path");
    }

    [Test]
    public async Task MockFile_GetLastAccessTime_ShouldReturnDefaultTimeIfFileDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var actualLastAccessTime = fileSystem.File.GetLastAccessTime(@"c:\does\not\exist.txt");

        // Assert
        await That(actualLastAccessTime).IsEqualTo(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc).ToLocalTime());
    }

    [Test]
    public async Task MockFile_GetLastAccessTime_ShouldBeSet()
    {
        var now = DateTime.Now.AddDays(10);
        var fileSystem = new MockFileSystem()
            .MockTime(() => now);
        fileSystem.File.WriteAllText("foo.txt", "xyz");

        var result = fileSystem.File.GetLastAccessTime("foo.txt");

        await That(result.Kind).IsEqualTo(DateTimeKind.Local);
        await That(result).IsEqualTo(now.ToLocalTime());
    }
}