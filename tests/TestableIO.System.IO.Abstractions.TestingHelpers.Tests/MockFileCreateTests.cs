namespace System.IO.Abstractions.TestingHelpers.Tests;

using Collections.Generic;

using Globalization;

using NUnit.Framework;

using Text;

using XFS = MockUnixSupport;

public class MockFileCreateTests
{
    [Test]
    public async Task Mockfile_Create_ShouldCreateNewStream()
    {
        string fullPath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"c:\something"));

        var sut = new MockFile(fileSystem);

        await That(fileSystem.FileExists(fullPath)).IsFalse();

        sut.Create(fullPath).Dispose();

        await That(fileSystem.FileExists(fullPath)).IsTrue();
    }

    [Test]
    public async Task Mockfile_Create_CanWriteToNewStream()
    {
        string fullPath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"c:\something"));
        var data = new UTF8Encoding(false).GetBytes("Test string");

        var sut = new MockFile(fileSystem);
        using (var stream = sut.Create(fullPath))
        {
            stream.Write(data, 0, data.Length);
        }

        var mockFileData = fileSystem.GetFile(fullPath);
        var fileData = mockFileData.Contents;

        await That(fileData).IsEqualTo(data);
    }

    [Test]
    public async Task Mockfile_Create_OverwritesExistingFile()
    {
        string path = XFS.Path(@"c:\some\file.txt");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"c:\some"));

        var mockFile = new MockFile(fileSystem);

        // Create a file
        using (var stream = mockFile.Create(path))
        {
            var contents = new UTF8Encoding(false).GetBytes("Test 1");
            stream.Write(contents, 0, contents.Length);
        }

        // Create new file that should overwrite existing file
        var expectedContents = new UTF8Encoding(false).GetBytes("Test 2");
        using (var stream = mockFile.Create(path))
        {
            stream.Write(expectedContents, 0, expectedContents.Length);
        }

        var actualContents = fileSystem.GetFile(path).Contents;

        await That(actualContents).IsEqualTo(expectedContents);
    }

    [Test]
    public async Task Mockfile_Create_ShouldThrowUnauthorizedAccessExceptionIfPathIsReadOnly()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\read-only.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { { path, new MockFileData("Content") } });
        var mockFile = new MockFile(fileSystem);

        // Act
        mockFile.SetAttributes(path, FileAttributes.ReadOnly);

        // Assert
        var exception = await That(() => mockFile.Create(path).Dispose()).Throws<UnauthorizedAccessException>();
        await That(exception.Message).IsEqualTo(string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path));
    }

    [Test]
    public async Task Mockfile_Create_ShouldThrowArgumentExceptionIfPathIsZeroLength()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.Create("");

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [TestCase("\"")]
    [TestCase("<")]
    [TestCase(">")]
    [TestCase("|")]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockFile_Create_ShouldThrowArgumentNullExceptionIfPathIsNull1(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.Create(path);

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [TestCase(" ")]
    [TestCase("   ")]
    public async Task MockFile_Create_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.Create(path);

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockFile_Create_ShouldThrowArgumentNullExceptionIfPathIsNull()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.Create(null);

        // Assert
        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.Message).StartsWith("Path cannot be null.");
    }

    [Test]
    public async Task MockFile_Create_ShouldThrowDirectoryNotFoundExceptionIfCreatingAndParentPathDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var file = XFS.Path("C:\\path\\NotFound.ext");

        // Act
        Action action = () => fileSystem.File.Create(file);

        // Assert
        var exception = await That(action).Throws<DirectoryNotFoundException>();
        await That(exception.Message).StartsWith("Could not find a part of the path");
    }

    [Test]
    public async Task MockFile_Create_TruncateShouldWriteNewContents()
    {
        // Arrange
        string testFileName = XFS.Path(@"c:\someFile.txt");
        var fileSystem = new MockFileSystem();

        using (var stream = fileSystem.FileStream.New(testFileName, FileMode.Create, FileAccess.Write))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("original_text");
            }
        }

        // Act
        using (var stream = fileSystem.FileStream.New(testFileName, FileMode.Truncate, FileAccess.Write))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("new_text");
            }
        }

        // Assert
        await That(fileSystem.File.ReadAllText(testFileName)).IsEqualTo("new_text");
    }

    [Test]
    public async Task MockFile_Create_TruncateShouldClearFileContentsOnOpen()
    {
        // Arrange
        string testFileName = XFS.Path(@"c:\someFile.txt");
        var fileSystem = new MockFileSystem();

        using (var stream = fileSystem.FileStream.New(testFileName, FileMode.Create, FileAccess.Write))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("original_text");
            }
        }

        // Act
        using (var stream = fileSystem.FileStream.New(testFileName, FileMode.Truncate, FileAccess.Write))
        {
            // Opening the stream is enough to reset the contents
        }

        // Assert
        await That(fileSystem.File.ReadAllText(testFileName)).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task MockFile_Create_DeleteOnCloseOption_FileExistsWhileStreamIsOpen()
    {
        var root = XFS.Path(@"C:\");
        var filePath = XFS.Path(@"C:\test.txt");
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(root);

        using (fileSystem.File.Create(filePath, 4096, FileOptions.DeleteOnClose))
        {
            await That(fileSystem.File.Exists(filePath)).IsTrue();
        }
    }

    [Test]
    public async Task MockFile_Create_DeleteOnCloseOption_FileDeletedWhenStreamIsClosed()
    {
        var root = XFS.Path(@"C:\");
        var filePath = XFS.Path(@"C:\test.txt");
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(root);

        using (fileSystem.File.Create(filePath, 4096, FileOptions.DeleteOnClose))
        {
        }

        await That(fileSystem.File.Exists(filePath)).IsFalse();
    }

    [Test]
    public async Task MockFile_Create_EncryptedOption_FileNotYetEncryptedWhenStreamIsOpen()
    {
        var root = XFS.Path(@"C:\");
        var filePath = XFS.Path(@"C:\test.txt");
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(root);

        using (var stream = fileSystem.File.Create(filePath, 4096, FileOptions.Encrypted))
        {
            var fileInfo = fileSystem.FileInfo.New(filePath);
            await That(fileInfo.Attributes.HasFlag(FileAttributes.Encrypted)).IsFalse();
        }
    }

    [Test]
    public async Task MockFile_Create_EncryptedOption_EncryptsFileWhenStreamIsClose()
    {
        var root = XFS.Path(@"C:\");
        var filePath = XFS.Path(@"C:\test.txt");
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(root);

        using (var stream = fileSystem.File.Create(filePath, 4096, FileOptions.Encrypted))
        {
        }

        var fileInfo = fileSystem.FileInfo.New(filePath);
        await That(fileInfo.Attributes.HasFlag(FileAttributes.Encrypted)).IsTrue();
    }

    [Test]
    public async Task MockFile_Create_ShouldWorkWithRelativePath()
    {
        var relativeFile = "file.txt";
        var fileSystem = new MockFileSystem();

        fileSystem.File.Create(relativeFile).Close();

        await That(fileSystem.File.Exists(relativeFile)).IsTrue();
    }

    [Test]
    public async Task MockFile_Create_CanReadFromNewStream()
    {
        string fullPath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"c:\something"));

        using (var stream = fileSystem.File.Create(fullPath))
        {
            await That(stream.CanRead).IsTrue();
        }
    }
}