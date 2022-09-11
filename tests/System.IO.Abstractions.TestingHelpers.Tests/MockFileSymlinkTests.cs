using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockFileSymlinkTests
    {

#if FEATURE_CREATE_SYMBOLIC_LINK

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldReturnFileSystemInfo()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            var path = XFS.Path(@"C:\bar.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            IFileSystemInfo fileSystemInfo = fileSystem.File.CreateSymbolicLink(path, pathToTarget);

            // Assert
            Assert.AreEqual(path, fileSystemInfo.FullName);
            Assert.AreEqual(pathToTarget, fileSystemInfo.LinkTarget);
        }

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldSucceedFromFileInfo()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            var path = XFS.Path(@"C:\bar.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            fileSystem.File.CreateSymbolicLink(path, pathToTarget);
            IFileInfo directoryInfo = fileSystem.FileInfo.FromFileName(path);

            // Assert
            Assert.AreEqual(path, directoryInfo.FullName);
            Assert.AreEqual(pathToTarget, directoryInfo.LinkTarget);
        }

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldFailWithNullPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => fileSystem.File.CreateSymbolicLink(null, pathToTarget));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldFailWithNullTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(path, data);

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => fileSystem.File.CreateSymbolicLink(path, null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("pathToTarget"));
        }

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldFailWithEmptyPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => fileSystem.File.CreateSymbolicLink("", pathToTarget));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldFailWithEmptyTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(path, data);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => fileSystem.File.CreateSymbolicLink(path, ""));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("pathToTarget"));
        }

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldFailWithIllegalPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => fileSystem.File.CreateSymbolicLink(" ", pathToTarget));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldFailWithIllegalTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(path, data);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => fileSystem.File.CreateSymbolicLink(path, " "));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("pathToTarget"));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockFile_CreateSymbolicLink_ShouldFailWithIllegalCharactersInPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            TestDelegate ex = () => fileSystem.File.CreateSymbolicLink(@"C:\bar.txt_?_", pathToTarget);

            // Assert
            Assert.Throws<ArgumentException>(ex);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]

        public void MockFile_CreateSymbolicLink_ShouldFailWithIllegalCharactersInTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo.txt");

            // Act
            TestDelegate ex = () => fileSystem.File.CreateSymbolicLink(path, @"C:\bar.txt_?_");

            // Assert
            Assert.Throws<ArgumentException>(ex);
        }

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldFailIfPathExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            string path = XFS.Path(@"C:\Folder\bar.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);
            fileSystem.AddFile(path, data);

            // Act
            var ex = Assert.Throws<IOException>(() => fileSystem.File.CreateSymbolicLink(path, pathToTarget));

            // Assert
            Assert.That(ex.Message.Contains("path"));
        }

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldFailIfTargetDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string dir = XFS.Path(@"C:\Folder");
            string path = XFS.Path(@"C:\Folder\foo.txt");
            string pathToTarget = XFS.Path(@"C:\bar.txt");
            fileSystem.AddDirectory(dir);

            // Act
            var ex = Assert.Throws<FileNotFoundException>(() => fileSystem.File.CreateSymbolicLink(path, pathToTarget));

            // Assert
            Assert.That(ex.Message.Contains(pathToTarget));
        }

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldFailIfPathDirectoryDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo.txt");
            string pathToTarget = XFS.Path(@"C:\bar.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            var ex = Assert.Throws<DirectoryNotFoundException>(() => fileSystem.File.CreateSymbolicLink(path, pathToTarget));

            // Assert
            Assert.That(ex.Message.Contains(path));
        }
#endif
    }
}
