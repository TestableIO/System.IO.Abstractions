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
        var fileSystemMock = new Moq.Mock<IFileSystem>();

        await That(() =>
            fileSystemMock.Setup(x => x.File.ToString()).Returns("")
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_Directory_Succeeds()
    {
        var fileSystemMock = new Moq.Mock<IFileSystem>();

        await That(() =>
            fileSystemMock.Setup(x => x.Directory.ToString()).Returns("")
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_FileInfo_Succeeds()
    {
        var fileSystemMock = new Moq.Mock<IFileSystem>();

        await That(() =>
            fileSystemMock.Setup(x => x.FileInfo.ToString()).Returns("")
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_FileStream_Succeeds()
    {
        var fileSystemMock = new Moq.Mock<IFileSystem>();

        await That(() =>
            fileSystemMock.Setup(x => x.FileStream.ToString()).Returns("")
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_Path_Succeeds()
    {
        var fileSystemMock = new Moq.Mock<IFileSystem>();

        await That(() =>
            fileSystemMock.Setup(x => x.Path.ToString()).Returns("")
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_DirectoryInfo_Succeeds()
    {
        var fileSystemMock = new Moq.Mock<IFileSystem>();

        await That(() =>
            fileSystemMock.Setup(x => x.DirectoryInfo.ToString()).Returns("")
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_DriveInfo_Succeeds()
    {
        var fileSystemMock = new Moq.Mock<IFileSystem>();

        await That(() =>
            fileSystemMock.Setup(x => x.DirectoryInfo.ToString()).Returns("")
        ).DoesNotThrow();
    }

    [Test]
    public async Task Mock_FileSystemWatcher_Succeeds()
    {
        var fileSystemMock = new Moq.Mock<IFileSystem>();

        await That(() =>
            fileSystemMock.Setup(x => x.FileSystemWatcher.ToString()).Returns("")
        ).DoesNotThrow();
    }
}