namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class DirectoryInfoTests
    {
        [Test]
        public async Task Parent_ForRootDirectory_ShouldReturnNull()
        {
            var wrapperFilesystem = new FileSystem();

            var current = wrapperFilesystem.Directory.GetCurrentDirectory();
            var root = wrapperFilesystem.DirectoryInfo.New(current).Root;
            var rootsParent = root.Parent;

            await That(rootsParent).IsNull();
        }
    }
}
