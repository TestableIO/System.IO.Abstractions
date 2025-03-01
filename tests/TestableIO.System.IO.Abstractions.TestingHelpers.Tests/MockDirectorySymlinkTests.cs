using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests;

using XFS = MockUnixSupport;

[TestFixture]
public class MockDirectorySymlinkTests
{

#if FEATURE_CREATE_SYMBOLIC_LINK

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldReturnFileSystemInfo()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var pathToTarget = XFS.Path(@"C:\Folder\foo");
        var path = XFS.Path(@"C:\bar");
        fileSystem.AddDirectory(pathToTarget);

        // Act
        IFileSystemInfo fileSystemInfo = fileSystem.Directory.CreateSymbolicLink(path, pathToTarget);

        // Assert
        await That(fileSystemInfo.FullName).IsEqualTo(path);
        await That(fileSystemInfo.LinkTarget).IsEqualTo(pathToTarget);
    }

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldSucceedFromDirectoryInfo()
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
        await That(directoryInfo.FullName).IsEqualTo(path);
        await That(directoryInfo.LinkTarget).IsEqualTo(pathToTarget);
    }

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldFailWithNullPath()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var pathToTarget = XFS.Path(@"C:\Folder\foo");
        fileSystem.AddDirectory(pathToTarget);

        // Act
        var ex = await That(() => fileSystem.Directory.CreateSymbolicLink(null, pathToTarget)).Throws<ArgumentNullException>();

        // Assert
        await That(ex.ParamName).IsEqualTo("path");
    }

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldFailWithNullTarget()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path = XFS.Path(@"C:\Folder\foo");
        fileSystem.AddDirectory(path);

        // Act
        var ex = await That(() => fileSystem.Directory.CreateSymbolicLink(path, null)).Throws<ArgumentNullException>();

        // Assert
        await That(ex.ParamName).IsEqualTo("pathToTarget");
    }

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldFailWithEmptyPath()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var pathToTarget = XFS.Path(@"C:\Folder\foo");
        fileSystem.AddDirectory(pathToTarget);

        // Act
        var ex = await That(() => fileSystem.Directory.CreateSymbolicLink("", pathToTarget)).Throws<ArgumentException>();

        // Assert
        await That(ex.ParamName).IsEqualTo("path");
    }

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldFailWithEmptyTarget()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        string path = XFS.Path(@"C:\Folder\foo");
        fileSystem.AddDirectory(path);

        // Act
        var ex = await That(() => fileSystem.Directory.CreateSymbolicLink(path, "")).Throws<ArgumentException>();

        // Assert
        await That(ex.ParamName).IsEqualTo("pathToTarget");
    }

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldFailWithIllegalPath()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        string pathToTarget = XFS.Path(@"C:\Folder\foo");
        fileSystem.AddDirectory(pathToTarget);

        // Act
        var ex = await That(() => fileSystem.Directory.CreateSymbolicLink(" ", pathToTarget)).Throws<ArgumentException>();

        // Assert
        await That(ex.ParamName).IsEqualTo("path");
    }

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldFailWithIllegalTarget()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        string path = XFS.Path(@"C:\Folder\foo");
        fileSystem.AddDirectory(path);

        // Act
        var ex = await That(() => fileSystem.Directory.CreateSymbolicLink(path, " ")).Throws<ArgumentException>();

        // Assert
        await That(ex.ParamName).IsEqualTo("pathToTarget");
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockDirectory_CreateSymbolicLink_ShouldFailWithIllegalCharactersInPath()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        string pathToTarget = XFS.Path(@"C:\Folder\foo");
        fileSystem.AddDirectory(pathToTarget);

        // Act
        Action ex = () => fileSystem.Directory.CreateSymbolicLink(@"C:\bar_?_", pathToTarget);

        // Assert
        await That(ex).Throws<ArgumentException>();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockDirectory_CreateSymbolicLink_ShouldFailWithIllegalCharactersInTarget()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        string path = XFS.Path(@"C:\foo");

        // Act
        Action ex = () => fileSystem.Directory.CreateSymbolicLink(path, @"C:\bar_?_");

        // Assert
        await That(ex).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldFailIfPathExists()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        string pathToTarget = XFS.Path(@"C:\Folder\foo");
        string path = XFS.Path(@"C:\Folder\bar");
        fileSystem.AddDirectory(pathToTarget);
        fileSystem.AddDirectory(path);

        // Act
        var ex = await That(() => fileSystem.Directory.CreateSymbolicLink(path, pathToTarget)).Throws<IOException>();

        // Assert
        await That(ex.Message).Contains("path");
    }

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldNotFailIfTargetDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        string path = XFS.Path(@"C:\Folder\foo");
        string pathToTarget = XFS.Path(@"C:\Target");

        // Act
        var fileSystemInfo = fileSystem.Directory.CreateSymbolicLink(path, pathToTarget);

        // Assert
        await That(fileSystemInfo.Exists).IsTrue();
    }

    [Test]
    public async Task MockDirectory_CreateSymbolicLink_ShouldSetReparsePointAttribute()
    {
        var path = "foo";
        var pathToTarget = "bar";
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(pathToTarget);

        fileSystem.Directory.CreateSymbolicLink(path, pathToTarget);

        var attributes = fileSystem.DirectoryInfo.New(path).Attributes;
        await That(attributes.HasFlag(FileAttributes.ReparsePoint)).IsTrue();
    }

    [Test]
    public async Task MockDirectory_ResolveLinkTarget_ShouldReturnPathOfTargetLink()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory("bar");
        fileSystem.Directory.CreateSymbolicLink("foo", "bar");

        var result = fileSystem.Directory.ResolveLinkTarget("foo", false);

        await That(result.Name).IsEqualTo("bar");
    }

    [Test]
    public async Task MockDirectory_ResolveLinkTarget_WithFinalTarget_ShouldReturnPathOfTargetLink()
    {
        // The maximum number of symbolic links that are followed:
        // https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.resolvelinktarget?view=net-6.0#remarks
        var maxResolveLinks = XFS.IsWindowsPlatform() ? 63 : 40;
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory("bar");
        var previousPath = "bar";
        for (int i = 0; i < maxResolveLinks; i++)
        {
            string newPath = $"foo-{i}";
            fileSystem.Directory.CreateSymbolicLink(newPath, previousPath);
            previousPath = newPath;
        }

        var result = fileSystem.Directory.ResolveLinkTarget(previousPath, true);

        await That(result.Name).IsEqualTo("bar");
    }

    [Test]
    public async Task MockDirectory_ResolveLinkTarget_WithFinalTargetWithTooManyLinks_ShouldThrowIOException()
    {
        // The maximum number of symbolic links that are followed:
        // https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.resolvelinktarget?view=net-6.0#remarks
        var maxResolveLinks = XFS.IsWindowsPlatform() ? 63 : 40;
        maxResolveLinks++;
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory("bar");
        var previousPath = "bar";
        for (int i = 0; i < maxResolveLinks; i++)
        {
            string newPath = $"foo-{i}";
            fileSystem.Directory.CreateSymbolicLink(newPath, previousPath);
            previousPath = newPath;
        }

        await That(() => fileSystem.Directory.ResolveLinkTarget(previousPath, true)).Throws<IOException>();
    }

    [Test]
    public async Task MockDirectory_ResolveLinkTarget_WithoutFinalTarget_ShouldReturnFirstLink()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory("bar");
        fileSystem.Directory.CreateSymbolicLink("foo", "bar");
        fileSystem.Directory.CreateSymbolicLink("foo1", "foo");

        var result = fileSystem.Directory.ResolveLinkTarget("foo1", false);

        await That(result.Name).IsEqualTo("foo");
    }

    [Test]
    public async Task MockDirectory_ResolveLinkTarget_WithoutTargetLink_ShouldThrowIOException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory("bar");
        fileSystem.Directory.CreateSymbolicLink("foo", "bar");

        await That(() =>
        {
            fileSystem.Directory.ResolveLinkTarget("bar", false);
        }).Throws<IOException>();
    }
#endif
}