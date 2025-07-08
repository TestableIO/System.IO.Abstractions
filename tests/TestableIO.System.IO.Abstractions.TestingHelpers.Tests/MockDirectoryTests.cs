using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using aweXpect.Equivalency;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests;

using XFS = MockUnixSupport;

[TestFixture]
public class MockDirectoryTests
{
    [Test]
    public async Task MockDirectory_GetFiles_ShouldReturnAllFilesBelowPathWhenPatternIsEmptyAndSearchOptionIsAllDirectories()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        var expected = new[]
        {
            XFS.Path(@"c:\a\a.txt"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\c.txt"),
            XFS.Path(@"c:\a\d"),
            XFS.Path(@"c:\a\a\a.txt"),
            XFS.Path(@"c:\a\a\b.txt"),
            XFS.Path(@"c:\a\a\c.gif"),
            XFS.Path(@"c:\a\a\d")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldReturnFilesDirectlyBelowPathWhenPatternIsEmptyAndSearchOptionIsTopDirectoryOnly()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        var expected = new[]
        {
            XFS.Path(@"c:\a\a.txt"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\c.txt"),
            XFS.Path(@"c:\a\d")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "", SearchOption.TopDirectoryOnly);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndSearchOptionIsAllDirectories()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        var expected = new[]
        {
            XFS.Path(@"c:\a\a.txt"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\c.txt"),
            XFS.Path(@"c:\a\d"),
            XFS.Path(@"c:\a\a\a.txt"),
            XFS.Path(@"c:\a\a\b.txt"),
            XFS.Path(@"c:\a\a\c.gif"),
            XFS.Path(@"c:\a\a\d")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "*", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

#if FEATURE_ENUMERATION_OPTIONS
    [Test]
    public async Task MockDirectory_GetFiles_ShouldReturnAllPatternMatchingFilesWhenEnumerationOptionHasRecurseSubdirectoriesSetToTrue()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        var expected = new[]
        {
            XFS.Path(@"c:\b.txt"),
            XFS.Path(@"c:\c.txt"),
            XFS.Path(@"c:\a\a.txt"),
            XFS.Path(@"c:\a\c.txt"),
            XFS.Path(@"c:\a\a\a.txt"),
            XFS.Path(@"c:\a\a\b.txt")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.txt", new EnumerationOptions { RecurseSubdirectories = true });

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }
#endif

    private MockFileSystem SetupFileSystem()
    {
        return new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\a.gif"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\b.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\c.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\d"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\b.gif"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\c.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\d"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\a.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\b.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\c.gif"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\d"), new MockFileData("Demo text content") }
        });

    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldReturnFilesDirectlyBelowPathWhenPatternIsWildcardAndSearchOptionIsTopDirectoryOnly()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        var expected = new[]
        {
            XFS.Path(@"c:\a\a.txt"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\c.txt"),
            XFS.Path(@"c:\a\d")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "*", SearchOption.TopDirectoryOnly);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPattern()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        var expected = new[]
        {
            XFS.Path(@"c:\a.gif"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\a\c.gif")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.gif", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternWithThreeCharacterLongFileExtension_RespectingAllDirectorySearchOption()
    {
        // Arrange
        var additionalFilePath = XFS.Path(@"c:\a\a\c.gifx");
        var fileSystem = SetupFileSystem();
        fileSystem.AddFile(additionalFilePath, new MockFileData(string.Empty));
        fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifx.xyz"), new MockFileData(string.Empty));
        fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifz\xyz"), new MockFileData(string.Empty));
        var expected = new[]
        {
            XFS.Path(@"c:\a.gif"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\a\c.gif"),
            additionalFilePath
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.gif", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternWithThreeCharacterLongFileExtension_RespectingTopDirectorySearchOption()
    {
        // Arrange
        var additionalFilePath = XFS.Path(@"c:\a\c.gifx");
        var fileSystem = SetupFileSystem();
        fileSystem.AddFile(additionalFilePath, new MockFileData(string.Empty));
        fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifx.xyz"), new MockFileData(string.Empty));
        fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifx"), new MockFileData(string.Empty));
        var expected = new[]
        {
            XFS.Path(@"c:\a\b.gif"),
            additionalFilePath
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "*.gif", SearchOption.TopDirectoryOnly);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternOnlyIfTheFileExtensionIsThreeCharacterLong()
    {
        // Arrange
        var additionalFilePath = XFS.Path(@"c:\a\c.gi");
        var fileSystem = SetupFileSystem();
        fileSystem.AddFile(additionalFilePath, new MockFileData(string.Empty));
        fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifx.xyz"), new MockFileData(string.Empty));
        fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gif"), new MockFileData(string.Empty));
        fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifx"), new MockFileData(string.Empty));
        var expected = new[]
        {
            additionalFilePath
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "*.gi", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternWithDotsInFilenames()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\a.there.are.dots.in.this.filename.gif"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\b.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\c.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\b.gif"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\c.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\a.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\b.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\c.gif"), new MockFileData("Demo text content") },
        });
        var expected = new[]
        {
            XFS.Path(@"c:\a.there.are.dots.in.this.filename.gif"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\a\c.gif")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.gif", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_FilterShouldFindFilesWithSpecialChars()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\a.1#.pdf"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\b\b #1.txt"), new MockFileData("Demo text content") }
        });
        var expected = new[]
        {
            XFS.Path(@"c:\a.1#.pdf"),
            XFS.Path(@"c:\b\b #1.txt")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.*", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternAndSearchOptionTopDirectoryOnly()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        var expected = new[] { XFS.Path(@"c:\a.gif") };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.gif", SearchOption.TopDirectoryOnly);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterForAllFilesWithNoExtensionsAndSearchOptionTopDirectoryOnly()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        fileSystem.AddFile(XFS.Path(@"C:\mytestfilename"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfilename."), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name."), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again."), new MockFileData("some content"));

        var expected = new[]
        {
            XFS.Path(@"c:\d"),
            XFS.Path(@"C:\mytestfilename"),
            XFS.Path(@"C:\mytestfilename."),
            XFS.Path(@"C:\mytestfile.name."),
            XFS.Path(@"C:\mytestfile.name.again.")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"C:\"), "*.", SearchOption.TopDirectoryOnly);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterForAllFilesWithNoExtensionsAndSearchOptionAllDirectories()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfilename"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfilename."), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfile.name"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfile.name."), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfile.name.again"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfile.name.again."), new MockFileData("some content"));

        var expected = new[]
        {
            XFS.Path(@"c:\d"),
            XFS.Path(@"c:\a\d"),
            XFS.Path(@"c:\a\a\d"),
            XFS.Path(@"C:\specialNameFormats\mytestfilename"),
            XFS.Path(@"C:\specialNameFormats\mytestfilename."),
            XFS.Path(@"C:\specialNameFormats\mytestfile.name."),
            XFS.Path(@"C:\specialNameFormats\mytestfile.name.again.")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterForFilesWithNoExtensionsAndNonTrivialFilterAndSearchOptionTopDirectoryOnly()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        fileSystem.AddFile(XFS.Path(@"C:\mytestfilename"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfilename."), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name."), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again."), new MockFileData("some content"));

        var expected = new[]
        {
            XFS.Path(@"C:\mytestfilename"),
            XFS.Path(@"C:\mytestfilename."),
            XFS.Path(@"C:\mytestfile.name."),
            XFS.Path(@"C:\mytestfile.name.again.")

        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"C:\"), "my??s*.", SearchOption.TopDirectoryOnly);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterForFilesWithNoExtensionsAndNonTrivialFilter2AndSearchOptionTopDirectoryOnly()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        fileSystem.AddFile(XFS.Path(@"C:\mytestfilename"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfilename."), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name."), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again."), new MockFileData("some content"));

        var expected = new[]
        {
            XFS.Path(@"C:\mytestfile.name"),
            XFS.Path(@"C:\mytestfile.name."),
            XFS.Path(@"C:\mytestfile.name.again.")

        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"C:\"), "my*.n*.", SearchOption.TopDirectoryOnly);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFilterForFilesWithNoExtensionsAndFilterThatIncludesDotAndSearchOptionTopDirectoryOnly()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        fileSystem.AddFile(XFS.Path(@"C:\mytestfilename"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfilename."), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name."), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again"), new MockFileData("some content"));
        fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again."), new MockFileData("some content"));

        var expected = new[]
        {
            XFS.Path(@"C:\mytestfile.name"),
            XFS.Path(@"C:\mytestfile.name."),
            XFS.Path(@"C:\mytestfile.name.again.")
        };

        // Act
        var result = fileSystem.Directory.GetFiles(XFS.Path(@"C:\"), "my*.n*.", SearchOption.TopDirectoryOnly);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    private async Task ExecuteTimeAttributeTest(DateTime time, Action<IFileSystem, string, DateTime> setter, Func<IFileSystem, string, DateTime> getter)
    {
        string path = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData("Demo text content") }
        });

        // Act
        setter(fileSystem, path, time);
        var result = getter(fileSystem, path);

        // Assert
        await That(result).IsEqualTo(time);
    }

    [Test]
    public async Task MockDirectory_GetCreationTime_ShouldReturnCreationTimeFromFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42),
            (fs, p, d) => fs.File.SetCreationTime(p, d),
            (fs, p) => fs.Directory.GetCreationTime(p));
    }

    [Test]
    public async Task MockDirectory_GetCreationTimeUtc_ShouldReturnCreationTimeUtcFromFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
            (fs, p, d) => fs.File.SetCreationTimeUtc(p, d),
            (fs, p) => fs.Directory.GetCreationTimeUtc(p));
    }

    [Test]
    public async Task MockDirectory_GetLastAccessTime_ShouldReturnLastAccessTimeFromFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42),
            (fs, p, d) => fs.File.SetLastAccessTime(p, d),
            (fs, p) => fs.Directory.GetLastAccessTime(p));
    }

    [Test]
    public async Task MockDirectory_GetLastAccessTimeUtc_ShouldReturnLastAccessTimeUtcFromFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
            (fs, p, d) => fs.File.SetLastAccessTimeUtc(p, d),
            (fs, p) => fs.Directory.GetLastAccessTimeUtc(p));
    }

    [Test]
    public async Task MockDirectory_GetLastWriteTime_ShouldReturnLastWriteTimeFromFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42),
            (fs, p, d) => fs.File.SetLastWriteTime(p, d),
            (fs, p) => fs.Directory.GetLastWriteTime(p));
    }

    [Test]
    public async Task MockDirectory_GetLastWriteTimeUtc_ShouldReturnLastWriteTimeUtcFromFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
            (fs, p, d) => fs.File.SetLastWriteTimeUtc(p, d),
            (fs, p) => fs.Directory.GetLastWriteTimeUtc(p));
    }

    [Test]
    public async Task MockDirectory_SetCreationTime_ShouldSetCreationTimeOnFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42),
            (fs, p, d) => fs.Directory.SetCreationTime(p, d),
            (fs, p) => fs.File.GetCreationTime(p));
    }

    [Test]
    public async Task MockDirectory_SetCreationTimeUtc_ShouldSetCreationTimeUtcOnFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
            (fs, p, d) => fs.Directory.SetCreationTimeUtc(p, d),
            (fs, p) => fs.File.GetCreationTimeUtc(p));
    }

    [Test]
    public async Task MockDirectory_SetLastAccessTime_ShouldSetLastAccessTimeOnFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42),
            (fs, p, d) => fs.Directory.SetLastAccessTime(p, d),
            (fs, p) => fs.File.GetLastAccessTime(p));
    }

    [Test]
    public async Task MockDirectory_SetLastAccessTimeUtc_ShouldSetLastAccessTimeUtcOnFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
            (fs, p, d) => fs.Directory.SetLastAccessTimeUtc(p, d),
            (fs, p) => fs.File.GetLastAccessTimeUtc(p));
    }

    [Test]
    public async Task MockDirectory_SetLastWriteTime_ShouldSetLastWriteTimeOnFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42),
            (fs, p, d) => fs.Directory.SetLastWriteTime(p, d),
            (fs, p) => fs.File.GetLastWriteTime(p));
    }

    [Test]
    public async Task MockDirectory_SetLastWriteTimeUtc_ShouldSetLastWriteTimeUtcOnFile()
    {
        await ExecuteTimeAttributeTest(
            new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
            (fs, p, d) => fs.Directory.SetLastWriteTimeUtc(p, d),
            (fs, p) => fs.File.GetLastWriteTimeUtc(p));
    }

    [Test]
    public async Task MockDirectory_Exists_ShouldReturnTrueForDirectoryDefinedInMemoryFileSystemWithoutTrailingSlash()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") }
        });

        // Act
        var result = fileSystem.Directory.Exists(XFS.Path(@"c:\foo"));

        // Assert
        await That(result).IsTrue();
    }

    [Test]
    public async Task MockDirectory_Exists_ShouldReturnTrueForDirectoryDefinedInMemoryFileSystemWithTrailingSlash()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") }
        });

        // Act
        var result = fileSystem.Directory.Exists(XFS.Path(@"c:\foo\"));

        // Assert
        await That(result).IsTrue();
    }

    [Test]
    public async Task MockDirectory_Exists_ShouldReturnFalseForDirectoryNotDefinedInMemoryFileSystemWithoutTrailingSlash()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") }
        });

        // Act
        var result = fileSystem.Directory.Exists(XFS.Path(@"c:\baz"));

        // Assert
        await That(result).IsFalse();
    }

    [Test]
    public async Task MockDirectory_Exists_ShouldReturnFalseForDirectoryNotDefinedInMemoryFileSystemWithTrailingSlash()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") }
        });

        // Act
        var result = fileSystem.Directory.Exists(XFS.Path(@"c:\baz\"));

        // Assert
        await That(result).IsFalse();
    }

    [Test]
    public async Task MockDirectory_Exists_ShouldReturnFalseForDirectoryNotDefinedInMemoryFileSystemWithSimilarFileName()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\baz.txt"), new MockFileData("Demo text content") }
        });

        // Act
        var result = fileSystem.Directory.Exists(XFS.Path(@"c:\baz"));

        // Assert
        await That(result).IsFalse();
    }

    [Test]
    public async Task MockDirectory_Exists_ShouldReturnTrueForDirectoryCreatedViaMocks()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") }
        });
        fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\bar"));

        // Act
        var result = fileSystem.Directory.Exists(XFS.Path(@"c:\bar"));

        // Assert
        await That(result).IsTrue();
    }

    [Test]
    public async Task MockDirectory_Exists_ShouldReturnTrueForFolderContainingFileAddedToMockFileSystem()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content"));

        // Act
        var result = fileSystem.Directory.Exists(XFS.Path(@"c:\foo\"));

        // Assert
        await That(result).IsTrue();
    }

    [TestCase(@"\\s")]
    [TestCase(@"<")]
    [TestCase("\t")]
    public async Task MockDirectory_Exists_ShouldReturnFalseForIllegalPath(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var result = fileSystem.Directory.Exists(path);

        // Assert
        await That(result).IsFalse();
    }

    [Test]
    public async Task MockDirectory_CreateDirectory_WithConflictingFile_ShouldThrowIOException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content"));
            
        // Act
        Action action = () => fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\foo\bar.txt"));

        // Assert
        await That(action).Throws<IOException>();
    }

    [Test]
    public async Task MockDirectory_Exists_ShouldReturnFalseForFiles()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content"));

        // Act
        var result = fileSystem.Directory.Exists(XFS.Path(@"c:\foo\bar.txt"));

        // Assert
        await That(result).IsFalse();
    }

    [Test]
    public async Task MockDirectory_CreateDirectory_ShouldCreateFolderInMemoryFileSystem()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo.txt"), new MockFileData("Demo text content") }
        });

        // Act
        fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\bar"));

        // Assert
        await That(fileSystem.FileExists(XFS.Path(@"c:\bar\"))).IsTrue();
        await That(fileSystem.AllDirectories.Any(d => d == XFS.Path(@"c:\bar"))).IsTrue();
    }

    [Test]
    public async Task MockDirectory_CreateDirectory_ShouldThrowIfIllegalCharacterInPath()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo.txt"), new MockFileData("Demo text content") }
        });

        // Act
        Action createDelegate = () => fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\bar_?_"));

        // Assert
        await That(createDelegate).Throws<ArgumentException>();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.UNCPaths)]
    public async Task MockDirectory_CreateDirectory_ShouldSupportExtendedLengthPaths()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var directoryInfo = fileSystem.Directory.CreateDirectory(XFS.Path(@"\\?\c:\bar"));
        fileSystem.File.WriteAllText(@"\\?\c:\bar\grok.txt", "hello world\n");

        // Assert
        await That(fileSystem.Directory.Exists(XFS.Path(@"\\?\c:\bar"))).IsTrue();
        await That(directoryInfo.FullName).IsEqualTo(@"\\?\c:\bar");
        await That(fileSystem.File.ReadAllText(@"\\?\c:\bar\grok.txt")).IsEqualTo("hello world\n");
        await That(fileSystem.Directory.GetFiles(@"\\?\c:\bar")).HasSingle()
            .Which.IsEqualTo(@"\\?\c:\bar\grok.txt");
    }

    // Issue #210
    [Test]
    public async Task MockDirectory_CreateDirectory_ShouldIgnoreExistingDirectoryRegardlessOfTrailingSlash()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo\"), new MockDirectoryData() }
        });

        // Act/Assert
        await That(() => fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\foo"))).DoesNotThrow();
        await That(() => fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\foo\"))).DoesNotThrow();
    }

    [Test]
    public async Task MockDirectory_CreateDirectory_ShouldReturnDirectoryInfoBase()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo.txt"), new MockFileData("Demo text content") }
        });

        // Act
        var result = fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\bar"));

        // Assert
        await That(result).IsNotNull();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockDirectory_CreateDirectory_ShouldTrimTrailingSpaces()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\temp\folder "));

        // Assert
        await That(fileSystem.Directory.Exists(XFS.Path(@"c:\temp\folder"))).IsTrue();
    }

    [Test]
    public async Task MockDirectory_CreMockDirectory_CreateDirectory_ShouldReturnDirectoryInfoBaseWhenDirectoryExists()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo\"), new MockDirectoryData() }
        });

        // Act
        var result = fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\foo\"));

        // Assert
        await That(result).IsNotNull();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.UNCPaths)]
    public async Task MockDirectory_CreateDirectory_ShouldWorkWithUNCPath()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        fileSystem.Directory.CreateDirectory(@"\\server\share\path\to\create");

        // Assert
        await That(fileSystem.Directory.Exists(@"\\server\share\path\to\create\")).IsTrue();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.UNCPaths)]
    public async Task MockDirectory_CreateDirectory_ShouldFailIfTryingToCreateUNCPathOnlyServer()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var ex = await That(() => fileSystem.Directory.CreateDirectory(@"\\server")).Throws<ArgumentException>();

        // Assert
        await That(ex.Message).StartsWith("The UNC path should be of the form \\\\server\\share.");
        await That(ex.ParamName).IsEqualTo("path");
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.UNCPaths)]
    public async Task MockDirectory_CreateDirectory_ShouldSucceedIfTryingToCreateUNCPathShare()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        fileSystem.Directory.CreateDirectory(@"\\server\share");

        // Assert
        await That(fileSystem.Directory.Exists(@"\\server\share\")).IsTrue();
    }

#if FEATURE_CREATE_TEMP_SUBDIRECTORY
        [Test]
        public async Task MockDirectory_CreateTempSubdirectory_ShouldCreateSubdirectoryInTempDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.Directory.CreateTempSubdirectory();

            // Assert
            await That(fileSystem.Directory.Exists(result.FullName)).IsTrue();
            await That(result.FullName).StartsWith(fileSystem.Path.GetTempPath());
        }

        [Test]
        public async Task MockDirectory_CreateTempSubdirectoryWithPrefix_ShouldCreateDirectoryWithGivenPrefixInTempDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.Directory.CreateTempSubdirectory("foo-");

            // Assert
            await That(fileSystem.Directory.Exists(result.FullName)).IsTrue();
            await That(Path.GetFileName(result.FullName).StartsWith("foo-")).IsTrue();
            await That(result.FullName.Contains(fileSystem.Path.GetTempPath())).IsTrue();
        }
#endif

    [Test]
    public async Task MockDirectory_Delete_ShouldDeleteDirectory()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\bar\foo.txt"), new MockFileData("Demo text content") }
        });

        // Act
        fileSystem.Directory.Delete(XFS.Path(@"c:\bar"), true);

        // Assert
        await That(fileSystem.Directory.Exists(XFS.Path(@"c:\bar"))).IsFalse();
    }

    [Test]
    public async Task MockDirectory_Delete_ShouldNotDeleteAllDirectories()
    {
        // Arrange
        var folder1Path = XFS.Path(@"D:\Test\Program");
        var folder1SubFolderPath = XFS.Path(@"D:\Test\Program\Subfolder");
        var folder2Path = XFS.Path(@"D:\Test\Program_bak");

        var fileSystem = new MockFileSystem();

        fileSystem.AddDirectory(folder1Path);
        fileSystem.AddDirectory(folder2Path);
        fileSystem.AddDirectory(folder1SubFolderPath);

        // Act
        fileSystem.Directory.Delete(folder1Path, recursive: true);

        // Assert
        await That(fileSystem.Directory.Exists(folder1Path)).IsFalse();
        await That(fileSystem.Directory.Exists(folder1SubFolderPath)).IsFalse();
        await That(fileSystem.Directory.Exists(folder2Path)).IsTrue();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.CaseInsensitivity)]
    public async Task MockDirectory_Delete_ShouldDeleteDirectoryCaseInsensitively()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\bar\foo.txt"), new MockFileData("Demo text content") }
        });

        // Act
        fileSystem.Directory.Delete(XFS.Path(@"c:\BAR"), true);

        // Assert
        await That(fileSystem.Directory.Exists(XFS.Path(@"c:\bar"))).IsFalse();
    }

    [Test]
    [UnixOnly(UnixSpecifics.CaseSensitivity)]
    public async Task MockDirectory_Delete_ShouldThrowDirectoryNotFoundException_WhenSpecifiedWithInDifferentCase()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { "/bar/foo.txt", new MockFileData("Demo text content") }
        });

        // Act
        Action action = () => fileSystem.Directory.Delete("/BAR", true);

        // Assert
        await That(action).Throws<DirectoryNotFoundException>();
    }

    [Test]
    [UnixOnly(UnixSpecifics.CaseSensitivity)]
    public async Task MockDirectory_Delete_ShouldDeleteDirectoryCaseSensitively()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { "/bar/foo.txt", new MockFileData("Demo text content") }
        });

        // Act
        fileSystem.Directory.Delete("/bar", true);

        // Assert
        await That(fileSystem.Directory.Exists("/bar")).IsFalse();
    }

    [Test]
    public async Task MockDirectory_Delete_ShouldThrowDirectoryNotFoundException()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\bar\foo.txt"), new MockFileData("Demo text content") }
        });

        var ex = await That(() => fileSystem.Directory.Delete(XFS.Path(@"c:\baz"))).Throws<DirectoryNotFoundException>();

        await That(ex.Message).IsEqualTo($"'{XFS.Path("c:\\baz")}' does not exist or could not be found.");
    }

    [Test]
    public async Task MockDirectory_Delete_ShouldThrowIOException()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\bar\foo.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\bar\baz.txt"), new MockFileData("Demo text content") }
        });

        var ex = await That(() => fileSystem.Directory.Delete(XFS.Path(@"c:\bar"))).Throws<IOException>();

        await That(ex.Message).IsEqualTo("The directory specified by " + XFS.Path("c:\\bar") + " is read-only, or recursive is false and " + XFS.Path("c:\\bar") + " is not an empty directory.");
    }

    [Test]
    public async Task MockDirectory_Delete_ShouldDeleteDirectoryRecursively()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\bar\foo.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\bar\bar2\foo.txt"), new MockFileData("Demo text content") }
        });

        // Act
        fileSystem.DirectoryInfo.New(XFS.Path(@"c:\bar")).Delete(true);

        // Assert
        await That(fileSystem.Directory.Exists(XFS.Path(@"c:\bar"))).IsFalse();
        await That(fileSystem.Directory.Exists(XFS.Path(@"c:\bar\bar2"))).IsFalse();
    }

    [Test]
    public async Task MockDirectory_Delete_ShouldThrowIOException_WhenPathIsAFile()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\foo.txt"), new MockFileData("Demo text content") },
        });

        // Act
        Action action = () => fileSystem.Directory.Delete(XFS.Path(@"c:\foo.txt"));

        // Assert
        await That(action).Throws<IOException>();
    }

    [Test]
    public async Task MockDirectory_GetFileSystemEntries_Returns_Files_And_Directories()
    {
        string testPath = XFS.Path(@"c:\foo\bar.txt");
        string testDir = XFS.Path(@"c:\foo\bar");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { testPath, new MockFileData("Demo text content") },
            { testDir,  new MockDirectoryData() }
        });

        var entries = fileSystem.Directory.GetFileSystemEntries(XFS.Path(@"c:\foo")).OrderBy(k => k);
        await That(entries.Count()).IsEqualTo(2);
        await That(entries.First()).IsEqualTo(testDir);
        await That(entries.Last()).IsEqualTo(testPath);
    }

    [Test]
    public async Task MockDirectory_GetFileSystemEntries_ShouldNotReturnSubDirectory_WithSearchOption()
    {
        string testPath = XFS.Path(@"c:\foo\bar.txt");
        string testDir = XFS.Path(@"c:\foo\bar");
        string testSubDir = XFS.Path(@"c:\foo\bar\baz");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { testPath, new MockFileData("Demo text content") },
            { testSubDir,  new MockDirectoryData() },
        });

        var entries = fileSystem.Directory.GetFileSystemEntries(XFS.Path(@"c:\foo"), "*", SearchOption.TopDirectoryOnly).OrderBy(k => k);
        await That(entries.Count()).IsEqualTo(2);
        await That(entries.First()).IsEqualTo(testDir);
        await That(entries.Last()).IsEqualTo(testPath);
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldThrowArgumentNullException_IfPathParamIsNull()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

        Action action = () => fileSystem.Directory.GetFiles(null);
        await That(action).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldThrowDirectoryNotFoundException_IfPathDoesNotExists()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.Directory.GetFiles(XFS.Path(@"c:\Foo"), "*a.txt");

        // Assert
        await That(action).Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task MockDirectory_GetFiles_Returns_Files()
    {
        string testPath = XFS.Path(@"c:\foo\bar.txt");
        string testDir = XFS.Path(@"c:\foo\bar\");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { testPath, new MockFileData("Demo text content") },
            { testDir,  new MockDirectoryData() }
        });

        var entries = fileSystem.Directory.GetFiles(XFS.Path(@"c:\foo")).OrderBy(k => k);
        await That(entries.Count()).IsEqualTo(1);
        await That(entries.First()).IsEqualTo(testPath);
    }

    [Test]
    public async Task MockDirectory_GetFiles_Returns_Files_WithRelativePath()
    {
        // arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

        string directory = XFS.Path(@"C:\foo");

        fileSystem.Directory.SetCurrentDirectory(directory);
        fileSystem.AddFile(XFS.Path(@"C:\test.txt"), new MockFileData("Some ASCII text."));

        await That(fileSystem.Directory.GetFiles(XFS.Path(@"..\")).Length).IsEqualTo(1); // Assert with relative path
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldThrowAnArgumentNullException_IfSearchPatternIsNull()
    {
        // Arrange
        var directoryPath = XFS.Path(@"c:\Foo");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(directoryPath);

        // Act
        Action action = () => fileSystem.Directory.GetFiles(directoryPath, null);

        // Assert
        await That(action).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldThrowAnArgumentException_IfSearchPatternEndsWithTwoDots()
    {
        // Arrange
        var directoryPath = XFS.Path(@"c:\Foo");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(directoryPath);

        // Act
        Action action = () => fileSystem.Directory.GetFiles(directoryPath, "*a..");

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [TestCase(@"..\")]
    [TestCase(@"aaa\vv..\")]
    [TestCase(@"a..\b")]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockDirectory_GetFiles_ShouldThrowAnArgumentException_IfSearchPatternContainsTwoDotsFollowedByOneBackslash(string searchPattern)
    {
        // Arrange
        var directoryPath = XFS.Path(@"c:\Foo");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(directoryPath);

        // Act
        Action action = () => fileSystem.Directory.GetFiles(directoryPath, searchPattern);

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [TestCase(@"a../b")]
    [TestCase(@"../")]
    public async Task MockDirectory_GetFiles_ShouldThrowAnArgumentException_IfSearchPatternContainsTwoDotsFollowedByOneSlash(string searchPattern)
    {
        // Arrange
        var directoryPath = XFS.Path(@"c:\Foo");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(directoryPath);

        // Act
        Action action = () => fileSystem.Directory.GetFiles(directoryPath, searchPattern);

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockDirectory_GetFiles_ShouldFindFilesContainingTwoOrMoreDots()
    {
        // Arrange
        string testPath = XFS.Path(@"c:\foo..r\bar.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { testPath, new MockFileData(string.Empty) }
        });

        // Act
        var actualResult = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), XFS.Path(@"foo..r\*"));

        // Assert
        await That(actualResult).IsEquivalentTo(new[] { testPath });
    }

    [TestCase("aa\t")]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockDirectory_GetFiles_ShouldThrowAnArgumentException_IfSearchPatternHasIllegalCharacters(string searchPattern)
    {
        // Arrange
        var directoryPath = XFS.Path(@"c:\Foo");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(directoryPath);

        // Act
        Action action = () => fileSystem.Directory.GetFiles(directoryPath, searchPattern);

        // Assert
        await That(action).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockDirectory_GetRoot_Returns_Root()
    {
        string testDir = XFS.Path(@"c:\foo\bar\");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { testDir,  new MockDirectoryData() }
        });

        await That(fileSystem.Directory.GetDirectoryRoot(XFS.Path(@"C:\foo\bar"))).IsEqualTo(XFS.Path("C:\\"));
    }

    [Test]
    public async Task MockDirectory_GetLogicalDrives_Returns_LogicalDrives()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {XFS.Path(@"c:\foo\bar\"), new MockDirectoryData()},
            {XFS.Path(@"c:\foo\baz\"), new MockDirectoryData()},
            {XFS.Path(@"d:\bash\"), new MockDirectoryData()},
        });

        var drives = fileSystem.Directory.GetLogicalDrives();

        if (XFS.IsUnixPlatform())
        {
            await That(drives.Length).IsEqualTo(1);
            await That(drives.Contains("/")).IsTrue();
        }
        else
        {
            await That(drives.Length).IsEqualTo(2);
            await That(drives.Contains(@"C:\")).IsTrue();
            await That(drives.Contains(@"D:\")).IsTrue();
        }
    }

    [Test]
    public async Task MockDirectory_GetDirectories_Returns_Child_Directories()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"A:\folder1\folder2\folder3\file.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"A:\folder1\folder4\file2.txt"), new MockFileData("Demo text content 2") },
        });

        var directories = fileSystem.Directory.GetDirectories(XFS.Path(@"A:\folder1")).ToArray();

        //Check that it does not returns itself
        await That(directories.Contains(XFS.Path(@"A:\folder1"))).IsFalse();

        //Check that it correctly returns all child directories
        await That(directories.Count()).IsEqualTo(2);
        await That(directories.Contains(XFS.Path(@"A:\folder1\folder2"))).IsTrue();
        await That(directories.Contains(XFS.Path(@"A:\folder1\folder4"))).IsTrue();
    }

    [Test]
    public async Task MockDirectory_GetDirectories_WithTopDirectories_ShouldOnlyReturnTopDirectories()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo.foo"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\.foo"));
        fileSystem.AddFile(XFS.Path(@"C:\Folder\.foo\bar"), new MockFileData(string.Empty));

        // Act
        var actualResult = fileSystem.Directory.GetDirectories(XFS.Path(@"C:\Folder\"), "*.foo");

        // Assert
        await That(actualResult).IsEquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo") });
    }

    [Test]
    public async Task MockDirectory_GetDirectories_RelativeWithNoSubDirectories_ShouldReturnDirectories()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory("Folder");

        // Act
        var actualResult = fileSystem.Directory.GetDirectories("Folder");

        // Assert
        await That(actualResult).IsEmpty();
    }

    [TestCase(@"Folder\SubFolder")]
    [TestCase(@"Folder")]
    public async Task MockDirectory_GetDirectories_RelativeDirectory_WithoutChildren_ShouldReturnNoChildDirectories(string relativeDirPath)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(relativeDirPath);

        // Act
        var actualResult = fileSystem.Directory.GetDirectories(relativeDirPath);

        // Assert
        await That(actualResult).IsEmpty();
    }

    [TestCase(@"Folder\SubFolder")]
    [TestCase(@"Folder")]
    public async Task MockDirectory_GetDirectories_RelativeDirectory_WithChildren_ShouldReturnChildDirectories(string relativeDirPath)
    {
        // Arrange
        var currentDirectory = XFS.Path(@"T:\foo");
        var fileSystem = new MockFileSystem(null, currentDirectory: currentDirectory);
        fileSystem.Directory.CreateDirectory(XFS.Path(relativeDirPath));
        fileSystem.Directory.CreateDirectory(XFS.Path(relativeDirPath + @"\child"));

        // Act
        var actualResult = fileSystem.Directory.GetDirectories(XFS.Path(relativeDirPath));

        // Assert
        await That(actualResult).IsEqualTo(new[] { XFS.Path(relativeDirPath + @"\child") });
    }

    [Test]
    public async Task MockDirectory_GetDirectories_AbsoluteWithNoSubDirectories_ShouldReturnDirectories()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory("Folder");

        // Act
        var fullPath = fileSystem.Path.GetFullPath("Folder");
        var actualResult = fileSystem.Directory.GetDirectories(fullPath);

        // Assert
        await That(actualResult).IsEmpty();
    }

    [Test]
    public async Task MockDirectory_GetDirectories_WithAllDirectories_ShouldReturnsAllMatchingSubFolders()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo.foo"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\.foo"));
        fileSystem.AddFile(XFS.Path(@"C:\Folder\.foo\bar"), new MockFileData(string.Empty));

        // Act
        var actualResult = fileSystem.Directory.GetDirectories(XFS.Path(@"C:\Folder\"), "*.foo", SearchOption.AllDirectories);

        // Assert
        await That(actualResult).IsEquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo"), XFS.Path(@"C:\Folder\.foo\.foo") });
    }

    [Test]
    public async Task MockDirectory_GetDirectories_ShouldThrowWhenPathIsNotMocked()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\a.gif"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\b.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\c.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\b.gif"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\c.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\a.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\b.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\c.gif"), new MockFileData("Demo text content") },
        });

        // Act
        Action action = () => fileSystem.Directory.GetDirectories(XFS.Path(@"c:\d"));

        // Assert
        await That(action).Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task MockDirectory_EnumerateDirectories_Returns_Child_Directories()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"A:\folder1\folder2\folder3\file.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"A:\folder1\folder4\file2.txt"), new MockFileData("Demo text content 2") },
        });

        var directories = fileSystem.Directory.EnumerateDirectories(XFS.Path(@"A:\folder1")).ToArray();

        //Check that it does not returns itself
        await That(directories.Contains(XFS.Path(@"A:\folder1"))).IsFalse();

        //Check that it correctly returns all child directories
        await That(directories.Count()).IsEqualTo(2);
        await That(directories.Contains(XFS.Path(@"A:\folder1\folder2"))).IsTrue();
        await That(directories.Contains(XFS.Path(@"A:\folder1\folder4"))).IsTrue();
    }

    [Test]
    public async Task MockDirectory_EnumerateDirectories_WithTopDirectories_ShouldOnlyReturnTopDirectories()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo.foo"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\.foo"));
        fileSystem.AddFile(XFS.Path(@"C:\Folder\.foo\bar"), new MockFileData(string.Empty));

        // Act
        var actualResult = fileSystem.Directory.EnumerateDirectories(XFS.Path(@"C:\Folder\"), "*.foo");

        // Assert
        await That(actualResult).IsEquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo") });
    }

#if FEATURE_ENUMERATION_OPTIONS
    [Test]
    public async Task MockDirectory_EnumerateDirectories_WithEnumerationOptionsTopDirectories_ShouldOnlyReturnTopDirectories()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo.foo"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\.foo"));
        fileSystem.AddFile(XFS.Path(@"C:\Folder\.foo\bar"), new MockFileData(string.Empty));

        var enumerationOptions = new EnumerationOptions
        {
            RecurseSubdirectories = false
        };

        // Act
        var actualResult = fileSystem.Directory.EnumerateDirectories(XFS.Path(@"C:\Folder\"), "*.foo", enumerationOptions);

        // Assert
        await That(actualResult).IsEquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo") });
    }
#endif
    [Test]
    public async Task MockDirectory_EnumerateDirectories_WithAllDirectories_ShouldReturnsAllMatchingSubFolders()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo.foo"));
        fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\.foo"));
        fileSystem.AddFile(XFS.Path(@"C:\Folder\.foo\bar"), new MockFileData(string.Empty));

        // Act
        var actualResult = fileSystem.Directory.EnumerateDirectories(XFS.Path(@"C:\Folder\"), "*.foo", SearchOption.AllDirectories);

        // Assert
        await That(actualResult).IsEquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo"), XFS.Path(@"C:\Folder\.foo\.foo") });
    }

    [Test]
    public async Task MockDirectory_EnumerateDirectories_ShouldThrowWhenPathIsNotMocked()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\a.gif"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\b.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\c.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\b.gif"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\c.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\a.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\b.txt"), new MockFileData("Demo text content") },
            { XFS.Path(@"c:\a\a\c.gif"), new MockFileData("Demo text content") },
        });

        // Act
        Action action = () => fileSystem.Directory.EnumerateDirectories(XFS.Path(@"c:\d"));

        // Assert
        await That(action).Throws<DirectoryNotFoundException>();
    }
        
    [TestCaseSource(nameof(GetPrefixTestPaths))]
    public async Task MockDirectory_EnumerateDirectories_ShouldReturnPathsPrefixedWithQueryPath(
        string queryPath, string expectedPath)
    {
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory("Folder/SubFolder");
            
        var actualResult = fileSystem.Directory.EnumerateDirectories(queryPath);
            
        await That(actualResult).IsEqualTo(new[] { expectedPath });
    }
        
    private static IEnumerable<object[]> GetPrefixTestPaths()
    {
        var sep = Path.DirectorySeparatorChar;
        yield return new object[] { "Folder", $"Folder{sep}SubFolder" };
        yield return new object[] { $"Folder{sep}", $"Folder{sep}SubFolder" };
        yield return new object[] { $"Folder{sep}..{sep}.{sep}Folder", $"Folder{sep}..{sep}.{sep}Folder{sep}SubFolder" };
    }

    public static IEnumerable<object[]> GetPathsForMoving()
    {
        yield return new object[] { XFS.Path(@"a:\folder1\"), XFS.Path(@"A:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"folder2\file2.txt") };
        yield return new object[] { XFS.Path(@"A:\folder1\"), XFS.Path(@"A:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"folder2\file2.txt") };
        yield return new object[] { XFS.Path(@"a:\folder1\"), XFS.Path(@"a:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"folder2\file2.txt") };
        yield return new object[] { XFS.Path(@"A:\folder1\"), XFS.Path(@"a:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"folder2\file2.txt") };
        yield return new object[] { XFS.Path(@"A:\folder1\"), XFS.Path(@"a:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"Folder2\file2.txt") };
        yield return new object[] { XFS.Path(@"A:\folder1\"), XFS.Path(@"a:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"Folder2\fiLe2.txt") };
        yield return new object[] { XFS.Path(@"A:\folder1\"), XFS.Path(@"a:\folder3\"), XFS.Path("folder444\\file.txt"), XFS.Path(@"Folder2\fiLe2.txt") };
    }

    [Test]
    public async Task Move_DirectoryExistsWithDifferentCase_DirectorySuccessfullyMoved()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\OLD_LOCATION\Data"));
        fileSystem.AddFile(XFS.Path(@"C:\old_location\Data\someFile.txt"), new MockFileData("abc"));

        // Act
        fileSystem.Directory.Move(XFS.Path(@"C:\old_location"), XFS.Path(@"C:\NewLocation\"));

        // Assert
        await That(fileSystem.File.Exists(XFS.Path(@"C:\NewLocation\Data\someFile.txt"))).IsTrue();
    }

    [TestCaseSource(nameof(GetPathsForMoving))]
    public async Task MockDirectory_Move_ShouldMoveDirectories(string sourceDirName, string destDirName, string filePathOne, string filePathTwo)
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(sourceDirName + filePathOne) , new MockFileData("aaa") },
            { XFS.Path(sourceDirName + filePathTwo) , new MockFileData("bbb") },
        });

        // Act
        fileSystem.DirectoryInfo.New(sourceDirName).MoveTo(destDirName);

        // Assert
        await That(fileSystem.Directory.Exists(sourceDirName)).IsFalse();
        await That(fileSystem.File.Exists(XFS.Path(destDirName + filePathOne))).IsTrue();
        await That(fileSystem.File.Exists(XFS.Path(destDirName + filePathTwo))).IsTrue();
    }

    [Test]
    public async Task MockDirectory_Move_ShouldMoveFiles()
    {
        string sourceFilePath = XFS.Path(@"c:\demo.txt");
        string sourceFileContent = "this is some content";

        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { sourceFilePath, new MockFileData(sourceFileContent) }
        });

        string destFilePath = XFS.Path(@"c:\demo1.txt");

        fileSystem.Directory.Move(sourceFilePath, destFilePath);

        await That(fileSystem.FileExists(destFilePath)).IsTrue();
        await That(fileSystem.FileExists(sourceFilePath)).IsFalse();
        await That(fileSystem.GetFile(destFilePath).TextContents).IsEqualTo(sourceFileContent);
    }

    [Test]
    public async Task MockDirectory_Move_ShouldMoveDirectoryAttributes()
    {
        // Arrange
        var sourceDirName = XFS.Path(@"a:\folder1\");
        var destDirName = XFS.Path(@"a:\folder2\");
        const string filePathOne = "file1.txt";
        const string filePathTwo = "file2.txt";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(sourceDirName + filePathOne) , new MockFileData("aaa") },
            { XFS.Path(sourceDirName + filePathTwo) , new MockFileData("bbb") },
        });

        var sourceDirectoryInfo = fileSystem.DirectoryInfo.New(sourceDirName);
        sourceDirectoryInfo.Attributes |= FileAttributes.System;

        // Act
        fileSystem.DirectoryInfo.New(sourceDirName).MoveTo(destDirName);

        // Assert
        var destDirectoryInfo = fileSystem.DirectoryInfo.New(destDirName);
        await That(destDirectoryInfo.Attributes.HasFlag(FileAttributes.System)).IsTrue();
    }

    [Test]
    public async Task MockDirectory_Move_ShouldMoveDirectoryWithReadOnlySubDirectory()
    {
        // Arrange
        var sourceDirName = XFS.Path(@"a:\folder1\");
        var sourceSubDirName = XFS.Path(@"a:\folder1\sub\");

        var destDirName = XFS.Path(@"a:\folder2\");
        var destSubDirName = XFS.Path(@"a:\folder2\sub\");

        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(sourceSubDirName);

        var subDirectoryInfo = fileSystem.DirectoryInfo.New(sourceSubDirName);
        subDirectoryInfo.Attributes |= FileAttributes.ReadOnly;

        var sourceDirectoryInfo = fileSystem.DirectoryInfo.New(sourceDirName);

        // Act
        fileSystem.DirectoryInfo.New(sourceDirName).MoveTo(destDirName);

        // Assert
        await That(fileSystem.Directory.Exists(sourceSubDirName)).IsFalse();
        await That(fileSystem.FileExists(destSubDirName)).IsTrue();
    }

    [Test]
    public async Task MockDirectory_Move_ShouldOnlyMoveDirAndFilesWithinDir()
    {
        // Arrange
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {XFS.Path(@"c:\source\dummy"), new MockDirectoryData()},
            {XFS.Path(@"c:\source\dummy\content.txt"), new MockFileData(new byte[] {0})},
            {XFS.Path(@"c:\source\dummy.txt"), new MockFileData(new byte[] {0})},
            {XFS.Path(@"c:\source\dummy2"), new MockDirectoryData()},
            {XFS.Path(@"c:\destination"), new MockDirectoryData()},
        });

        // Act
        fileSystem.Directory.Move(XFS.Path(@"c:\source\dummy"), XFS.Path(@"c:\destination\dummy"));

        // Assert
        await That(fileSystem.FileExists(XFS.Path(@"c:\source\dummy.txt"))).IsTrue();
        await That(fileSystem.Directory.Exists(XFS.Path(@"c:\source\dummy2"))).IsTrue();
    }

    [Test]
    public async Task MockDirectory_GetCurrentDirectory_ShouldReturnValueFromFileSystemConstructor()
    {
        string directory = XFS.Path(@"D:\folder1\folder2");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>(), directory);

        var actual = fileSystem.Directory.GetCurrentDirectory();

        await That(actual).IsEqualTo(directory);
    }

    [Test]
    public async Task MockDirectory_GetCurrentDirectory_ShouldReturnDefaultPathWhenNotSet()
    {
        string directory = XFS.Path(@"C:\");

        var fileSystem = new MockFileSystem();

        var actual = fileSystem.Directory.GetCurrentDirectory();

        await That(actual).IsEqualTo(directory);
    }

    [Test]
    public async Task MockDirectory_SetCurrentDirectory_ShouldChangeCurrentDirectory()
    {
        string directory = XFS.Path(@"D:\folder1\folder2");
        var fileSystem = new MockFileSystem();

        // Precondition
        await That(fileSystem.Directory.GetCurrentDirectory()).IsNotEqualTo(directory);

        fileSystem.Directory.SetCurrentDirectory(directory);

        await That(fileSystem.Directory.GetCurrentDirectory()).IsEqualTo(directory);
    }

    [Test]
    public async Task MockDirectory_SetCurrentDirectory_WithRelativePath_ShouldUseFullPath()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.SetCurrentDirectory(".");

        var result = fileSystem.Directory.GetCurrentDirectory();

        await That(fileSystem.Path.IsPathRooted(result)).IsTrue();
    }

    [Test]
    public async Task MockDirectory_GetParent_ShouldThrowArgumentNullExceptionIfPathIsNull()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action act = () => fileSystem.Directory.GetParent(null);

        // Assert
        await That(act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task MockDirectory_GetParent_ShouldThrowArgumentExceptionIfPathIsEmpty()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action act = () => fileSystem.Directory.GetParent(string.Empty);

        // Assert
        await That(act).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockDirectory_GetParent_ShouldReturnADirectoryInfoIfPathDoesNotExist()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var actualResult = fileSystem.Directory.GetParent(XFS.Path(@"c:\directory\does\not\exist"));

        // Assert
        await That(actualResult).IsNotNull();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockDirectory_GetParent_ShouldThrowArgumentExceptionIfPathHasIllegalCharacters()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action act = () => fileSystem.Directory.GetParent(XFS.Path("c:\\director\ty\\has\\illegal\\character"));

        // Assert
        await That(act).Throws<ArgumentException>();
    }

    [Test]
    public async Task MockDirectory_GetParent_ShouldReturnNullIfPathIsRoot()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"c:\"));

        // Act
        var actualResult = fileSystem.Directory.GetParent(XFS.Path(@"c:\"));

        // Assert
        await That(actualResult).IsNull();
    }

    [Test]
    [UnixOnly(UnixSpecifics.SlashRoot)]
    public async Task MockDirectory_GetParent_ShouldReturnRootIfDirectoryIsInRoot()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory("/bar");

        // Act
        var parent = fileSystem.Directory.GetParent("/bar");

        // Assert
        await That(parent.FullName).IsEqualTo("/");
    }

    public static IEnumerable<string[]> MockDirectory_GetParent_Cases
    {
        get
        {
            yield return new[] { XFS.Path(@"c:\a"), XFS.Path(@"c:\") };
            yield return new[] { XFS.Path(@"c:\a\b\c\d"), XFS.Path(@"c:\a\b\c") };
            yield return new[] { XFS.Path(@"c:\a\b\c\d\"), XFS.Path(@"c:\a\b\c") };
        }
    }

    public async Task MockDirectory_GetParent_ShouldReturnTheParentWithoutTrailingDirectorySeparatorChar(string path, string expectedResult)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(path);

        // Act
        var actualResult = fileSystem.Directory.GetParent(path);

        // Assert
        await That(actualResult.FullName).IsEqualTo(expectedResult);
    }

    [Test]
    public async Task MockDirectory_Move_ShouldThrowAnIOExceptionIfBothPathAreIdentical()
    {
        // Arrange
        string path = XFS.Path(@"c:\a");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(path);

        // Act
        Action action = () => fileSystem.Directory.Move(path, path);

        // Assert
        await That(action, "Source and destination path must be different.").Throws<IOException>();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockDirectory_Move_ShouldThrowAnIOExceptionIfDirectoriesAreOnDifferentVolumes()
    {
        // Arrange
        string sourcePath = XFS.Path(@"c:\a");
        string destPath = XFS.Path(@"d:\v");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(sourcePath);

        // Act
        Action action = () => fileSystem.Directory.Move(sourcePath, destPath);

        // Assert
        await That(action, "Source and destination path must have identical roots. Move will not work across volumes.").Throws<IOException>();
    }

    [Test]
    public async Task MockDirectory_Move_ShouldThrowADirectoryNotFoundExceptionIfDestinationDirectoryDoesNotExist()
    {
        // Arrange
        string sourcePath = XFS.Path(@"c:\a");
        string destPath = XFS.Path(@"c:\b");
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.Directory.Move(sourcePath, destPath);

        // Assert
        await That(action, "Could not find a part of the path 'c:\a'.").Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task MockDirectory_Move_ShouldThrowAnIOExceptionIfDestinationDirectoryExists()
    {
        // Arrange
        string sourcePath = XFS.Path(@"c:\a");
        string destPath = XFS.Path(@"c:\b");
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(sourcePath);
        fileSystem.AddDirectory(destPath);

        // Act
        Action action = () => fileSystem.Directory.Move(sourcePath, destPath);

        // Assert
        await That(action, "Cannot create 'c:\b\' because a file or directory with the same name already exists.'").Throws<IOException>();
    }

    [Test]
    public async Task MockDirectory_EnumerateFiles_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndSearchOptionIsAllDirectories()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        IEnumerable<string> expected = new[]
        {
            XFS.Path(@"c:\a\a.txt"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\c.txt"),
            XFS.Path(@"c:\a\d"),
            XFS.Path(@"c:\a\a\a.txt"),
            XFS.Path(@"c:\a\a\b.txt"),
            XFS.Path(@"c:\a\a\c.gif"),
            XFS.Path(@"c:\a\a\d")
        };

        // Act
        var result = fileSystem.Directory.EnumerateFiles(XFS.Path(@"c:\a"), "*", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

#if FEATURE_ENUMERATION_OPTIONS
    [Test]
    public async Task MockDirectory_EnumerateFiles_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndEnumerationOptionsIsAllDirectories()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        IEnumerable<string> expected = new[]
        {
            XFS.Path(@"c:\a\a.txt"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\c.txt"),
            XFS.Path(@"c:\a\d"),
            XFS.Path(@"c:\a\a\a.txt"),
            XFS.Path(@"c:\a\a\b.txt"),
            XFS.Path(@"c:\a\a\c.gif"),
            XFS.Path(@"c:\a\a\d")
        };

        var enumerationOptions = new EnumerationOptions
        {
            RecurseSubdirectories = true
        };

        // Act
        var result = fileSystem.Directory.EnumerateFiles(XFS.Path(@"c:\a"), "*", enumerationOptions);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

#endif

    [Test]
    public async Task MockDirectory_EnumerateFiles_ShouldFilterByExtensionBasedSearchPattern()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        var expected = new[]
        {
            XFS.Path(@"c:\a.gif"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\a\c.gif")
        };

        // Act
        var result = fileSystem.Directory.EnumerateFiles(XFS.Path(@"c:\"), "*.gif", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_EnumerateFiles_WhenFilterIsUnRooted_ShouldFindFilesInCurrentDirectory()
    {
        // Arrange
        var someContent = new MockFileData(String.Empty);
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\a.txt"), someContent },
            { XFS.Path(@"c:\a\a.txt"), someContent },
            { XFS.Path(@"c:\a\b\b.txt"), someContent },
            { XFS.Path(@"c:\a\c\c.txt"), someContent },
        });

        var expected = new[]
        {
            XFS.Path(@"c:\a\b\b.txt"),
        };

        fileSystem.Directory.SetCurrentDirectory(XFS.Path(@"c:\a"));

        // Act
        var result = fileSystem.Directory.EnumerateFiles(XFS.Path("b"));

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_EnumerateFiles_WhenFilterIsUnRooted_ShouldNotFindFilesInPathOutsideCurrentDirectory()
    {
        // Arrange
        var someContent = new MockFileData(String.Empty);
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { XFS.Path(@"c:\a.txt"), someContent },
            { XFS.Path(@"c:\a\b\b.txt"), someContent },
            { XFS.Path(@"c:\c\b\b.txt"), someContent },
        });

        var expected = new[]
        {
            XFS.Path(@"c:\a\b\b.txt"),
        };

        fileSystem.Directory.SetCurrentDirectory(XFS.Path(@"c:\a"));

        // Act
        var result = fileSystem.Directory.EnumerateFiles(XFS.Path("b"));

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_EnumerateFileSystemEntries_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndSearchOptionIsAllDirectories()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        IEnumerable<string> expected = new[]
        {
            XFS.Path(@"c:\a\a.txt"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\c.txt"),
            XFS.Path(@"c:\a\d"),
            XFS.Path(@"c:\a\a\a.txt"),
            XFS.Path(@"c:\a\a\b.txt"),
            XFS.Path(@"c:\a\a\c.gif"),
            XFS.Path(@"c:\a\a\d"),
            XFS.Path(@"c:\a\a")
        };

        // Act
        var result = fileSystem.Directory.EnumerateFileSystemEntries(XFS.Path(@"c:\a"), "*", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

    [Test]
    public async Task MockDirectory_EnumerateFileSystemEntries_ShouldFilterByExtensionBasedSearchPattern()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        var expected = new[]
        {
            XFS.Path(@"c:\a.gif"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\a\c.gif")
        };

        // Act
        var result = fileSystem.Directory.EnumerateFileSystemEntries(XFS.Path(@"c:\"), "*.gif", SearchOption.AllDirectories);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }

#if FEATURE_ENUMERATION_OPTIONS
    [Test]
    public async Task MockDirectory_EnumerateFileSystemEntries_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndEnumerationOptionsIsAllDirectories()
    {
        // Arrange
        var fileSystem = SetupFileSystem();
        IEnumerable<string> expected = new[]
        {
            XFS.Path(@"c:\a\a.txt"),
            XFS.Path(@"c:\a\b.gif"),
            XFS.Path(@"c:\a\c.txt"),
            XFS.Path(@"c:\a\d"),
            XFS.Path(@"c:\a\a\a.txt"),
            XFS.Path(@"c:\a\a\b.txt"),
            XFS.Path(@"c:\a\a\c.gif"),
            XFS.Path(@"c:\a\a\d"),
            XFS.Path(@"c:\a\a")
        };

        var enumerationOptions = new EnumerationOptions
        {
            RecurseSubdirectories = true
        };

        // Act
        var result = fileSystem.Directory.EnumerateFileSystemEntries(XFS.Path(@"c:\a"), "*", enumerationOptions);

        // Assert
        await That(result).IsEqualTo(expected).InAnyOrder();
    }
#endif

    [Test]
    public async Task MockDirectory_GetAccessControl_ShouldThrowExceptionOnDirectoryNotFound()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
#pragma warning disable CA1416
        await That(() => fileSystem.Directory.GetAccessControl(XFS.Path(@"c:\foo"))).Throws<DirectoryNotFoundException>();
#pragma warning restore CA1416
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.AccessControlLists)]
    [SupportedOSPlatform("windows")]
    public async Task MockDirectory_GetAccessControl_ShouldReturnNewDirectorySecurity()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\foo\"));

        // Act
        DirectorySecurity result = fileSystem.Directory.GetAccessControl(XFS.Path(@"c:\foo\"));

        // Assert
        await That(result).IsNotNull();
    }

    [Test]
    public async Task MockDirectory_SetCreationTime_ShouldNotThrowWithoutTrailingBackslash()
    {
        var path = XFS.Path(@"C:\NoTrailingBackslash");
        var fs = new MockFileSystem();
        fs.Directory.CreateDirectory(path);
        await That(()=> fs.Directory.SetCreationTime(path, DateTime.Now)).DoesNotThrow();
        fs.Directory.Delete(path);
    }

    private static IEnumerable<TestCaseData> Failing_DirectoryMoveFromToPaths
    {
        get
        {
            var testTargetDirs = new[]
            {
                @"c:\temp2\fd\df", @"c:\temp2\fd\", @"c:\temp2\fd\..\fd", @"c:\temp2\fd", @".\..\temp2\fd\df",
                @".\..\temp2\fd\df\..", @".\..\temp2\fd\df\..\", @"..\temp2\fd\", @".\temp2\fd", @"temp2\fd",
                @"c:\temp3\exists2\d3", @"c:\temp4\exists"
            };

            var testSourceDirs = new[] { @"c:\temp\exists\foldertomove", @"c:\temp3\exists", @"c:\temp3" };

            return
                from s in testSourceDirs
                from t in testTargetDirs
                select new TestCaseData(XFS.Path(s), XFS.Path(t));
        }
    }

    [Test]
    [TestCaseSource(nameof(Failing_DirectoryMoveFromToPaths))]
    public async Task Move_Directory_Throws_When_Target_Directory_Parent_Does_Not_Exist(
        string sourceDirName,
        string targetDirName)
    {
        // Arange
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(sourceDirName);

        // Act
        await That(() => fileSystem.Directory.Move(sourceDirName, targetDirName))
            .Throws<DirectoryNotFoundException>();

        // Assert
        await That(fileSystem.Directory.Exists(targetDirName)).IsFalse();
        await That(fileSystem.Directory.Exists(sourceDirName)).IsTrue();
    }

    private static IEnumerable<TestCaseData> Success_DirectoryMoveFromToPaths
    {
        get
        {
            var testTargetDirs = new[]
            {
                @"c:\temp2\", @"c:\temp2", @"c:\temp2\..\temp2", @".\..\temp2", @".\..\temp2\..\temp2",
                @".\..\temp2\fd\df\..\..", @".\..\temp2\fd\df\..\..\", @"..\temp2", @".\temp2", @"\temp2", @"temp2",
            };

            var testSourceDirs = new[] { @"c:\temp3\exists\foldertomove", @"c:\temp3\exists", @"c:\temp4" };

            return
                from s in testSourceDirs
                from t in testTargetDirs
                select new TestCaseData(XFS.Path(s), XFS.Path(t));
        }
    }

    [Test]
    [TestCaseSource(nameof(Success_DirectoryMoveFromToPaths))]
    public async Task Move_Directory_DoesNotThrow_When_Target_Directory_Parent_Exists(
        string sourceDirName,
        string targetDirName)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.Directory.CreateDirectory(sourceDirName);

        // Act
        await That(() => fileSystem.Directory.Move(sourceDirName, targetDirName)).DoesNotThrow();

        // Assert
        await That(fileSystem.Directory.Exists(targetDirName)).IsTrue();
        await That(fileSystem.Directory.Exists(sourceDirName)).IsFalse();
    }

    [Test]
    public async Task MockDirectory_Exists_ShouldReturnTrue_IfArgIsFrontSlashAndRootDirExists()
    {
        string testDir = XFS.Path(@"c:\foo\bar\");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { testDir,  new MockDirectoryData() }
        });

        await That(fileSystem.Directory.Exists("/")).IsEqualTo(true);
    }

    [Test]
    public static void MockDirectory_Move_ShouldNotThrowException_InWindows_When_SourceAndDestinationDifferOnlyInCasing()
    {
        // Arrange
        MockFileSystem mockFs = new MockFileSystem();
        string tempDir = mockFs.Path.GetTempPath();
        string src = mockFs.Path.Combine(tempDir, "src");
        string dest = mockFs.Path.Combine(tempDir, "SRC");
        IDirectoryInfo srcDir = mockFs.DirectoryInfo.New(src);
        srcDir.Create();
        
        // Act & Assert
        Assert.DoesNotThrow(() => mockFs.Directory.Move(src, dest));
    }
}
