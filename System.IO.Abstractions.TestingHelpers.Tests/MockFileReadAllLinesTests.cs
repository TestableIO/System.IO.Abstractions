namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using NUnit.Framework;

    using Text;

    using XFS = MockUnixSupport;

#if NETCOREAPP2_0
    using System.Threading.Tasks;
#endif

    public class MockFileReadAllLinesTests {
        [Test]
        public void MockFile_ReadAllLines_ShouldReturnOriginalTextData()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo\r\ntext\ncontent\rvalue") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllLines(XFS.Path(@"c:\something\demo.txt"));

            // Assert
            CollectionAssert.AreEqual(
                new[] { "Demo", "text", "content", "value" },
                result);
        }

        [Test]
        public void MockFile_ReadAllLines_ShouldReturnOriginalDataWithCustomEncoding()
        {
            // Arrange
            string text = "Hello\r\nthere\rBob\nBob!";
            var encodedText = Encoding.BigEndianUnicode.GetBytes(text);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData(encodedText) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllLines(XFS.Path(@"c:\something\demo.txt"), Encoding.BigEndianUnicode);

            // Assert
            CollectionAssert.AreEqual(
                new [] { "Hello", "there", "Bob", "Bob!" },
                result);
        }

        [Test]
        public void MockFile_ReadAllLines_NotExistingFile_ThrowsCorrectFileNotFoundException()
        {
            var absentFileNameFullPath = XFS.Path(@"c:\you surely don't have such file.hope-so");
            var mockFileSystem = new MockFileSystem();

            var act = new TestDelegate(() => 
                mockFileSystem.File.ReadAllText(absentFileNameFullPath)
            );

            var exception = Assert.Catch<FileNotFoundException>(act);
            Assert.That(exception.FileName, Is.EqualTo(absentFileNameFullPath));
            Assert.That(exception.Message, Is.EqualTo("Could not find file '" + absentFileNameFullPath + "'."));
        }

#if NETCOREAPP2_0
        [Test]
        public async Task MockFile_ReadAllLinesAsync_ShouldReturnOriginalTextData()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo\r\ntext\ncontent\rvalue") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = await file.ReadAllLinesAsync(XFS.Path(@"c:\something\demo.txt"));

            // Assert
            CollectionAssert.AreEqual(
                new[] { "Demo", "text", "content", "value" },
                result);
        }

        [Test]
        public async Task MockFile_ReadAllLinesAsync_ShouldReturnOriginalDataWithCustomEncoding()
        {
            // Arrange
            string text = "Hello\r\nthere\rBob\nBob!";
            var encodedText = Encoding.BigEndianUnicode.GetBytes(text);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData(encodedText) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = await file.ReadAllLinesAsync(XFS.Path(@"c:\something\demo.txt"), Encoding.BigEndianUnicode);

            // Assert
            CollectionAssert.AreEqual(
                new[] { "Hello", "there", "Bob", "Bob!" },
                result);
        }

        [Test]
        public void MockFile_ReadAllLinesAsync_NotExistingFile_ThrowsCorrectFileNotFoundException()
        {
            var absentFileNameFullPath = XFS.Path(@"c:\you surely don't have such file.hope-so");
            var mockFileSystem = new MockFileSystem();

            var act = new AsyncTestDelegate(() =>
                mockFileSystem.File.ReadAllTextAsync(absentFileNameFullPath)
            );

            var exception = Assert.CatchAsync<FileNotFoundException>(act);
            Assert.That(exception.FileName, Is.EqualTo(absentFileNameFullPath));
            Assert.That(exception.Message, Is.EqualTo("Could not find file '" + absentFileNameFullPath + "'."));
        }
#endif
    }
}