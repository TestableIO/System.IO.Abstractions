namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class FileSystemWatcherFactoryTests
    {
        [Test]
        public async Task Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new FileSystem();

            var result = fileSystem.FileSystemWatcher.Wrap(null);
            
            await That(result).IsNull();
        }
    }
}
