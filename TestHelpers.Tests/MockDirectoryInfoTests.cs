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
        public void MockDirectoryInfo_CreationTimeUtc_ShouldReturnCreationTimeUtcOfFileInMemoryFileSystem() 
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
        public void MockDirectoryInfo_CreationTimeUtc_ShouldSetCreationTimeUtcOfFileInMemoryFileSystem() {
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
        public void MockDirectoryInfo_CreationTime_ShouldReturnCreationTimeOfFileInMemoryFileSystem() {
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
        public void MockDirectoryInfo_CreationTime_ShouldSetCreationTimeOfFileInMemoryFileSystem() 
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
        public void MockDirectoryInfo_LastAccessTimeUtc_ShouldReturnCreationTimeUtcOfFileInMemoryFileSystem() 
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
        public void MockDirectoryInfo_LastAccessTimeUtc_ShouldSetCreationTimeUtcOfFileInMemoryFileSystem() 
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
        public void MockDirectoryInfo_LastAccessTime_ShouldReturnCreationTimeOfFileInMemoryFileSystem()
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
        public void MockDirectoryInfo_LastAccessTime_ShouldSetCreationTimeOfFileInMemoryFileSystem() 
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
        public void MockDirectoryInfo_LastWriteTimeUtc_ShouldReturnCreationTimeUtcOfFileInMemoryFileSystem() 
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
        public void MockDirectoryInfo_LastWriteTimeUtc_ShouldSetCreationTimeUtcOfFileInMemoryFileSystem() 
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
        public void MockDirectoryInfo_LastWriteTime_ShouldReturnCreationTimeOfFileInMemoryFileSystem() 
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
        public void MockDirectoryInfo_LastWriteTime_ShouldSetCreationTimeOfFileInMemoryFileSystem() 
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