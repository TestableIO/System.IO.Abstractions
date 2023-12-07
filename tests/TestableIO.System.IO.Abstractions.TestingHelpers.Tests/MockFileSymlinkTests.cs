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
            Assert.That(fileSystemInfo.FullName, Is.EqualTo(path));
            Assert.That(fileSystemInfo.LinkTarget, Is.EqualTo(pathToTarget));
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
            IFileInfo directoryInfo = fileSystem.FileInfo.New(path);

            // Assert
            Assert.That(directoryInfo.FullName, Is.EqualTo(path));
            Assert.That(directoryInfo.LinkTarget, Is.EqualTo(pathToTarget));
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

        [Test]
        public void MockFile_CreateSymbolicLink_ShouldSetReparsePointAttribute()
        {
            var path = "foo.txt";
            var pathToTarget = "bar.txt";
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText(pathToTarget, "some content");

            fileSystem.File.CreateSymbolicLink(path, pathToTarget);

            var attributes = fileSystem.FileInfo.New(path).Attributes;
            Assert.That(attributes.HasFlag(FileAttributes.ReparsePoint), Is.True);
        }

        [Test]
        public void MockFile_ResolveLinkTarget_ShouldReturnPathOfTargetLink()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("bar", "some content");
            fileSystem.File.CreateSymbolicLink("foo", "bar");

            var result = fileSystem.File.ResolveLinkTarget("foo", false);

            Assert.That(result.Name, Is.EqualTo("bar"));
        }

        [Test]
        public void MockFile_ResolveLinkTarget_WithFinalTarget_ShouldReturnPathOfTargetLink()
        {
            // The maximum number of symbolic links that are followed:
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.resolvelinktarget?view=net-6.0#remarks
            var maxResolveLinks = XFS.IsWindowsPlatform() ? 63 : 40;
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("bar", "some content");
            var previousPath = "bar";
            for (int i = 0; i < maxResolveLinks; i++)
            {
                string newPath = $"foo-{i}";
                fileSystem.File.CreateSymbolicLink(newPath, previousPath);
                previousPath = newPath;
            }

            var result = fileSystem.File.ResolveLinkTarget(previousPath, true);

            Assert.That(result.Name, Is.EqualTo("bar"));
        }

        [Test]
        public void MockFile_ResolveLinkTarget_WithFinalTargetWithTooManyLinks_ShouldThrowIOException()
        {
            // The maximum number of symbolic links that are followed:
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.resolvelinktarget?view=net-6.0#remarks
            var maxResolveLinks = XFS.IsWindowsPlatform() ? 63 : 40;
            maxResolveLinks++;
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("bar", "some content");
            var previousPath = "bar";
            for (int i = 0; i < maxResolveLinks; i++)
            {
                string newPath = $"foo-{i}";
                fileSystem.File.CreateSymbolicLink(newPath, previousPath);
                previousPath = newPath;
            }

            Assert.Throws<IOException>(() => fileSystem.File.ResolveLinkTarget(previousPath, true));
        }

        [Test]
        public void MockFile_ResolveLinkTarget_WithoutFinalTarget_ShouldReturnFirstLink()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("bar", "some content");
            fileSystem.File.CreateSymbolicLink("foo", "bar");
            fileSystem.File.CreateSymbolicLink("foo1", "foo");

            var result = fileSystem.File.ResolveLinkTarget("foo1", false);

            Assert.That(result.Name, Is.EqualTo("foo"));
        }

        [Test]
        public void MockFile_ResolveLinkTarget_WithoutTargetLink_ShouldThrowIOException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("bar", "some content");
            fileSystem.File.CreateSymbolicLink("foo", "bar");

            Assert.Throws<IOException>(() =>
            {
                fileSystem.File.ResolveLinkTarget("bar", false);
            });
        }
#endif
    }
}
