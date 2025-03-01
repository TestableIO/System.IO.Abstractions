using System.Collections.Generic;
using NUnit.Framework;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers.Tests;

using XFS = MockUnixSupport;

[TestFixture]
public class MockFileStreamFactoryTests
{
    [Test]
    [TestCase(FileMode.Create)]
    [TestCase(FileMode.Append)]
    public async Task MockFileStreamFactory_CreateForExistingFile_ShouldReturnStream(FileMode fileMode)
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"c:\existing.txt", string.Empty }
        });

        var fileStreamFactory = new MockFileStreamFactory(fileSystem);

        // Act
        var result = fileStreamFactory.New(@"c:\existing.txt", fileMode, FileAccess.Write);

        // Assert
        await That(result).IsNotNull();
    }

    [Test]
    [TestCase(FileMode.Create)]
    [TestCase(FileMode.Append)]
    public async Task MockFileStreamFactory_CreateForNonExistingFile_ShouldReturnStream(FileMode fileMode)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var fileStreamFactory = new MockFileStreamFactory(fileSystem);

        // Act
        var result = fileStreamFactory.New(XFS.Path(@"c:\not_existing.txt"), fileMode, FileAccess.Write);

        // Assert
        await That(result).IsNotNull();
    }

    [Test]
    [TestCase(FileMode.Create)]
    public async Task MockFileStreamFactory_CreateForAnExistingFile_ShouldReplaceFileContents(FileMode fileMode)
    {
        var fileSystem = new MockFileSystem();
        string FilePath = XFS.Path("C:\\File.txt");

        using (var stream = fileSystem.FileStream.New(FilePath, fileMode, System.IO.FileAccess.Write))
        {
            var data = Encoding.UTF8.GetBytes("1234567890");
            stream.Write(data, 0, data.Length);
        }

        using (var stream = fileSystem.FileStream.New(FilePath, fileMode, System.IO.FileAccess.Write))
        {
            var data = Encoding.UTF8.GetBytes("AAAAA");
            stream.Write(data, 0, data.Length);
        }

        var text = fileSystem.File.ReadAllText(FilePath);
        await That(text).IsEqualTo("AAAAA");
    }

    [Test]
    [TestCase(FileMode.Create)]
    [TestCase(FileMode.Open)]
    [TestCase(FileMode.CreateNew)]
    public async Task MockFileStreamFactory_CreateInNonExistingDirectory_ShouldThrowDirectoryNotFoundException(FileMode fileMode)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\Test"));

        // Act
        var fileStreamFactory = new MockFileStreamFactory(fileSystem);

        // Assert
        await That(() => fileStreamFactory.New(XFS.Path(@"C:\Test\NonExistingDirectory\some_random_file.txt"), fileMode)).Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task MockFileStreamFactory_AppendAccessWithReadWriteMode_ShouldThrowArgumentException()
    {
        var fileSystem = new MockFileSystem();
            
        await That(() =>
        {
            fileSystem.FileStream.New(XFS.Path(@"c:\path.txt"), FileMode.Append, FileAccess.ReadWrite);
        }).Throws<ArgumentException>();
    }

    [Test]
    [TestCase(FileMode.Append)]
    [TestCase(FileMode.Truncate)]
    [TestCase(FileMode.Create)]
    [TestCase(FileMode.CreateNew)]
    [TestCase(FileMode.Append)]
    public async Task MockFileStreamFactory_InvalidModeForReadAccess_ShouldThrowArgumentException(FileMode fileMode)
    {
        var fileSystem = new MockFileSystem();

        await That(() =>
        {
            fileSystem.FileStream.New(XFS.Path(@"c:\path.txt"), fileMode, FileAccess.Read);
        }).Throws<ArgumentException>();
    }

    [Test]
    [TestCase(FileMode.Open)]
    [TestCase(FileMode.Truncate)]
    public async Task MockFileStreamFactory_OpenNonExistingFile_ShouldThrowFileNotFoundException(FileMode fileMode)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\Test"));

        // Act
        var fileStreamFactory = new MockFileStreamFactory(fileSystem);

        // Assert
        await That(() => fileStreamFactory.New(XFS.Path(@"C:\Test\some_random_file.txt"), fileMode)).Throws<FileNotFoundException>();
    }

    [Test]
    [TestCase(FileMode.CreateNew)]
    public async Task MockFileStreamFactory_CreateExistingFile_Should_Throw_IOException(FileMode fileMode)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path = XFS.Path(@"C:\Test\some_random_file.txt");
        fileSystem.AddFile(path, string.Empty);

        // Act
        var fileStreamFactory = new MockFileStreamFactory(fileSystem);

        // Assert
        await That(() => fileStreamFactory.New(path, fileMode)).Throws<IOException>();

    }

    [Test]
    [TestCase(FileMode.Create)]
    [TestCase(FileMode.CreateNew)]
    [TestCase(FileMode.OpenOrCreate)]
    public async Task MockFileStreamFactory_CreateWithRelativePath_CreatesFileInCurrentDirectory(FileMode fileMode)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var fileStreamFactory = new MockFileStreamFactory(fileSystem);
        fileStreamFactory.New("some_random_file.txt", fileMode);

        // Assert
        await That(fileSystem.File.Exists(XFS.Path("./some_random_file.txt"))).IsTrue();
    }

    [Test]
    public async Task MockFileStream_CanRead_ReturnsFalseForAWriteOnlyStream()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var fileStream = fileSystem.FileStream.New("file.txt", FileMode.CreateNew, FileAccess.Write);

        // Assert
        await That(fileStream.CanRead).IsFalse();
    }
}