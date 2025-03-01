using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockFileSymlinkTests
    {

#if FEATURE_CREATE_SYMBOLIC_LINK

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldReturnFileSystemInfo()
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
            await That(fileSystemInfo.FullName).IsEqualTo(path);
            await That(fileSystemInfo.LinkTarget).IsEqualTo(pathToTarget);
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldSucceedFromFileInfo()
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
            await That(directoryInfo.FullName).IsEqualTo(path);
            await That(directoryInfo.LinkTarget).IsEqualTo(pathToTarget);
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldFailWithNullPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            var ex = await That(() => fileSystem.File.CreateSymbolicLink(null, pathToTarget)).Throws<ArgumentNullException>();

            // Assert
            await That(ex.ParamName).IsEqualTo("path");
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldFailWithNullTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(path, data);

            // Act
            var ex = await That(() => fileSystem.File.CreateSymbolicLink(path, null)).Throws<ArgumentNullException>();

            // Assert
            await That(ex.ParamName).IsEqualTo("pathToTarget");
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldFailWithEmptyPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            var ex = await That(() => fileSystem.File.CreateSymbolicLink("", pathToTarget)).Throws<ArgumentException>();

            // Assert
            await That(ex.ParamName).IsEqualTo("path");
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldFailWithEmptyTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(path, data);

            // Act
            var ex = await That(() => fileSystem.File.CreateSymbolicLink(path, "")).Throws<ArgumentException>();

            // Assert
            await That(ex.ParamName).IsEqualTo("pathToTarget");
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldFailWithIllegalPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            var ex = await That(() => fileSystem.File.CreateSymbolicLink(" ", pathToTarget)).Throws<ArgumentException>();

            // Assert
            await That(ex.ParamName).IsEqualTo("path");
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldFailWithIllegalTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(path, data);

            // Act
            var ex = await That(() => fileSystem.File.CreateSymbolicLink(path, " ")).Throws<ArgumentException>();

            // Assert
            await That(ex.ParamName).IsEqualTo("pathToTarget");
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public async Task MockFile_CreateSymbolicLink_ShouldFailWithIllegalCharactersInPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            Action ex = () => fileSystem.File.CreateSymbolicLink(@"C:\bar.txt_?_", pathToTarget);

            // Assert
            await That(ex).Throws<ArgumentException>();
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]

        public async Task MockFile_CreateSymbolicLink_ShouldFailWithIllegalCharactersInTarget()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo.txt");

            // Act
            Action ex = () => fileSystem.File.CreateSymbolicLink(path, @"C:\bar.txt_?_");

            // Assert
            await That(ex).Throws<ArgumentException>();
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldFailIfPathExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string pathToTarget = XFS.Path(@"C:\Folder\foo.txt");
            string path = XFS.Path(@"C:\Folder\bar.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);
            fileSystem.AddFile(path, data);

            // Act
            var ex = await That(() => fileSystem.File.CreateSymbolicLink(path, pathToTarget)).Throws<IOException>();

            // Assert
            await That(ex.Message).Contains("path");
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldFailIfTargetDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string dir = XFS.Path(@"C:\Folder");
            string path = XFS.Path(@"C:\Folder\foo.txt");
            string pathToTarget = XFS.Path(@"C:\bar.txt");
            fileSystem.AddDirectory(dir);

            // Act
            var ex = await That(() => fileSystem.File.CreateSymbolicLink(path, pathToTarget)).Throws<FileNotFoundException>();

            // Assert
            await That(ex.Message).Contains(pathToTarget);
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldFailIfPathDirectoryDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"C:\Folder\foo.txt");
            string pathToTarget = XFS.Path(@"C:\bar.txt");
            var data = new MockFileData("foobar");
            fileSystem.AddFile(pathToTarget, data);

            // Act
            var ex = await That(() => fileSystem.File.CreateSymbolicLink(path, pathToTarget)).Throws<DirectoryNotFoundException>();

            // Assert
            await That(ex.Message).Contains(path);
        }

        [Test]
        public async Task MockFile_CreateSymbolicLink_ShouldSetReparsePointAttribute()
        {
            var path = "foo.txt";
            var pathToTarget = "bar.txt";
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText(pathToTarget, "some content");

            fileSystem.File.CreateSymbolicLink(path, pathToTarget);

            var attributes = fileSystem.FileInfo.New(path).Attributes;
            await That(attributes.HasFlag(FileAttributes.ReparsePoint)).IsTrue();
        }

        [Test]
        public async Task MockFile_ResolveLinkTarget_ShouldReturnPathOfTargetLink()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("bar", "some content");
            fileSystem.File.CreateSymbolicLink("foo", "bar");

            var result = fileSystem.File.ResolveLinkTarget("foo", false);

            await That(result.Name).IsEqualTo("bar");
        }

        [Test]
        public async Task MockFile_ResolveLinkTarget_WithFinalTarget_ShouldReturnPathOfTargetLink()
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

            await That(result.Name).IsEqualTo("bar");
        }

        [Test]
        public async Task MockFile_ResolveLinkTarget_WithFinalTargetWithTooManyLinks_ShouldThrowIOException()
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

            await That(() => fileSystem.File.ResolveLinkTarget(previousPath, true)).Throws<IOException>();
        }

        [Test]
        public async Task MockFile_ResolveLinkTarget_WithoutFinalTarget_ShouldReturnFirstLink()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("bar", "some content");
            fileSystem.File.CreateSymbolicLink("foo", "bar");
            fileSystem.File.CreateSymbolicLink("foo1", "foo");

            var result = fileSystem.File.ResolveLinkTarget("foo1", false);

            await That(result.Name).IsEqualTo("foo");
        }

        [Test]
        public async Task MockFile_ResolveLinkTarget_WithoutTargetLink_ShouldThrowIOException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllText("bar", "some content");
            fileSystem.File.CreateSymbolicLink("foo", "bar");

            await That(() =>
            {
                fileSystem.File.ResolveLinkTarget("bar", false);
            }).Throws<IOException>();
        }
#endif
    }
}
