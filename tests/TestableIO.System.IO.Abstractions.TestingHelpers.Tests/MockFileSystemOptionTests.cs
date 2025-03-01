using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSystemOptionTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task CreateDefaultTempDir_ShouldBeConsidered(bool createTempDir)
        {
            var fileSystem = new MockFileSystem(new MockFileSystemOptions
            {
                CreateDefaultTempDir = createTempDir
            });

            var result = fileSystem.Directory.Exists(fileSystem.Path.GetTempPath());

            await That(result).IsEqualTo(createTempDir);
        }

        [Test]
        [TestCase(@"C:\path")]
        [TestCase(@"C:\foo\bar")]
        public async Task CurrentDirectory_ShouldBeConsidered(string currentDirectory)
        {
            currentDirectory = XFS.Path(currentDirectory);
            var fileSystem = new MockFileSystem(new MockFileSystemOptions
            {
                CurrentDirectory = currentDirectory
            });

            var result = fileSystem.Directory.GetCurrentDirectory();

            await That(result).IsEqualTo(currentDirectory);
        }
    }
}
