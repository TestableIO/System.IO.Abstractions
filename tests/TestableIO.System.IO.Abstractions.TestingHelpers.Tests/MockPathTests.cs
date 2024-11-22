﻿using NUnit.Framework;
using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    public class MockPathTests
    {
        static readonly string TestPath = XFS.Path("C:\\test\\test.bmp");

        [Test]
        public void ChangeExtension_ExtensionNoPeriod_PeriodAdded()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.ChangeExtension(TestPath, "doc");

            //Assert
            Assert.That(result, Is.EqualTo(XFS.Path("C:\\test\\test.doc")));
        }

        [Test]
        public void Combine_SentTwoPaths_Combines()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.Combine(XFS.Path("C:\\test"), "test.bmp");

            //Assert
            Assert.That(result, Is.EqualTo(XFS.Path("C:\\test\\test.bmp")));
        }

        [Test]
        public void Combine_SentThreePaths_Combines()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.Combine(XFS.Path("C:\\test"), "subdir1", "test.bmp");

            //Assert
            Assert.That(result, Is.EqualTo(XFS.Path("C:\\test\\subdir1\\test.bmp")));
        }

        [Test]
        public void Combine_SentFourPaths_Combines()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.Combine(XFS.Path("C:\\test"), "subdir1", "subdir2", "test.bmp");

            //Assert
            Assert.That(result, Is.EqualTo(XFS.Path("C:\\test\\subdir1\\subdir2\\test.bmp")));
        }

        [Test]
        public void Combine_SentFivePaths_Combines()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.Combine(XFS.Path("C:\\test"), "subdir1", "subdir2", "subdir3", "test.bmp");

            //Assert
            Assert.That(result, Is.EqualTo(XFS.Path("C:\\test\\subdir1\\subdir2\\subdir3\\test.bmp")));
        }

        [Test]
        public void GetDirectoryName_SentPath_ReturnsDirectory()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetDirectoryName(TestPath);

            //Assert
            Assert.That(result, Is.EqualTo(XFS.Path("C:\\test")));
        }

        [Test]
        public void GetExtension_SendInPath_ReturnsExtension()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetExtension(TestPath);

            //Assert
            Assert.That(result, Is.EqualTo(".bmp"));
        }

        [Test]
        public void GetFileName_SendInPath_ReturnsFilename()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetFileName(TestPath);

            //Assert
            Assert.That(result, Is.EqualTo("test.bmp"));
        }

        [Test]
        public void GetFileNameWithoutExtension_SendInPath_ReturnsFileNameNoExt()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetFileNameWithoutExtension(TestPath);

            //Assert
            Assert.That(result, Is.EqualTo("test"));
        }

        [Test]
        public void GetFullPath_SendInPath_ReturnsFullPath()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetFullPath(TestPath);

            //Assert
            Assert.That(result, Is.EqualTo(TestPath));
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
        public void GetFullPath_RelativePaths_ShouldReturnTheAbsolutePathWithCurrentDirectory(string currentDir, string relativePath, string expectedResult)
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Directory.SetCurrentDirectory(currentDir);
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualResult = mockPath.GetFullPath(relativePath);

            //Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
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
        public void GetFullPath_RootedPathWithRelativeSegments_ShouldReturnAnRootedAbsolutePath(string rootedPath, string expectedResult)
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualResult = mockPath.GetFullPath(rootedPath);

            //Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
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
        public void GetFullPath_AbsolutePaths_ShouldReturnThePathWithTheRoot_Or_Unc(string currentDir, string absolutePath, string expectedResult)
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Directory.SetCurrentDirectory(currentDir);
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualResult = mockPath.GetFullPath(absolutePath);

            //Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetFullPath_InvalidUNCPaths_ShouldThrowArgumentException()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            TestDelegate action = () => mockPath.GetFullPath(XFS.Path(@"\\shareZ"));

            //Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void GetFullPath_NullValue_ShouldThrowArgumentNullException()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            TestDelegate action = () => mockPath.GetFullPath(null);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void GetFullPath_EmptyValue_ShouldThrowArgumentException()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            TestDelegate action = () => mockPath.GetFullPath(string.Empty);

            //Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void GetFullPath_WithWhiteSpace_ShouldThrowArgumentException()
        {
            var mockFileSystem = new MockFileSystem();

            TestDelegate action = () => mockFileSystem.Path.GetFullPath("  ");

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void GetFullPath_WithMultipleDirectorySeparators_ShouldReturnTheNormalizedForm()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualFullPath = mockPath.GetFullPath(XFS.Path(@"c:\foo\\//bar\file.dat"));

            //Assert
            Assert.That(actualFullPath, Is.EqualTo(XFS.Path(@"c:\foo\bar\file.dat")));
        }

        [Test]
        public void GetInvalidFileNameChars_Called_ReturnsChars()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetInvalidFileNameChars();

            //Assert
            Assert.That(result.Length > 0, Is.True);
        }

        [Test]
        public void GetInvalidPathChars_Called_ReturnsChars()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetInvalidPathChars();

            //Assert
            Assert.That(result.Length > 0, Is.True);
        }

        [Test]
        public void GetPathRoot_SendInPath_ReturnsRoot()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetPathRoot(TestPath);

            //Assert
            Assert.That(result, Is.EqualTo(XFS.Path("C:\\")));
        }

        [Test]
        public void GetRandomFileName_Called_ReturnsStringLengthGreaterThanZero()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetRandomFileName();

            //Assert
            Assert.That(result.Length > 0, Is.True);
        }

        [Test]
        public void GetTempFileName_Called_ReturnsStringLengthGreaterThanZero()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetTempFileName();

            //Assert
            Assert.That(result.Length > 0, Is.True);
        }

        [Test]
        public void GetTempFileName_Called_CreatesEmptyFileInTempDirectory()
        {
            //Arrange
            var fileSystem = new MockFileSystem();
            var mockPath = new MockPath(fileSystem);

            //Act
            var result = mockPath.GetTempFileName();

            Assert.That(fileSystem.FileExists(result), Is.True);
            Assert.That(fileSystem.FileInfo.New(result).Length, Is.Zero);
        }

        [Test]
        public void GetTempPath_Called_ReturnsStringLengthGreaterThanZero()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            Assert.That(result.Length > 0, Is.True);
        }

        [Test]
        public void GetTempPath_ShouldEndWithDirectorySeparator()
        {
            //Arrange
            var mockPath = new MockFileSystem().Path;
            var directorySeparator = mockPath.DirectorySeparatorChar.ToString();

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            Assert.That(result, Does.EndWith(directorySeparator));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(@"C:\temp")]
        public void GetTempPath_Called_ReturnsStringLengthGreaterThanZero(string tempDirectory)
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem(), string.IsNullOrEmpty(tempDirectory) ? tempDirectory : XFS.Path(tempDirectory));

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            Assert.That(result.Length > 0, Is.True);
        }

        [Test]
        public void GetTempPath_Called_WithNonNullVirtualTempDirectory_ReturnsVirtualTempDirectory()
        {
            //Arrange
            var tempDirectory = XFS.Path(@"C:\temp");

            var mockPath = new MockPath(new MockFileSystem(), tempDirectory);

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            Assert.That(result, Is.EqualTo(tempDirectory));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void GetTempPath_Called_WithNullOrEmptyVirtualTempDirectory_ReturnsFallbackTempDirectory(string tempDirectory)
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem(), tempDirectory);

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            Assert.That(result.Length > 0, Is.True);
        }

        [Test]
        public void HasExtension_PathSentIn_DeterminesExtension()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.HasExtension(TestPath);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsPathRooted_PathSentIn_DeterminesPathExists()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.IsPathRooted(TestPath);

            //Assert
            Assert.That(result, Is.True);
        }

#if FEATURE_ADVANCED_PATH_OPERATIONS
        [Test]
        public void IsPathFullyQualified_WithAbsolutePath_ReturnsTrue()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.IsPathFullyQualified(XFS.Path("C:\\directory\\file.txt"));

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsPathFullyQualified_WithRelativePath_ReturnsFalse()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.IsPathRooted(XFS.Path("directory\\file.txt"));

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsPathFullyQualified_WithRelativePathParts_ReturnsFalse()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.IsPathRooted(XFS.Path("directory\\..\\file.txt"));

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetRelativePath_Works()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetRelativePath(XFS.Path("c:\\d"), XFS.Path("c:\\d\\e\\f.txt"));

            //Assert
            Assert.That(result, Is.EqualTo(XFS.Path("e\\f.txt")));
        }

        [Test]
        public void GetRelativePath_WhenPathIsNull_ShouldThrowArgumentNullException()
        {
            var mockPath = new MockFileSystem().Path;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                mockPath.GetRelativePath("foo", null);
            });

            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void GetRelativePath_WhenPathIsWhitespace_ShouldThrowArgumentException()
        {
            var mockPath = new MockFileSystem().Path;

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                mockPath.GetRelativePath("foo", " ");
            });

            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void GetRelativePath_WhenRelativeToNull_ShouldThrowArgumentNullException()
        {
            var mockPath = new MockFileSystem().Path;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                mockPath.GetRelativePath(null, "foo");
            });

            Assert.That(exception.ParamName, Is.EqualTo("relativeTo"));
        }

        [Test]
        public void GetRelativePath_WhenRelativeToIsWhitespace_ShouldThrowArgumentException()
        {
            var mockPath = new MockFileSystem().Path;

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                mockPath.GetRelativePath(" ", "foo");
            });

            Assert.That(exception.ParamName, Is.EqualTo("relativeTo"));
        }
#endif

#if FEATURE_PATH_EXISTS
        [Test]
        public void Exists_Null_ShouldReturnFalse()
        {
            var fileSystem = new MockFileSystem();
            bool result = fileSystem.Path.Exists(null);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Exists_ShouldWorkWithAbsolutePaths()
        {
            var fileSystem = new MockFileSystem();
            string path = "some-path";
            string absolutePath = fileSystem.Path.GetFullPath(path);
            fileSystem.Directory.CreateDirectory(path);

            bool result = fileSystem.Path.Exists(absolutePath);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Exists_ExistingFile_ShouldReturnTrue()
        {
            var fileSystem = new MockFileSystem();
            string path = "some-path";
            fileSystem.File.WriteAllText(path, "some content");

            bool result = fileSystem.Path.Exists(path);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Exists_ExistingDirectory_ShouldReturnTrue()
        {
            var fileSystem = new MockFileSystem();
            string path = "some-path";
            fileSystem.Directory.CreateDirectory(path);

            bool result = fileSystem.Path.Exists(path);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Exists_ExistingFileOrDirectory_ShouldReturnTrue()
        {
            var fileSystem = new MockFileSystem();
            string path = "some-path";
            bool result = fileSystem.Path.Exists(path);

            Assert.That(result, Is.False);
        }
#endif

#if FEATURE_ADVANCED_PATH_OPERATIONS
        [Test]
        public void GetRelativePath_ShouldUseCurrentDirectoryFromMockFileSystem()
        {
            var fs = new MockFileSystem();

            fs.AddDirectory("input");
            fs.AddDirectory("output");
            fs.Directory.SetCurrentDirectory("input");

            fs.AddFile("input/a.txt", "foo");

            var result = fs.Path.GetRelativePath("/input", "a.txt");

            Assert.That(result, Is.EqualTo("a.txt"));
        }
#endif
    }
}
