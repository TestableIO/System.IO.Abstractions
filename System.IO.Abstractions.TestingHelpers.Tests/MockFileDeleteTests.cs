namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using NUnit.Framework;

    using XFS = MockUnixSupport;

    public class MockFileDeleteTests
    {
        [Test]
        public void MockFile_Delete_ShouldDeleteFile()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:\\test");
            var directory = fileSystem.Path.GetDirectoryName(path);
            fileSystem.AddFile(path, new MockFileData("Bla"));

            var fileCount1 = fileSystem.Directory.GetFiles(directory, "*").Length;
            fileSystem.File.Delete(path);
            var fileCount2 = fileSystem.Directory.GetFiles(directory, "*").Length;

            Assert.AreEqual(1, fileCount1, "File should have existed");
            Assert.AreEqual(0, fileCount2, "File should have been deleted");
        }

        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_Delete_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.Delete(path);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}