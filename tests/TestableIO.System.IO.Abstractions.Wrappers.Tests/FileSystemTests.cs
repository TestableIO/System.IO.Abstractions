
using NUnit.Framework;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class FileSystemTests
    {
#if !NET9_0_OR_GREATER
        [Test]
        public void Is_Serializable()
        {
            var fileSystem = new FileSystem();
            var memoryStream = new MemoryStream();

#pragma warning disable SYSLIB0011
            var serializer = new Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            serializer.Serialize(memoryStream, fileSystem);
#pragma warning restore SYSLIB0011

            Assert.That(memoryStream.Length > 0, "Length didn't increase after serialization task.");
        }
#endif

        [Test]
        public void Mock_File_Succeeds()
        {
            var fileSystemMock = new Moq.Mock<IFileSystem>();

            Assert.DoesNotThrow(() =>
                fileSystemMock.Setup(x => x.File.ToString()).Returns("")
            );
        }

        [Test]
        public void Mock_Directory_Succeeds()
        {
            var fileSystemMock = new Moq.Mock<IFileSystem>();

            Assert.DoesNotThrow(() =>
                fileSystemMock.Setup(x => x.Directory.ToString()).Returns("")
            );
        }

        [Test]
        public void Mock_FileInfo_Succeeds()
        {
            var fileSystemMock = new Moq.Mock<IFileSystem>();

            Assert.DoesNotThrow(() =>
                fileSystemMock.Setup(x => x.FileInfo.ToString()).Returns("")
            );
        }

        [Test]
        public void Mock_FileStream_Succeeds()
        {
            var fileSystemMock = new Moq.Mock<IFileSystem>();

            Assert.DoesNotThrow(() =>
                fileSystemMock.Setup(x => x.FileStream.ToString()).Returns("")
            );
        }

        [Test]
        public void Mock_Path_Succeeds()
        {
            var fileSystemMock = new Moq.Mock<IFileSystem>();

            Assert.DoesNotThrow(() =>
                fileSystemMock.Setup(x => x.Path.ToString()).Returns("")
            );
        }

        [Test]
        public void Mock_DirectoryInfo_Succeeds()
        {
            var fileSystemMock = new Moq.Mock<IFileSystem>();

            Assert.DoesNotThrow(() =>
                fileSystemMock.Setup(x => x.DirectoryInfo.ToString()).Returns("")
            );
        }

        [Test]
        public void Mock_DriveInfo_Succeeds()
        {
            var fileSystemMock = new Moq.Mock<IFileSystem>();

            Assert.DoesNotThrow(() =>
                fileSystemMock.Setup(x => x.DirectoryInfo.ToString()).Returns("")
            );
        }

        [Test]
        public void Mock_FileSystemWatcher_Succeeds()
        {
            var fileSystemMock = new Moq.Mock<IFileSystem>();

            Assert.DoesNotThrow(() =>
                fileSystemMock.Setup(x => x.FileSystemWatcher.ToString()).Returns("")
            );
        }
    }
}
