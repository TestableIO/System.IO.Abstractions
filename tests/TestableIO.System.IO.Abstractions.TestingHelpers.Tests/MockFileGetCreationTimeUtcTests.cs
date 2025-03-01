using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests;

[TestFixture]
public class MockFileGetCreationTimeUtcTests
{
    [TestCase(" ")]
    [TestCase("   ")]
    public async Task MockFile_GetCreationTimeUtc_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.GetCreationTimeUtc(path);

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.ParamName).IsEqualTo("path");
    }

    [Test]
    public async Task MockFile_GetCreationTimeUtc_ShouldReturnDefaultTimeIfFileDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var actualCreationTime = fileSystem.File.GetCreationTimeUtc(@"c:\does\not\exist.txt");

        // Assert
        await That(actualCreationTime).IsEqualTo(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc));
    }

    [Test]
    public async Task MockFile_GetCreationTimeUtc_ShouldBeSet()
    {
        var now = DateTime.Now.AddDays(10);
        var fileSystem = new MockFileSystem()
            .MockTime(() => now);
        fileSystem.File.WriteAllText("foo.txt", "xyz");

        var result = fileSystem.File.GetCreationTimeUtc("foo.txt");

        await That(result.Kind).IsEqualTo(DateTimeKind.Utc);
        await That(result).IsEqualTo(now.ToUniversalTime());
    }
}