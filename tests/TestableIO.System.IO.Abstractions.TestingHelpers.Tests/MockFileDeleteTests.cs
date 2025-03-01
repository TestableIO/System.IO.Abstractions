namespace System.IO.Abstractions.TestingHelpers.Tests;

using System.Collections.Generic;
using NUnit.Framework;

using XFS = MockUnixSupport;

public class MockFileDeleteTests
{
    [Test]
    public async Task MockFile_Delete_ShouldDeleteFile()
    {
        var fileSystem = new MockFileSystem();
        var path = XFS.Path("C:\\some_folder\\test");
        var directory = fileSystem.Path.GetDirectoryName(path);
        fileSystem.AddFile(path, new MockFileData("Bla"));

        var fileCount1 = fileSystem.Directory.GetFiles(directory, "*").Length;
        fileSystem.File.Delete(path);
        var fileCount2 = fileSystem.Directory.GetFiles(directory, "*").Length;

        await That(fileCount1).IsEqualTo(1).Because("File should have existed");
        await That(fileCount2).IsEqualTo(0).Because("File should have been deleted");
    }

    [TestCase(" ")]
    [TestCase("   ")]
    public async Task MockFile_Delete_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.Delete(path);

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockFile_Delete_ShouldThrowDirectoryNotFoundExceptionIfParentFolderAbsent()
    {
        var fileSystem = new MockFileSystem();
        var path = XFS.Path("C:\\test\\somefile.txt");

        await That(() => fileSystem.File.Delete(path)).Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task MockFile_Delete_ShouldSilentlyReturnIfNonExistingFileInExistingFolder()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { XFS.Path("C:\\temp\\exist.txt"), new MockFileData("foobar") },
        });

        string filePath = XFS.Path("C:\\temp\\somefile.txt");

        // Delete() returns void, so there is nothing to check here beside absense of an exception
        await That(() => fileSystem.File.Delete(filePath)).DoesNotThrow();
    }
}