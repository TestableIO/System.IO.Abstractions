namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using Globalization;

    using NUnit.Framework;
    using Text;

    using XFS = MockUnixSupport;

    using System.Threading.Tasks;
    using System.Threading;

    public class MockFileAppendAllTextTests
    {
        [Test]
        public void MockFile_AppendAllText_ShouldPersistNewText()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {path, new MockFileData("Demo text content")}
            });

            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllText(path, "+ some text");

            // Assert
            Assert.That(file.ReadAllText(path),
              Is.EqualTo("Demo text content+ some text"));
        }

        [Test]
        public void MockFile_AppendAllText_ShouldPersistNewTextWithDifferentEncoding()
        {
            // Arrange
            const string Path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {Path, new MockFileData("AA", Encoding.UTF32)}
            });

            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllText(Path, "BB", Encoding.UTF8);

            // Assert
            CollectionAssert.AreEqual(
                new byte[] { 255, 254, 0, 0, 65, 0, 0, 0, 65, 0, 0, 0, 66, 66 },
                fileSystem.GetFile(Path).Contents);
        }

        [Test]
        public void MockFile_AppendAllText_ShouldCreateIfNotExist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {path, new MockFileData("Demo text content")}
            });

            // Act
            fileSystem.File.AppendAllText(path, " some text");

            // Assert
            Assert.That(fileSystem.File.ReadAllText(path),
              Is.EqualTo("Demo text content some text"));
        }

        [Test]
        public void MockFile_AppendAllText_ShouldCreateIfNotExistWithBom()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var path = XFS.Path(@"c:\something\demo3.txt");
            fileSystem.AddDirectory(XFS.Path(@"c:\something\"));

            // Act
            fileSystem.File.AppendAllText(path, "AA", Encoding.UTF32);

            // Assert
            CollectionAssert.AreEqual(
                new byte[] { 255, 254, 0, 0, 65, 0, 0, 0, 65, 0, 0, 0 },
                fileSystem.GetFile(path).Contents);
        }

        [Test]
        public void MockFile_AppendAllText_ShouldFailIfNotExistButDirectoryAlsoNotExist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {path, new MockFileData("Demo text content")}
            });

            // Act
            path = XFS.Path(@"c:\something2\demo.txt");

            // Assert
            Exception ex;
            ex = Assert.Throws<DirectoryNotFoundException>(() => fileSystem.File.AppendAllText(path, "some text"));
            Assert.That(ex.Message,
                Is.EqualTo(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path)));

            ex =
                Assert.Throws<DirectoryNotFoundException>(
                    () => fileSystem.File.AppendAllText(path, "some text", Encoding.Unicode));
            Assert.That(ex.Message,
                Is.EqualTo(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path)));
        }

        [Test]
        public void MockFile_AppendAllText_ShouldPersistNewTextWithCustomEncoding()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {path, new MockFileData("Demo text content")}
            });

            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllText(path, "+ some text", Encoding.BigEndianUnicode);

            // Assert
            var expected = new byte[]
            {
                68, 101, 109, 111, 32, 116, 101, 120, 116, 32, 99, 111, 110, 116,
                101, 110, 116, 0, 43, 0, 32, 0, 115, 0, 111, 0, 109, 0, 101,
                0, 32, 0, 116, 0, 101, 0, 120, 0, 116
            };

            CollectionAssert.AreEqual(
                expected,
                file.ReadAllBytes(path));
        }

        [Test]
        public void MockFile_AppendAllText_ShouldWorkWithRelativePath()
        {
            var file = "file.txt";
            var fileSystem = new MockFileSystem();

            fileSystem.File.AppendAllText(file, "Foo");

            Assert.That(fileSystem.File.Exists(file));
        }

#if FEATURE_ASYNC_FILE
        [Test]
        public async Task MockFile_AppendAllTextAsync_ShouldPersistNewText()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {path, new MockFileData("Demo text content")}
            });

            var file = new MockFile(fileSystem);

            // Act
            await file.AppendAllTextAsync(path, "+ some text");

            // Assert
            Assert.That(file.ReadAllText(path),
              Is.EqualTo("Demo text content+ some text"));
        }

        [Test]
        public void MockFile_AppendAllTextAsync_ShouldThrowOperationCanceledExceptionIfCancelled()
        {
            // Arrange
            const string path = "test.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("line 1") }
            });

            // Act
            Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await fileSystem.File.AppendAllTextAsync(
                    path,
                    "line 2",
                    new CancellationToken(canceled: true))
            );

            // Assert
            Assert.That(fileSystem.File.ReadAllText(path), Is.EqualTo("line 1"));
        }

        [Test]
        public async Task MockFile_AppendAllTextAsync_ShouldPersistNewTextWithDifferentEncoding()
        {
            // Arrange
            const string Path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {Path, new MockFileData("AA", Encoding.UTF32)}
            });

            var file = new MockFile(fileSystem);

            // Act
            await file.AppendAllTextAsync(Path, "BB", Encoding.UTF8);

            // Assert
            CollectionAssert.AreEqual(
                new byte[] { 255, 254, 0, 0, 65, 0, 0, 0, 65, 0, 0, 0, 66, 66 },
                fileSystem.GetFile(Path).Contents);
        }

        [Test]
        public async Task MockFile_AppendAllTextAsync_ShouldCreateIfNotExist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {path, new MockFileData("Demo text content")}
            });

            // Act
            await fileSystem.File.AppendAllTextAsync(path, " some text");

            // Assert
            Assert.That(fileSystem.File.ReadAllText(path),
              Is.EqualTo("Demo text content some text"));
        }

        [Test]
        public async Task MockFile_AppendAllTextAsync_ShouldCreateIfNotExistWithBom()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var path = XFS.Path(@"c:\something\demo3.txt");
            fileSystem.AddDirectory(XFS.Path(@"c:\something\"));

            // Act
            await fileSystem.File.AppendAllTextAsync(path, "AA", Encoding.UTF32);

            // Assert
            CollectionAssert.AreEqual(
                new byte[] { 255, 254, 0, 0, 65, 0, 0, 0, 65, 0, 0, 0 },
                fileSystem.GetFile(path).Contents);
        }

        [Test]
        public void MockFile_AppendAllTextAsync_ShouldFailIfNotExistButDirectoryAlsoNotExist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {path, new MockFileData("Demo text content")}
            });

            // Act
            path = XFS.Path(@"c:\something2\demo.txt");

            // Assert
            Exception ex;
            ex = Assert.ThrowsAsync<DirectoryNotFoundException>(async () => await fileSystem.File.AppendAllTextAsync(path, "some text"));
            Assert.That(ex.Message,
                Is.EqualTo(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path)));

            ex =
                Assert.ThrowsAsync<DirectoryNotFoundException>(
                    async () => await fileSystem.File.AppendAllTextAsync(path, "some text", Encoding.Unicode));
            Assert.That(ex.Message,
                Is.EqualTo(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path)));
        }

        [Test]
        public async Task MockFile_AppendAllTextAsync_ShouldPersistNewTextWithCustomEncoding()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {path, new MockFileData("Demo text content")}
            });

            var file = new MockFile(fileSystem);

            // Act
            await file.AppendAllTextAsync(path, "+ some text", Encoding.BigEndianUnicode);

            // Assert
            var expected = new byte[]
            {
                68, 101, 109, 111, 32, 116, 101, 120, 116, 32, 99, 111, 110, 116,
                101, 110, 116, 0, 43, 0, 32, 0, 115, 0, 111, 0, 109, 0, 101,
                0, 32, 0, 116, 0, 101, 0, 120, 0, 116
            };

            CollectionAssert.AreEqual(
                expected,
                file.ReadAllBytes(path));
        }

        [Test]
        public async Task MockFile_AppendAllTextAsync_ShouldWorkWithRelativePath()
        {
            var file = "file.txt";
            var fileSystem = new MockFileSystem();

            await fileSystem.File.AppendAllTextAsync(file, "Foo");

            Assert.That(fileSystem.File.Exists(file));
        }
#endif
    }
}