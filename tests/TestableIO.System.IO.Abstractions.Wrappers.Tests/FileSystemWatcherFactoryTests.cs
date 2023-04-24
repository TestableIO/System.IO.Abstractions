using NUnit.Framework;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class FileSystemWatcherFactoryTests
    {
        [Test]
        public void Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new FileSystem();

            var result = fileSystem.FileSystemWatcher.Wrap(null);
            
            Assert.IsNull(result);
        }
    }
}
