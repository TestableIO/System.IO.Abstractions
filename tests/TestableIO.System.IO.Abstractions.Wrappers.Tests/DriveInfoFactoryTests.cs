namespace System.IO.Abstractions.Tests;

[TestFixture]
public class DriveInfoFactoryTests
{
    [Test]
    public async Task Wrap_WithNull_ShouldReturnNull()
    {
        var fileSystem = new FileSystem();

        var result = fileSystem.DriveInfo.Wrap(null);

        await That(result).IsNull();
    }
}