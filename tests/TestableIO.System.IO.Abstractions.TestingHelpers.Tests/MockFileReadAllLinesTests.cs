namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;
    using Collections.Specialized;
    using Threading;
    using Threading.Tasks;

    using NUnit.Framework;

    using Text;

    using XFS = MockUnixSupport;

    public class MockFileReadAllLinesTests
    {
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
                new[] { "Hello", "there", "Bob", "Bob!" },
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

        [Test]
        public void MockFile_ReadAllLines_ShouldNotReturnBom()
        {
            // Arrange
            var testFilePath = XFS.Path(@"c:\a test file.txt");
            const string testText = "Hello World";
            var fileSystem = new MockFileSystem();
            fileSystem.File.WriteAllLines(testFilePath, new[] { testText }, Encoding.UTF8);

            // Act
            var result = fileSystem.File.ReadAllLines(testFilePath, Encoding.UTF8);

            // Assert
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(testText));
        }
#if FEATURE_ASYNC_FILE
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
        public void MockFile_ReadAllLinesAsync_ShouldThrowOperationCanceledExceptionIfCanceled()
        {
            var fileSystem = new MockFileSystem();

            AsyncTestDelegate action = async () =>
                await fileSystem.File.ReadAllLinesAsync(@"C:\a.txt", new CancellationToken(canceled: true));

            Assert.ThrowsAsync<OperationCanceledException>(action);
        }

        [Test]
        public void MockFile_ReadAllLinesAsync_NotExistingFile_ThrowsCorrectFileNotFoundException()
        {
            var absentFileNameFullPath = XFS.Path(@"c:\you surely don't have such file.hope-so");
            var mockFileSystem = new MockFileSystem();

            var act = new AsyncTestDelegate(async () =>
                await mockFileSystem.File.ReadAllTextAsync(absentFileNameFullPath)
            );

            var exception = Assert.CatchAsync<FileNotFoundException>(act);
            Assert.That(exception.FileName, Is.EqualTo(absentFileNameFullPath));
            Assert.That(exception.Message, Is.EqualTo("Could not find file '" + absentFileNameFullPath + "'."));
        }

#if FEATURE_READ_LINES_ASYNC
        [Test]
        public async Task MockFile_ReadLinesAsync_ShouldReturnOriginalTextData()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo\r\ntext\ncontent\rvalue") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var enumerable = file.ReadLinesAsync(XFS.Path(@"c:\something\demo.txt"));
            StringCollection result = new();
            await foreach (var line in enumerable)
                result.Add(line);

            // Assert
            CollectionAssert.AreEqual(
                new[] { "Demo", "text", "content", "value" },
                result);
        }

        [Test]
        public async Task MockFile_ReadLinesAsync_ShouldReturnOriginalDataWithCustomEncoding()
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
            var enumerable = file.ReadLinesAsync(XFS.Path(@"c:\something\demo.txt"), Encoding.BigEndianUnicode);
            StringCollection result = new();
            await foreach (var line in enumerable)
                result.Add(line);

            // Assert
            CollectionAssert.AreEqual(
                new[] { "Hello", "there", "Bob", "Bob!" },
                result);
        }

        [Test]
        public void MockFile_ReadLinesAsync_ShouldThrowOperationCanceledExceptionIfCanceled()
        {
            var fileSystem = new MockFileSystem();

            AsyncTestDelegate action = async () =>
            {
                var enumerable = fileSystem.File.ReadLinesAsync(@"C:\a.txt", new CancellationToken(canceled: true));
                await foreach (var line in enumerable);
            };

            Assert.ThrowsAsync<OperationCanceledException>(action);
        }

        [Test]
        public void MockFile_ReadLinesAsync_NotExistingFile_ThrowsCorrectFileNotFoundException()
        {
            var absentFileNameFullPath = XFS.Path(@"c:\you surely don't have such file.hope-so");
            var mockFileSystem = new MockFileSystem();

            AsyncTestDelegate action = async () =>
            {
                var enumerable = mockFileSystem.File.ReadLinesAsync(absentFileNameFullPath);
                await foreach (var line in enumerable) ;
            };

            var exception = Assert.CatchAsync<FileNotFoundException>(action);
            Assert.That(exception.FileName, Is.EqualTo(absentFileNameFullPath));
            Assert.That(exception.Message, Is.EqualTo("Could not find file '" + absentFileNameFullPath + "'."));
        }
#endif
#endif
    }
}
