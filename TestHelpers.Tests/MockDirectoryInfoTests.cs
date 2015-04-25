using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockDirectoryInfoTests
    {
        [Test]
        public void MockDirectoryInfo_GetExtension_ShouldReturnEmptyString()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp"));

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
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\"));

            // Act
            var result = directoryInfo.Extension;

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        public static IEnumerable<object[]> MockDirectoryInfo_Exists_Cases
        {
            get
            {
                yield return new object[]{ XFS.Path(@"c:\temp\folder"), true };
                yield return new object[]{ XFS.Path(@"c:\temp\folder\notExistant"), false };
            }
        }

        [TestCaseSource("MockDirectoryInfo_Exists_Cases")]
        public void MockDirectoryInfo_Exists(string path, bool expected) 
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> 
            {
                {XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("Hello World")}
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
                    XFS.Path(@"c:\temp\folder\file.txt"),
                        new MockFileData("Hello World")
                }
            });
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

            var result = directoryInfo.FullName;

            Assert.That(result, Is.EqualTo(XFS.Path(@"c:\temp\folder")));
        }

        [Test]
        public void MockDirectoryInfo_GetFileSystemInfos_ShouldReturnBothDirectoriesAndFiles()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("Hello World") },
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() }
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));
            var result = directoryInfo.GetFileSystemInfos();

            Assert.That(result.Length, Is.EqualTo(2));
        }

        [Test]
        public void MockDirectoryInfo_GetFileSystemInfos_ShouldReturnDirectoriesAndNamesWithSearchPattern()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("Hello World") },
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
                { XFS.Path(@"c:\temp\folder\older"), new MockDirectoryData() }
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));
            var result = directoryInfo.GetFileSystemInfos("f*");

            Assert.That(result.Length, Is.EqualTo(2));
        }

        [Test]
        public void MockDirectoryInfo_GetParent_ShouldReturnDirectoriesAndNamesWithSearchPattern()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\a\b\c"));
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\a\b\c"));

            var result = directoryInfo.Parent;

            Assert.AreEqual(XFS.Path(@"c:\a\b"), result.FullName);
        }

        [Test]
        public void MockDirectoryInfo_EnumerateFiles_ShouldReturnCorrectFiles()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\test1.txt"), "" },
                { XFS.Path(@"c:\temp\folder\test2.txt"), "" },
                { XFS.Path(@"c:\temp\folder\test1.so"), "" },
                { XFS.Path(@"c:\temp\folder\test1.so1"), "" },
                { XFS.Path(@"c:\temp\folder\subdir\test2.so"), "" },
                { XFS.Path(@"c:\temp\folder\subdir\test2a.so"), "" },
                { XFS.Path(@"c:\temp\folder\subdir\test3.txt"), ""},
                { XFS.Path(@"c:\temp\folder\subdir\test3.txtold"), ""},
                { XFS.Path(@"c:\temp\folder\emptysub\"), new MockDirectoryData()},
                { XFS.Path(@"c:\temp\test4.txt"), ""}
            });
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));
            
            // Assertions for the defaults:
            var allFilesInCurrentDir = new[] { "test1.txt", "test2.txt", "test1.so", "test1.so1" };
            Assert.That(directoryInfo.EnumerateFiles().Select(x => x.Name).ToArray(), Is.EquivalentTo(allFilesInCurrentDir));
            Assert.That(directoryInfo.EnumerateFiles("*").Select(x => x.Name).ToArray(), Is.EquivalentTo(allFilesInCurrentDir));
            Assert.That(directoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly).Select(x => x.Name).ToArray(), Is.EquivalentTo(allFilesInCurrentDir));

            // Assertions for search patterns with wildcards
            var allFilesUnderCurrentDir = allFilesInCurrentDir.Concat(new[] { "test2.so", "test2a.so", "test3.txt", "test3.txtold" }).ToArray();
            AssertThatEnumerateFilesIs(directoryInfo, "*", SearchOption.AllDirectories, allFilesUnderCurrentDir);
            AssertThatEnumerateFilesIs(directoryInfo, "te*.so", SearchOption.AllDirectories, "test1.so", "test2.so", "test2a.so");
            AssertThatEnumerateFilesIs(directoryInfo, "test?.so", SearchOption.AllDirectories, "test1.so", "test2.so");

            // Assertions for search patterns with a 3-letter file extension
            AssertThatEnumerateFilesIs(directoryInfo, "test?.txt", SearchOption.AllDirectories, "test1.txt", "test2.txt", "test3.txt");
            AssertThatEnumerateFilesIs(directoryInfo, "test*.txt", SearchOption.AllDirectories, "test1.txt", "test2.txt", "test3.txt", "test3.txtold");
        }

        private void AssertThatEnumerateFilesIs(MockDirectoryInfo directoryInfo, string searchPattern, SearchOption searchOption, params string[] files)
        {
            var enumeratedFiles = directoryInfo.EnumerateFiles(searchPattern, searchOption);
            var arrayOfFilenames = enumeratedFiles.Select(x => x.Name).ToArray();

            Assert.That(arrayOfFilenames, Is.EquivalentTo(files));
        }

        [Test]
        public void MockDirectoryInfo_EnumerateFiles_Blub()
        {
            //var fileSystem = new MockFileSystem(new Directory<string, MockFileData>)
        }
    }
}