using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockDirectorySymlinkTests
    {

#if FEATURE_CREATE_SYMBOLIC_LINK

        [Test]
        public void MockDirectory_CreateSymbolicLink_ShouldReturnFileSystemInfo()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var pathToTarget = XFS.Path(@"C:\Folder\foo");
            var path = XFS.Path(@"C:\bar");
            fileSystem.AddDirectory(pathToTarget);

            // Act
            IFileSystemInfo fileSystemInfo = fileSystem.Directory.CreateSymbolicLink(path, pathToTarget);

            // Assert
            Assert.AreEqual(path, fileSystemInfo.FullName);
            Assert.AreEqual(pathToTarget, fileSystemInfo.LinkTarget);
        }

        [Test]
        public void MockDirectory_CreateSymbolicLink_ShouldSucceedFromDirectoryInfo()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var pathToTarget = XFS.Path(@"C:\Folder\foo");
            var path = XFS.Path(@"C:\bar");
            fileSystem.AddDirectory(pathToTarget);

            // Act
            fileSystem.Directory.CreateSymbolicLink(path, pathToTarget);
            IDirectoryInfo directoryInfo = fileSystem.DirectoryInfo.New(path);

            // Assert
            Assert.AreEqual(path, directoryInfo.FullName);
            Assert.AreEqual(pathToTarget, directoryInfo.LinkTarget);
        }

        [Test]
        public void MockDirectory_CreateSymbolicLink_ShouldFailWithNullPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var pathToTarget = XFS.Path(@"C:\Folder\foo");
            fileSystem.AddDirectory(pathToTarget);

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => fileSystem.Directory.CreateSymbolicLink(null, pathToTarget));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockDirectory_CreateSymbolicLink_ShouldFailWithNullTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path = XFS.Path(@"C:\Folder\foo");
            fileSystem.AddDirectory(path);

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => fileSystem.Directory.CreateSymbolicLink(path, null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("pathToTarget"));
        }

        [Test]
        public void MockDirectory_CreateSymbolicLink_ShouldFailWithEmptyPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var pathToTarget = XFS.Path(@"C:\Folder\foo");
            fileSystem.AddDirectory(pathToTarget);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => fileSystem.Directory.CreateSymbolicLink("", pathToTarget));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockDirectory_CreateSymbolicLink_ShouldFailWithEmptyTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo");
            fileSystem.AddDirectory(path);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => fileSystem.Directory.CreateSymbolicLink(path, ""));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("pathToTarget"));
        }

        [Test]
        public void MockDirectory_CreateSymbolicLink_ShouldFailWithIllegalPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string pathToTarget = XFS.Path(@"C:\Folder\foo");
            fileSystem.AddDirectory(pathToTarget);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => fileSystem.Directory.CreateSymbolicLink(" ", pathToTarget));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockDirectory_CreateSymbolicLink_ShouldFailWithIllegalTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo");
            fileSystem.AddDirectory(path);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => fileSystem.Directory.CreateSymbolicLink(path, " "));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("pathToTarget"));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockDirectory_CreateSymbolicLink_ShouldFailWithIllegalCharactersInPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string pathToTarget = XFS.Path(@"C:\Folder\foo");
            fileSystem.AddDirectory(pathToTarget);

            // Act
            TestDelegate ex = () => fileSystem.Directory.CreateSymbolicLink(@"C:\bar_?_", pathToTarget);

            // Assert
            Assert.Throws<ArgumentException>(ex);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockDirectory_CreateSymbolicLink_ShouldFailWithIllegalCharactersInTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\foo");

            // Act
            TestDelegate ex = () => fileSystem.Directory.CreateSymbolicLink(path, @"C:\bar_?_");

            // Assert
            Assert.Throws<ArgumentException>(ex);
        }

        [Test]
        public void MockDirectory_CreateSymbolicLink_ShouldFailIfPathExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string pathToTarget = XFS.Path(@"C:\Folder\foo");
            string path = XFS.Path(@"C:\Folder\bar");
            fileSystem.AddDirectory(pathToTarget);
            fileSystem.AddDirectory(path);

            // Act
            var ex = Assert.Throws<IOException>(() => fileSystem.Directory.CreateSymbolicLink(path, pathToTarget));

            // Assert
            Assert.That(ex.Message.Contains("path"));
        }

        [Test]
        public void MockDirectory_CreateSymbolicLink_ShouldFailIfTargetDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo");
            string pathToTarget = XFS.Path(@"C:\Target");

            // Act
            var ex = Assert.Throws<FileNotFoundException>(() => fileSystem.Directory.CreateSymbolicLink(path, pathToTarget));

            // Assert
            Assert.That(ex.Message.Contains(pathToTarget));
        }

        [Test]
        public void MockDirectory_ResolveLinkTarget_ShouldReturnPathOfTargetLink()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("bar");
            fileSystem.Directory.CreateSymbolicLink("foo", "bar");

            var result = fileSystem.Directory.ResolveLinkTarget("foo", false);

            Assert.AreEqual("bar", result.Name);
        }

        [Test]
        public void MockDirectory_ResolveLinkTarget_WithFinalTarget_ShouldReturnPathOfTargetLink()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("bar");
            fileSystem.Directory.CreateSymbolicLink("foo", "bar");
            fileSystem.Directory.CreateSymbolicLink("foo1", "foo");

            var result = fileSystem.Directory.ResolveLinkTarget("foo1", true);

            Assert.AreEqual("bar", result.Name);
        }

        [Test]
        public void MockDirectory_ResolveLinkTarget_WithoutFinalTarget_ShouldReturnFirstLink()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("bar");
            fileSystem.Directory.CreateSymbolicLink("foo", "bar");
            fileSystem.Directory.CreateSymbolicLink("foo1", "foo");

            var result = fileSystem.Directory.ResolveLinkTarget("foo1", false);

            Assert.AreEqual("foo", result.Name);
        }

        [Test]
        public void MockDirectory_ResolveLinkTarget_WithoutTargetLink_ShouldThrowIOException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("bar");
            fileSystem.Directory.CreateSymbolicLink("foo", "bar");

            Assert.Throws<IOException>(() =>
            {
                fileSystem.Directory.ResolveLinkTarget("bar", false);
            });
        }
#endif
    }
}
