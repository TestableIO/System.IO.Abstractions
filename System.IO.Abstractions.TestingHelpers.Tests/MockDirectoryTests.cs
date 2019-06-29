using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockDirectoryTests
    {
        [Test]
        public void MockDirectory_GetFiles_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndSearchOptionIsAllDirectories()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            var expected = new[]
            {
                XFS.Path(@"c:\a\a.txt"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\c.txt"),
                XFS.Path(@"c:\a\a\a.txt"),
                XFS.Path(@"c:\a\a\b.txt"),
                XFS.Path(@"c:\a\a\c.gif")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "*", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        private MockFileSystem SetupFileSystem()
        {
            return new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\b.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\c.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\c.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\b.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\c.gif"), new MockFileData("Demo text content") },
            });

        }

        [Test]
        public void MockDirectory_GetFiles_ShouldReturnFilesDirectlyBelowPathWhenPatternIsWildcardAndSearchOptionIsTopDirectoryOnly()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            var expected = new[]
            {
                XFS.Path(@"c:\a\a.txt"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\c.txt")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "*", SearchOption.TopDirectoryOnly);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPattern()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            var expected = new[]
            {
                XFS.Path(@"c:\a.gif"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\a\c.gif")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.gif", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternWithThreeCharacterLongFileExtension_RepectingAllDirectorySearchOption()
        {
            // Arrange
            var additionalFilePath = XFS.Path(@"c:\a\a\c.gifx");
            var fileSystem = SetupFileSystem();
            fileSystem.AddFile(additionalFilePath, new MockFileData(string.Empty));
            fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifx.xyz"), new MockFileData(string.Empty));
            fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifz\xyz"), new MockFileData(string.Empty));
            var expected = new[]
                {
                    XFS.Path(@"c:\a.gif"),
                    XFS.Path(@"c:\a\b.gif"),
                    XFS.Path(@"c:\a\a\c.gif"),
                    additionalFilePath
                };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.gif", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternWithThreeCharacterLongFileExtension_RepectingTopDirectorySearchOption()
        {
            // Arrange
            var additionalFilePath = XFS.Path(@"c:\a\c.gifx");
            var fileSystem = SetupFileSystem();
            fileSystem.AddFile(additionalFilePath, new MockFileData(string.Empty));
            fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifx.xyz"), new MockFileData(string.Empty));
            fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifx"), new MockFileData(string.Empty));
            var expected = new[]
                {
                    XFS.Path(@"c:\a\b.gif"),
                    additionalFilePath
                };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "*.gif", SearchOption.TopDirectoryOnly);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternOnlyIfTheFileExtensionIsThreeCharacterLong()
        {
            // Arrange
            var additionalFilePath = XFS.Path(@"c:\a\c.gi");
            var fileSystem = SetupFileSystem();
            fileSystem.AddFile(additionalFilePath, new MockFileData(string.Empty));
            fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifx.xyz"), new MockFileData(string.Empty));
            fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gif"), new MockFileData(string.Empty));
            fileSystem.AddFile(XFS.Path(@"c:\a\a\c.gifx"), new MockFileData(string.Empty));
            var expected = new[]
                {
                    additionalFilePath
                };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "*.gi", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternWithDotsInFilenames()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.there.are.dots.in.this.filename.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\b.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\c.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\c.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\b.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\c.gif"), new MockFileData("Demo text content") },
            });
            var expected = new[]
            {
                XFS.Path(@"c:\a.there.are.dots.in.this.filename.gif"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\a\c.gif")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.gif", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_FilterShouldFindFilesWithSpecialChars()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.1#.pdf"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\b\b #1.txt"), new MockFileData("Demo text content") }
            });
            var expected = new[]
            {
                XFS.Path(@"c:\a.1#.pdf"),
                XFS.Path(@"c:\b\b #1.txt")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.*", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternAndSearchOptionTopDirectoryOnly()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            var expected = new[] { XFS.Path(@"c:\a.gif") };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.gif", SearchOption.TopDirectoryOnly);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        private void ExecuteTimeAttributeTest(Action<IFileSystem, string, DateTime> setter, Func<IFileSystem, string, DateTime> getter)
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            setter(fileSystem, path, time);
            var result = getter(fileSystem, path);

            // Assert
            Assert.That(result, Is.EqualTo(time));
        }

        [Test]
        public void MockDirectory_GetCreationTime_ShouldReturnCreationTimeFromFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.File.SetCreationTime(p, d),
                (fs, p) => fs.Directory.GetCreationTime(p));
        }

        [Test]
        public void MockDirectory_GetCreationTimeUtc_ShouldReturnCreationTimeUtcFromFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.File.SetCreationTimeUtc(p, d),
                (fs, p) => fs.Directory.GetCreationTimeUtc(p));
        }

        [Test]
        public void MockDirectory_GetLastAccessTime_ShouldReturnLastAccessTimeFromFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.File.SetLastAccessTime(p, d),
                (fs, p) => fs.Directory.GetLastAccessTime(p));
        }

        [Test]
        public void MockDirectory_GetLastAccessTimeUtc_ShouldReturnLastAccessTimeUtcFromFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.File.SetLastAccessTimeUtc(p, d),
                (fs, p) => fs.Directory.GetLastAccessTimeUtc(p));
        }

        [Test]
        public void MockDirectory_GetLastWriteTime_ShouldReturnLastWriteTimeFromFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.File.SetLastWriteTime(p, d),
                (fs, p) => fs.Directory.GetLastWriteTime(p));
        }

        [Test]
        public void MockDirectory_GetLastWriteTimeUtc_ShouldReturnLastWriteTimeUtcFromFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.File.SetLastWriteTimeUtc(p, d),
                (fs, p) => fs.Directory.GetLastWriteTimeUtc(p));
        }

        [Test]
        public void MockDirectory_SetCreationTime_ShouldSetCreationTimeOnFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.Directory.SetCreationTime(p, d),
                (fs, p) => fs.File.GetCreationTime(p));
        }

        [Test]
        public void MockDirectory_SetCreationTimeUtc_ShouldSetCreationTimeUtcOnFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.Directory.SetCreationTimeUtc(p, d),
                (fs, p) => fs.File.GetCreationTimeUtc(p));
        }

        [Test]
        public void MockDirectory_SetLastAccessTime_ShouldSetLastAccessTimeOnFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.Directory.SetLastAccessTime(p, d),
                (fs, p) => fs.File.GetLastAccessTime(p));
        }

        [Test]
        public void MockDirectory_SetLastAccessTimeUtc_ShouldSetLastAccessTimeUtcOnFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.Directory.SetLastAccessTimeUtc(p, d),
                (fs, p) => fs.File.GetLastAccessTimeUtc(p));
        }

        [Test]
        public void MockDirectory_SetLastWriteTime_ShouldSetLastWriteTimeOnFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.Directory.SetLastWriteTime(p, d),
                (fs, p) => fs.File.GetLastWriteTime(p));
        }

        [Test]
        public void MockDirectory_SetLastWriteTimeUtc_ShouldSetLastWriteTimeUtcOnFile()
        {
            ExecuteTimeAttributeTest(
                (fs, p, d) => fs.Directory.SetLastWriteTimeUtc(p, d),
                (fs, p) => fs.File.GetLastWriteTimeUtc(p));
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnTrueForDirectoryDefinedInMemoryFileSystemWithoutTrailingSlash()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.Exists(XFS.Path(@"c:\foo"));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnTrueForDirectoryDefinedInMemoryFileSystemWithTrailingSlash()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.Exists(XFS.Path(@"c:\foo\"));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnFalseForDirectoryNotDefinedInMemoryFileSystemWithoutTrailingSlash()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.Exists(XFS.Path(@"c:\baz"));

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnFalseForDirectoryNotDefinedInMemoryFileSystemWithTrailingSlash()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.Exists(XFS.Path(@"c:\baz\"));

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnFalseForDirectoryNotDefinedInMemoryFileSystemWithSimilarFileName()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\baz.txt"), new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.Exists(XFS.Path(@"c:\baz"));

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnTrueForDirectoryCreatedViaMocks()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content") }
            });
            fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\bar"));

            // Act
            var result = fileSystem.Directory.Exists(XFS.Path(@"c:\bar"));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnTrueForFolderContainingFileAddedToMockFileSystem()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content"));

            // Act
            var result = fileSystem.Directory.Exists(XFS.Path(@"c:\foo\"));

            // Assert
            Assert.IsTrue(result);
        }

        [TestCase(@"\\s")]
        [TestCase(@"<")]
        [TestCase("\t")]
        public void MockDirectory_Exists_ShouldReturnFalseForIllegalPath(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.Directory.Exists(path);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockDirectory_Exists_ShouldReturnFalseForFiles()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content"));

            // Act
            var result = fileSystem.Directory.Exists(XFS.Path(@"c:\foo\bar.txt"));

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockDirectory_CreateDirectory_ShouldCreateFolderInMemoryFileSystem()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo.txt"), new MockFileData("Demo text content") }
            });

            // Act
            fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\bar"));

            // Assert
            Assert.IsTrue(fileSystem.FileExists(XFS.Path(@"c:\bar\")));
            Assert.IsTrue(fileSystem.AllDirectories.Any(d => d == XFS.Path(@"c:\bar")));
        }

        // Issue #210
        [Test]
        public void MockDirectory_CreateDirectory_ShouldIgnoreExistingDirectoryRegardlessOfTrailingSlash()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo\"), new MockDirectoryData() }
            });

            // Act/Assert
            Assert.That(() => fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\foo")), Throws.Nothing);
            Assert.That(() => fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\foo\")), Throws.Nothing);
        }

        [Test]
        public void MockDirectory_CreateDirectory_ShouldReturnDirectoryInfoBase()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo.txt"), new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\bar"));

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void MockDirectory_CreateDirectory_ShouldTrimTrailingSpaces()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\temp\folder "));

            // Assert
            Assert.IsTrue(fileSystem.Directory.Exists(XFS.Path(@"c:\temp\folder")));
        }

        [Test]
        public void MockDirectory_CreMockDirectory_CreateDirectory_ShouldReturnDirectoryInfoBaseWhenDirectoryExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo\"), new MockDirectoryData() }
            });

            // Act
            var result = fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\foo\"));

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.UNCPaths)]
        public void MockDirectory_CreateDirectory_ShouldWorkWithUNCPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.Directory.CreateDirectory(@"\\server\share\path\to\create");

            // Assert
            Assert.IsTrue(fileSystem.Directory.Exists(@"\\server\share\path\to\create\"));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.UNCPaths)]
        public void MockDirectory_CreateDirectory_ShouldFailIfTryingToCreateUNCPathOnlyServer()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var ex = Assert.Throws<ArgumentException>(() => fileSystem.Directory.CreateDirectory(@"\\server"));

            // Assert
            StringAssert.StartsWith("The UNC path should be of the form \\\\server\\share.", ex.Message);
            Assert.That(ex.ParamName, Is.EqualTo("path"));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.UNCPaths)]
        public void MockDirectory_CreateDirectory_ShouldSucceedIfTryingToCreateUNCPathShare()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.Directory.CreateDirectory(@"\\server\share");

            // Assert
            Assert.IsTrue(fileSystem.Directory.Exists(@"\\server\share\"));
        }

        [Test]
        public void MockDirectory_Delete_ShouldDeleteDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\bar\foo.txt"), new MockFileData("Demo text content") }
            });

            // Act
            fileSystem.Directory.Delete(XFS.Path(@"c:\bar"), true);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(XFS.Path(@"c:\bar")));
        }

        [Test]
        public void MockDirectory_Delete_ShouldNotDeleteAllDirectories()
        {
            // Arrange
            var folder1Path = XFS.Path(@"D:\Test\Program");
            var folder1SubFolderPath = XFS.Path(@"D:\Test\Program\Subfolder");
            var folder2Path = XFS.Path(@"D:\Test\Program_bak");

            var fileSystem = new MockFileSystem();

            fileSystem.AddDirectory(folder1Path);
            fileSystem.AddDirectory(folder2Path);
            fileSystem.AddDirectory(folder1SubFolderPath);

            // Act
            fileSystem.Directory.Delete(folder1Path, recursive: true);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(folder1Path));
            Assert.IsFalse(fileSystem.Directory.Exists(folder1SubFolderPath));
            Assert.IsTrue(fileSystem.Directory.Exists(folder2Path));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.CaseInsensitivity)]
        public void MockDirectory_Delete_ShouldDeleteDirectoryCaseInsensitively()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\bar\foo.txt"), new MockFileData("Demo text content") }
            });

            // Act
            fileSystem.Directory.Delete(XFS.Path(@"c:\BAR"), true);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(XFS.Path(@"c:\bar")));
        }

        [Test]
        [UnixOnly(UnixSpecifics.CaseSensitivity)]
        public void MockDirectory_Delete_ShouldThrowDirectoryNotFoundException_WhenSpecifiedWithInDifferentCase()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { "/bar/foo.txt", new MockFileData("Demo text content") }
            });

            // Act
            TestDelegate action = () => fileSystem.Directory.Delete("/BAR", true);

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action);
        }

        [Test]
        [UnixOnly(UnixSpecifics.CaseSensitivity)]
        public void MockDirectory_Delete_ShouldDeleteDirectoryCaseSensitively()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { "/bar/foo.txt", new MockFileData("Demo text content") }
            });

            // Act
            fileSystem.Directory.Delete("/bar", true);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists("/bar"));
        }

        [Test]
        public void MockDirectory_Delete_ShouldThrowDirectoryNotFoundException()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\bar\foo.txt"), new MockFileData("Demo text content") }
            });

            var ex = Assert.Throws<DirectoryNotFoundException>(() => fileSystem.Directory.Delete(XFS.Path(@"c:\baz")));

            Assert.That(ex.Message, Is.EqualTo(XFS.Path("c:\\baz") + " does not exist or could not be found."));
        }

        [Test]
        public void MockDirectory_Delete_ShouldThrowIOException()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\bar\foo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\bar\baz.txt"), new MockFileData("Demo text content") }
            });

            var ex = Assert.Throws<IOException>(() => fileSystem.Directory.Delete(XFS.Path(@"c:\bar")));

            Assert.That(ex.Message, Is.EqualTo("The directory specified by " + XFS.Path("c:\\bar") + " is read-only, or recursive is false and " + XFS.Path("c:\\bar") + " is not an empty directory."));
        }

        [Test]
        public void MockDirectory_Delete_ShouldDeleteDirectoryRecursively()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\bar\foo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\bar\bar2\foo.txt"), new MockFileData("Demo text content") }
            });

            // Act
            fileSystem.DirectoryInfo.FromDirectoryName(XFS.Path(@"c:\bar")).Delete(true);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(XFS.Path(@"c:\bar")));
            Assert.IsFalse(fileSystem.Directory.Exists(XFS.Path(@"c:\bar\bar2")));
        }

        [Test]
        public void MockDirectory_GetFileSystemEntries_Returns_Files_And_Directories()
        {
            string testPath = XFS.Path(@"c:\foo\bar.txt");
            string testDir = XFS.Path(@"c:\foo\bar");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { testPath, new MockFileData("Demo text content") },
                { testDir,  new MockDirectoryData() }
            });

            var entries = fileSystem.Directory.GetFileSystemEntries(XFS.Path(@"c:\foo")).OrderBy(k => k);
            Assert.AreEqual(2, entries.Count());
            Assert.AreEqual(testDir, entries.First());
            Assert.AreEqual(testPath, entries.Last());
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldThrowArgumentNullException_IfPathParamIsNull()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            TestDelegate action = () => fileSystem.Directory.GetFiles(null);
            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldThrowDirectoryNotFoundException_IfPathDoesNotExists()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.Directory.GetFiles(XFS.Path(@"c:\Foo"), "*a.txt");

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action);
        }

        [Test]
        public void MockDirectory_GetFiles_Returns_Files()
        {
            string testPath = XFS.Path(@"c:\foo\bar.txt");
            string testDir = XFS.Path(@"c:\foo\bar\");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { testPath, new MockFileData("Demo text content") },
                { testDir,  new MockDirectoryData() }
            });

            var entries = fileSystem.Directory.GetFiles(XFS.Path(@"c:\foo")).OrderBy(k => k);
            Assert.AreEqual(1, entries.Count());
            Assert.AreEqual(testPath, entries.First());
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldThrowAnArgumentNullException_IfSearchPatternIsNull()
        {
            // Arrange
            var directoryPath = XFS.Path(@"c:\Foo");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(directoryPath);

            // Act
            TestDelegate action = () => fileSystem.Directory.GetFiles(directoryPath, null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldThrowAnArgumentException_IfSearchPatternEndsWithTwoDots()
        {
            // Arrange
            var directoryPath = XFS.Path(@"c:\Foo");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(directoryPath);

            // Act
            TestDelegate action = () => fileSystem.Directory.GetFiles(directoryPath, "*a..");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [TestCase(@"..\")]
        [TestCase(@"aaa\vv..\")]
        [TestCase(@"a..\b")]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockDirectory_GetFiles_ShouldThrowAnArgumentException_IfSearchPatternContainsTwoDotsFollowedByOneBackslash(string searchPattern)
        {
            // Arrange
            var directoryPath = XFS.Path(@"c:\Foo");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(directoryPath);

            // Act
            TestDelegate action = () => fileSystem.Directory.GetFiles(directoryPath, searchPattern);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [TestCase(@"a../b")]
        [TestCase(@"../")]
        public void MockDirectory_GetFiles_ShouldThrowAnArgumentException_IfSearchPatternContainsTwoDotsFollowedByOneSlash(string searchPattern)
        {
            // Arrange
            var directoryPath = XFS.Path(@"c:\Foo");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(directoryPath);

            // Act
            TestDelegate action = () => fileSystem.Directory.GetFiles(directoryPath, searchPattern);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFindFilesContainingTwoOrMoreDots()
        {
            // Arrange
            string testPath = XFS.Path(@"c:\foo..r\bar.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { testPath, new MockFileData(string.Empty) }
                });

            // Act
            var actualResult = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), XFS.Path(@"foo..r\*"));

            // Assert
            Assert.That(actualResult, Is.EquivalentTo(new[] { testPath }));
        }

#if NET40
        [TestCase(@"""")]
#endif
        [TestCase("aa\t")]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockDirectory_GetFiles_ShouldThrowAnArgumentException_IfSearchPatternHasIllegalCharacters(string searchPattern)
        {
            // Arrange
            var directoryPath = XFS.Path(@"c:\Foo");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(directoryPath);

            // Act
            TestDelegate action = () => fileSystem.Directory.GetFiles(directoryPath, searchPattern);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void MockDirectory_GetRoot_Returns_Root()
        {
            string testDir = XFS.Path(@"c:\foo\bar\");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { testDir,  new MockDirectoryData() }
            });

            Assert.AreEqual(XFS.Path("C:\\"), fileSystem.Directory.GetDirectoryRoot(XFS.Path(@"C:\foo\bar")));
        }

#if NET40
        [Test]
        public void MockDirectory_GetLogicalDrives_Returns_LogicalDrives()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {XFS.Path(@"c:\foo\bar\"), new MockDirectoryData()},
                    {XFS.Path(@"c:\foo\baz\"), new MockDirectoryData()},
                    {XFS.Path(@"d:\bash\"), new MockDirectoryData()},
                });

            var drives = fileSystem.Directory.GetLogicalDrives();

            if (XFS.IsUnixPlatform())
            {
                Assert.AreEqual(1, drives.Length);
                Assert.IsTrue(drives.Contains("/"));
            }
            else
            {
                Assert.AreEqual(2, drives.Length);
                Assert.IsTrue(drives.Contains(@"C:\"));
                Assert.IsTrue(drives.Contains(@"D:\"));
            }
        }
#endif

        [Test]
        public void MockDirectory_GetDirectories_Returns_Child_Directories()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"A:\folder1\folder2\folder3\file.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"A:\folder1\folder4\file2.txt"), new MockFileData("Demo text content 2") },
            });

            var directories = fileSystem.Directory.GetDirectories(XFS.Path(@"A:\folder1")).ToArray();

            //Check that it does not returns itself
            Assert.IsFalse(directories.Contains(XFS.Path(@"A:\folder1")));

            //Check that it correctly returns all child directories
            Assert.AreEqual(2, directories.Count());
            Assert.IsTrue(directories.Contains(XFS.Path(@"A:\folder1\folder2")));
            Assert.IsTrue(directories.Contains(XFS.Path(@"A:\folder1\folder4")));
        }

        [Test]
        public void MockDirectory_GetDirectories_WithTopDirectories_ShouldOnlyReturnTopDirectories()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo.foo"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\.foo"));
            fileSystem.AddFile(XFS.Path(@"C:\Folder\.foo\bar"), new MockFileData(string.Empty));

            // Act
            var actualResult = fileSystem.Directory.GetDirectories(XFS.Path(@"c:\Folder\"), "*.foo");

            // Assert
            Assert.That(actualResult, Is.EquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo") }));
        }

        [Test]
        public void MockDirectory_GetDirectories_RelativeWithNoSubDirectories_ShouldReturnDirectories()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("Folder");

            // Act
            var actualResult = fileSystem.Directory.GetDirectories("Folder");

            // Assert
            Assert.That(actualResult, Is.Empty);
        }

        [TestCase(@"Folder\SubFolder")]
        [TestCase(@"Folder")]
        public void MockDirectory_GetDirectories_RelativeDirectory_WithoutChildren_ShouldReturnNoChildDirectories(string relativeDirPath)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(relativeDirPath);

            // Act
            var actualResult = fileSystem.Directory.GetDirectories(relativeDirPath);

            // Assert
            Assert.That(actualResult, Is.Empty);
        }

        [TestCase(@"Folder\SubFolder")]
        [TestCase(@"Folder")]
        public void MockDirectory_GetDirectories_RelativeDirectory_WithChildren_ShouldReturnChildDirectories(string relativeDirPath)
        {
            // Arrange
            var currentDirectory = XFS.Path(@"T:\foo");
            var fileSystem = new MockFileSystem(null, currentDirectory: currentDirectory);
            fileSystem.Directory.CreateDirectory(XFS.Path(relativeDirPath));
            fileSystem.Directory.CreateDirectory(XFS.Path(relativeDirPath + @"\child"));

            // Act
            var actualResult = fileSystem.Directory.GetDirectories(XFS.Path(relativeDirPath));

            // Assert
            CollectionAssert.AreEqual(
                new[] { XFS.Path(currentDirectory + @"\" + relativeDirPath + @"\child") },
                actualResult
            );
        }

        [Test]
        public void MockDirectory_GetDirectories_AbsoluteWithNoSubDirectories_ShouldReturnDirectories()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("Folder");

            // Act
            var fullPath = fileSystem.Path.GetFullPath("Folder");
            var actualResult = fileSystem.Directory.GetDirectories(fullPath);

            // Assert
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public void MockDirectory_GetDirectories_WithAllDirectories_ShouldReturnsAllMatchingSubFolders()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo.foo"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\.foo"));
            fileSystem.AddFile(XFS.Path(@"C:\Folder\.foo\bar"), new MockFileData(string.Empty));

            // Act
            var actualResult = fileSystem.Directory.GetDirectories(XFS.Path(@"c:\Folder\"), "*.foo", SearchOption.AllDirectories);

            // Assert
            Assert.That(actualResult, Is.EquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo"), XFS.Path(@"C:\Folder\.foo\.foo") }));
        }

        [Test]
        public void MockDirectory_GetDirectories_ShouldThrowWhenPathIsNotMocked()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\b.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\c.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\c.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\b.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\c.gif"), new MockFileData("Demo text content") },
            });

            // Act
            TestDelegate action = () => fileSystem.Directory.GetDirectories(XFS.Path(@"c:\d"));

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action);
        }

        [Test]
        public void MockDirectory_EnumerateDirectories_Returns_Child_Directories()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"A:\folder1\folder2\folder3\file.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"A:\folder1\folder4\file2.txt"), new MockFileData("Demo text content 2") },
            });

            var directories = fileSystem.Directory.EnumerateDirectories(XFS.Path(@"A:\folder1")).ToArray();

            //Check that it does not returns itself
            Assert.IsFalse(directories.Contains(XFS.Path(@"A:\folder1")));

            //Check that it correctly returns all child directories
            Assert.AreEqual(2, directories.Count());
            Assert.IsTrue(directories.Contains(XFS.Path(@"A:\folder1\folder2")));
            Assert.IsTrue(directories.Contains(XFS.Path(@"A:\folder1\folder4")));
        }

        [Test]
        public void MockDirectory_EnumerateDirectories_WithTopDirectories_ShouldOnlyReturnTopDirectories()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo.foo"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\.foo"));
            fileSystem.AddFile(XFS.Path(@"C:\Folder\.foo\bar"), new MockFileData(string.Empty));

            // Act
            var actualResult = fileSystem.Directory.EnumerateDirectories(XFS.Path(@"c:\Folder\"), "*.foo");

            // Assert
            Assert.That(actualResult, Is.EquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo") }));
        }

        [Test]
        public void MockDirectory_EnumerateDirectories_WithAllDirectories_ShouldReturnsAllMatchingSubFolders()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo.foo"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\.foo"));
            fileSystem.AddFile(XFS.Path(@"C:\Folder\.foo\bar"), new MockFileData(string.Empty));

            // Act
            var actualResult = fileSystem.Directory.EnumerateDirectories(XFS.Path(@"c:\Folder\"), "*.foo", SearchOption.AllDirectories);

            // Assert
            Assert.That(actualResult, Is.EquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo"), XFS.Path(@"C:\Folder\.foo\.foo") }));
        }

        [Test]
        public void MockDirectory_EnumerateDirectories_ShouldThrowWhenPathIsNotMocked()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\b.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\c.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\c.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\b.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\c.gif"), new MockFileData("Demo text content") },
            });

            // Act
            TestDelegate action = () => fileSystem.Directory.EnumerateDirectories(XFS.Path(@"c:\d"));

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action);
        }

        public static IEnumerable<object[]> GetPathsForMoving()
        {
            yield return new object[] { XFS.Path(@"a:\folder1\"), XFS.Path(@"A:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"folder2\file2.txt") };
            yield return new object[] { XFS.Path(@"A:\folder1\"), XFS.Path(@"A:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"folder2\file2.txt") };
            yield return new object[] { XFS.Path(@"a:\folder1\"), XFS.Path(@"a:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"folder2\file2.txt") };
            yield return new object[] { XFS.Path(@"A:\folder1\"), XFS.Path(@"a:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"folder2\file2.txt") };
            yield return new object[] { XFS.Path(@"A:\folder1\"), XFS.Path(@"a:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"Folder2\file2.txt") };
            yield return new object[] { XFS.Path(@"A:\folder1\"), XFS.Path(@"a:\folder3\"), XFS.Path("file.txt"), XFS.Path(@"Folder2\fiLe2.txt") };
            yield return new object[] { XFS.Path(@"A:\folder1\"), XFS.Path(@"a:\folder3\"), XFS.Path("folder444\\file.txt"), XFS.Path(@"Folder2\fiLe2.txt") };
        }

        [Test]
        public void Move_DirectoryExistsWithDifferentCase_DirectorySuccessfullyMoved()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\OLD_LOCATION\Data"));
            fileSystem.AddFile(XFS.Path(@"C:\old_location\Data\someFile.txt"), new MockFileData("abc"));

            // Act
            fileSystem.Directory.Move(XFS.Path(@"C:\old_location"), XFS.Path(@"C:\NewLocation\"));

            // Assert
            Assert.IsTrue(fileSystem.File.Exists(XFS.Path(@"C:\NewLocation\Data\someFile.txt")));
        }

        [TestCaseSource("GetPathsForMoving")]
        public void MockDirectory_Move_ShouldMoveDirectories(string sourceDirName, string destDirName, string filePathOne, string filePathTwo)
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(sourceDirName + filePathOne) , new MockFileData("aaa") },
                { XFS.Path(sourceDirName + filePathTwo) , new MockFileData("bbb") },
            });

            // Act
            fileSystem.DirectoryInfo.FromDirectoryName(sourceDirName).MoveTo(destDirName);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(sourceDirName));
            Assert.IsTrue(fileSystem.File.Exists(XFS.Path(destDirName + filePathOne)));
            Assert.IsTrue(fileSystem.File.Exists(XFS.Path(destDirName + filePathTwo)));
        }

        [Test]
        public void MockDirectory_Move_ShouldMoveFiles()
        {
            string sourceFilePath = XFS.Path(@"c:\demo.txt");
            string sourceFileContent = "this is some content";
            
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { sourceFilePath, new MockFileData(sourceFileContent) }
            });

            string destFilePath = XFS.Path(@"c:\demo1.txt");

            fileSystem.Directory.Move(sourceFilePath, destFilePath);

            Assert.That(fileSystem.FileExists(destFilePath), Is.True);
            Assert.That(fileSystem.FileExists(sourceFilePath), Is.False);
            Assert.That(fileSystem.GetFile(destFilePath).TextContents, Is.EqualTo(sourceFileContent));
        }

        [Test]
        public void MockDirectory_Move_ShouldMoveDirectoryAtrributes()
        {
            // Arrange
            var sourceDirName = XFS.Path(@"a:\folder1\");
            var destDirName = XFS.Path(@"a:\folder2\");
            const string filePathOne = "file1.txt";
            const string filePathTwo = "file2.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(sourceDirName + filePathOne) , new MockFileData("aaa") },
                { XFS.Path(sourceDirName + filePathTwo) , new MockFileData("bbb") },
            });

            var sourceDirectoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(sourceDirName);
            sourceDirectoryInfo.Attributes |= FileAttributes.System;

            // Act
            fileSystem.DirectoryInfo.FromDirectoryName(sourceDirName).MoveTo(destDirName);

            // Assert
            var destDirectoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(destDirName);
            Assert.IsTrue(destDirectoryInfo.Attributes.HasFlag(FileAttributes.System));
        }

        [Test]
        public void MockDirectory_Move_ShouldMoveDirectoryWithReadOnlySubDirectory()
        {
            // Arrange
            var sourceDirName = XFS.Path(@"a:\folder1\");
            var sourceSubDirName = XFS.Path(@"a:\folder1\sub\");

            var destDirName = XFS.Path(@"a:\folder2\");
            var destSubDirName = XFS.Path(@"a:\folder2\sub\");

            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(sourceSubDirName);

            var subDirectoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(sourceSubDirName);
            subDirectoryInfo.Attributes |= FileAttributes.ReadOnly;

            var sourceDirectoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(sourceDirName);

            // Act
            fileSystem.DirectoryInfo.FromDirectoryName(sourceDirName).MoveTo(destDirName);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(sourceSubDirName));
            Assert.IsTrue(fileSystem.FileExists(destSubDirName));
        }

        [Test]
        public void MockDirectory_GetCurrentDirectory_ShouldReturnValueFromFileSystemConstructor()
        {
            string directory = XFS.Path(@"D:\folder1\folder2");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>(), directory);

            var actual = fileSystem.Directory.GetCurrentDirectory();

            Assert.AreEqual(directory, actual);
        }

        [Test]
        public void MockDirectory_GetCurrentDirectory_ShouldReturnDefaultPathWhenNotSet()
        {
            string directory = XFS.Path(@"C:\");

            var fileSystem = new MockFileSystem();

            var actual = fileSystem.Directory.GetCurrentDirectory();

            Assert.AreEqual(directory, actual);
        }

        [Test]
        public void MockDirectory_SetCurrentDirectory_ShouldChangeCurrentDirectory()
        {
            string directory = XFS.Path(@"D:\folder1\folder2");
            var fileSystem = new MockFileSystem();

            // Precondition
            Assert.AreNotEqual(directory, fileSystem.Directory.GetCurrentDirectory());

            fileSystem.Directory.SetCurrentDirectory(directory);

            Assert.AreEqual(directory, fileSystem.Directory.GetCurrentDirectory());
        }

        [Test]
        public void MockDirectory_GetParent_ShouldThrowArgumentNullExceptionIfPathIsNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate act = () => fileSystem.Directory.GetParent(null);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Test]
        public void MockDirectory_GetParent_ShouldThrowArgumentExceptionIfPathIsEmpty()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate act = () => fileSystem.Directory.GetParent(string.Empty);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Test]
        public void MockDirectory_GetParent_ShouldReturnADirectoryInfoIfPathDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualResult = fileSystem.Directory.GetParent(XFS.Path(@"c:\directory\does\not\exist"));

            // Assert
            Assert.IsNotNull(actualResult);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockDirectory_GetParent_ShouldThrowArgumentExceptionIfPathHasIllegalCharacters()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate act = () => fileSystem.Directory.GetParent(XFS.Path("c:\\director\ty\\has\\illegal\\character"));

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Test]
        public void MockDirectory_GetParent_ShouldReturnNullIfPathIsRoot()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\"));

            // Act
            var actualResult = fileSystem.Directory.GetParent(XFS.Path(@"c:\"));

            // Assert
            Assert.IsNull(actualResult);
        }

        [Test]
        [UnixOnly(UnixSpecifics.SlashRoot)]
        public void MockDirectory_GetParent_ShouldReturnRootIfDirectoryIsInRoot()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory("/bar");

            // Act
            var parent = fileSystem.Directory.GetParent("/bar");

            // Assert
            Assert.AreEqual("/", parent.FullName);
        }

        public static IEnumerable<string[]> MockDirectory_GetParent_Cases
        {
            get
            {
                yield return new[] { XFS.Path(@"c:\a"), XFS.Path(@"c:\") };
                yield return new[] { XFS.Path(@"c:\a\b\c\d"), XFS.Path(@"c:\a\b\c") };
                yield return new[] { XFS.Path(@"c:\a\b\c\d\"), XFS.Path(@"c:\a\b\c") };
            }
        }

        public void MockDirectory_GetParent_ShouldReturnTheParentWithoutTrailingDirectorySeparatorChar(string path, string expectedResult)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(path);

            // Act
            var actualResult = fileSystem.Directory.GetParent(path);

            // Assert
            Assert.AreEqual(expectedResult, actualResult.FullName);
        }

        [Test]
        public void MockDirectory_Move_ShouldThrowAnIOExceptionIfBothPathAreIdentical()
        {
            // Arrange
            string path = XFS.Path(@"c:\a");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(path);

            // Act
            TestDelegate action = () => fileSystem.Directory.Move(path, path);

            // Assert
            Assert.Throws<IOException>(action, "Source and destination path must be different.");
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void MockDirectory_Move_ShouldThrowAnIOExceptionIfDirectoriesAreOnDifferentVolumes()
        {
            // Arrange
            string sourcePath = XFS.Path(@"c:\a");
            string destPath = XFS.Path(@"d:\v");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(sourcePath);

            // Act
            TestDelegate action = () => fileSystem.Directory.Move(sourcePath, destPath);

            // Assert
            Assert.Throws<IOException>(action, "Source and destination path must have identical roots. Move will not work across volumes.");
        }

        [Test]
        public void MockDirectory_Move_ShouldThrowADirectoryNotFoundExceptionIfDesinationDirectoryDoesNotExist()
        {
            // Arrange
            string sourcePath = XFS.Path(@"c:\a");
            string destPath = XFS.Path(@"c:\b");
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.Directory.Move(sourcePath, destPath);

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action, "Could not find a part of the path 'c:\a'.");
        }

        [Test]
        public void MockDirectory_Move_ShouldThrowAnIOExceptionIfDesinationDirectoryExists()
        {
            // Arrange
            string sourcePath = XFS.Path(@"c:\a");
            string destPath = XFS.Path(@"c:\b");
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(sourcePath);
            fileSystem.AddDirectory(destPath);

            // Act
            TestDelegate action = () => fileSystem.Directory.Move(sourcePath, destPath);

            // Assert
            Assert.Throws<IOException>(action, "Cannot create 'c:\b\' because a file or directory with the same name already exists.'");
        }

        [Test]
        public void MockDirectory_EnumerateFiles_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndSearchOptionIsAllDirectories()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            IEnumerable<string> expected = new[]
            {
                XFS.Path(@"c:\a\a.txt"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\c.txt"),
                XFS.Path(@"c:\a\a\a.txt"),
                XFS.Path(@"c:\a\a\b.txt"),
                XFS.Path(@"c:\a\a\c.gif")
            };

            // Act
            var result = fileSystem.Directory.EnumerateFiles(XFS.Path(@"c:\a"), "*", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_EnumerateFiles_ShouldFilterByExtensionBasedSearchPattern()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            var expected = new[]
            {
                XFS.Path(@"c:\a.gif"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\a\c.gif")
            };

            // Act
            var result = fileSystem.Directory.EnumerateFiles(XFS.Path(@"c:\"), "*.gif", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_EnumerateFiles_WhenFilterIsUnRooted_ShouldFindFilesInCurrentDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), MockFileData.NullObject },
                { XFS.Path(@"c:\a\a.txt"), MockFileData.NullObject },
                { XFS.Path(@"c:\a\b\b.txt"), MockFileData.NullObject },
                { XFS.Path(@"c:\a\c\c.txt"), MockFileData.NullObject },
            });

            var expected = new[]
            {
                XFS.Path(@"c:\a\b\b.txt"),
            };

            fileSystem.Directory.SetCurrentDirectory(XFS.Path(@"c:\a"));

            // Act
            var result = fileSystem.Directory.EnumerateFiles(XFS.Path("b"));

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_EnumerateFiles_WhenFilterIsUnRooted_ShouldNotFindFilesInPathOutsideCurrentDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), MockFileData.NullObject },
                { XFS.Path(@"c:\a\b\b.txt"), MockFileData.NullObject },
                { XFS.Path(@"c:\c\b\b.txt"), MockFileData.NullObject },
            });

            var expected = new[]
            {
                XFS.Path(@"c:\a\b\b.txt"),
            };

            fileSystem.Directory.SetCurrentDirectory(XFS.Path(@"c:\a"));

            // Act
            var result = fileSystem.Directory.EnumerateFiles(XFS.Path("b"));

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_EnumerateFileSystemEntries_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndSearchOptionIsAllDirectories()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            IEnumerable<string> expected = new[]
            {
                XFS.Path(@"c:\a\a.txt"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\c.txt"),
                XFS.Path(@"c:\a\a\a.txt"),
                XFS.Path(@"c:\a\a\b.txt"),
                XFS.Path(@"c:\a\a\c.gif"),
                XFS.Path(@"c:\a\a")
            };

            // Act
            var result = fileSystem.Directory.EnumerateFileSystemEntries(XFS.Path(@"c:\a"), "*", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_EnumerateFileSystemEntries_ShouldFilterByExtensionBasedSearchPattern()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            var expected = new[]
            {
                XFS.Path(@"c:\a.gif"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\a\c.gif")
            };

            // Act
            var result = fileSystem.Directory.EnumerateFileSystemEntries(XFS.Path(@"c:\"), "*.gif", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetAccessControl_ShouldThrowExceptionOnDirectoryNotFound()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Assert.Throws<DirectoryNotFoundException>(() => fileSystem.Directory.GetAccessControl(XFS.Path(@"c:\foo")));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.AccessControlLists)]
        public void MockDirectory_GetAccessControl_ShouldReturnNewDirectorySecurity()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\foo\"));

            // Act
            DirectorySecurity result = fileSystem.Directory.GetAccessControl(XFS.Path(@"c:\foo\"));

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void MockDirectory_SetCreationTime_ShouldNotThrowWithoutTrailingBackslash()
        {
            var path = XFS.Path(@"C:\NoTrailingBackslash");
            var fs = new MockFileSystem();
            fs.Directory.CreateDirectory(path);
            fs.Directory.SetCreationTime(path, DateTime.Now);
            fs.Directory.Delete(path);
        }
    }
}
