using Mockolate;

namespace System.IO.Abstractions.Tests;

[TestFixture]
public class FileSystemTests
{
#if !NET9_0_OR_GREATER
    [Test]
    public async Task Is_Serializable()
    {
        var fileSystem = new FileSystem();
        var memoryStream = new MemoryStream();

#pragma warning disable SYSLIB0011
        var serializer = new Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        serializer.Serialize(memoryStream, fileSystem);
#pragma warning restore SYSLIB0011

        await That(memoryStream).HasLength().GreaterThan(0)
            .Because("Length didn't increase after serialization task.");
    }
#endif

    [Test]
    public async Task Mock_File_Succeeds()
    {
        var fileSystem = IFileSystem.CreateMock(fs =>
            fs.File.InitializeWith(IFile.CreateMock()));
        fileSystem.File.Mock.Setup.ReadAllText(It.IsAny<string>()).Returns("foo");
        
        var result = fileSystem.File.ReadAllText("any path");
        
        await That(result).IsEqualTo("foo");
        await That(fileSystem.File.Mock.Verify.ReadAllText(It.Is("any path"))).Once();
    }

    [Test]
    public async Task Mock_Directory_Succeeds()
    {
        var fileSystemMock = IFileSystem.CreateMock(fs =>
            fs.Directory.InitializeWith(IDirectory.CreateMock()));

        await That(() =>
            fileSystemMock.Directory.Mock.Setup.CreateDirectory(It.IsAny<string>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_FileInfo_Succeeds()
    {
        var fileSystemMock = IFileSystem.CreateMock(fs =>
            fs.FileInfo.InitializeWith(IFileInfoFactory.CreateMock()));

        await That(() =>
            fileSystemMock.FileInfo.Mock.Setup.New(It.IsAny<string>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_FileStream_Succeeds()
    {
        var fileSystemMock = IFileSystem.CreateMock(fs =>
            fs.FileStream.InitializeWith(IFileStreamFactory.CreateMock()));

        await That(() =>
            fileSystemMock.FileStream.Mock.Setup.New(It.IsAny<string>(), It.IsAny<FileMode>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_Path_Succeeds()
    {
        var fileSystemMock = IFileSystem.CreateMock(fs =>
            fs.Path.InitializeWith(IPath.CreateMock()));

        await That(() =>
            fileSystemMock.Path.Mock.Setup.Combine(It.IsAny<string>(), It.IsAny<string>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_DirectoryInfo_Succeeds()
    {
        var fileSystemMock = IFileSystem.CreateMock(fs =>
            fs.DirectoryInfo.InitializeWith(IDirectoryInfoFactory.CreateMock()));

        await That(() =>
            fileSystemMock.DirectoryInfo.Mock.Setup.New(It.IsAny<string>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_DriveInfo_Succeeds()
    {
        var fileSystemMock = IFileSystem.CreateMock(fs =>
            fs.DriveInfo.InitializeWith(IDriveInfoFactory.CreateMock()));

        await That(() =>
            fileSystemMock.DriveInfo.Mock.Setup.New(It.IsAny<string>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_FileSystemWatcher_Succeeds()
    {
        var fileSystemMock = IFileSystem.CreateMock(fs =>
            fs.FileSystemWatcher.InitializeWith(IFileSystemWatcherFactory.CreateMock()));

        await That(() =>
            fileSystemMock.FileSystemWatcher.Mock.Setup.New(It.IsAny<string>())
        ).DoesNotThrow();
    }
}