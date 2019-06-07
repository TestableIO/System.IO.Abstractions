using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
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

        [TestCaseSource("MockDirectoryInfo_GetExtension_Cases")]
        public void MockDirectoryInfo_GetExtension_ShouldReturnEmptyString(string directoryPath)
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

            // Act
            var result = directoryInfo.Extension;

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        public static IEnumerable<object[]> MockDirectoryInfo_Exists_Cases
        {
            get
            {
                yield return new object[] { XFS.Path(@"c:\temp\folder"), true };
                yield return new object[] { XFS.Path(@"c:\temp\folder\notExistant"), false };
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
        [WindowsOnly(WindowsSpecifics.UNCPaths)]
        public void MockDirectoryInfo_GetFiles_ShouldWorkWithUNCPath()
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
            Assert.AreEqual(fileName, files[0].FullName);
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
        public void MockDirectoryInfo_EnumerateFileSystemInfos_ShouldReturnBothDirectoriesAndFiles()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("Hello World") },
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() }
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));
            var result = directoryInfo.EnumerateFileSystemInfos().ToArray();

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
        public void MockDirectoryInfo_EnumerateFileSystemInfos_ShouldReturnDirectoriesAndNamesWithSearchPattern()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("Hello World") },
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
                { XFS.Path(@"c:\temp\folder\older"), new MockDirectoryData() }
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));
            var result = directoryInfo.EnumerateFileSystemInfos("f*", SearchOption.AllDirectories).ToArray();

            Assert.That(result.Length, Is.EqualTo(2));
        }

        [Test]
        public void MockDirectoryInfo_GetParent_ShouldReturnDirectoriesAndNamesWithSearchPattern()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\a\b\c"));
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\a\b\c"));

            // Act
            var result = directoryInfo.Parent;

            // Assert
            Assert.AreEqual(XFS.Path(@"c:\a\b"), result.FullName);
        }

        [Test]
        public void MockDirectoryInfo_EnumerateFiles_ShouldReturnAllFiles()
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
            Assert.AreEqual(new[] { "b.txt", "c.txt" }, directoryInfo.EnumerateFiles().ToList().Select(x => x.Name).ToArray());
        }

        [Test]
        public void MockDirectoryInfo_EnumerateDirectories_ShouldReturnAllDirectories()
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
            Assert.AreEqual(new[] { "b", "c" }, directories);
        }

        [TestCase(@"\\unc\folder", @"\\unc\folder")]
        [TestCase(@"\\unc/folder\\foo", @"\\unc\folder\foo")]
        [WindowsOnly(WindowsSpecifics.UNCPaths)]
        public void MockDirectoryInfo_FullName_ShouldReturnNormalizedUNCPath(string directoryPath, string expectedFullName)
        {
            // Arrange
            directoryPath = XFS.Path(directoryPath);
            expectedFullName = XFS.Path(expectedFullName);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

            // Act
            var actualFullName = directoryInfo.FullName;

            // Assert
            Assert.AreEqual(expectedFullName, actualFullName);
        }

        [TestCase(@"c:\temp\\folder", @"c:\temp\folder")]
        [TestCase(@"c:\temp//folder", @"c:\temp\folder")]
        [TestCase(@"c:\temp//\\///folder", @"c:\temp\folder")]
        public void MockDirectoryInfo_FullName_ShouldReturnNormalizedPath(string directoryPath, string expectedFullName)
        {
            // Arrange
            directoryPath = XFS.Path(directoryPath);
            expectedFullName = XFS.Path(expectedFullName);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

            // Act
            var actualFullName = directoryInfo.FullName;

            // Assert
            Assert.AreEqual(expectedFullName, actualFullName);
        }

        [TestCase(@"c:\temp\folder  ", @"c:\temp\folder")]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void MockDirectoryInfo_FullName_ShouldReturnPathWithTrimmedTrailingSpaces(string directoryPath, string expectedFullName)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

            // Act
            var actualFullName = directoryInfo.FullName;

            // Assert
            Assert.AreEqual(expectedFullName, actualFullName);
        }

        [TestCase(@"c:\temp\\folder ", @"folder")]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void MockDirectoryInfo_Name_ShouldReturnNameWithTrimmedTrailingSpaces(string directoryPath, string expectedName)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

            // Act
            var actualName = directoryInfo.Name;

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }

        [Test]
        public void MockDirectoryInfo_Constructor_ShouldThrowArgumentNullException_IfArgumentDirectoryIsNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => new MockDirectoryInfo(fileSystem, null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.That(exception.Message, Does.StartWith("Value cannot be null."));
        }

        [Test]
        public void MockDirectoryInfo_Constructor_ShouldThrowArgumentNullException_IfArgumentFileSystemIsNull()
        {
            // Arrange
            // nothing to do

            // Act
            TestDelegate action = () => new MockDirectoryInfo(null, XFS.Path(@"c:\foo\bar\folder"));

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void MockDirectoryInfo_Constructor_ShouldThrowArgumentException_IfArgumentDirectoryIsEmpty()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => new MockDirectoryInfo(fileSystem, string.Empty);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.Message, Does.StartWith("The path is not of a legal form."));
        }

        [TestCase(@"c:\temp\folder\folder")]
        [TestCase(@"..\..\..\Desktop")]
        public void MockDirectoryInfo_ToString_ShouldReturnDirectoryName(string directoryName)
        {
            // Arrange
            var directoryPath = XFS.Path(directoryName);

            // Act
            var mockDirectoryInfo = new MockDirectoryInfo(new MockFileSystem(), directoryPath);

            // Assert
            Assert.AreEqual(directoryPath, mockDirectoryInfo.ToString());
        }
    }
}