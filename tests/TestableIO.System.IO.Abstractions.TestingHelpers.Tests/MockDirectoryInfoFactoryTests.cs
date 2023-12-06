using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockDirectoryInfoFactoryTests
    {
        [Test]
        public void MockDirectoryInfoFactory_Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new MockFileSystem();

            var result = fileSystem.DirectoryInfo.Wrap(null);
            
            Assert.That(result, Is.Null);
        }
    }
}
