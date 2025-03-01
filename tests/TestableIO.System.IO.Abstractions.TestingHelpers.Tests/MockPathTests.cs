using NUnit.Framework;
using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    public class MockPathTests
    {
        static readonly string TestPath = XFS.Path("C:\\test\\test.bmp");

        [Test]
        public async Task ChangeExtension_ExtensionNoPeriod_PeriodAdded()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.ChangeExtension(TestPath, "doc");

            //Assert
            await That(result).IsEqualTo(XFS.Path("C:\\test\\test.doc"));
        }

        [Test]
        public async Task Combine_SentTwoPaths_Combines()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.Combine(XFS.Path("C:\\test"), "test.bmp");

            //Assert
            await That(result).IsEqualTo(XFS.Path("C:\\test\\test.bmp"));
        }

        [Test]
        public async Task Combine_SentThreePaths_Combines()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.Combine(XFS.Path("C:\\test"), "subdir1", "test.bmp");

            //Assert
            await That(result).IsEqualTo(XFS.Path("C:\\test\\subdir1\\test.bmp"));
        }

        [Test]
        public async Task Combine_SentFourPaths_Combines()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.Combine(XFS.Path("C:\\test"), "subdir1", "subdir2", "test.bmp");

            //Assert
            await That(result).IsEqualTo(XFS.Path("C:\\test\\subdir1\\subdir2\\test.bmp"));
        }

        [Test]
        public async Task Combine_SentFivePaths_Combines()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.Combine(XFS.Path("C:\\test"), "subdir1", "subdir2", "subdir3", "test.bmp");

            //Assert
            await That(result).IsEqualTo(XFS.Path("C:\\test\\subdir1\\subdir2\\subdir3\\test.bmp"));
        }

        [Test]
        public async Task GetDirectoryName_SentPath_ReturnsDirectory()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetDirectoryName(TestPath);

            //Assert
            await That(result).IsEqualTo(XFS.Path("C:\\test"));
        }

        [Test]
        public async Task GetExtension_SendInPath_ReturnsExtension()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetExtension(TestPath);

            //Assert
            await That(result).IsEqualTo(".bmp");
        }

        [Test]
        public async Task GetFileName_SendInPath_ReturnsFilename()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetFileName(TestPath);

            //Assert
            await That(result).IsEqualTo("test.bmp");
        }

        [Test]
        public async Task GetFileNameWithoutExtension_SendInPath_ReturnsFileNameNoExt()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetFileNameWithoutExtension(TestPath);

            //Assert
            await That(result).IsEqualTo("test");
        }

        [Test]
        public async Task GetFullPath_SendInPath_ReturnsFullPath()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetFullPath(TestPath);

            //Assert
            await That(result).IsEqualTo(TestPath);
        }

        public static IEnumerable<string[]> GetFullPath_RelativePaths_Cases
        {
            get
            {
                yield return new[] { XFS.Path(@"c:\a"), "b", XFS.Path(@"c:\a\b") };
                yield return new[] { XFS.Path(@"c:\a\b"), "c", XFS.Path(@"c:\a\b\c") };
                yield return new[] { XFS.Path(@"c:\a\b"), XFS.Path(@"c\"), XFS.Path(@"c:\a\b\c\") };
                yield return new[] { XFS.Path(@"c:\a\b"), XFS.Path(@"..\c"), XFS.Path(@"c:\a\c") };
                yield return new[] { XFS.Path(@"c:\a\b\c"), XFS.Path(@"..\c\..\"), XFS.Path(@"c:\a\b\") };
                yield return new[] { XFS.Path(@"c:\a\b\c"), XFS.Path(@"..\..\..\..\..\d"), XFS.Path(@"c:\d") };
                yield return new[] { XFS.Path(@"c:\a\b\c"), XFS.Path(@"..\..\..\..\..\d\"), XFS.Path(@"c:\d\") };
            }
        }

        [TestCaseSource(nameof(GetFullPath_RelativePaths_Cases))]
        public async Task GetFullPath_RelativePaths_ShouldReturnTheAbsolutePathWithCurrentDirectory(string currentDir, string relativePath, string expectedResult)
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Directory.SetCurrentDirectory(currentDir);
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualResult = mockPath.GetFullPath(relativePath);

            //Assert
            await That(actualResult).IsEqualTo(expectedResult);
        }

        public static IEnumerable<string[]> GetFullPath_RootedPathWithRelativeSegments_Cases
        {
            get
            {
                yield return new[] { XFS.Path(@"c:\a\b\..\c"), XFS.Path(@"c:\a\c") };
                yield return new[] { XFS.Path(@"c:\a\b\.\.\..\.\c"), XFS.Path(@"c:\a\c") };
                yield return new[] { XFS.Path(@"c:\a\b\.\c"), XFS.Path(@"c:\a\b\c") };
                yield return new[] { XFS.Path(@"c:\a\b\.\.\.\.\c"), XFS.Path(@"c:\a\b\c") };
                yield return new[] { XFS.Path(@"c:\a\..\..\c"), XFS.Path(@"c:\c") };
            }
        }

        [TestCaseSource(nameof(GetFullPath_RootedPathWithRelativeSegments_Cases))]
        public async Task GetFullPath_RootedPathWithRelativeSegments_ShouldReturnAnRootedAbsolutePath(string rootedPath, string expectedResult)
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualResult = mockPath.GetFullPath(rootedPath);

            //Assert
            await That(actualResult).IsEqualTo(expectedResult);
        }

        public static IEnumerable<string[]> GetFullPath_AbsolutePaths_Cases
        {
            get
            {
                yield return new[] { XFS.Path(@"c:\a"), XFS.Path(@"/b"), XFS.Path(@"c:\b") };
                yield return new[] { XFS.Path(@"c:\a"), XFS.Path(@"/b\"), XFS.Path(@"c:\b\") };
                yield return new[] { XFS.Path(@"c:\a"), XFS.Path(@"\b"), XFS.Path(@"c:\b") };
                yield return new[] { XFS.Path(@"c:\a"), XFS.Path(@"\b\..\c"), XFS.Path(@"c:\c") };
                yield return new[] { XFS.Path(@"z:\a"), XFS.Path(@"\b\..\c"), XFS.Path(@"z:\c") };
                yield return new[] { XFS.Path(@"z:\a"), XFS.Path(@"\\computer\share\c"), XFS.Path(@"\\computer\share\c") };
                yield return new[] { XFS.Path(@"z:\a"), XFS.Path(@"\\computer\share\c\..\d"), XFS.Path(@"\\computer\share\d") };
                yield return new[] { XFS.Path(@"z:\a"), XFS.Path(@"\\computer\share\c\..\..\d"), XFS.Path(@"\\computer\share\d") };
            }
        }

        [TestCaseSource(nameof(GetFullPath_AbsolutePaths_Cases))]
        public async Task GetFullPath_AbsolutePaths_ShouldReturnThePathWithTheRoot_Or_Unc(string currentDir, string absolutePath, string expectedResult)
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Directory.SetCurrentDirectory(currentDir);
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualResult = mockPath.GetFullPath(absolutePath);

            //Assert
            await That(actualResult).IsEqualTo(expectedResult);
        }

        [Test]
        public async Task GetFullPath_InvalidUNCPaths_ShouldThrowArgumentException()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            Action action = () => mockPath.GetFullPath(XFS.Path(@"\\shareZ"));

            //Assert
            await That(action).Throws<ArgumentException>();
        }

        [Test]
        public async Task GetFullPath_NullValue_ShouldThrowArgumentNullException()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            Action action = () => mockPath.GetFullPath(null);

            //Assert
            await That(action).Throws<ArgumentNullException>();
        }

        [Test]
        public async Task GetFullPath_EmptyValue_ShouldThrowArgumentException()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            Action action = () => mockPath.GetFullPath(string.Empty);

            //Assert
            await That(action).Throws<ArgumentException>();
        }

        [Test]
        public async Task GetFullPath_WithWhiteSpace_ShouldThrowArgumentException()
        {
            var mockFileSystem = new MockFileSystem();

            Action action = () => mockFileSystem.Path.GetFullPath("  ");

            await That(action).Throws<ArgumentException>();
        }

        [Test]
        public async Task GetFullPath_WithMultipleDirectorySeparators_ShouldReturnTheNormalizedForm()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualFullPath = mockPath.GetFullPath(XFS.Path(@"c:\foo\\//bar\file.dat"));

            //Assert
            await That(actualFullPath).IsEqualTo(XFS.Path(@"c:\foo\bar\file.dat"));
        }

        [Test]
        public async Task GetInvalidFileNameChars_Called_ReturnsChars()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetInvalidFileNameChars();

            //Assert
            await That(result.Length > 0).IsTrue();
        }

        [Test]
        public async Task GetInvalidPathChars_Called_ReturnsChars()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetInvalidPathChars();

            //Assert
            await That(result.Length > 0).IsTrue();
        }

        [Test]
        public async Task GetPathRoot_SendInPath_ReturnsRoot()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetPathRoot(TestPath);

            //Assert
            await That(result).IsEqualTo(XFS.Path("C:\\"));
        }

        [Test]
        public async Task GetRandomFileName_Called_ReturnsStringLengthGreaterThanZero()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetRandomFileName();

            //Assert
            await That(result.Length > 0).IsTrue();
        }

        [Test]
        public async Task GetTempFileName_Called_ReturnsStringLengthGreaterThanZero()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetTempFileName();

            //Assert
            await That(result.Length > 0).IsTrue();
        }

        [Test]
        public async Task GetTempFileName_Called_CreatesEmptyFileInTempDirectory()
        {
            //Arrange
            var fileSystem = new MockFileSystem();
            var mockPath = new MockPath(fileSystem);

            //Act
            var result = mockPath.GetTempFileName();

            await That(fileSystem.FileExists(result)).IsTrue();
            await That(fileSystem.FileInfo.New(result).Length).IsEqualTo(0);
        }

        [Test]
        public async Task GetTempPath_Called_ReturnsStringLengthGreaterThanZero()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            await That(result.Length > 0).IsTrue();
        }

        [Test]
        public async Task GetTempPath_ShouldEndWithDirectorySeparator()
        {
            //Arrange
            var mockPath = new MockFileSystem().Path;
            var directorySeparator = mockPath.DirectorySeparatorChar.ToString();

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            await That(result).EndsWith(directorySeparator);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(@"C:\temp")]
        public async Task GetTempPath_Called_ReturnsStringLengthGreaterThanZero(string tempDirectory)
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem(), string.IsNullOrEmpty(tempDirectory) ? tempDirectory : XFS.Path(tempDirectory));

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            await That(result.Length > 0).IsTrue();
        }

        [Test]
        public async Task GetTempPath_Called_WithNonNullVirtualTempDirectory_ReturnsVirtualTempDirectory()
        {
            //Arrange
            var tempDirectory = XFS.Path(@"C:\temp");

            var mockPath = new MockPath(new MockFileSystem(), tempDirectory);

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            await That(result).IsEqualTo(tempDirectory);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task GetTempPath_Called_WithNullOrEmptyVirtualTempDirectory_ReturnsFallbackTempDirectory(string tempDirectory)
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem(), tempDirectory);

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            await That(result.Length > 0).IsTrue();
        }

        [Test]
        public async Task HasExtension_PathSentIn_DeterminesExtension()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.HasExtension(TestPath);

            //Assert
            await That(result).IsTrue();
        }

        [Test]
        public async Task IsPathRooted_PathSentIn_DeterminesPathExists()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.IsPathRooted(TestPath);

            //Assert
            await That(result).IsTrue();
        }

#if FEATURE_ADVANCED_PATH_OPERATIONS
        [Test]
        public async Task IsPathFullyQualified_WithAbsolutePath_ReturnsTrue()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.IsPathFullyQualified(XFS.Path("C:\\directory\\file.txt"));

            //Assert
            await That(result).IsTrue();
        }

        [Test]
        public async Task IsPathFullyQualified_WithRelativePath_ReturnsFalse()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.IsPathRooted(XFS.Path("directory\\file.txt"));

            //Assert
            await That(result).IsFalse();
        }

        [Test]
        public async Task IsPathFullyQualified_WithRelativePathParts_ReturnsFalse()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.IsPathRooted(XFS.Path("directory\\..\\file.txt"));

            //Assert
            await That(result).IsFalse();
        }

        [Test]
        public async Task GetRelativePath_Works()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetRelativePath(XFS.Path("c:\\d"), XFS.Path("c:\\d\\e\\f.txt"));

            //Assert
            await That(result).IsEqualTo(XFS.Path("e\\f.txt"));
        }

        [Test]
        public async Task GetRelativePath_WhenPathIsNull_ShouldThrowArgumentNullException()
        {
            var mockPath = new MockFileSystem().Path;

            var exception = await That(() =>
            {
                mockPath.GetRelativePath("foo", null);
            }).Throws<ArgumentNullException>();

            await That(exception.ParamName).IsEqualTo("path");
        }

        [Test]
        public async Task GetRelativePath_WhenPathIsWhitespace_ShouldThrowArgumentException()
        {
            var mockPath = new MockFileSystem().Path;

            var exception = await That(() =>
            {
                mockPath.GetRelativePath("foo", " ");
            }).Throws<ArgumentException>();

            await That(exception.ParamName).IsEqualTo("path");
        }

        [Test]
        public async Task GetRelativePath_WhenRelativeToNull_ShouldThrowArgumentNullException()
        {
            var mockPath = new MockFileSystem().Path;

            var exception = await That(() =>
            {
                mockPath.GetRelativePath(null, "foo");
            }).Throws<ArgumentNullException>();

            await That(exception.ParamName).IsEqualTo("relativeTo");
        }

        [Test]
        public async Task GetRelativePath_WhenRelativeToIsWhitespace_ShouldThrowArgumentException()
        {
            var mockPath = new MockFileSystem().Path;

            var exception = await That(() =>
            {
                mockPath.GetRelativePath(" ", "foo");
            }).Throws<ArgumentException>();

            await That(exception.ParamName).IsEqualTo("relativeTo");
        }
#endif

#if FEATURE_PATH_EXISTS
        [Test]
        public async Task Exists_Null_ShouldReturnFalse()
        {
            var fileSystem = new MockFileSystem();
            bool result = fileSystem.Path.Exists(null);

            await That(result).IsFalse();
        }

        [Test]
        public async Task Exists_ShouldWorkWithAbsolutePaths()
        {
            var fileSystem = new MockFileSystem();
            string path = "some-path";
            string absolutePath = fileSystem.Path.GetFullPath(path);
            fileSystem.Directory.CreateDirectory(path);

            bool result = fileSystem.Path.Exists(absolutePath);

            await That(result).IsTrue();
        }

        [Test]
        public async Task Exists_ExistingFile_ShouldReturnTrue()
        {
            var fileSystem = new MockFileSystem();
            string path = "some-path";
            fileSystem.File.WriteAllText(path, "some content");

            bool result = fileSystem.Path.Exists(path);

            await That(result).IsTrue();
        }

        [Test]
        public async Task Exists_ExistingDirectory_ShouldReturnTrue()
        {
            var fileSystem = new MockFileSystem();
            string path = "some-path";
            fileSystem.Directory.CreateDirectory(path);

            bool result = fileSystem.Path.Exists(path);

            await That(result).IsTrue();
        }

        [Test]
        public async Task Exists_ExistingFileOrDirectory_ShouldReturnTrue()
        {
            var fileSystem = new MockFileSystem();
            string path = "some-path";
            bool result = fileSystem.Path.Exists(path);

            await That(result).IsFalse();
        }
#endif

#if FEATURE_ADVANCED_PATH_OPERATIONS
        [Test]
        public async Task GetRelativePath_ShouldUseCurrentDirectoryFromMockFileSystem()
        {
            var fs = new MockFileSystem();

            fs.AddDirectory("input");
            fs.AddDirectory("output");
            fs.Directory.SetCurrentDirectory("input");

            fs.AddFile("input/a.txt", "foo");

            var result = fs.Path.GetRelativePath("/input", "a.txt");

            await That(result).IsEqualTo("a.txt");
        }
#endif
    }
}
