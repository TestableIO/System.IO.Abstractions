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

        [Test]
        public void MockDirectoryInfoFactory_Wrap_ShouldKeepNameAndFullName()
        {
            var fs = new MockFileSystem();
            var directoryInfo = new DirectoryInfo(@"C:\subfolder\file");
            var wrappedDirectoryInfo = fs.DirectoryInfo.Wrap(directoryInfo);

            Assert.That(wrappedDirectoryInfo.FullName, Is.EqualTo(directoryInfo.FullName));
            Assert.That(wrappedDirectoryInfo.Name, Is.EqualTo(directoryInfo.Name));
        }
    }
}
