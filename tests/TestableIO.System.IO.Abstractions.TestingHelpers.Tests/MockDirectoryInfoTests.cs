using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests;

using XFS = MockUnixSupport;

[TestFixture]
public class MockDirectoryInfoTests
{
    public static IEnumerable<object[]> MockDirectoryInfo_GetExtension_Cases
    {
        get
        {
            yield return new object[] { XFS.Path(@"c:\temp") };
            yield return new object[] { XFS.Path(@"c:\temp\") };
        }
    }

    [TestCaseSource(nameof(MockDirectoryInfo_GetExtension_Cases))]
    public async Task MockDirectoryInfo_GetExtension_ShouldReturnEmptyString(string directoryPath)
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

        // Act
        var result = directoryInfo.Extension;

        // Assert
        await That(result).IsEmpty();
    }

    public static IEnumerable<object[]> MockDirectoryInfo_Exists_Cases
    {
        get
        {
            yield return new object[] { XFS.Path(@"c:\temp\folder"), true };
            yield return new object[] { XFS.Path(@"c:\temp\folder\notExistant"), false };
        }
    }

    [TestCaseSource(nameof(MockDirectoryInfo_Exists_Cases))]
    public async Task MockDirectoryInfo_Exists(string path, bool expected)
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("Hello World")}
        });
        var directoryInfo = new MockDirectoryInfo(fileSystem, path);

        var result = directoryInfo.Exists;

        await That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task MockDirectoryInfo_Attributes_ShouldReturnMinusOneForNonExistingFile()
    {
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));
        FileAttributes expected = (FileAttributes)(-1);

        await That(directoryInfo.Attributes).IsEqualTo(expected);
    }

    [Test]
    public async Task MockDirectoryInfo_Attributes_Clear_ShouldRemainDirectory()
    {
        var fileSystem = new MockFileSystem();
        var path = XFS.Path(@"c:\existing\directory");
        fileSystem.Directory.CreateDirectory(path);
        var directoryInfo = fileSystem.DirectoryInfo.New(path);
        directoryInfo.Attributes = 0;

        await That(fileSystem.File.Exists(path)).IsFalse();
        await That(directoryInfo.Attributes).IsEqualTo(FileAttributes.Directory);
    }

    [Test]
    public async Task MockDirectoryInfo_Attributes_SetterShouldThrowDirectoryNotFoundExceptionOnNonExistingFileOrDirectory()
    {
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        await That(() => directoryInfo.Attributes = FileAttributes.Hidden).Throws<DirectoryNotFoundException>();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.UNCPaths)]
    public async Task MockDirectoryInfo_GetFiles_ShouldWorkWithUNCPath()
    {
        var fileName = XFS.Path(@"\\unc\folder\file.txt");
        var directoryName = XFS.Path(@"\\unc\folder");
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {fileName, ""}
        });

        var directoryInfo = new MockDirectoryInfo(fileSystem, directoryName);

        // Act
        var files = directoryInfo.GetFiles();

        // Assert
        await That(files[0].FullName).IsEqualTo(fileName);
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.UNCPaths)]
    public async Task MockDirectoryInfo_GetFiles_ShouldWorkWithUNCPath_WhenCurrentDirectoryIsUnc()
    {
        var fileName = XFS.Path(@"\\unc\folder\file.txt");
        var directoryName = XFS.Path(@"\\unc\folder");
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {fileName, ""}
        });
            
        fileSystem.Directory.SetCurrentDirectory(directoryName);

        var directoryInfo = new MockDirectoryInfo(fileSystem, directoryName);
            
        // Act
        var files = directoryInfo.GetFiles();

        // Assert
        await That(files[0].FullName).IsEqualTo(fileName);
    }

    [Test]
    public async Task MockDirectoryInfo_FullName_ShouldReturnFullNameWithoutIncludingTrailingPathDelimiter()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {
                XFS.Path(@"c:\temp\folder\file.txt"),
                new MockFileData("Hello World")
            }
        });
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

        var result = directoryInfo.FullName;

        await That(result).IsEqualTo(XFS.Path(@"c:\temp\folder"));
    }

    [Test]
    public async Task MockDirectoryInfo_GetFileSystemInfos_ShouldReturnBothDirectoriesAndFiles()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("Hello World") },
            { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() }
        });

        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));
        var result = directoryInfo.GetFileSystemInfos();

        await That(result.Length).IsEqualTo(2);
    }

    [Test]
    public async Task MockDirectoryInfo_EnumerateFileSystemInfos_ShouldReturnBothDirectoriesAndFiles()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("Hello World") },
            { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() }
        });

        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));
        var result = directoryInfo.EnumerateFileSystemInfos().ToArray();

        await That(result.Length).IsEqualTo(2);
    }

    [Test]
    public async Task MockDirectoryInfo_GetFileSystemInfos_ShouldReturnDirectoriesAndNamesWithSearchPattern()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("Hello World") },
            { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            { XFS.Path(@"c:\temp\folder\older"), new MockDirectoryData() }
        });

        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));
        var result = directoryInfo.GetFileSystemInfos("f*");

        await That(result.Length).IsEqualTo(2);
    }

    [Test]
    public async Task MockDirectoryInfo_EnumerateFileSystemInfos_ShouldReturnDirectoriesAndNamesWithSearchPattern()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("Hello World") },
            { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            { XFS.Path(@"c:\temp\folder\older"), new MockDirectoryData() }
        });

        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));
        var result = directoryInfo.EnumerateFileSystemInfos("f*", SearchOption.AllDirectories).ToArray();

        await That(result.Length).IsEqualTo(2);
    }

    [Test]
    public async Task MockDirectoryInfo_EnumerateFileSystemInfos_ShouldReturnDirectoriesAndNamesWithSearchPatternRecursive()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("") },
            { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            { XFS.Path(@"c:\temp\folder\older"), new MockDirectoryData() }
        });

        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\"));
        var result = directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).ToArray();

        await That(result.Length).IsEqualTo(5);
    }

#if FEATURE_ENUMERATION_OPTIONS
        [Test]
        public async Task MockDirectoryInfo_EnumerateFileSystemInfos_ShouldReturnDirectoriesAndNamesWithSearchPatternRecursiveEnumerateOptions()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("") },
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
                { XFS.Path(@"c:\temp\folder\older"), new MockDirectoryData() }
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\"));

            var enumerationOptions = new EnumerationOptions()
            {
                RecurseSubdirectories = true,
            };

            var result = directoryInfo.EnumerateFileSystemInfos("*", enumerationOptions).ToArray();

            await That(result.Length).IsEqualTo(5);
        }
#endif

    [Test]
    public async Task MockDirectoryInfo_GetParent_ShouldReturnDirectoriesAndNamesWithSearchPattern()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"c:\a\b\c"));
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\a\b\c"));

        // Act
        var result = directoryInfo.Parent;

        // Assert
        await That(result.FullName).IsEqualTo(XFS.Path(@"c:\a\b"));
    }

    [Test]
    public async Task MockDirectoryInfo_EnumerateFiles_ShouldReturnAllFiles()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            //Files "above" in folder we're querying
            { XFS.Path(@"c:\temp\a.txt"), "" },

            //Files in the folder we're querying
            { XFS.Path(@"c:\temp\folder\b.txt"), "" },
            { XFS.Path(@"c:\temp\folder\c.txt"), "" },

            //Files "below" the folder we're querying
            { XFS.Path(@"c:\temp\folder\deeper\d.txt"), "" }
        });

        // Act
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

        // Assert
        await That(directoryInfo.EnumerateFiles().ToList().Select(x => x.Name).ToArray()).IsEqualTo(new[] { "b.txt", "c.txt" });
    }

    [Test]
    public async Task MockDirectoryInfo_EnumerateDirectories_ShouldReturnAllDirectories()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            //A file we want to ignore entirely
            { XFS.Path(@"c:\temp\folder\a.txt"), "" },

            //Some files in sub folders (which we also want to ignore entirely)
            { XFS.Path(@"c:\temp\folder\b\file.txt"), "" },
            { XFS.Path(@"c:\temp\folder\c\other.txt"), "" },
        });
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

        // Act
        var directories = directoryInfo.EnumerateDirectories().Select(a => a.Name).ToArray();

        // Assert
        await That(directories).IsEqualTo(new[] { "b", "c" });
    }

    [TestCase(@"\\unc\folder", @"\\unc\folder")]
    [TestCase(@"\\unc/folder\\foo", @"\\unc\folder\foo")]
    [WindowsOnly(WindowsSpecifics.UNCPaths)]
    public async Task MockDirectoryInfo_FullName_ShouldReturnNormalizedUNCPath(string directoryPath, string expectedFullName)
    {
        // Arrange
        directoryPath = XFS.Path(directoryPath);
        expectedFullName = XFS.Path(expectedFullName);
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

        // Act
        var actualFullName = directoryInfo.FullName;

        // Assert
        await That(actualFullName).IsEqualTo(expectedFullName);
    }

    [TestCase(@"c:\temp\\folder", @"c:\temp\folder")]
    [TestCase(@"c:\temp//folder", @"c:\temp\folder")]
    [TestCase(@"c:\temp//\\///folder", @"c:\temp\folder")]
    public async Task MockDirectoryInfo_FullName_ShouldReturnNormalizedPath(string directoryPath, string expectedFullName)
    {
        // Arrange
        directoryPath = XFS.Path(directoryPath);
        expectedFullName = XFS.Path(expectedFullName);
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

        // Act
        var actualFullName = directoryInfo.FullName;

        // Assert
        await That(actualFullName).IsEqualTo(expectedFullName);
    }

    [TestCase(@"c:\temp\folder  ", @"c:\temp\folder")]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockDirectoryInfo_FullName_ShouldReturnPathWithTrimmedTrailingSpaces(string directoryPath, string expectedFullName)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

        // Act
        var actualFullName = directoryInfo.FullName;

        // Assert
        await That(actualFullName).IsEqualTo(expectedFullName);
    }

    [Test]
    public async Task MockDirectoryInfo_MoveTo_ShouldUpdateFullName()
    {
        // Arrange
        var path = XFS.Path(@"c:\source");
        var destination = XFS.Path(@"c:\destination");
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(path);
        var directoryInfo = fileSystem.DirectoryInfo.New(path);

        // Act
        directoryInfo.MoveTo(destination);

        // Assert
        await That(directoryInfo.FullName).IsEqualTo(destination);
    }

    [TestCase(@"c:\temp\\folder ", @"folder")]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockDirectoryInfo_Name_ShouldReturnNameWithTrimmedTrailingSpaces(string directoryPath, string expectedName)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

        // Act
        var actualName = directoryInfo.Name;

        // Assert
        await That(actualName).IsEqualTo(expectedName);
    }

    [TestCase(@"c:\", @"c:\")]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockDirectoryInfo_Name_ShouldReturnPathRoot_IfDirectoryPathIsPathRoot(string directoryPath, string expectedName)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

        // Act
        var actualName = directoryInfo.Name;

        // Assert
        await That(actualName).IsEqualTo(expectedName);
    }

    [Test]
    public async Task MockDirectoryInfo_Constructor_ShouldThrowArgumentNullException_IfArgumentDirectoryIsNull()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => new MockDirectoryInfo(fileSystem, null);

        // Assert
        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.Message).StartsWith("Value cannot be null.");
    }

    [Test]
    public async Task MockDirectoryInfo_Constructor_ShouldThrowArgumentNullException_IfArgumentFileSystemIsNull()
    {
        // Arrange
        // nothing to do

        // Act
        Action action = () => new MockDirectoryInfo(null, XFS.Path(@"c:\foo\bar\folder"));

        // Assert
        await That(action).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task MockDirectoryInfo_Constructor_ShouldThrowArgumentException_IfArgumentDirectoryIsEmpty()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => new MockDirectoryInfo(fileSystem, string.Empty);

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.Message).StartsWith("The path is not of a legal form.");
    }

    [TestCase(@"c:\temp\folder\folder")]
    [TestCase(@"..\..\..\Desktop")]
    public async Task MockDirectoryInfo_ToString_ShouldReturnDirectoryName(string directoryName)
    {
        // Arrange
        var directoryPath = XFS.Path(directoryName);

        // Act
        var mockDirectoryInfo = new MockDirectoryInfo(new MockFileSystem(), directoryPath);

        // Assert
        await That(mockDirectoryInfo.ToString()).IsEqualTo(directoryPath);
    }

    [Test]
    public async Task MockDirectoryInfo_Exists_ShouldReturnCachedData()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path = XFS.Path(@"c:\abc");
        var directoryInfo = fileSystem.DirectoryInfo.New(path);

        // Act
        fileSystem.AddDirectory(path);

        // Assert
        await That(directoryInfo.Exists).IsFalse();
    }

    [Test]
    public async Task MockDirectoryInfo_Exists_ShouldUpdateCachedDataOnRefresh()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var path = XFS.Path(@"c:\abc");
        var directoryInfo = fileSystem.DirectoryInfo.New(path);

        // Act
        fileSystem.AddDirectory(path);
        directoryInfo.Refresh();

        // Assert
        await That(directoryInfo.Exists).IsTrue();
    }

    [Test]
    public async Task Directory_exists_after_creation()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var directoryInfo = fileSystem.DirectoryInfo.New(XFS.Path(@"c:\abc"));

        // Act
        directoryInfo.Create();

        // Assert
        await That(directoryInfo.Exists).IsTrue();
    }

    [Test, WindowsOnly(WindowsSpecifics.AccessControlLists)]
    public async Task Directory_exists_after_creation_with_security()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var directoryInfo = fileSystem.DirectoryInfo.New(XFS.Path(@"c:\abc"));

        // Act
#pragma warning disable CA1416
        directoryInfo.Create(new DirectorySecurity());
#pragma warning restore CA1416

        // Assert
        await That(directoryInfo.Exists).IsTrue();
    }

    [Test]
    public async Task Directory_does_not_exist_after_delete()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var directoryInfo = fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\abc"));

        // Act
        directoryInfo.Delete();

        // Assert
        await That(directoryInfo.Exists).IsFalse();
    }

    [Test]
    public async Task Directory_does_not_exist_after_recursive_delete()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var directoryInfo = fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\abc"));

        // Act
        directoryInfo.Delete(true);

        // Assert
        await That(directoryInfo.Exists).IsFalse();
    }

    [Test]
    public async Task Directory_still_exists_after_move()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var directoryInfo = fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\abc"));

        // Act
        directoryInfo.MoveTo(XFS.Path(@"c:\abc2"));

        // Assert
        await That(directoryInfo.Exists).IsTrue();
    }

    [Test]
    public async Task MockDirectoryInfo_LastAccessTime_ShouldReflectChangedValue()
    {
        // Arrange  
        var path = XFS.Path(@"c:\abc");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockDirectoryData() }
        });
        var directoryInfo = fileSystem.DirectoryInfo.New(path);
        var lastAccessTime = new DateTime(2022, 1, 8);

        // Act
        directoryInfo.LastAccessTime = lastAccessTime;

        // Assert
        await That(directoryInfo.LastAccessTime).IsEqualTo(lastAccessTime);
    }

    [Test]
    public async Task MockDirectoryInfo_CreationTime_ShouldReturnDefaultTimeForNonExistingFile()
    {
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        var result = directoryInfo.CreationTime;

        await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.LocalDateTime);
    }

    [Test]
    public async Task MockDirectoryInfo_LastAccessTime_ShouldReturnDefaultTimeForNonExistingFile()
    {
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        var result = directoryInfo.LastAccessTime;

        await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.LocalDateTime);
    }

    [Test]
    public async Task MockDirectoryInfo_LastWriteTime_ShouldReturnDefaultTimeForNonExistingFile()
    {
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        var result = directoryInfo.LastWriteTime;

        await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.LocalDateTime);
    }

    [Test]
    public async Task MockDirectoryInfo_CreationTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
    {
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        var result = directoryInfo.CreationTimeUtc;

        await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime);
    }

    [Test]
    public async Task MockDirectoryInfo_LastAccessTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
    {
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        var result = directoryInfo.LastAccessTimeUtc;

        await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime);
    }

    [Test]
    public async Task MockDirectoryInfo_LastWriteTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
    {
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        var result = directoryInfo.LastWriteTimeUtc;

        await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime);
    }

    [Test]
    public async Task MockDirectoryInfo_Create_WithConflictingFile_ShouldThrowIOException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content"));
        var sut = fileSystem.DirectoryInfo.New(XFS.Path(@"c:\foo\bar.txt"));

        // Act
        Action action = () => sut.Create();

        // Assert
        await That(action).Throws<IOException>();
    }

    public async Task MockDirectoryInfo_CreationTime_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
    {
        var newTime = new DateTime(2022, 04, 06);
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        await That(() => directoryInfo.CreationTime = newTime).Throws<DirectoryNotFoundException>();
    }

    public async Task MockDirectoryInfo_LastAccessTime_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
    {
        var newTime = new DateTime(2022, 04, 06);
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        await That(() => directoryInfo.LastAccessTime = newTime).Throws<DirectoryNotFoundException>();
    }

    public async Task MockDirectoryInfo_LastWriteTime_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
    {
        var newTime = new DateTime(2022, 04, 06);
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        await That(() => directoryInfo.LastWriteTime = newTime).Throws<DirectoryNotFoundException>();
    }

    public async Task MockDirectoryInfo_CreationTimeUtc_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
    {
        var newTime = new DateTime(2022, 04, 06);
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        await That(() => directoryInfo.CreationTimeUtc = newTime).Throws<DirectoryNotFoundException>();
    }

    public async Task MockDirectoryInfo_LastAccessTimeUtc_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
    {
        var newTime = new DateTime(2022, 04, 06);
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        await That(() => directoryInfo.LastAccessTimeUtc = newTime).Throws<DirectoryNotFoundException>();
    }

    public async Task MockDirectoryInfo_LastWriteTimeUtc_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
    {
        var newTime = new DateTime(2022, 04, 06);
        var fileSystem = new MockFileSystem();
        var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

        await That(() => directoryInfo.LastWriteTime = newTime).Throws<DirectoryNotFoundException>();
    }

}