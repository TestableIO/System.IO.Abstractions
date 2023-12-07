using NUnit.Framework;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class DirectoryInfoFactoryTests
    {
        [Test]
        public void Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new FileSystem();

            var result = fileSystem.DirectoryInfo.Wrap(null);
            
            Assert.That(result, Is.Null);
        }
    }
}
