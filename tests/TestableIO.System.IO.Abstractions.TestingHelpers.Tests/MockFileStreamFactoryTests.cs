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
                { @"c:\existing.txt", string.Empty }
            });

            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Act
            var result = fileStreamFactory.Create(@"c:\existing.txt", fileMode, FileAccess.Write);

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
            var result = fileStreamFactory.Create(XFS.Path(@"c:\not_existing.txt"), fileMode, FileAccess.Write);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase(FileMode.Create)]
        public void MockFileStreamFactory_CreateForAnExistingFile_ShouldReplaceFileContents(FileMode fileMode)
        {
            var fileSystem = new MockFileSystem();
            string FilePath = XFS.Path("C:\\File.txt");

            using (var stream = fileSystem.FileStream.New(FilePath, fileMode, System.IO.FileAccess.Write))
            {
                var data = Encoding.UTF8.GetBytes("1234567890");
                stream.Write(data, 0, data.Length);
            }

            using (var stream = fileSystem.FileStream.New(FilePath, fileMode, System.IO.FileAccess.Write))
            {
                var data = Encoding.UTF8.GetBytes("AAAAA");
                stream.Write(data, 0, data.Length);
            }

            var text = fileSystem.File.ReadAllText(FilePath);
            Assert.That(text, Is.EqualTo("AAAAA"));
        }

        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.Open)]
        [TestCase(FileMode.CreateNew)]
        public void MockFileStreamFactory_CreateInNonExistingDirectory_ShouldThrowDirectoryNotFoundException(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Test"));

            // Act
            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Assert
            Assert.Throws<DirectoryNotFoundException>(() => fileStreamFactory.Create(XFS.Path(@"C:\Test\NonExistingDirectory\some_random_file.txt"), fileMode));
        }

        [Test]
        public void MockFileStreamFactory_AppendAccessWithReadWriteMode_ShouldThrowArgumentException()
        {
            var fileSystem = new MockFileSystem();
            
            Assert.Throws<ArgumentException>(() =>
            {
                fileSystem.FileStream.New(XFS.Path(@"c:\path.txt"), FileMode.Append, FileAccess.ReadWrite);
            });
        }

        [Test]
        [TestCase(FileMode.Append)]
        [TestCase(FileMode.Truncate)]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.CreateNew)]
        [TestCase(FileMode.Append)]
        public void MockFileStreamFactory_InvalidModeForReadAccess_ShouldThrowArgumentException(FileMode fileMode)
        {
            var fileSystem = new MockFileSystem();

            Assert.Throws<ArgumentException>(() =>
            {
                fileSystem.FileStream.New(XFS.Path(@"c:\path.txt"), fileMode, FileAccess.Read);
            });
        }

        [Test]
        [TestCase(FileMode.Open)]
        [TestCase(FileMode.Truncate)]
        public void MockFileStreamFactory_OpenNonExistingFile_ShouldThrowFileNotFoundException(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Test"));

            // Act
            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Assert
            Assert.Throws<FileNotFoundException>(() => fileStreamFactory.Create(XFS.Path(@"C:\Test\some_random_file.txt"), fileMode));
        }

        [Test]
        [TestCase(FileMode.CreateNew)]
        public void MockFileStreamFactory_CreateExistingFile_Should_Throw_IOException(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path = XFS.Path(@"C:\Test\some_random_file.txt");
            fileSystem.AddFile(path, string.Empty);

            // Act
            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Assert
            Assert.Throws<IOException>(() => fileStreamFactory.Create(path, fileMode));

        }

        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.CreateNew)]
        [TestCase(FileMode.OpenOrCreate)]
        public void MockFileStreamFactory_CreateWithRelativePath_CreatesFileInCurrentDirectory(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var fileStreamFactory = new MockFileStreamFactory(fileSystem);
            fileStreamFactory.Create("some_random_file.txt", fileMode);

            // Assert
            Assert.That(fileSystem.File.Exists(XFS.Path("./some_random_file.txt")), Is.True);
        }

        [Test]
        public void MockFileStream_CanRead_ReturnsFalseForAWriteOnlyStream()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var fileStream = fileSystem.FileStream.New("file.txt", FileMode.CreateNew, FileAccess.Write);

            // Assert
            Assert.That(fileStream.CanRead, Is.False);
        }
    }
}