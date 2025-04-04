using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests;

using XFS = MockUnixSupport;

[TestFixture]
public class MockFileTests
{
    [Test]
    public async Task MockFile_Constructor_ShouldThrowArgumentNullExceptionIfMockFileDataAccessorIsNull()
    {
        // Arrange
        // nothing to do

        // Act
        Action action = () => new MockFile(null);

        // Assert
        await That(action).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task MockFile_GetSetCreationTime_ShouldPersist()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });
        var file = new MockFile(fileSystem);

        // Act
        var creationTime = new DateTime(2010, 6, 4, 13, 26, 42);
        file.SetCreationTime(path, creationTime);
        var result = file.GetCreationTime(path);

        // Assert
        await That(result).IsEqualTo(creationTime);
    }

    [Test]
    public async Task MockFile_SetCreationTimeUtc_ShouldAffectCreationTime()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });
        var file = new MockFile(fileSystem);

        // Act
        var creationTime = new DateTime(2010, 6, 4, 13, 26, 42);
        file.SetCreationTimeUtc(path, creationTime.ToUniversalTime());
        var result = file.GetCreationTime(path);

        // Assert
        await That(result).IsEqualTo(creationTime);
    }

    [Test]
    public async Task MockFile_SetCreationTime_ShouldAffectCreationTimeUtc()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });
        var file = new MockFile(fileSystem);

        // Act
        var creationTime = new DateTime(2010, 6, 4, 13, 26, 42);
        file.SetCreationTime(path, creationTime);
        var result = file.GetCreationTimeUtc(path);

        // Assert
        await That(result).IsEqualTo(creationTime.ToUniversalTime());
    }

    [Test]
    public async Task MockFile_GetSetLastAccessTime_ShouldPersist()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });
        var file = new MockFile(fileSystem);

        // Act
        var lastAccessTime = new DateTime(2010, 6, 4, 13, 26, 42);
        file.SetLastAccessTime(path, lastAccessTime);
        var result = file.GetLastAccessTime(path);

        // Assert
        await That(result).IsEqualTo(lastAccessTime);
    }

    [Test]
    public async Task MockFile_SetLastAccessTimeUtc_ShouldAffectLastAccessTime()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });
        var file = new MockFile(fileSystem);

        // Act
        var lastAccessTime = new DateTime(2010, 6, 4, 13, 26, 42);
        file.SetLastAccessTimeUtc(path, lastAccessTime.ToUniversalTime());
        var result = file.GetLastAccessTime(path);

        // Assert
        await That(result).IsEqualTo(lastAccessTime);
    }

    [Test]
    public async Task MockFile_SetLastAccessTime_ShouldAffectLastAccessTimeUtc()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });
        var file = new MockFile(fileSystem);

        // Act
        var lastAccessTime = new DateTime(2010, 6, 4, 13, 26, 42);
        file.SetLastAccessTime(path, lastAccessTime);
        var result = file.GetLastAccessTimeUtc(path);

        // Assert
        await That(result).IsEqualTo(lastAccessTime.ToUniversalTime());
    }

    [Test]
    public async Task MockFile_GetSetLastWriteTime_ShouldPersist()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });
        var file = new MockFile(fileSystem);

        // Act
        var lastWriteTime = new DateTime(2010, 6, 4, 13, 26, 42);
        file.SetLastWriteTime(path, lastWriteTime);
        var result = file.GetLastWriteTime(path);

        // Assert
        await That(result).IsEqualTo(lastWriteTime);
    }

    static async Task ExecuteDefaultValueTest(Func<MockFile, string, DateTime> getDateValue)
    {
        var expected = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();
        var file = new MockFile(fileSystem);

        var actual = getDateValue(file, path);

        await That(actual.ToUniversalTime()).IsEqualTo(expected);
    }

    [Test]
    public async Task MockFile_GetLastWriteTimeOfNonExistentFile_ShouldReturnDefaultValue()
    {
        await ExecuteDefaultValueTest((f, p) => f.GetLastWriteTime(p));
    }

    [Test]
    public async Task MockFile_GetLastWriteTimeUtcOfNonExistentFile_ShouldReturnDefaultValue()
    {
        await ExecuteDefaultValueTest((f, p) => f.GetLastWriteTimeUtc(p));
    }

    [Test]
    public async Task MockFile_GetLastAccessTimeUtcOfNonExistentFile_ShouldReturnDefaultValue()
    {
        await ExecuteDefaultValueTest((f, p) => f.GetLastAccessTimeUtc(p));
    }

    [Test]
    public async Task MockFile_GetLastAccessTimeOfNonExistentFile_ShouldReturnDefaultValue()
    {
        await ExecuteDefaultValueTest((f, p) => f.GetLastAccessTime(p));
    }

    [Test]
    public async Task MockFile_GetAttributeOfNonExistentFileButParentDirectoryExists_ShouldThrowOneFileNotFoundException()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"c:\something"));

        // Act
        Action action = () => fileSystem.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));

        // Assert
        await That(action).Throws<FileNotFoundException>();
    }

    [Test]
    public async Task MockFile_GetAttributeOfNonExistentFile_ShouldThrowOneDirectoryNotFoundException()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));

        // Assert
        await That(action).Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task MockFile_GetAttributeOfExistingFile_ShouldReturnCorrectValue()
    {
        var filedata = new MockFileData("test")
        {
            Attributes = FileAttributes.Hidden
        };
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\something\demo.txt"),  filedata }
        });

        var attributes = fileSystem.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));
        await That(attributes).IsEqualTo(FileAttributes.Hidden);
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.UNCPaths)]
    public async Task MockFile_GetAttributeOfExistingUncDirectory_ShouldReturnCorrectValue()
    {
        var filedata = new MockFileData("test");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"\\share\folder\demo.txt"), filedata }
        });

        var attributes = fileSystem.File.GetAttributes(XFS.Path(@"\\share\folder"));
        await That(attributes).IsEqualTo(FileAttributes.Directory);
    }

    [Test]
    public async Task MockFile_GetAttributeWithEmptyParameter_ShouldThrowOneArgumentException()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.GetAttributes(string.Empty);

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.Message).StartsWith("The path is not of a legal form.");
    }

    [Test]
    public async Task MockFile_GetAttributeWithIllegalParameter_ShouldThrowOneArgumentException()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.GetAttributes(string.Empty);

        // Assert
        // Note: The actual type of the exception differs from the documentation.
        //       According to the documentation it should be of type NotSupportedException.
        await That(action).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockFile_GetCreationTimeOfNonExistentFile_ShouldReturnDefaultValue()
    {
        await ExecuteDefaultValueTest((f, p) => f.GetCreationTime(p));
    }

    [Test]
    public async Task MockFile_GetCreationTimeUtcOfNonExistentFile_ShouldReturnDefaultValue()
    {
        await ExecuteDefaultValueTest((f, p) => f.GetCreationTimeUtc(p));
    }

    [Test]
    public async Task MockFile_SetLastWriteTimeUtc_ShouldAffectLastWriteTime()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });
        var file = new MockFile(fileSystem);

        // Act
        var lastWriteTime = new DateTime(2010, 6, 4, 13, 26, 42);
        file.SetLastWriteTimeUtc(path, lastWriteTime.ToUniversalTime());
        var result = file.GetLastWriteTime(path);

        // Assert
        await That(result).IsEqualTo(lastWriteTime);
    }

    [Test]
    public async Task MockFile_SetLastWriteTime_ShouldAffectLastWriteTimeUtc()
    {
        // Arrange
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });
        var file = new MockFile(fileSystem);

        // Act
        var lastWriteTime = new DateTime(2010, 6, 4, 13, 26, 42);
        file.SetLastWriteTime(path, lastWriteTime);
        var result = file.GetLastWriteTimeUtc(path);

        // Assert
        await That(result).IsEqualTo(lastWriteTime.ToUniversalTime());
    }

    [Test]
    public async Task MockFile_ReadAllText_ShouldReturnOriginalTextData()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
        });

        var file = new MockFile(fileSystem);

        // Act
        var result = file.ReadAllText(XFS.Path(@"c:\something\demo.txt"));

        // Assert
        await That(result).IsEqualTo("Demo text content");
    }

    [Test]
    public async Task MockFile_ReadAllText_ShouldReturnOriginalDataWithCustomEncoding()
    {
        // Arrange
        string text = "Hello there!";
        var encodedText = Encoding.BigEndianUnicode.GetBytes(text);
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\something\demo.txt"), new MockFileData(encodedText) }
        });

        var file = new MockFile(fileSystem);

        // Act
        var result = file.ReadAllText(XFS.Path(@"c:\something\demo.txt"), Encoding.BigEndianUnicode);

        // Assert
        await That(result).IsEqualTo(text);
    }

    public static IEnumerable<Encoding> GetEncodingsForReadAllText()
    {
        // little endian
        yield return new UTF32Encoding(false, true, true);

        // big endian
        yield return new UTF32Encoding(true, true, true);
        yield return new UTF8Encoding(true, true);

        yield return new ASCIIEncoding();
    }

    [TestCaseSource(typeof(MockFileTests), nameof(GetEncodingsForReadAllText))]
    public async Task MockFile_ReadAllText_ShouldReturnTheOriginalContentWhenTheFileContainsDifferentEncodings(Encoding encoding)
    {
        // Arrange
        string text = "Hello there!";
        var encodedText = encoding.GetPreamble().Concat(encoding.GetBytes(text)).ToArray();
        var path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData(encodedText) }
        });

        // Act
        var actualText = fileSystem.File.ReadAllText(path);

        // Assert
        await That(actualText).IsEqualTo(text);
    }

    [Test]
    public async Task MockFile_OpenWrite_ShouldCreateNewFiles()
    {
        string filePath = XFS.Path(@"c:\something\demo.txt");
        string fileContent = "this is some content";
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"c:\something"));

        var bytes = new UTF8Encoding(true).GetBytes(fileContent);
        var stream = fileSystem.File.OpenWrite(filePath);
        stream.Write(bytes, 0, bytes.Length);
        stream.Dispose();

        await That(fileSystem.FileExists(filePath)).IsTrue();
        await That(fileSystem.GetFile(filePath).TextContents).IsEqualTo(fileContent);
    }

    [Test]
    public async Task MockFile_OpenWrite_ShouldNotCreateFolders()
    {
        string filePath = XFS.Path(@"c:\something\demo.txt"); // c:\something does not exist: OpenWrite should fail
        var fileSystem = new MockFileSystem();

        await That(() => fileSystem.File.OpenWrite(filePath)).Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task MockFile_OpenWrite_ShouldOverwriteExistingFiles()
    {
        string filePath = XFS.Path(@"c:\something\demo.txt");
        string startFileContent = "this is some content";
        string endFileContent = "this is some other content";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {filePath, new MockFileData(startFileContent)}
        });

        var bytes = new UTF8Encoding(true).GetBytes(endFileContent);
        var stream = fileSystem.File.OpenWrite(filePath);
        stream.Write(bytes, 0, bytes.Length);
        stream.Dispose();

        await That(fileSystem.FileExists(filePath)).IsTrue();
        await That(fileSystem.GetFile(filePath).TextContents).IsEqualTo(endFileContent);
    }

    [Test]
    public async Task MockFile_Delete_ShouldRemoveFileFromFileSystem()
    {
        string fullPath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { fullPath, new MockFileData("Demo text content") }
        });

        var file = new MockFile(fileSystem);

        file.Delete(fullPath);

        await That(fileSystem.FileExists(fullPath)).IsFalse();
    }

    [Test]
    public async Task MockFile_Delete_Should_RemoveFiles()
    {
        string filePath = XFS.Path(@"c:\something\demo.txt");
        string fileContent = "this is some content";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { { filePath, new MockFileData(fileContent) } });
        await That(fileSystem.AllFiles.Count()).IsEqualTo(1);
        fileSystem.File.Delete(filePath);
        await That(fileSystem.AllFiles.Count()).IsEqualTo(0);
    }

    [Test]
    public async Task MockFile_Delete_No_File_Does_Nothing()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { XFS.Path(@"c:\something\exist.txt"), new MockFileData("Demo text content") },
        });

        string filePath = XFS.Path(@"c:\something\not_exist.txt");

        await That(() => fileSystem.File.Delete(filePath)).DoesNotThrow();
    }

    [Test]
    public async Task MockFile_Delete_ShouldThrowUnauthorizedAccessException_WhenPathIsADirectory()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\bar"), new MockDirectoryData() },
        });

        // Act
        Action action = () => fileSystem.File.Delete(XFS.Path(@"c:\bar"));

        // Assert
        await That(action).Throws<UnauthorizedAccessException>();
    }

    [Test]
    public async Task MockFile_AppendText_AppendTextToAnExistingFile()
    {
        string filepath = XFS.Path(@"c:\something\does\exist.txt");
        var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filepath, new MockFileData("I'm here. ") }
        });

        var stream = filesystem.File.AppendText(filepath);

        stream.Write("Me too!");
        stream.Flush();
        stream.Dispose();

        var file = filesystem.GetFile(filepath);
        await That(file.TextContents).IsEqualTo("I'm here. Me too!");
    }

    [Test]
    public async Task MockFile_AppendText_CreatesNewFileForAppendToNonExistingFile()
    {
        string filepath = XFS.Path(@"c:\something\doesnt\exist.txt");
        var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        filesystem.AddDirectory(XFS.Path(@"c:\something\doesnt"));

        var stream = filesystem.File.AppendText(filepath);

        stream.Write("New too!");
        stream.Flush();
        stream.Dispose();

        var file = filesystem.GetFile(filepath);
        await That(file.TextContents).IsEqualTo("New too!");
        await That(filesystem.FileExists(filepath)).IsTrue();
    }

#if !NET9_0_OR_GREATER
    [Test]
    public void Serializable_works()
    {
        //Arrange
        MockFileData data = new MockFileData("Text Contents");

        //Act
#pragma warning disable SYSLIB0011
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();

        formatter.Serialize(stream, data);
#pragma warning restore SYSLIB0011

        //Assert
        Assert.Pass();
    }
#endif

#if !NET9_0_OR_GREATER
    [Test]
    public async Task Serializable_can_deserialize()
    {
        //Arrange
        string textContentStr = "Text Contents";

        //Act
        MockFileData data = new MockFileData(textContentStr);

#pragma warning disable SYSLIB0011
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();
        formatter.Serialize(stream, data);

        stream.Seek(0, SeekOrigin.Begin);

        MockFileData deserialized = (MockFileData)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011

        //Assert
        await That(deserialized.TextContents).IsEqualTo(textContentStr);
    }
#endif

    [Test]
    public async Task MockFile_Encrypt_ShouldSetEncryptedAttribute()
    {
        // Arrange
        var fileData = new MockFileData("Demo text content");
        var filePath = XFS.Path(@"c:\a.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {filePath, fileData }
        });

        // Act
#pragma warning disable CA1416
        fileSystem.File.Encrypt(filePath);
#pragma warning restore CA1416
        var attributes = fileSystem.File.GetAttributes(filePath);

        // Assert
        await That(attributes & FileAttributes.Encrypted).IsEqualTo(FileAttributes.Encrypted);
    }

    [Test]
    public async Task MockFile_Decrypt_ShouldRemoveEncryptedAttribute()
    {
        // Arrange
        const string Content = "Demo text content";
        var fileData = new MockFileData(Content);
        var filePath = XFS.Path(@"c:\a.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {filePath, fileData }
        });
#pragma warning disable CA1416
        fileSystem.File.Encrypt(filePath);
#pragma warning restore CA1416

        // Act
#pragma warning disable CA1416
        fileSystem.File.Decrypt(filePath);
#pragma warning restore CA1416
        var attributes = fileSystem.File.GetAttributes(filePath);

        // Assert
        await That(attributes & FileAttributes.Encrypted).IsNotEqualTo(FileAttributes.Encrypted);
    }

    [Test]
    public async Task MockFile_Replace_ShouldReplaceFileContents()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path1 = XFS.Path(@"c:\temp\file1.txt");
        var path2 = XFS.Path(@"c:\temp\file2.txt");
        fileSystem.AddFile(path1, new MockFileData("1"));
        fileSystem.AddFile(path2, new MockFileData("2"));

        // Act
        fileSystem.File.Replace(path1, path2, null);

        await That(fileSystem.File.ReadAllText(path2)).IsEqualTo("1");
    }

    [Test]
    public async Task MockFile_Replace_ShouldCreateBackup()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path1 = XFS.Path(@"c:\temp\file1.txt");
        var path2 = XFS.Path(@"c:\temp\file2.txt");
        var path3 = XFS.Path(@"c:\temp\file3.txt");
        fileSystem.AddFile(path1, new MockFileData("1"));
        fileSystem.AddFile(path2, new MockFileData("2"));

        // Act
        fileSystem.File.Replace(path1, path2, path3);

        await That(fileSystem.File.ReadAllText(path3)).IsEqualTo("2");
    }

    [Test]
    public async Task MockFile_Replace_ShouldThrowIfDirectoryOfBackupPathDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path1 = XFS.Path(@"c:\temp\file1.txt");
        var path2 = XFS.Path(@"c:\temp\file2.txt");
        var path3 = XFS.Path(@"c:\temp\subdirectory\file3.txt");
        fileSystem.AddFile(path1, new MockFileData("1"));
        fileSystem.AddFile(path2, new MockFileData("2"));

        // Act
        await That(() => fileSystem.File.Replace(path1, path2, path3)).Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task MockFile_Replace_ShouldThrowIfSourceFileDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path1 = XFS.Path(@"c:\temp\file1.txt");
        var path2 = XFS.Path(@"c:\temp\file2.txt");
        fileSystem.AddFile(path2, new MockFileData("2"));

        await That(() => fileSystem.File.Replace(path1, path2, null)).Throws<FileNotFoundException>();
    }

    [Test]
    public async Task MockFile_Replace_ShouldThrowIfDestinationFileDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path1 = XFS.Path(@"c:\temp\file1.txt");
        var path2 = XFS.Path(@"c:\temp\file2.txt");
        fileSystem.AddFile(path1, new MockFileData("1"));

        await That(() => fileSystem.File.Replace(path1, path2, null)).Throws<FileNotFoundException>();
    }

    [Test]
    public async Task MockFile_OpenRead_ShouldReturnReadOnlyStream()
    {
        // Tests issue #230
        // Arrange
        string filePath = XFS.Path(@"c:\something\demo.txt");
        string startContent = "hi there";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filePath, new MockFileData(startContent) }
        });

        // Act
        var stream = fileSystem.File.OpenRead(filePath);

        // Assert
        await That(stream.CanWrite).IsFalse();
        await That(() => stream.WriteByte(0)).Throws<NotSupportedException>();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockFile_Replace_SourceAndDestinationDifferOnlyInCasing_ShouldThrowIOException()
    {
        var fileSystem = new MockFileSystem();
        string sourceFilePath = @"c:\temp\demo.txt";
        string destFilePath = @"c:\temp\DEMO.txt";
        string fileContent = "content";
        fileSystem.File.WriteAllText(sourceFilePath, fileContent);

        void Act() => fileSystem.File.Replace(sourceFilePath, destFilePath, null, true);

        await That(Act).Throws<IOException>()
            .HasMessage("The process cannot access the file because it is being used by another process.");
    }
}