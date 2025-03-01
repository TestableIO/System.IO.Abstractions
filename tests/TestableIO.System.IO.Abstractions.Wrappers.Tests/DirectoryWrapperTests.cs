namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class DirectoryWrapperTests
    {
        [Test]
        public async Task GetParent_ForRootDirectory_ShouldReturnNull()
        {
            // Arrange
            var wrapperFilesystem = new FileSystem();
            var root = wrapperFilesystem.Directory.GetDirectoryRoot(".");

            // Act
            var result = wrapperFilesystem.Directory.GetParent(root);

            // Assert
            await That(result).IsNull();
        }

        [Test]
        public async Task GetParent_ForSimpleSubfolderPath_ShouldReturnRoot()
        {
            // Arrange
            var wrapperFilesystem = new FileSystem();
            var root = wrapperFilesystem.Directory.GetDirectoryRoot(".");
            var subfolder = wrapperFilesystem.Path.Combine(root, "some-folder");

            // Act
            var result = wrapperFilesystem.Directory.GetParent(subfolder);

            // Assert
            await That(result.FullName).IsEqualTo(root);
        }

        [Test]
        public async Task GetParent_ForSimpleFilePath_ShouldReturnSubfolder()
        {
            // Arrange
            var wrapperFilesystem = new FileSystem();
            var root = wrapperFilesystem.Directory.GetDirectoryRoot(".");
            var subfolder = wrapperFilesystem.Path.Combine(root, "some-folder");
            var file = wrapperFilesystem.Path.Combine(subfolder, "some-file.txt");

            // Act
            var result = wrapperFilesystem.Directory.GetParent(file);

            // Assert
            await That(result.FullName).IsEqualTo(subfolder);
        }
    }
}
