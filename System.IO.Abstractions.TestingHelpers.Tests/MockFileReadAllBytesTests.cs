using System.Collections.Generic;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

#if NETCOREAPP2_0
using System.Threading.Tasks;
#endif

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileReadAllBytesTests
    {
        [Test]
        public void MockFile_ReadAllBytes_ShouldReturnOriginalByteData()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            var result = file.ReadAllBytes(XFS.Path(@"c:\something\other.gif"));

            CollectionAssert.AreEqual(
                new byte[] { 0x21, 0x58, 0x3f, 0xa9 },
                result);
        }

        [Test]
        public void MockFile_ReadAllBytes_ShouldReturnDataSavedByWriteAllBytes()
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            fileSystem.File.WriteAllBytes(path, fileContent);

            Assert.AreEqual(fileContent, fileSystem.File.ReadAllBytes(path));
        }

        [Test]
        public void MockFile_ReadAllBytes_ShouldThrowFileNotFoundExceptionIfFileDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            var file = new MockFile(fileSystem);

            TestDelegate action = () => file.ReadAllBytes(@"C:\a.txt");

            Assert.Throws<FileNotFoundException>(action);
        }

        [Test]
        public void MockFile_ReadAllBytes_ShouldTolerateAltDirectorySeparatorInPath()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:" + fileSystem.Path.DirectorySeparatorChar + "test.dat");
            var altPath = XFS.Path("C:" + fileSystem.Path.AltDirectorySeparatorChar + "test.dat");
            var data = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            fileSystem.AddFile(path, new MockFileData(data));

            Assert.AreEqual(data, fileSystem.File.ReadAllBytes(altPath));
        }
#if NETCOREAPP2_0
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

            CollectionAssert.AreEqual(
                new byte[] { 0x21, 0x58, 0x3f, 0xa9 },
                result);
        }

        [Test]
        public async Task MockFile_ReadAllBytesAsync_ShouldReturnDataSavedByWriteAllBytes()
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            fileSystem.File.WriteAllBytes(path, fileContent);

            Assert.AreEqual(fileContent, await fileSystem.File.ReadAllBytesAsync(path));
        }

        [Test]
        public void MockFile_ReadAllBytesAsync_ShouldThrowFileNotFoundExceptionIfFileDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            var file = new MockFile(fileSystem);

            AsyncTestDelegate action = async () => await file.ReadAllBytesAsync(@"C:\a.txt");

            Assert.ThrowsAsync<FileNotFoundException>(action);
        }

        [Test]
        public async Task MockFile_ReadAllBytesAsync_ShouldTolerateAltDirectorySeparatorInPath()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:" + fileSystem.Path.DirectorySeparatorChar + "test.dat");
            var altPath = XFS.Path("C:" + fileSystem.Path.AltDirectorySeparatorChar + "test.dat");
            var data = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            fileSystem.AddFile(path, new MockFileData(data));

            Assert.AreEqual(data, await fileSystem.File.ReadAllBytesAsync(altPath));
        }
#endif
    }
}
