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

            // Act
            var result = wrapperFilesystem.Directory.GetParent(@"C:\");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetParent_ForSimpleDirectory_ShouldReturnParent()
        {
            // Arrange
            var expectedBaseDirectory = @"C:\directory1";

            var wrapperFilesystem = new FileSystem();

            // Act
            var result = wrapperFilesystem.Directory.GetParent($@"{expectedBaseDirectory}\file1.txt");

            // Assert
            Assert.AreEqual(expectedBaseDirectory, result.FullName);
        }
    }
}
