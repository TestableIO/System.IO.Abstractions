using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockDirectoryInfoFactoryTests
    {
        [Test]
        public async Task MockDirectoryInfoFactory_Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new MockFileSystem();

            var result = fileSystem.DirectoryInfo.Wrap(null);
            
            await That(result).IsNull();
        }

        [Test]
        public async Task MockDirectoryInfoFactory_Wrap_ShouldKeepNameAndFullName()
        {
            var fs = new MockFileSystem();
            var directoryInfo = new DirectoryInfo(@"C:\subfolder\file");
            var wrappedDirectoryInfo = fs.DirectoryInfo.Wrap(directoryInfo);

            await That(wrappedDirectoryInfo.FullName).IsEqualTo(directoryInfo.FullName);
            await That(wrappedDirectoryInfo.Name).IsEqualTo(directoryInfo.Name);
        }
    }
}
