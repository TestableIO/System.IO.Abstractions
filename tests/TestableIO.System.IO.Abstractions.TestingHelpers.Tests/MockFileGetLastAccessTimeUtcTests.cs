using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests;

[TestFixture]
public class MockFileGetLastAccessTimeUtcTests
{
    [TestCase(" ")]
    [TestCase("   ")]
    public async Task MockFile_GetLastAccessTimeUtc_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.GetLastAccessTimeUtc(path);

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.ParamName).IsEqualTo("path");
    }

    [Test]
    public async Task MockFile_GetLastAccessTimeUtc_ShouldReturnDefaultTimeIfFileDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc(@"c:\does\not\exist.txt");

        // Assert
        await That(actualLastAccessTime).IsEqualTo(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc));
    }

    [Test]
    public async Task MockFile_GetLastAccessTimeUtc_ShouldBeSet()
    {
        var now = DateTime.Now.AddDays(10);
        var fileSystem = new MockFileSystem()
            .MockTime(() => now);
        fileSystem.File.WriteAllText("foo.txt", "xyz");

        var result = fileSystem.File.GetLastAccessTimeUtc("foo.txt");

        await That(result.Kind).IsEqualTo(DateTimeKind.Utc);
        await That(result).IsEqualTo(now.ToUniversalTime());
    }
}