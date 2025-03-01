using System.Collections.Generic;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;
using System.Threading.Tasks;
using System.Threading;

namespace System.IO.Abstractions.TestingHelpers.Tests;

public class MockFileWriteAllBytesTests
{
    [Test]
    public async Task MockFile_WriteAllBytes_ShouldThrowDirectoryNotFoundExceptionIfPathDoesNotExists()
    {
        var fileSystem = new MockFileSystem();
        string path = XFS.Path(@"c:\something\file.txt");
        var fileContent = new byte[] { 1, 2, 3, 4 };

        Action action = () => fileSystem.File.WriteAllBytes(path, fileContent);

        await That(action).Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task MockFile_WriteAllBytes_ShouldWriteDataToMemoryFileSystem()
    {
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();
        var fileContent = new byte[] { 1, 2, 3, 4 };
        fileSystem.AddDirectory(XFS.Path(@"c:\something"));

        fileSystem.File.WriteAllBytes(path, fileContent);

        await That(fileSystem.GetFile(path).Contents).IsEqualTo(fileContent);
    }

    [Test]
    public async Task MockFile_WriteAllBytes_ShouldThrowAnUnauthorizedAccessExceptionIfFileIsHidden()
    {
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("this is hidden") },
        });
        fileSystem.File.SetAttributes(path, FileAttributes.Hidden);

        Action action = () => fileSystem.File.WriteAllBytes(path, new byte[] { 123 });

        await That(action).Throws<UnauthorizedAccessException>()
            .Because($"Access to the path '{path}' is denied.");
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockFile_WriteAllBytes_ShouldThrowAnArgumentExceptionIfContainsIllegalCharacters()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:"));

        Action action = () => fileSystem.File.WriteAllBytes(XFS.Path(@"C:\a<b.txt"), new byte[] { 123 });

        await That(action).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockFile_WriteAllBytes_ShouldThrowAnArgumentNullExceptionIfPathIsNull()
    {
        var fileSystem = new MockFileSystem();

        Action action = () => fileSystem.File.WriteAllBytes(null, new byte[] { 123 });

        await That(action).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task MockFile_WriteAllBytes_ShouldThrowAnArgumentNullExceptionIfBytesAreNull()
    {
        var fileSystem = new MockFileSystem();
        string path = XFS.Path(@"c:\something\demo.txt");

        Action action = () => fileSystem.File.WriteAllBytes(path, null);

        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.Message).StartsWith("Value cannot be null.");
        await That(exception.ParamName).IsEqualTo("bytes");
    }

    [Test]
    public async Task MockFile_WriteAllBytes_ShouldWriteASeparateCopyToTheFileSystem()
    {
        var fileSystem = new MockFileSystem();
        string path = XFS.Path(@"c:\something\file.bin");
        fileSystem.AddDirectory(XFS.Path(@"c:\something"));
        var fileContent = new byte[] { 1, 2, 3, 4 };

        fileSystem.File.WriteAllBytes(path, fileContent);

        for(int i = 0; i < fileContent.Length; i++)
        {
            fileContent[i] += 1;
        }

        var readAgain = fileSystem.File.ReadAllBytes(path);

        await That(fileContent).IsNotEqualTo(readAgain);
    }


#if FEATURE_ASYNC_FILE
        [Test]
        public async Task MockFile_WriteAllBytesAsync_ShouldThrowDirectoryNotFoundExceptionIfPathDoesNotExists()
        {
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\file.txt");
            var fileContent = new byte[] { 1, 2, 3, 4 };

            Func<Task> action = () => fileSystem.File.WriteAllBytesAsync(path, fileContent);

            await That(action).Throws<DirectoryNotFoundException>();
        }

        [Test]
        public async Task MockFile_WriteAllTextAsync_ShouldThrowOperationCanceledExceptionIfCancelled()
        {
            // Arrange
            const string path = "test.txt";
            var fileSystem = new MockFileSystem();

            // Act
            async Task Act() =>
                await fileSystem.File.WriteAllTextAsync(
                    path,
                    "content",
                    new CancellationToken(canceled: true));
            await That(Act).Throws<OperationCanceledException>();

            // Assert
            await That(fileSystem.File.Exists(path)).IsFalse();
        }

        [Test]
        public async Task MockFile_WriteAllBytesAsync_ShouldWriteDataToMemoryFileSystem()
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            await fileSystem.File.WriteAllBytesAsync(path, fileContent);

            await That(fileSystem.GetFile(path).Contents).IsEqualTo(fileContent);
        }

        [Test]
        public async Task MockFile_WriteAllBytesAsync_ShouldThrowAnUnauthorizedAccessExceptionIfFileIsHidden()
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("this is hidden") },
            });
            fileSystem.File.SetAttributes(path, FileAttributes.Hidden);

            Func<Task> action = () => fileSystem.File.WriteAllBytesAsync(path, new byte[] { 123 });

            await That(action).Throws<UnauthorizedAccessException>()
                .Because($"Access to the path '{path}' is denied.");
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public async Task MockFile_WriteAllBytesAsync_ShouldThrowAnArgumentExceptionIfContainsIllegalCharacters()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:"));

            Func<Task> action = () => fileSystem.File.WriteAllBytesAsync(XFS.Path(@"C:\a<b.txt"), new byte[] { 123 });

            await That(action).Throws<ArgumentException>();
        }

        [Test]
        public async Task MockFile_WriteAllBytesAsync_ShouldThrowAnArgumentNullExceptionIfPathIsNull()
        {
            var fileSystem = new MockFileSystem();

            Func<Task> action = () => fileSystem.File.WriteAllBytesAsync(null, new byte[] { 123 });

            await That(action).Throws<ArgumentNullException>();
        }

        [Test]
        public async Task MockFile_WriteAllBytesAsync_ShouldThrowAnArgumentNullExceptionIfBytesAreNull()
        {
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\demo.txt");

            Func<Task> action = () => fileSystem.File.WriteAllBytesAsync(path, null);

            var exception = await That(action).Throws<ArgumentNullException>();
            await That(exception.Message).StartsWith("Value cannot be null.");
            await That(exception.ParamName).IsEqualTo("bytes");
        }
#endif
}