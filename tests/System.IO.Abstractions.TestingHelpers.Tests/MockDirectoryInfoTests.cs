using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
        public void MockDirectoryInfo_Attributes_ShouldReturnMinusOneForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));
            FileAttributes expected = (FileAttributes)(-1);

            Assert.That(directoryInfo.Attributes, Is.EqualTo(expected));
        }

        [Test]
        public void MockDirectoryInfo_Attributes_SetterShouldThrowDirectoryNotFoundEceptionOnNonExistingFileOrDirectory()
        {
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            Assert.That(() => directoryInfo.Attributes = FileAttributes.Hidden, Throws.TypeOf<DirectoryNotFoundException>());
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
        public void MockDirectoryInfo_EnumerateFileSystemInfos_ShouldReturnDirectoriesAndNamesWithSearchPatternRecursive()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\file.txt"), new MockFileData("") },
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
                { XFS.Path(@"c:\temp\folder\older"), new MockDirectoryData() }
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\"));
            var result = directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).ToArray();

            Assert.That(result.Length, Is.EqualTo(5));
        }

#if FEATURE_ENUMERATION_OPTIONS
        [Test]
        public void MockDirectoryInfo_EnumerateFileSystemInfos_ShouldReturnDirectoriesAndNamesWithSearchPatternRecursiveEnumerateOptions()
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

            Assert.That(result.Length, Is.EqualTo(5));
        }
#endif

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

        [TestCase(@"c:\", @"c:\")]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void MockDirectoryInfo_Name_ShouldReturnPathRoot_IfDirectoryPathIsPathRoot(string directoryPath, string expectedName)
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

        [Test]
        public void MockDirectoryInfo_Exists_ShouldReturnCachedData()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path = XFS.Path(@"c:\abc");
            var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(path);

            // Act
            fileSystem.AddDirectory(path);

            // Assert
            Assert.IsFalse(directoryInfo.Exists);
        }

        [Test]
        public void MockDirectoryInfo_Exists_ShouldUpdateCachedDataOnRefresh()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path = XFS.Path(@"c:\abc");
            var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(path);

            // Act
            fileSystem.AddDirectory(path);
            directoryInfo.Refresh();

            // Assert
            Assert.IsTrue(directoryInfo.Exists);
        }

        [Test]
        public void Directory_exists_after_creation()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(XFS.Path(@"c:\abc"));

            // Act
            directoryInfo.Create();

            // Assert
            Assert.IsTrue(directoryInfo.Exists);
        }

        [Test, WindowsOnly(WindowsSpecifics.AccessControlLists)]
        public void Directory_exists_after_creation_with_security()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(XFS.Path(@"c:\abc"));

            // Act
            directoryInfo.Create(new DirectorySecurity());

            // Assert
            Assert.IsTrue(directoryInfo.Exists);
        }

        [Test]
        public void Directory_does_not_exist_after_delete()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\abc"));

            // Act
            directoryInfo.Delete();

            // Assert
            Assert.IsFalse(directoryInfo.Exists);
        }

        [Test]
        public void Directory_does_not_exist_after_recursive_delete()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\abc"));

            // Act
            directoryInfo.Delete(true);

            // Assert
            Assert.IsFalse(directoryInfo.Exists);
        }

        [Test]
        public void Directory_still_exists_after_move()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\abc"));

            // Act
            directoryInfo.MoveTo(XFS.Path(@"c:\abc2"));

            // Assert
            Assert.IsTrue(directoryInfo.Exists);
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTime_ShouldReflectChangedValue()
        {
            // Arrange  
            var path = XFS.Path(@"c:\abc");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockDirectoryData() }
            });
            var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(path);
            var lastAccessTime = new DateTime(2022, 1, 8);

            // Act
            directoryInfo.LastAccessTime = lastAccessTime;

            // Assert
            Assert.AreEqual(lastAccessTime, directoryInfo.LastAccessTime);
        }

        [Test]
        public void MockDirectoryInfo_CreationTime_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            var result = directoryInfo.CreationTime;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.LocalDateTime));
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTime_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            var result = directoryInfo.LastAccessTime;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.LocalDateTime));
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTime_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            var result = directoryInfo.LastWriteTime;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.LocalDateTime));
        }

        [Test]
        public void MockDirectoryInfo_CreationTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            var result = directoryInfo.CreationTimeUtc;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime));
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            var result = directoryInfo.LastAccessTimeUtc;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime));
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            var result = directoryInfo.LastWriteTimeUtc;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime));
        }

        public void MockDirectoryInfo_CreationTime_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
        {
            var newTime = new DateTime(2022, 04, 06);
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            Assert.That(() => directoryInfo.CreationTime = newTime, Throws.TypeOf<DirectoryNotFoundException>());
        }

        public void MockDirectoryInfo_LastAccessTime_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
        {
            var newTime = new DateTime(2022, 04, 06);
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            Assert.That(() => directoryInfo.LastAccessTime = newTime, Throws.TypeOf<DirectoryNotFoundException>());
        }

        public void MockDirectoryInfo_LastWriteTime_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
        {
            var newTime = new DateTime(2022, 04, 06);
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            Assert.That(() => directoryInfo.LastWriteTime = newTime, Throws.TypeOf<DirectoryNotFoundException>());
        }

        public void MockDirectoryInfo_CreationTimeUtc_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
        {
            var newTime = new DateTime(2022, 04, 06);
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            Assert.That(() => directoryInfo.CreationTimeUtc = newTime, Throws.TypeOf<DirectoryNotFoundException>());
        }

        public void MockDirectoryInfo_LastAccessTimeUtc_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
        {
            var newTime = new DateTime(2022, 04, 06);
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            Assert.That(() => directoryInfo.LastAccessTimeUtc = newTime, Throws.TypeOf<DirectoryNotFoundException>());
        }

        public void MockDirectoryInfo_LastWriteTimeUtc_SetterShouldThrowDirectoryNotFoundExceptionForNonExistingDirectory()
        {
            var newTime = new DateTime(2022, 04, 06);
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\non\existing"));

            Assert.That(() => directoryInfo.LastWriteTime = newTime, Throws.TypeOf<DirectoryNotFoundException>());
        }

    }
}
