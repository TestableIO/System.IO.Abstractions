using System.Collections.Generic;
using NUnit.Framework; 
using System.Text;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockFileStreamFactoryTests
    {
        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.Append)]
        public void MockFileStreamFactory_CreateForExistingFile_ShouldReturnStream(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\existing.txt", MockFileData.NullObject }
            });

            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Act
            var result = fileStreamFactory.Create(@"c:\existing.txt", fileMode);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.Append)]
        public void MockFileStreamFactory_CreateForNonExistingFile_ShouldReturnStream(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Act
            var result = fileStreamFactory.Create(XFS.Path(@"c:\not_existing.txt"), fileMode);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.CreateNew)]
        public void MockFileStreamFactory_CreateForAnExistingFile_ShouldTruncateExistingFile(FileMode fileMode)
        {
            var fileSystem = new MockFileSystem();
            string FilePath = XFS.Path("C:\\File.txt");

            using(var stream = fileSystem.FileStream.Create(FilePath, fileMode, System.IO.FileAccess.Write))
            {
                var data = Encoding.UTF8.GetBytes("1234567890");
                stream.Write(data, 0, data.Length);
            }

            using(var stream = fileSystem.FileStream.Create(FilePath, fileMode, System.IO.FileAccess.Write))
            {
                var data = Encoding.UTF8.GetBytes("AAAAA");
                stream.Write(data, 0, data.Length);
            }

            var text = fileSystem.File.ReadAllText(FilePath);
            Assert.AreEqual("AAAAA", text); 
        }

        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.Open)]
        public void MockFileStreamFactory_CreateInNonExistingDirectory_ShouldThrowDirectoryNotFoundException(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Test"));

            // Act
            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Assert
            Assert.Throws<DirectoryNotFoundException>(() => fileStreamFactory.Create(@"C:\Test\NonExistingDirectory\some_random_file.txt", fileMode));
        }
    }
}