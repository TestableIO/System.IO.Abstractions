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
          Assert.AreEqual(new[]{"b.txt", "c.txt"}, directoryInfo.EnumerateFiles().ToList().Select(x => x.Name).ToArray());
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

        [Test]
        public void MockDirectoryInfo_Attributes_ShouldReturnDefaultAttributeIfNotExists() 
        {
            ExecuteDefaultValueTest((d) => d.Attributes, MockFileData.NullObject.Attributes);
        }

        [Test]
        public void MockDirectoryInfo_Attributes_ShouldThrowIfAttributeSetAndNotExists() 
        {
            ExecuteSetDefaultValueThrowsTest((d) => d.Attributes = FileAttributes.Normal);
        }

        [Test]
        public void MockDirectoryInfo_CreationTimeUtc_ShouldReturnDefaultCreationTimeUtcIfNotExists() 
        {
            ExecuteDefaultValueTest((d) => d.CreationTimeUtc, MockFileData.NullObject.CreationTime.UtcDateTime);
        }


        [Test]
        public void MockDirectoryInfo_CreationTimeUtc_ShouldThrowIfCreationTimeUtcSetAndIfNotExists() 
        {
            ExecuteSetDefaultValueThrowsTest((d) => d.CreationTimeUtc = DateTime.FromFileTimeUtc(100));
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
        public void MockDirectoryInfo_CreationTimeUtc_ShouldSetCreationTimeUtcOfFolderInMemoryFileSystem() {
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
            ExecuteDefaultValueTest((d) => d.CreationTime, MockFileData.NullObject.CreationTime.DateTime);
        }

        [Test]
        public void MockDirectoryInfo_CreationTime_ShouldThrowIfCreationTimeSetAndIfNotExists() 
        {
            ExecuteSetDefaultValueThrowsTest((d) => d.CreationTime = DateTime.FromFileTime(100));
        }

        [Test]
        public void MockDirectoryInfo_CreationTime_ShouldReturnCreationTimeOfFolderInMemoryFileSystem() {
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
            ExecuteDefaultValueTest((d) => d.LastAccessTimeUtc, MockFileData.NullObject.LastAccessTime.UtcDateTime);
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTimeUtc_ShouldThrowIfLastAccessTimeUtcSetAndIfNotExists() 
        {
            ExecuteSetDefaultValueThrowsTest((d) => d.LastAccessTimeUtc = DateTime.FromFileTimeUtc(100));
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
            ExecuteDefaultValueTest((d) => d.LastAccessTime, MockFileData.NullObject.LastAccessTime.DateTime);
        }

        [Test]
        public void MockDirectoryInfo_LastAccessTime_ShouldThrowIfLastAccessTimeSetAndIfNotExists() 
        {
            ExecuteSetDefaultValueThrowsTest((d) => d.LastAccessTime = DateTime.FromFileTime(100));
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
            ExecuteDefaultValueTest((d) => d.LastWriteTimeUtc, MockFileData.NullObject.LastWriteTime.UtcDateTime);
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTimeUtc_ShouldThrowIfLastWriteTimeUtcSetAndIfNotExists() 
        {
            ExecuteSetDefaultValueThrowsTest((d) => d.LastWriteTimeUtc = DateTime.FromFileTimeUtc(100));
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
        public void MockDirectoryInfo_LastWriteTime_ShouldReturnDefaultLastWriteTimeIfNotExists() {
            ExecuteDefaultValueTest((d) => d.LastWriteTime, MockFileData.NullObject.LastWriteTime.DateTime);
        }

        [Test]
        public void MockDirectoryInfo_LastWriteTime_ShouldThrowIfLastWriteTimeSetAndIfNotExists() {
            ExecuteSetDefaultValueThrowsTest((d) => d.LastWriteTime = DateTime.FromFileTime(100));
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

        static void ExecuteDefaultValueTest<T>(Func<MockDirectoryInfo, T> getDateValue, T expected) 
        {
            // Arrange
            string path = XFS.Path(@"c:\temp\folder");
            var fileSystem = new MockFileSystem();
            var file = new MockDirectoryInfo(fileSystem, path);

            // Act
            var actual = getDateValue(file);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        static void ExecuteSetDefaultValueThrowsTest(Action<MockDirectoryInfo> setDateValue) 
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directoryInfo = new MockDirectoryInfo(fileSystem, XFS.Path(@"c:\temp\folder"));

            // Assert
            Assert.Throws<FileNotFoundException>(() => setDateValue(directoryInfo));
        }
    }
}