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

        public static IEnumerable<object[]> MockDirectoryInfo_FullName_Data
        {
            get
            {
                yield return new object[] { XFS.Path(@"c:\temp\\folder"), XFS.Path(@"c:\temp\folder") };
                yield return new object[] { XFS.Path(@"c:\temp//folder"), XFS.Path(@"c:\temp\folder") };
                yield return new object[] { XFS.Path(@"c:\temp//\\///folder"), XFS.Path(@"c:\temp\folder") };
                if (!MockUnixSupport.IsUnixPlatform())
                {
                    yield return new object[] { XFS.Path(@"\\unc\folder"), XFS.Path(@"\\unc\folder") };
                    yield return new object[] { XFS.Path(@"\\unc/folder\\foo"), XFS.Path(@"\\unc\folder\foo") };
                }
            }
        }

        [TestCaseSource("MockDirectoryInfo_FullName_Data")]
        public void MockDirectoryInfo_FullName_ShouldReturnNormalizedPath(string directoryPath, string expectedFullName)
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\a.txt"), "" }
            });
            var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

            // Act
            var actualFullName = directoryInfo.FullName;

            // Assert
            Assert.AreEqual(expectedFullName, actualFullName);
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

        [Test]
        public void MockDirectoryInfo_ToString_ShouldReturnDirectoryName()
        {
            var directoryPath = XFS.Path(@"c:\temp\folder\folder");

            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, directoryPath);

            // Act
            var str = directoryInfo.ToString();

            // Assert
            Assert.AreEqual(directoryPath, str);
        }

        [Test]
        public void MockDirectoryInfo_Attributes_ShouldReturnDefaultAttributeIfNotExists()
        {
            // Arrange
            string path = XFS.Path(@"c:\temp\folder");
            var fileSystem = new MockFileSystem();
            var file = new MockDirectoryInfo(fileSystem, path);

            // Act
            var actual = file.Attributes;

            // Assert
            Assert.AreEqual(MockFileData.NullObject.Attributes, actual);
        }

        [Test]
        public void MockDirectoryInfo_Attributes_ShouldThrowIfAttributeSetAndNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

            // Assert
            Assert.Throws<FileNotFoundException>(() => directoryInfo.Attributes = FileAttributes.Normal);
        }

        [Test]
        public void MockDirectoryInfo_CreationTimeUtc_ShouldReturnDefaultCreationTimeUtcIfNotExists()
        {
            // Arrange
            string path = XFS.Path(@"c:\temp\folder");
            var fileSystem = new MockFileSystem();
            var file = new MockDirectoryInfo(fileSystem, path);

            // Act
            var actual = file.CreationTimeUtc;

            // Assert
            Assert.AreEqual(MockFileData.NullObject.CreationTime.UtcDateTime, actual);
        }
        
        [Test]
        public void MockDirectoryInfo_CreationTimeUtc_ShouldThrowIfCreationTimeUtcSetAndIfNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

            // Assert
            Assert.Throws<FileNotFoundException>(() => directoryInfo.CreationTimeUtc = DateTime.FromFileTimeUtc(100));
        }

        [Test]
        public void MockDirectoryInfo_CreationTimeUtc_ShouldReturnCreationTimeUtcOfFolderInMemoryFileSystem()
        {
            // Arrange
            var creationTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { CreationTimeUtc = creationTime };

            // Act
            var result = directoryInfo.CreationTimeUtc;

            // Assert
            Assert.AreEqual(creationTime.ToUniversalTime(), result);
        }

        [Test]
        public void MockDirectoryInfo_CreationTimeUtc_ShouldSetCreationTimeUtcOfFolderInMemoryFileSystem()
        {
            // Arrange
            var creationTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { CreationTimeUtc = creationTime };

            // Act
            var newUtcTime = DateTime.UtcNow;
            directoryInfo.CreationTimeUtc = newUtcTime;

            // Assert
            Assert.AreEqual(newUtcTime, directoryInfo.CreationTimeUtc);
        }

        [Test]
        public void MockDirectoryInfo_CreationTime_ShouldReturnDefaultCreationTimeIfNotExists()
        {
            // Arrange
            string path = XFS.Path(@"c:\temp\folder");
            var fileSystem = new MockFileSystem();
            var file = new MockDirectoryInfo(fileSystem, path);

            // Act
            var actual = file.CreationTime;

            // Assert
            Assert.AreEqual(MockFileData.NullObject.CreationTime.DateTime, actual);
        }

        [Test]
        public void MockDirectoryInfo_CreationTime_ShouldThrowIfCreationTimeSetAndIfNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

            // Assert
            Assert.Throws<FileNotFoundException>(() => directoryInfo.CreationTime = DateTime.FromFileTime(100));
        }

        [Test]
        public void MockDirectoryInfo_CreationTime_ShouldReturnCreationTimeOfFolderInMemoryFileSystem()
        {
            // Arrange
            var creationTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { CreationTime = creationTime };

            // Act
            var result = directoryInfo.CreationTime;

            // Assert
            Assert.AreEqual(creationTime, result);
        }

        [Test]
        public void MockDirectoryInfo_CreationTime_ShouldSetCreationTimeOfFolderInMemoryFileSystem()
        {
            // Arrange
            var creationTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { CreationTime = creationTime };

            // Act
            var newTime = DateTime.Now;
            directoryInfo.CreationTime = newTime;

            // Assert
            Assert.AreEqual(newTime, directoryInfo.CreationTime);
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTimeUtc_ShouldReturnDefaultLastAccessTimeUtcIfNotExists()
        {
            // Arrange
            string path = XFS.Path(@"c:\temp\folder");
            var fileSystem = new MockFileSystem();
            var file = new MockDirectoryInfo(fileSystem, path);

            // Act
            var actual = file.LastAccessTimeUtc;

            // Assert
            Assert.AreEqual(MockFileData.NullObject.LastAccessTime.UtcDateTime, actual);
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTimeUtc_ShouldThrowIfLastAccessTimeUtcSetAndIfNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

            // Assert
            Assert.Throws<FileNotFoundException>(() => directoryInfo.LastAccessTimeUtc = DateTime.FromFileTimeUtc(100));
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTimeUtc_ShouldReturnCreationTimeUtcOfFolderInMemoryFileSystem()
        {
            // Arrange
            var lastAccessTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { LastAccessTime = lastAccessTime };

            // Act
            var result = directoryInfo.LastAccessTimeUtc;

            // Assert
            Assert.AreEqual(lastAccessTime.ToUniversalTime(), result);
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTimeUtc_ShouldSetCreationTimeUtcOfFolderInMemoryFileSystem()
        {
            // Arrange
            var lastAccessTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { LastAccessTimeUtc = lastAccessTime };

            // Act
            var newUtcTime = DateTime.UtcNow;
            directoryInfo.LastAccessTimeUtc = newUtcTime;

            // Assert
            Assert.AreEqual(newUtcTime, directoryInfo.LastAccessTimeUtc);
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTime_ShouldReturnDefaultLastAccessTimeIfNotExists()
        {
            // Arrange
            string path = XFS.Path(@"c:\temp\folder");
            var fileSystem = new MockFileSystem();
            var file = new MockDirectoryInfo(fileSystem, path);

            // Act
            var actual = file.LastAccessTime;

            // Assert
            Assert.AreEqual(MockFileData.NullObject.LastAccessTime.DateTime, actual);
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTime_ShouldThrowIfLastAccessTimeSetAndIfNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

            // Assert
            Assert.Throws<FileNotFoundException>(() => directoryInfo.LastAccessTime = DateTime.FromFileTime(100));
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTime_ShouldReturnCreationTimeOfFolderInMemoryFileSystem()
        {
            // Arrange
            var lastAccessTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { LastAccessTime = lastAccessTime };

            // Act
            var result = directoryInfo.LastAccessTime;

            // Assert
            Assert.AreEqual(lastAccessTime, result);
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTime_ShouldSetCreationTimeOfFolderInMemoryFileSystem()
        {
            // Arrange
            var lastAccessTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { LastAccessTime = lastAccessTime };

            // Act
            var newTime = DateTime.Now;
            directoryInfo.LastAccessTime = newTime;

            // Assert
            Assert.AreEqual(newTime, directoryInfo.LastAccessTime);
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTimeUtc_ShouldReturnDefaultLastWriteTimeUtcIfNotExists()
        {
            // Arrange
            string path = XFS.Path(@"c:\temp\folder");
            var fileSystem = new MockFileSystem();
            var file = new MockDirectoryInfo(fileSystem, path);

            // Act
            var actual = file.LastWriteTimeUtc;

            // Assert
            Assert.AreEqual(MockFileData.NullObject.LastWriteTime.UtcDateTime, actual);
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTimeUtc_ShouldThrowIfLastWriteTimeUtcSetAndIfNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

            // Assert
            Assert.Throws<FileNotFoundException>(() => directoryInfo.LastWriteTimeUtc = DateTime.FromFileTimeUtc(100));
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTimeUtc_ShouldReturnCreationTimeUtcOfFolderInMemoryFileSystem()
        {
            // Arrange
            var lastWriteTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { LastWriteTimeUtc = lastWriteTime };

            // Act
            var result = directoryInfo.LastWriteTimeUtc;

            // Assert
            Assert.AreEqual(lastWriteTime.ToUniversalTime(), result);
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTimeUtc_ShouldSetCreationTimeUtcOfFolderInMemoryFileSystem()
        {
            // Arrange
            var lastWriteTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { LastWriteTimeUtc = lastWriteTime };

            // Act
            var newUtcTime = DateTime.UtcNow;
            directoryInfo.LastWriteTimeUtc = newUtcTime;

            // Assert
            Assert.AreEqual(newUtcTime, directoryInfo.LastWriteTimeUtc);
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTime_ShouldReturnDefaultLastWriteTimeIfNotExists()
        {
            // Arrange
            string path = XFS.Path(@"c:\temp\folder");
            var fileSystem = new MockFileSystem();
            var file = new MockDirectoryInfo(fileSystem, path);

            // Act
            var actual = file.LastAccessTime;

            // Assert
            Assert.AreEqual(MockFileData.NullObject.LastWriteTime.DateTime, actual);
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTime_ShouldThrowIfLastWriteTimeSetAndIfNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

            // Assert
            Assert.Throws<FileNotFoundException>(() => directoryInfo.LastWriteTime = DateTime.FromFileTime(100));
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTime_ShouldReturnCreationTimeOfFolderInMemoryFileSystem()
        {
            // Arrange
            var lastWriteTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { LastWriteTime = lastWriteTime };

            // Act
            var result = directoryInfo.LastWriteTime;

            // Assert
            Assert.AreEqual(lastWriteTime, result);
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTime_ShouldSetCreationTimeOfFolderInMemoryFileSystem()
        {
            // Arrange
            var lastWriteTime = DateTime.Now.AddHours(-4);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\temp\folder\folder"), new MockDirectoryData() },
            });

            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder")) { LastWriteTime = lastWriteTime };

            // Act
            var newTime = DateTime.Now;
            directoryInfo.LastWriteTime = newTime;

            // Assert
            Assert.AreEqual(newTime, directoryInfo.LastWriteTime);
        }
    }
}