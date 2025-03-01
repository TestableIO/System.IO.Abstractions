using System.Collections.Generic;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests;

[TestFixture]
public class MockFileSetAttributesTests
{
    [Test]
    public async Task MockFile_SetAttributes_ShouldSetAttributesOnFile()
    {
        var path = XFS.Path(@"c:\something\demo.txt");
        var filedata = new MockFileData("test");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {path, filedata},
        });

        fileSystem.File.SetAttributes(path, FileAttributes.Hidden);

        var attributes = fileSystem.File.GetAttributes(path);
        await That(attributes).IsEqualTo(FileAttributes.Hidden);
    }

    [Test]
    public async Task MockFile_SetAttributes_ShouldSetAttributesOnDirectory()
    {
        var fileSystem = new MockFileSystem();
        var path = XFS.Path(@"c:\something");
        fileSystem.AddDirectory(path);
        var directoryInfo = fileSystem.DirectoryInfo.New(path);
        directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Normal;

        fileSystem.File.SetAttributes(path, FileAttributes.Directory | FileAttributes.Hidden);

        var attributes = fileSystem.File.GetAttributes(path);
        await That(attributes).IsEqualTo(FileAttributes.Directory | FileAttributes.Hidden);
    }

    [Test]
    [TestCase("", FileAttributes.Normal)]
    [TestCase("   ", FileAttributes.Normal)]
    public async Task MockFile_SetAttributes_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path, FileAttributes attributes)
    {
        var fileSystem = new MockFileSystem();

        Action action = () => fileSystem.File.SetAttributes(path, attributes);

        await That(action).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockFile_SetAttributes_ShouldThrowFileNotFoundExceptionIfMissingDirectory()
    {
        var path = XFS.Path(@"C:\something");
        var attributes = FileAttributes.Normal;
        var fileSystem = new MockFileSystem();

        Action action = () => fileSystem.File.SetAttributes(path, attributes);

        var exception = await That(action).Throws<FileNotFoundException>();
        await That(exception.FileName).IsEqualTo(path);
    }
}