using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests;

public class MockFileGetLastWriteTimeUtcTests
{
    [TestCase(" ")]
    [TestCase("   ")]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockFile_GetLastWriteTimeUtc_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.GetLastWriteTimeUtc(path);

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.ParamName).IsEqualTo("path");
    }

    [Test]
    public async Task MockFile_GetLastWriteTimeUtc_ShouldReturnDefaultTimeIfFileDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc(@"c:\does\not\exist.txt");

        // Assert
        await That(actualLastWriteTime).IsEqualTo(1.January(1601).At(0, 0).AsUtc());
    }

    [Test]
    public async Task MockFile_GetLastWriteTimeUtc_ShouldBeSet()
    {
        var now = DateTime.Now.AddDays(10);
        var fileSystem = new MockFileSystem()
            .MockTime(() => now);
        fileSystem.File.WriteAllText("foo.txt", "xyz");

        var result = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

        await That(result.Kind).IsEqualTo(DateTimeKind.Utc);
        await That(result).IsEqualTo(now.ToUniversalTime());
    }
}