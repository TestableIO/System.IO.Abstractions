using System.Collections.Generic;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;
using System.Threading.Tasks;
using System.Threading;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileReadAllBytesTests
    {
        [Test]
        public async Task MockFile_ReadAllBytes_ShouldReturnOriginalByteData()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            var result = file.ReadAllBytes(XFS.Path(@"c:\something\other.gif"));

            await That(result)
                .IsEqualTo(new byte[] { 0x21, 0x58, 0x3f, 0xa9 });
        }

        [Test]
        public async Task MockFile_ReadAllBytes_ShouldReturnDataSavedByWriteAllBytes()
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            fileSystem.File.WriteAllBytes(path, fileContent);

            await That(fileSystem.File.ReadAllBytes(path)).IsEqualTo(fileContent);
        }

        [Test]
        public async Task MockFile_ReadAllBytes_ShouldThrowFileNotFoundExceptionIfFileDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            var file = new MockFile(fileSystem);

            Action action = () => file.ReadAllBytes(@"C:\a.txt");

            await That(action).Throws<FileNotFoundException>();
        }

        [Test]
        public async Task MockFile_ReadAllBytes_ShouldTolerateAltDirectorySeparatorInPath()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:" + fileSystem.Path.DirectorySeparatorChar + "test.dat");
            var altPath = XFS.Path("C:" + fileSystem.Path.AltDirectorySeparatorChar + "test.dat");
            var data = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            fileSystem.AddFile(path, new MockFileData(data));

            await That(fileSystem.File.ReadAllBytes(altPath)).IsEqualTo(data);
        }

        [Test]
        public async Task MockFile_ReadAllBytes_ShouldReturnANewCopyOfTheFileContents()
        {
            var path = XFS.Path(@"c:\something\demo.bin");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData(new byte[] { 1, 2, 3, 4 }) }
            });

            var firstRead = fileSystem.File.ReadAllBytes(path);

            var secondRead = fileSystem.File.ReadAllBytes(path);

            for (int i = 0; i < firstRead.Length; i++)
            {
                firstRead[i] += 1;
            }

            await That(firstRead).IsNotEqualTo(secondRead);
        }

#if FEATURE_ASYNC_FILE
        [Test]
        public async Task MockFile_ReadAllBytesAsync_ShouldReturnOriginalByteData()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            var result = await file.ReadAllBytesAsync(XFS.Path(@"c:\something\other.gif"));

            await That(result)
                .IsEqualTo(new byte[] { 0x21, 0x58, 0x3f, 0xa9 });
        }

        [Test]
        public async Task MockFile_ReadAllBytesAsync_ShouldReturnDataSavedByWriteAllBytes()
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            fileSystem.File.WriteAllBytes(path, fileContent);

            await That(await fileSystem.File.ReadAllBytesAsync(path)).IsEqualTo(fileContent);
        }

        [Test]
        public async Task MockFile_ReadAllBytesAsync_ShouldThrowFileNotFoundExceptionIfFileDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            var file = new MockFile(fileSystem);

            Func<Task> action = async () => await file.ReadAllBytesAsync(@"C:\a.txt");

            await That(action).Throws<FileNotFoundException>();
        }

        [Test]
        public async Task MockFile_ReadAllBytesAsync_ShouldThrowOperationCanceledExceptionIfCanceled()
        {
            var fileSystem = new MockFileSystem();

            Func<Task> action = async () =>
                await fileSystem.File.ReadAllBytesAsync(@"C:\a.txt", new CancellationToken(canceled: true));

            await That(action).Throws<OperationCanceledException>();
        }

        [Test]
        public async Task MockFile_ReadAllBytesAsync_ShouldTolerateAltDirectorySeparatorInPath()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:" + fileSystem.Path.DirectorySeparatorChar + "test.dat");
            var altPath = XFS.Path("C:" + fileSystem.Path.AltDirectorySeparatorChar + "test.dat");
            var data = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            fileSystem.AddFile(path, new MockFileData(data));

            await That(await fileSystem.File.ReadAllBytesAsync(altPath)).IsEqualTo(data);
        }
#endif
    }
}
