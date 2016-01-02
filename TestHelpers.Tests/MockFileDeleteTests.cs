namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Xunit;
    using XFS = MockUnixSupport;

    public class MockFileDeleteTests
    {
        [Fact]
        public void MockFile_Delete_ShouldDeleteFile()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:\\test");
            var directory = fileSystem.Path.GetDirectoryName(path);
            fileSystem.AddFile(path, new MockFileData("Bla"));

            var fileCount1 = fileSystem.Directory.GetFiles(directory, "*").Length;
            fileSystem.File.Delete(path);
            var fileCount2 = fileSystem.Directory.GetFiles(directory, "*").Length;

            Assert.Equal(1, fileCount1);
            Assert.Equal(0, fileCount2);
        }
    }
}