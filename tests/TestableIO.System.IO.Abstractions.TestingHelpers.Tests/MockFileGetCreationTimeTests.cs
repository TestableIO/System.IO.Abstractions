using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests;

[TestFixture]
public class MockFileGetCreationTimeTests
{
    [TestCase(" ")]
    [TestCase("   ")]
    public async Task MockFile_GetCreationTime_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.GetCreationTime(path);

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.ParamName).IsEqualTo("path");
    }

    [Test]
    public async Task MockFile_GetCreationTime_ShouldReturnDefaultTimeIfFileDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var actualCreationTime = fileSystem.File.GetCreationTime(@"c:\does\not\exist.txt");

        // Assert
        await That(actualCreationTime).IsEqualTo(1.January(1601).At(0, 0).AsUtc().ToLocalTime());
    }

    [Test]
    public async Task MockFile_GetCreationTime_ShouldBeSet()
    {
        var now = DateTime.Now.AddDays(10);
        var fileSystem = new MockFileSystem()
            .MockTime(() => now);
        fileSystem.File.WriteAllText("foo.txt", "xyz");

        var result = fileSystem.File.GetCreationTime("foo.txt");

        await That(result.Kind).IsEqualTo(DateTimeKind.Local);
        await That(result).IsEqualTo(now.ToLocalTime());
    }
}