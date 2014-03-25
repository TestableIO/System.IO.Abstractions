using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockPathTests
    {
        const string TestPath = "C:\\test\\test.bmp";

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
            Assert.AreEqual("C:\\test\\test.doc", result);
        }

        [Test]
        public void Combine_SentTwoPaths_Combines()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.Combine("C:\\test", "test.bmp");

            //Assert
            Assert.AreEqual("C:\\test\\test.bmp", result);
        }

        [Test]
        public void GetDirectoryName_SentPath_ReturnsDirectory()
        {
            //Arrange
            var mockPath = new MockPath(new MockFileSystem());

            //Act
            var result = mockPath.GetDirectoryName(TestPath);

            //Assert
            Assert.AreEqual("C:\\test", result);
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

        [TestCase(@"c:\a", @"b", @"c:\a\b")]
        [TestCase(@"c:\a\b", @"c", @"c:\a\b\c")]
        [TestCase(@"c:\a\b", @"c\", @"c:\a\b\c\")]
        [TestCase(@"c:\a\b", @".\c\", @"c:\a\b\c\")]
        [TestCase(@"c:\a\b", @"..\c", @"c:\a\c")]
        [TestCase(@"c:\a\b\c", @"..\c\..\", @"c:\a\b\")]
        [TestCase(@"c:\a\b\c", @"..\..\..\..\..\d", @"c:\d")]
        [TestCase(@"c:\a\b\c\", @"..\..\..\..\..\d\", @"c:\d\")]
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

        [TestCase(@"c:\a\b\..\c", @"c:\a\c")]
        [TestCase(@"c:\a\b\.\.\..\.\c", @"c:\a\c")]
        [TestCase(@"c:\a\b\.\c", @"c:\a\b\c")]
        [TestCase(@"c:\a\b\.\.\.\.\c", @"c:\a\b\c")]
        [TestCase(@"c:\a\..\..\c", @"c:\c")]
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

        [TestCase(@"c:\a", @"/b", @"c:\b")]
        [TestCase(@"c:\a", @"/b\", @"c:\b\")]
        [TestCase(@"c:\a", @"\b", @"c:\b")]
        [TestCase(@"c:\a", @"\b\..\c", @"c:\c")]
        [TestCase(@"z:\a", @"\b\..\c", @"z:\c")]
        [TestCase(@"z:\a", @"\b\..\c", @"z:\c")]
        [TestCase(@"z:\a", @"\\computer\share\c", @"\\computer\share\c")]
        [TestCase(@"z:\a", @"\\computer\share\c\..\d", @"\\computer\share\d")]
        [TestCase(@"z:\a", @"\\computer\share\c\..\..\d", @"\\computer\share\d")]
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
            TestDelegate action = () => mockPath.GetFullPath(@"\\shareZ");

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
            Assert.AreEqual("C:\\", result);
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