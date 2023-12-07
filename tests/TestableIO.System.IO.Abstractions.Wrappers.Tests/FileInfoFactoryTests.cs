using NUnit.Framework;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class FileInfoFactoryTests
    {
        [Test]
        public void Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new FileSystem();

            var result = fileSystem.FileInfo.Wrap(null);
            
            Assert.That(result, Is.Null);
        }
    }
}
