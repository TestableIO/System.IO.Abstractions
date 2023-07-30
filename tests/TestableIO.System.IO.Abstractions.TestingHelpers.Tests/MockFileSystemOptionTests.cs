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
        public void CreateDefaultTempDir_ShouldBeConsidered(bool createTempDir)
        {
            var fileSystem = new MockFileSystem(new MockFileSystemOptions
            {
                CreateDefaultTempDir = createTempDir
            });

            var result = fileSystem.Directory.Exists(fileSystem.Path.GetTempPath());

            Assert.AreEqual(createTempDir, result);
        }

        [Test]
        [TestCase(@"C:\path")]
        [TestCase(@"C:\foo\bar")]
        public void CurrentDirectory_ShouldBeConsidered(string currentDirectory)
        {
            var fileSystem = new MockFileSystem(new MockFileSystemOptions
            {
                CurrentDirectory = currentDirectory
            });

            var result = fileSystem.Directory.GetCurrentDirectory();

            Assert.AreEqual(currentDirectory, result);
        }
    }
}