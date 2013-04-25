using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockDirectoryTests
    {
        [Test]
        public void MockDirectory_GetFiles_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndSearchOptionIsAllDirectories()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\b.txt", new MockFileData("Demo text content") },
                { @"c:\c.txt", new MockFileData("Demo text content") },
                { @"c:\a\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b.txt", new MockFileData("Demo text content") },
                { @"c:\a\c.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\b.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\c.txt", new MockFileData("Demo text content") },
            });

            // Act
            var result = fileSystem.Directory.GetFiles(@"c:\a", "*", SearchOption.AllDirectories);

            // Assert
            CollectionAssert.AreEqual
            (
                new []
                {
                    @"c:\a\a.txt",
                    @"c:\a\b.txt",
                    @"c:\a\c.txt",
                    @"c:\a\a\a.txt",
                    @"c:\a\a\b.txt",
                    @"c:\a\a\c.txt"
                },
                result
            );
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldReturnFilesDirectlyBelowPathWhenPatternIsWildcardAndSearchOptionIsTopDirectoryOnly()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\b.txt", new MockFileData("Demo text content") },
                { @"c:\c.txt", new MockFileData("Demo text content") },
                { @"c:\a\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b.txt", new MockFileData("Demo text content") },
                { @"c:\a\c.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\b.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\c.txt", new MockFileData("Demo text content") },
            });

            // Act
            var result = fileSystem.Directory.GetFiles(@"c:\a", "*", SearchOption.TopDirectoryOnly);

            // Assert
            CollectionAssert.AreEqual
            (
                new[]
                {
                    @"c:\a\a.txt",
                    @"c:\a\b.txt",
                    @"c:\a\c.txt",
                },
                result
            );
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPattern()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.gif", new MockFileData("Demo text content") },
                { @"c:\b.txt", new MockFileData("Demo text content") },
                { @"c:\c.txt", new MockFileData("Demo text content") },
                { @"c:\a\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b.gif", new MockFileData("Demo text content") },
                { @"c:\a\c.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\b.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\c.gif", new MockFileData("Demo text content") },
            });

            // Act
            var result = fileSystem.Directory.GetFiles(@"c:\", "*.gif", SearchOption.AllDirectories);

            // Assert
            CollectionAssert.AreEqual
            (
                new[]
                {
                    @"c:\a.gif",
                    @"c:\a\b.gif",
                    @"c:\a\a\c.gif",
                },
                result
            );
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternWithDotsInFilenames()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.there.are.dots.in.this.filename.gif", new MockFileData("Demo text content") },
                { @"c:\b.txt", new MockFileData("Demo text content") },
                { @"c:\c.txt", new MockFileData("Demo text content") },
                { @"c:\a\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b.gif", new MockFileData("Demo text content") },
                { @"c:\a\c.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\b.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\c.gif", new MockFileData("Demo text content") },
            });

            // Act
            var result = fileSystem.Directory.GetFiles(@"c:\", "*.gif", SearchOption.AllDirectories);

            // Assert
            CollectionAssert.AreEqual
            (
                new[]
                {
                    @"c:\a.there.are.dots.in.this.filename.gif",
                    @"c:\a\b.gif",
                    @"c:\a\a\c.gif",
                },
                result
            );
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternAndSearchOptionTopDirectoryOnly()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.gif", new MockFileData("Demo text content") },
                { @"c:\b.txt", new MockFileData("Demo text content") },
                { @"c:\c.txt", new MockFileData("Demo text content") },
                { @"c:\a\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b.gif", new MockFileData("Demo text content") },
                { @"c:\a\c.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\b.txt", new MockFileData("Demo text content") },
                { @"c:\a\a\c.gif", new MockFileData("Demo text content") },
            });

            // Act
            var result = fileSystem.Directory.GetFiles(@"c:\", "*.gif", SearchOption.TopDirectoryOnly);

            // Assert
            CollectionAssert.AreEqual
            (
                new[]
                {
                    @"c:\a.gif",
                },
                result
            );
        }

        [Test]
        public void MockDirectory_GetCreationTime_ShouldReturnCreationTimeFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            
            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetCreationTime(path, time);
            var result = fileSystem.Directory.GetCreationTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_GetCreationTimeUtc_ShouldReturnCreationTimeUtcFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetCreationTimeUtc(path, time);
            var result = fileSystem.Directory.GetCreationTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_GetLastAccessTime_ShouldReturnLastAccessTimeFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetLastAccessTime(path, time);
            var result = fileSystem.Directory.GetLastAccessTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_GetLastAccessTimeUtc_ShouldReturnLastAccessTimeUtcFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetLastAccessTimeUtc(path, time);
            var result = fileSystem.Directory.GetLastAccessTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_GetLastWriteTime_ShouldReturnLastWriteTimeFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetLastWriteTime(path, time);
            var result = fileSystem.Directory.GetLastWriteTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_GetLastWriteTimeUtc_ShouldReturnLastWriteTimeUtcFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetLastWriteTimeUtc(path, time);
            var result = fileSystem.Directory.GetLastWriteTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_SetCreationTime_ShouldSetCreationTimeOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetCreationTime(path, time);
            var result = fileSystem.File.GetCreationTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_SetCreationTimeUtc_ShouldSetCreationTimeUtcOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetCreationTimeUtc(path, time);
            var result = fileSystem.File.GetCreationTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_SetLastAccessTime_ShouldSetLastAccessTimeOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetLastAccessTime(path, time);
            var result = fileSystem.File.GetLastAccessTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_SetLastAccessTimeUtc_ShouldSetLastAccessTimeUtcOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetLastAccessTimeUtc(path, time);
            var result = fileSystem.File.GetLastAccessTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_SetLastWriteTime_ShouldSetLastWriteTimeOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetLastWriteTime(path, time);
            var result = fileSystem.File.GetLastWriteTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_SetLastWriteTimeUtc_ShouldSetLastWriteTimeUtcOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetLastWriteTimeUtc(path, time);
            var result = fileSystem.File.GetLastWriteTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnTrueForDirectoryDefinedInMemoryFileSystemWithoutTrailingSlash()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\foo\bar.txt", new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.Exists(@"c:\foo");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnTrueForDirectoryDefinedInMemoryFileSystemWithTrailingSlash()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\foo\bar.txt", new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.Exists(@"c:\foo\");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnFalseForDirectoryNotDefinedInMemoryFileSystemWithoutTrailingSlash()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\foo\bar.txt", new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.Exists(@"c:\baz");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnFalseForDirectoryNotDefinedInMemoryFileSystemWithTrailingSlash()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\foo\bar.txt", new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.Exists(@"c:\baz\");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnFalseForDirectoryNotDefinedInMemoryFileSystemWithSimilarFileName()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\foo\bar.txt", new MockFileData("Demo text content") },
                { @"c:\baz.txt", new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.Exists(@"c:\baz");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnTrueForDirectoryCreatedViaMocks()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\foo\bar.txt", new MockFileData("Demo text content") }
            });
            fileSystem.Directory.CreateDirectory(@"c:\bar");

            // Act
            var result = fileSystem.Directory.Exists(@"c:\bar");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MockDirectory_CreateDirectory_ShouldCreateFolderInMemoryFileSystem()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\foo.txt", new MockFileData("Demo text content") }
            });

            // Act
            fileSystem.Directory.CreateDirectory(@"c:\bar");

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"c:\bar\"));
            Assert.IsTrue(fileSystem.AllDirectories.Any(d => d == @"c:\bar\"));
        }

        [Test]
        public void MockDirectory_CreateDirectory_ShouldReturnDirectoryInfoBase()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\foo.txt", new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.CreateDirectory(@"c:\bar");

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void MockDirectory_Delete_ShouldDeleteDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\bar\foo.txt", new MockFileData("Demo text content") }
            });

            // Act
            fileSystem.Directory.Delete(@"c:\bar", true);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(@"c:\bar"));
        }

        [Test]
        public void MockDirectory_Delete_ShouldDeleteDirectoryCaseInsensitively()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\bar\foo.txt", new MockFileData("Demo text content") }
            });

            // Act
            fileSystem.Directory.Delete(@"c:\BAR", true);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(@"c:\bar"));
        }

        [Test]
        public void MockDirectory_Delete_ShouldThrowDirectoryNotFoundException()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\bar\foo.txt", new MockFileData("Demo text content") }
            });

            // Act
            try
            {
                fileSystem.Directory.Delete(@"c:\baz");
                Assert.Fail();
            }
            catch (DirectoryNotFoundException)
            {
            }
        }

        [Test]
        public void MockDirectory_Delete_ShouldThrowIOException()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\bar\foo.txt", new MockFileData("Demo text content") },
                { @"c:\bar\baz.txt", new MockFileData("Demo text content") }
            });

            // Act
            try
            {
                fileSystem.Directory.Delete(@"c:\bar");
                Assert.Fail();
            }
            catch (IOException ex)
            {
                Assert.AreEqual(
                    @"The directory specified by c:\bar\ is read-only, or recursive is false and c:\bar\ is not an empty directory.",
                    ex.Message);
            }
        }

        [Test]
        public void MockDirectory_Delete_ShouldDeleteDirectoryRecursively()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\bar\foo.txt", new MockFileData("Demo text content") },
                { @"c:\bar\bar2\foo.txt", new MockFileData("Demo text content") }
            });

            // Act
            fileSystem.DirectoryInfo.FromDirectoryName(@"c:\bar").Delete(true);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(@"c:\bar"));
            Assert.IsFalse(fileSystem.Directory.Exists(@"c:\bar\bar2"));
        }

        [Test]
        public void MockDirectory_GetFileSystemEntries_Returns_Files_And_Directories()
        {
            const string testPath = @"c:\foo\bar.txt";
            const string testDir =  @"c:\foo\bar\";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { testPath, new MockFileData("Demo text content") },
                { testDir,  new MockDirectoryData() }
            });

            var entries = fileSystem.Directory.GetFileSystemEntries(@"c:\foo").OrderBy(k => k);
            Assert.AreEqual(2, entries.Count());
            Assert.AreEqual(testDir, entries.Last());
            Assert.AreEqual(testPath, entries.First());
        }

        [Test]
        public void MockDirectory_GetFiles_Returns_Files()
        {
            const string testPath = @"c:\foo\bar.txt";
            const string testDir = @"c:\foo\bar\";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { testPath, new MockFileData("Demo text content") },
                { testDir,  new MockDirectoryData() }
            });

            var entries = fileSystem.Directory.GetFiles(@"c:\foo").OrderBy(k => k);
            Assert.AreEqual(1, entries.Count());
            Assert.AreEqual(testPath, entries.First());
        }

        [Test]
        public void MockDirectory_GetRoot_Returns_Root()
        {
            const string testDir = @"c:\foo\bar\";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { testDir,  new MockDirectoryData() }
            });

            Assert.AreEqual("C:\\", fileSystem.Directory.GetDirectoryRoot(@"C:\foo\bar"));
        }

        [Test]
        public void MockDirectory_GetLogicalDrives_Returns_LogicalDrives()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {@"c:\foo\bar\", new MockDirectoryData()},
                    {@"c:\foo\baz\", new MockDirectoryData()},
                    {@"d:\bash\", new MockDirectoryData()},
                });

            var drives = fileSystem.Directory.GetLogicalDrives();

            Assert.AreEqual(2, drives.Length);
            Assert.IsTrue(drives.Contains("c:\\"));
            Assert.IsTrue(drives.Contains("d:\\"));
        }

        [Test]
        public void MockDirectory_GetDirectories_Returns_Child_Directories()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"A:\folder1\folder2\folder3\file.txt", new MockFileData("Demo text content") },
                { @"A:\folder1\folder4\file2.txt", new MockFileData("Demo text content 2") },
            });

            var directories = fileSystem.Directory.GetDirectories(@"A:\folder1").ToArray();

            //Check that it does not returns itself
            Assert.IsFalse(directories.Contains(@"A:\folder1\"));

            //Check that it correctly returns all child directories
            Assert.AreEqual(2, directories.Count());
            Assert.IsTrue(directories.Contains(@"A:\folder1\folder2\"));
            Assert.IsTrue(directories.Contains(@"A:\folder1\folder4\"));
        }

        [Test]
        public void MockDirectory_Move_ShouldMove()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"A:\folder1\file.txt", new MockFileData("aaa") },
                { @"A:\folder1\folder2\file2.txt", new MockFileData("bbb") },
            });

            fileSystem.DirectoryInfo.FromDirectoryName(@"A:\folder1").MoveTo(@"B:\folder1");

            Assert.IsFalse(fileSystem.Directory.Exists(@"A:\folder1"));
            Assert.IsTrue(fileSystem.Directory.Exists(@"B:\folder1"));
            Assert.IsTrue(fileSystem.Directory.Exists(@"B:\folder1\folder2"));
            Assert.IsTrue(fileSystem.File.Exists(@"B:\folder1\file.txt"));
            Assert.IsTrue(fileSystem.File.Exists(@"B:\folder1\folder2\file2.txt"));
        }

        [Test]
        public void MockDirectory_GetCurrentDirectory_ShouldReturnValueFromFileSystemConstructor() {
            string directory = @"D:\folder1\folder2";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>(), directory);
            
            var actual = fileSystem.Directory.GetCurrentDirectory();

            Assert.AreEqual(directory, actual);
        }

      
        [Test]
        public void MockDirectory_GetCurrentDirectory_ShouldReturnDefaultPathWhenNotSet() {
            string directory = @"C:\Foo\Bar";
            var fileSystem = new MockFileSystem();
            
            var actual = fileSystem.Directory.GetCurrentDirectory();

            Assert.AreEqual(directory, actual);
        }

        [Test]
        public void MockDirectory_SetCurrentDirectory_ShouldChangeCurrentDirectory() {
          string directory = @"D:\folder1\folder2";
          var fileSystem = new MockFileSystem();
          
          // Precondition
          Assert.AreNotEqual(directory, fileSystem.Directory.GetCurrentDirectory());

          fileSystem.Directory.SetCurrentDirectory(directory);

          Assert.AreEqual(directory, fileSystem.Directory.GetCurrentDirectory());
        }
    }
}