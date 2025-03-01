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
        public async Task MockFile_AppendAllText_ShouldPersistNewText()
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
            await That(file.ReadAllText(path))
              .IsEqualTo("Demo text content+ some text");
        }

        [Test]
        public async Task MockFile_AppendAllText_ShouldPersistNewTextWithDifferentEncoding()
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
            await That(fileSystem.GetFile(Path).Contents)
              .IsEqualTo(new byte[] { 255, 254, 0, 0, 65, 0, 0, 0, 65, 0, 0, 0, 66, 66 });
        }

        [Test]
        public async Task MockFile_AppendAllText_ShouldCreateIfNotExist()
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
            await That(fileSystem.File.ReadAllText(path))
              .IsEqualTo("Demo text content some text");
        }

        [Test]
        public async Task MockFile_AppendAllText_ShouldCreateIfNotExistWithBom()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var path = XFS.Path(@"c:\something\demo3.txt");
            fileSystem.AddDirectory(XFS.Path(@"c:\something\"));

            // Act
            fileSystem.File.AppendAllText(path, "AA", Encoding.UTF32);

            // Assert
            await That(fileSystem.GetFile(path).Contents)
              .IsEqualTo(new byte[] { 255, 254, 0, 0, 65, 0, 0, 0, 65, 0, 0, 0 });
        }

        [Test]
        public async Task MockFile_AppendAllText_ShouldFailIfNotExistButDirectoryAlsoNotExist()
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
            ex = await That(() => fileSystem.File.AppendAllText(path, "some text")).Throws<DirectoryNotFoundException>();
            await That(ex.Message)
              .IsEqualTo(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path));

            ex =
                await That(
                    () => fileSystem.File.AppendAllText(path, "some text", Encoding.Unicode)).Throws<DirectoryNotFoundException>();
            await That(ex.Message)
              .IsEqualTo(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path));
        }

        [Test]
        public async Task MockFile_AppendAllText_ShouldPersistNewTextWithCustomEncoding()
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

            await That(file.ReadAllBytes(path)).IsEqualTo(expected);
        }

        [Test]
        public async Task MockFile_AppendAllText_ShouldWorkWithRelativePath()
        {
            var file = "file.txt";
            var fileSystem = new MockFileSystem();

            fileSystem.File.AppendAllText(file, "Foo");

            await That(fileSystem.File.Exists(file)).IsTrue();
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
            await That(file.ReadAllText(path))
              .IsEqualTo("Demo text content+ some text");
        }

        [Test]
        public async Task MockFile_AppendAllTextAsync_ShouldThrowOperationCanceledExceptionIfCancelled()
        {
            // Arrange
            const string path = "test.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("line 1") }
            });

            // Act
            async Task Act() =>
                await fileSystem.File.AppendAllTextAsync(
                    path,
                    "line 2",
                    new CancellationToken(canceled: true));
            await That(Act).Throws<OperationCanceledException>();

            // Assert
            await That(fileSystem.File.ReadAllText(path)).IsEqualTo("line 1");
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
            await That(fileSystem.GetFile(Path).Contents)
              .IsEqualTo(new byte[] { 255, 254, 0, 0, 65, 0, 0, 0, 65, 0, 0, 0, 66, 66 });
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
            await That(fileSystem.File.ReadAllText(path))
              .IsEqualTo("Demo text content some text");
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
            await That(fileSystem.GetFile(path).Contents)
              .IsEqualTo(new byte[] { 255, 254, 0, 0, 65, 0, 0, 0, 65, 0, 0, 0 });
        }

        [Test]
        public async Task MockFile_AppendAllTextAsync_ShouldFailIfNotExistButDirectoryAlsoNotExist()
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
            Func<Task> action = async () => await fileSystem.File.AppendAllTextAsync(path, "some text");
            ex = await That(action).Throws<DirectoryNotFoundException>();
            await That(ex.Message)
              .IsEqualTo(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path));

            async Task Act() => await fileSystem.File.AppendAllTextAsync(path, "some text", Encoding.Unicode);
            ex = await That(Act).Throws<DirectoryNotFoundException>();
            await That(ex.Message)
              .IsEqualTo(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path));
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

            await That(file.ReadAllBytes(path)).IsEqualTo(expected);
        }

        [Test]
        public async Task MockFile_AppendAllTextAsync_ShouldWorkWithRelativePath()
        {
            var file = "file.txt";
            var fileSystem = new MockFileSystem();

            await fileSystem.File.AppendAllTextAsync(file, "Foo");

            await That(fileSystem.File.Exists(file)).IsTrue();
        }
#endif
    }
}