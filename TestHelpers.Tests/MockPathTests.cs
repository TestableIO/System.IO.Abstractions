using NUnit.Framework;
using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockPathTests
    {
        static readonly string TestPath = XFS
            .ForWin("C:\\test\\test.bmp")
            .ForUnix("/test/test.bmp");

        private MockPath SetupMockPath()
        {
            return new MockPath(null);
        }

        [Test]
        public void ChangeExtension_ExtensionNoPeriod_PeriodAdded()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.ChangeExtension(TestPath, "doc");

            //Assert
            Assert.AreEqual(XFS
                .ForWin("C:\\test\\test.doc")
                .ForUnix("/test/test.doc")
                , result);
        }

        [Test]
        public void Combine_SentTwoPaths_Combines()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.Combine(XFS
                .ForWin("C:\\test")
                .ForUnix("/test")
                , "test.bmp");

            //Assert
            Assert.AreEqual(XFS
                .ForWin("C:\\test\\test.bmp")
                .ForUnix("/test/test.bmp")
                , result);
        }

        [Test]
        public void GetDirectoryName_SentPath_ReturnsDirectory()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetDirectoryName(TestPath);

            //Assert
            Assert.AreEqual(XFS
                .ForWin("C:\\test")
                .ForUnix("/test")
                , result);
        }

        [Test]
        public void GetExtension_SendInPath_ReturnsExtension()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetExtension(TestPath);

            //Assert
            Assert.AreEqual(".bmp", result);
        }

        [Test]
        public void GetFileName_SendInPath_ReturnsFilename()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetFileName(TestPath);

            //Assert
            Assert.AreEqual("test.bmp", result);
        }

        [Test]
        public void GetFileNameWithoutExtension_SendInPath_ReturnsFileNameNoExt()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetFileNameWithoutExtension(TestPath);

            //Assert
            Assert.AreEqual("test", result);
        }

        [Test]
        public void GetFullPath_SendInPath_ReturnsFullPath()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetFullPath(TestPath);

            //Assert
            Assert.AreEqual(TestPath, result);
        }

        public static IEnumerable<string[]> GetFullPath_RelativePaths_Cases
        {
            get
            {
                yield return new [] { XFS.ForWin(@"c:\a").ForUnix("/a"), "b", XFS.ForWin(@"c:\a\b").ForUnix("/a/b") };
                yield return new [] { XFS.ForWin(@"c:\a\b").ForUnix("/a/b"), "c", XFS.ForWin(@"c:\a\b\c").ForUnix("/a/b/c") };
                yield return new [] { XFS.ForWin(@"c:\a\b").ForUnix("/a/b"), XFS.ForWin(@"c\").ForUnix("c/"), XFS.ForWin(@"c:\a\b\c\").ForUnix("/a/b/c/") };
                yield return new [] { XFS.ForWin(@"c:\a\b").ForUnix("/a/b"), XFS.ForWin(@"..\c").ForUnix("../c"), XFS.ForWin(@"c:\a\c").ForUnix("/a/c") };
                yield return new [] { XFS.ForWin(@"c:\a\b\c").ForUnix("/a/b/c"), XFS.ForWin(@"..\c\..\").ForUnix("../c/../"), XFS.ForWin(@"c:\a\b\").ForUnix("/a/b/") };
                yield return new [] { XFS.ForWin(@"c:\a\b\c").ForUnix("/a/b/c"), XFS.ForWin(@"..\..\..\..\..\d").ForUnix("../../../../../d"), XFS.ForWin(@"c:\d").ForUnix("/d") };
                yield return new [] { XFS.ForWin(@"c:\a\b\c").ForUnix("/a/b/c"), XFS.ForWin(@"..\..\..\..\..\d\").ForUnix("../../../../../d/"), XFS.ForWin(@"c:\d\").ForUnix("/d/") };
            }
        }

        [TestCaseSource("GetFullPath_RelativePaths_Cases")]
        public void GetFullPath_RelativePaths_ShouldReturnTheAbsolutePathWithCurrentDirectory(string currentDir, string relativePath, string expectedResult)
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Directory.SetCurrentDirectory(currentDir);
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualResult = mockPath.GetFullPath(relativePath);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        public static IEnumerable<string[]> GetFullPath_RootedPathWithRelativeSegments_Cases
        {
            get
            {
                yield return new [] { XFS.ForWin(@"c:\a\b\..\c").ForUnix("/a/b/../c"), XFS.ForWin(@"c:\a\c").ForUnix("/a/c") };
                yield return new [] { XFS.ForWin(@"c:\a\b\.\.\..\.\c").ForUnix("/a/b/././.././c"), XFS.ForWin(@"c:\a\c").ForUnix("/a/c") };
                yield return new [] { XFS.ForWin(@"c:\a\b\.\c").ForUnix("/a/b/./c"), XFS.ForWin(@"c:\a\b\c").ForUnix("/a/b/c") };
                yield return new [] { XFS.ForWin(@"c:\a\b\.\.\.\.\c").ForUnix("/a/b/././././c"), XFS.ForWin(@"c:\a\b\c").ForUnix("/a/b/c") };
                yield return new [] { XFS.ForWin(@"c:\a\..\..\c").ForUnix("/a/../../c"), XFS.ForWin(@"c:\c").ForUnix("/c") };
            }
        }

        [TestCaseSource("GetFullPath_RootedPathWithRelativeSegments_Cases")]
        public void GetFullPath_RootedPathWithRelativeSegments_ShouldReturnAnRootedAbsolutePath(string rootedPath, string expectedResult)
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualResult = mockPath.GetFullPath(rootedPath);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        public static IEnumerable<string[]> GetFullPath_AbsolutePaths_Cases
        {
            get
            {
                yield return new [] { XFS.ForWin(@"c:\a").ForUnix("/a"), XFS.ForWin(@"/b").ForUnix(@"/b"), XFS.ForWin(@"c:\b").ForUnix("/b") };
                yield return new [] { XFS.ForWin(@"c:\a").ForUnix("/a"), XFS.ForWin(@"/b\").ForUnix(@"/b/"), XFS.ForWin(@"c:\b\").ForUnix("/b/") };
                yield return new [] { XFS.ForWin(@"c:\a").ForUnix("/a"), XFS.ForWin(@"\b").ForUnix(@"/b"), XFS.ForWin(@"c:\b").ForUnix("/b") };
                yield return new [] { XFS.ForWin(@"c:\a").ForUnix("/a"), XFS.ForWin(@"\b\..c").ForUnix(@"/b/../c"), XFS.ForWin(@"c:\c").ForUnix("/c") };
                yield return new [] { XFS.ForWin(@"z:\a").ForUnix("/a"), XFS.ForWin(@"\b\..c").ForUnix(@"/b/../c"), XFS.ForWin(@"z:\c").ForUnix("/c") };
                yield return new [] { XFS.ForWin(@"z:\a").ForUnix("/a"), XFS.ForWin(@"\\computer\share\c").ForUnix(@"//computer/share/c"), XFS.ForWin(@"\\computer\share\c").ForUnix("//computer/share/c") };
                yield return new [] { XFS.ForWin(@"z:\a").ForUnix("/a"), XFS.ForWin(@"\\computer\share\c\..\d").ForUnix(@"//computer/share/c/../d"), XFS.ForWin(@"\\computer\share\d").ForUnix("//computer/share/d") };
                yield return new [] { XFS.ForWin(@"z:\a").ForUnix("/a"), XFS.ForWin(@"\\computer\share\c\..\..\d").ForUnix(@"//computer/share/c/../../d"), XFS.ForWin(@"\\computer\share\d").ForUnix("//computer/share/d") };
            }
        }

        [TestCaseSource("GetFullPath_AbsolutePaths_Cases")]
        public void GetFullPath_AbsolutePaths_ShouldReturnThePathWithTheRoot_Or_Unc(string currentDir, string absolutePath, string expectedResult)
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Directory.SetCurrentDirectory(currentDir);
            var mockPath = new MockPath(mockFileSystem);

            //Act
            var actualResult = mockPath.GetFullPath(absolutePath);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void GetFullPath_InvalidUNCPaths_ShouldThrowArgumentException()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockPath = new MockPath(mockFileSystem);

            //Act
            TestDelegate action = () => mockPath.GetFullPath(XFS.ForWin(@"\\shareZ").ForUnix(@"//shareZ"));

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
        public void GetInvalidFileNameChars_Called_ReturnsChars()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetInvalidFileNameChars();

            //Assert
            Assert.IsTrue(result.Length > 0);
        }

        [Test]
        public void GetInvalidPathChars_Called_ReturnsChars()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetInvalidPathChars();

            //Assert
            Assert.IsTrue(result.Length > 0);
        }

        [Test]
        public void GetPathRoot_SendInPath_ReturnsRoot()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetPathRoot(TestPath);

            //Assert
            Assert.AreEqual(XFS
                .ForWin("C:\\")
                .ForUnix("/")
                , result);
        }

        [Test]
        public void GetRandomFileName_Called_ReturnsStringLengthGreaterThanZero()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetRandomFileName();

            //Assert
            Assert.IsTrue(result.Length > 0);
        }

        [Test]
        public void GetTempFileName_Called_ReturnsStringLengthGreaterThanZero()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetTempFileName();

            //Assert
            Assert.IsTrue(result.Length > 0);
        }

        [Test]
        public void GetTempFileName_Called_CreatesEmptyFileInTempDirectory()
        {
            //Arrange
            var fileSystem = new MockFileSystem();
            var mockPath = new MockPath(fileSystem);

            //Act
            var result = mockPath.GetTempFileName();

            Assert.True(fileSystem.FileExists(result));
            Assert.AreEqual(0, fileSystem.FileInfo.FromFileName(result).Length);
        }

        [Test]
        public void GetTempPath_Called_ReturnsStringLengthGreaterThanZero()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetTempPath();

            //Assert
            Assert.IsTrue(result.Length > 0);
        }

        [Test]
        public void HasExtension_PathSentIn_DeterminesExtension()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.HasExtension(TestPath);

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsPathRooted_PathSentIn_DeterminesPathExists()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.IsPathRooted(TestPath);

            //Assert
            Assert.AreEqual(true, result);
        }
    }
}