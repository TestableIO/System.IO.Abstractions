using System.Collections.Generic;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

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
    }
}
