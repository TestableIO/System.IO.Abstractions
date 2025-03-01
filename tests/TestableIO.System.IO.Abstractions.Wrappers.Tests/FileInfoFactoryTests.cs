namespace System.IO.Abstractions.Tests;

[TestFixture]
public class FileInfoFactoryTests
{
    [Test]
    public async Task Wrap_WithNull_ShouldReturnNull()
    {
        var fileSystem = new FileSystem();

        var result = fileSystem.FileInfo.Wrap(null);
            
        await That(result).IsNull();
    }
}