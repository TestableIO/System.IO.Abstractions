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
        var fileSystemMock = Mock.Create<IFileSystem>(fs =>
            fs.Property.File.InitializeWith(Mock.Create<IFile>()));

        await That(() =>
            fileSystemMock.File.SetupMock.Method.ReadAllText(It.IsAny<string>()).Returns("")
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_Directory_Succeeds()
    {
        var fileSystemMock = Mock.Create<IFileSystem>(fs =>
            fs.Property.Directory.InitializeWith(Mock.Create<IDirectory>()));

        await That(() =>
            fileSystemMock.Directory.SetupMock.Method.CreateDirectory(It.IsAny<string>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_FileInfo_Succeeds()
    {
        var fileSystemMock = Mock.Create<IFileSystem>(fs =>
            fs.Property.FileInfo.InitializeWith(Mock.Create<IFileInfoFactory>()));

        await That(() =>
            fileSystemMock.FileInfo.SetupMock.Method.New(It.IsAny<string>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_FileStream_Succeeds()
    {
        var fileSystemMock = Mock.Create<IFileSystem>(fs =>
            fs.Property.FileStream.InitializeWith(Mock.Create<IFileStreamFactory>()));

        await That(() =>
            fileSystemMock.FileStream.SetupMock.Method.New(It.IsAny<string>(), It.IsAny<FileMode>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_Path_Succeeds()
    {
        var fileSystemMock = Mock.Create<IFileSystem>(fs =>
            fs.Property.Path.InitializeWith(Mock.Create<IPath>()));

        await That(() =>
            fileSystemMock.Path.SetupMock.Method.Combine(It.IsAny<string>(), It.IsAny<string>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_DirectoryInfo_Succeeds()
    {
        var fileSystemMock = Mock.Create<IFileSystem>(fs =>
            fs.Property.DirectoryInfo.InitializeWith(Mock.Create<IDirectoryInfoFactory>()));

        await That(() =>
            fileSystemMock.DirectoryInfo.SetupMock.Method.New(It.IsAny<string>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_DriveInfo_Succeeds()
    {
        var fileSystemMock = Mock.Create<IFileSystem>(fs =>
            fs.Property.DriveInfo.InitializeWith(Mock.Create<IDriveInfoFactory>()));

        await That(() =>
            fileSystemMock.DriveInfo.SetupMock.Method.New(It.IsAny<string>())
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_FileSystemWatcher_Succeeds()
    {
        var fileSystemMock = Mock.Create<IFileSystem>(fs =>
            fs.Property.FileSystemWatcher.InitializeWith(Mock.Create<IFileSystemWatcherFactory>()));

        await That(() =>
            fileSystemMock.FileSystemWatcher.SetupMock.Method.New(It.IsAny<string>())
        ).DoesNotThrow();
    }
}