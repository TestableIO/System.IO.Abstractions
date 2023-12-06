using NUnit.Framework;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class DirectoryWrapperTests
    {
        [Test]
        public void GetParent_ForRootDirectory_ShouldReturnNull()
        {
            // Arrange
            var wrapperFilesystem = new FileSystem();
            var root = wrapperFilesystem.Directory.GetDirectoryRoot(".");

            // Act
            var result = wrapperFilesystem.Directory.GetParent(root);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetParent_ForSimpleSubfolderPath_ShouldReturnRoot()
        {
            // Arrange
            var wrapperFilesystem = new FileSystem();
            var root = wrapperFilesystem.Directory.GetDirectoryRoot(".");
            var subfolder = wrapperFilesystem.Path.Combine(root, "some-folder");

            // Act
            var result = wrapperFilesystem.Directory.GetParent(subfolder);

            // Assert
            Assert.That(result.FullName, Is.EqualTo(root));
        }

        [Test]
        public void GetParent_ForSimpleFilePath_ShouldReturnSubfolder()
        {
            // Arrange
            var wrapperFilesystem = new FileSystem();
            var root = wrapperFilesystem.Directory.GetDirectoryRoot(".");
            var subfolder = wrapperFilesystem.Path.Combine(root, "some-folder");
            var file = wrapperFilesystem.Path.Combine(subfolder, "some-file.txt");

            // Act
            var result = wrapperFilesystem.Directory.GetParent(file);

            // Assert
            Assert.That(result.FullName, Is.EqualTo(subfolder));
        }
    }
}
