namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class DirectoryInfoFactoryTests
    {
        [Test]
        public async Task Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new FileSystem();

            var result = fileSystem.DirectoryInfo.Wrap(null);

            await That(result).IsNull();
        }
    }
}
