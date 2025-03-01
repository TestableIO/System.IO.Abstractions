using System.Collections.Generic;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;
using System.Threading.Tasks;
using System.Threading;

namespace System.IO.Abstractions.TestingHelpers.Tests;

public class MockFileAppendAllLinesTests
{
    [Test]
    public async Task MockFile_AppendAllLines_ShouldPersistNewLinesToExistingFile()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });

        var file = new MockFile(fileSystem);

        // Act
        file.AppendAllLines(path, new[] { "line 1", "line 2", "line 3" });

        // Assert
        await That(file.ReadAllText(path))
            .IsEqualTo("Demo text contentline 1" + Environment.NewLine + "line 2" + Environment.NewLine + "line 3" + Environment.NewLine);
    }

    [Test]
    public async Task MockFile_AppendAllLines_ShouldPersistNewLinesToNewFile()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\something\"), new MockDirectoryData() }
        });
        var file = new MockFile(fileSystem);

        // Act
        file.AppendAllLines(path, new[] { "line 1", "line 2", "line 3" });

        // Assert
        await That(file.ReadAllText(path))
            .IsEqualTo("line 1" + Environment.NewLine + "line 2" + Environment.NewLine + "line 3" + Environment.NewLine);
    }

    [Test]
    public async Task MockFile_AppendAllLines_ShouldThrowArgumentExceptionIfPathIsZeroLength()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.AppendAllLines(string.Empty, new[] { "does not matter" });

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [TestCase(" ")]
    [TestCase("   ")]
    public async Task MockFile_AppendAllLines_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.AppendAllLines(path, new[] { "does not matter" });

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [TestCase("\"")]
    [TestCase("<")]
    [TestCase(">")]
    [TestCase("|")]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockFile_AppendAllLines_ShouldThrowArgumentExceptionIfPathContainsInvalidChar(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.AppendAllLines(path, new[] { "does not matter" });

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockFile_AppendAllLines_ShouldThrowArgumentNullExceptionIfContentIsNull()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.AppendAllLines("foo", null);

        // Assert
        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.ParamName).IsEqualTo("contents");
    }

    [Test]
    public async Task MockFile_AppendAllLines_ShouldThrowArgumentNullExceptionIfEncodingIsNull()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.AppendAllLines("foo.txt", new[] { "bar" }, null);

        // Assert
        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.ParamName).IsEqualTo("encoding");
    }

#if FEATURE_ASYNC_FILE
        [Test]
        public async Task MockFile_AppendAllLinesAsync_ShouldPersistNewLinesToExistingFile()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            var file = new MockFile(fileSystem);

            // Act
            await file.AppendAllLinesAsync(path, new[] { "line 1", "line 2", "line 3" });

            // Assert
            await That(file.ReadAllText(path))
                .IsEqualTo("Demo text contentline 1" + Environment.NewLine + "line 2" + Environment.NewLine + "line 3" + Environment.NewLine);
        }

        [Test]
        public async Task MockFile_AppendAllLinesAsync_ShouldPersistNewLinesToNewFile()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\"), new MockDirectoryData() }
            });
            var file = new MockFile(fileSystem);

            // Act
            await file.AppendAllLinesAsync(path, new[] { "line 1", "line 2", "line 3" });

            // Assert
            await That(file.ReadAllText(path))
                .IsEqualTo("line 1" + Environment.NewLine + "line 2" + Environment.NewLine + "line 3" + Environment.NewLine);
        }

        [Test]
        public async Task MockFile_AppendAllLinesAsync_ShouldThrowOperationCanceledExceptionIfCancelled()
        {
            // Arrange
            const string path = "test.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("line 1") }
            });

            // Act
            async Task Act() =>
                await fileSystem.File.AppendAllLinesAsync(
                    path,
                    new[] { "line 2" },
                    new CancellationToken(canceled: true));
            await That(Act).Throws<OperationCanceledException>();

            // Assert
            await That(fileSystem.File.ReadAllText(path)).IsEqualTo("line 1");
        }

        [Test]
        public async Task MockFile_AppendAllLinesAsync_ShouldThrowArgumentExceptionIfPathIsZeroLength()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Func<Task> action = async () => await fileSystem.File.AppendAllLinesAsync(string.Empty, new[] { "does not matter" });

            // Assert
            await That(action).Throws<ArgumentException>();
        }

        [TestCase(" ")]
        [TestCase("   ")]
        public async Task MockFile_AppendAllLinesAsync_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Func<Task> action = async () => await fileSystem.File.AppendAllLinesAsync(path, new[] { "does not matter" });

            // Assert
            await That(action).Throws<ArgumentException>();
        }

        [TestCase("\"")]
        [TestCase("<")]
        [TestCase(">")]
        [TestCase("|")]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public async Task MockFile_AppendAllLinesAsync_ShouldThrowArgumentExceptionIfPathContainsInvalidChar(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Func<Task> action = async () => await fileSystem.File.AppendAllLinesAsync(path, new[] { "does not matter" });

            // Assert
            await That(action).Throws<ArgumentException>();
        }

        [Test]
        public async Task MockFile_AppendAllLinesAsync_ShouldThrowArgumentNullExceptionIfContentIsNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Func<Task> action = async () => await fileSystem.File.AppendAllLinesAsync("foo", null);

            // Assert
            var exception = await That(action).Throws<ArgumentNullException>();
            await That(exception.ParamName).IsEqualTo("contents");
        }

        [Test]
        public async Task MockFile_AppendAllLinesAsync_ShouldThrowArgumentNullExceptionIfEncodingIsNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Func<Task> action = async () => await fileSystem.File.AppendAllLinesAsync("foo.txt", new[] { "bar" }, null);

            // Assert
            var exception = await That(action).Throws<ArgumentNullException>();
            await That(exception.ParamName).IsEqualTo("encoding");
        }
#endif
}