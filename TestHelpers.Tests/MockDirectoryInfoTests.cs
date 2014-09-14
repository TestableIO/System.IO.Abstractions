using System.Collections.Generic;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockDirectoryInfoTests
    {
        [Test]
        public void MockDirectoryInfo_GetExtension_ShouldReturnEmptyString()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.ForWin(@"c:\temp").ForUnix("/temp"));

            // Act
            var result = directoryInfo.Extension;

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void MockDirectoryInfo_GetExtensionWithTrailingSlash_ShouldReturnEmptyString()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.ForWin(@"c:\temp\").ForUnix("/temp/"));

            // Act
            var result = directoryInfo.Extension;

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        public static IEnumerable<object[]> MockDirectoryInfo_Exists_Cases
        {
            get
            {
                yield return new object[]{ XFS.ForWin(@"c:\temp\folder").ForUnix("/temp/folder"), true };
                yield return new object[]{ XFS.ForWin(@"c:\temp\folder\notExistant").ForUnix("/temp/folder/notExistant"), false };
            }
        }

        [TestCaseSource("MockDirectoryInfo_Exists_Cases")]
        public void MockDirectoryInfo_Exists(string path, bool expected) 
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> 
            {
                {XFS.ForWin(@"c:\temp\folder\file.txt").ForUnix("/temp/folder/file.txt"), new MockFileData("Hello World")}
            });
            var directoryInfo = new MockDirectoryInfo(fileSystem, path);

            var result = directoryInfo.Exists;

            Assert.That(result, Is.EqualTo(expected));
        }
  
        [Test]
        public void MockDirectoryInfo_FullName_ShouldReturnFullNameWithoutIncludingTrailingPathDelimiter() 
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {
                    XFS.ForWin(@"c:\temp\folder\file.txt").ForUnix("/temp/folder/file.txt"),
                        new MockFileData("Hello World")
                }
            });
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.ForWin(@"c:\temp\folder").ForUnix("/temp/folder"));

            var result = directoryInfo.FullName;

            Assert.That(result, Is.EqualTo(XFS.ForWin(@"c:\temp\folder").ForUnix("/temp/folder")));
        }

        [Test]
        public void MockDirectoryInfo_GetFileSystemInfos_ShouldReturnBothDirectoriesAndFiles()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.ForWin(@"c:\temp\folder\file.txt").ForUnix("/temp/folder/file.txt"), new MockFileData("Hello World") },
                { XFS.ForWin(@"c:\temp\folder\folder").ForUnix("/temp/folder/folder"), new MockDirectoryData() }
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.ForWin(@"c:\temp\folder").ForUnix("/temp/folder"));
            var result = directoryInfo.GetFileSystemInfos();

            Assert.That(result.Length, Is.EqualTo(2));
        }

        [Test]
        public void MockDirectoryInfo_GetFileSystemInfos_ShouldReturnDirectoriesAndNamesWithSearchPattern()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.ForWin(@"c:\temp\folder\file.txt").ForUnix("/temp/folder/file.txt"), new MockFileData("Hello World") },
                { XFS.ForWin(@"c:\temp\folder\folder").ForUnix("/temp/folder/folder"), new MockDirectoryData() },
                { XFS.ForWin(@"c:\temp\folder\older").ForUnix("/temp/folder/older"), new MockDirectoryData() }
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.ForWin(@"c:\temp\folder").ForUnix("/temp/folder"));
            var result = directoryInfo.GetFileSystemInfos("f*");

            Assert.That(result.Length, Is.EqualTo(2));
        }

        [Test]
        public void MockDirectoryInfo_GetParent_ShouldReturnDirectoriesAndNamesWithSearchPattern()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.ForWin(@"c:\a\b\c").ForUnix("/a/b/c"));
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.ForWin(@"c:\a\b\c").ForUnix("/a/b/c"));

            // Act
            var result = directoryInfo.Parent;

            // Assert
            Assert.AreEqual(XFS.ForWin(@"c:\a\b").ForUnix("/a/b"), result.FullName);
        }
    }
}