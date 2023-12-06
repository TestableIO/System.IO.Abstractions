using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockDirectoryTests
    {
        [Test]
        public void MockDirectory_GetFiles_ShouldReturnAllFilesBelowPathWhenPatternIsEmptyAndSearchOptionIsAllDirectories()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            var expected = new[]
            {
                XFS.Path(@"c:\a\a.txt"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\c.txt"),
                XFS.Path(@"c:\a\d"),
                XFS.Path(@"c:\a\a\a.txt"),
                XFS.Path(@"c:\a\a\b.txt"),
                XFS.Path(@"c:\a\a\c.gif"),
                XFS.Path(@"c:\a\a\d")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldReturnFilesDirectlyBelowPathWhenPatternIsEmptyAndSearchOptionIsTopDirectoryOnly()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            var expected = new[]
            {
                XFS.Path(@"c:\a\a.txt"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\c.txt"),
                XFS.Path(@"c:\a\d")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "", SearchOption.TopDirectoryOnly);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

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
                XFS.Path(@"c:\a\d"),
                XFS.Path(@"c:\a\a\a.txt"),
                XFS.Path(@"c:\a\a\b.txt"),
                XFS.Path(@"c:\a\a\c.gif"),
                XFS.Path(@"c:\a\a\d")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\a"), "*", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

#if FEATURE_ENUMERATION_OPTIONS
        [Test]
        public void MockDirectory_GetFiles_ShouldReturnAllPatternMatchingFilesWhenEnumerationOptionHasRecurseSubdirectoriesSetToTrue()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            var expected = new[]
            {
                XFS.Path(@"c:\b.txt"),
                XFS.Path(@"c:\c.txt"),
                XFS.Path(@"c:\a\a.txt"),
                XFS.Path(@"c:\a\c.txt"),
                XFS.Path(@"c:\a\a\a.txt"),
                XFS.Path(@"c:\a\a\b.txt")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.txt", new EnumerationOptions { RecurseSubdirectories = true });

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }
#endif

        private MockFileSystem SetupFileSystem()
        {
            return new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\b.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\c.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\d"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\c.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\d"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\b.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\c.gif"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\a\d"), new MockFileData("Demo text content") }
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
                XFS.Path(@"c:\a\c.txt"),
                XFS.Path(@"c:\a\d")
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
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternWithThreeCharacterLongFileExtension_RespectingAllDirectorySearchOption()
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
        public void MockDirectory_GetFiles_ShouldFilterByExtensionBasedSearchPatternWithThreeCharacterLongFileExtension_RespectingTopDirectorySearchOption()
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

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterForAllFilesWithNoExtensionsAndSearchOptionTopDirectoryOnly()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            fileSystem.AddFile(XFS.Path(@"C:\mytestfilename"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfilename."), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name."), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again."), new MockFileData("some content"));

            var expected = new[]
            {
                XFS.Path(@"c:\d"),
                XFS.Path(@"C:\mytestfilename"),
                XFS.Path(@"C:\mytestfilename."),
                XFS.Path(@"C:\mytestfile.name."),
                XFS.Path(@"C:\mytestfile.name.again.")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"C:\"), "*.", SearchOption.TopDirectoryOnly);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterForAllFilesWithNoExtensionsAndSearchOptionAllDirectories()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfilename"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfilename."), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfile.name"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfile.name."), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfile.name.again"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\specialNameFormats\mytestfile.name.again."), new MockFileData("some content"));

            var expected = new[]
            {
                XFS.Path(@"c:\d"),
                XFS.Path(@"c:\a\d"),
                XFS.Path(@"c:\a\a\d"),
                XFS.Path(@"C:\specialNameFormats\mytestfilename"),
                XFS.Path(@"C:\specialNameFormats\mytestfilename."),
                XFS.Path(@"C:\specialNameFormats\mytestfile.name."),
                XFS.Path(@"C:\specialNameFormats\mytestfile.name.again.")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"c:\"), "*.", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterForFilesWithNoExtensionsAndNonTrivialFilterAndSearchOptionTopDirectoryOnly()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            fileSystem.AddFile(XFS.Path(@"C:\mytestfilename"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfilename."), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name."), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again."), new MockFileData("some content"));

            var expected = new[]
            {
                XFS.Path(@"C:\mytestfilename"),
                XFS.Path(@"C:\mytestfilename."),
                XFS.Path(@"C:\mytestfile.name."),
                XFS.Path(@"C:\mytestfile.name.again.")

            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"C:\"), "my??s*.", SearchOption.TopDirectoryOnly);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterForFilesWithNoExtensionsAndNonTrivialFilter2AndSearchOptionTopDirectoryOnly()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            fileSystem.AddFile(XFS.Path(@"C:\mytestfilename"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfilename."), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name."), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again."), new MockFileData("some content"));

            var expected = new[]
            {
                XFS.Path(@"C:\mytestfile.name"),
                XFS.Path(@"C:\mytestfile.name."),
                XFS.Path(@"C:\mytestfile.name.again.")

            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"C:\"), "my*.n*.", SearchOption.TopDirectoryOnly);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MockDirectory_GetFiles_ShouldFilterForFilesWithNoExtensionsAndFilterThatIncludesDotAndSearchOptionTopDirectoryOnly()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            fileSystem.AddFile(XFS.Path(@"C:\mytestfilename"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfilename."), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name."), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again"), new MockFileData("some content"));
            fileSystem.AddFile(XFS.Path(@"C:\mytestfile.name.again."), new MockFileData("some content"));

            var expected = new[]
            {
                XFS.Path(@"C:\mytestfile.name"),
                XFS.Path(@"C:\mytestfile.name."),
                XFS.Path(@"C:\mytestfile.name.again.")
            };

            // Act
            var result = fileSystem.Directory.GetFiles(XFS.Path(@"C:\"), "my*.n*.", SearchOption.TopDirectoryOnly);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        private void ExecuteTimeAttributeTest(DateTime time, Action<IFileSystem, string, DateTime> setter, Func<IFileSystem, string, DateTime> getter)
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            setter(fileSystem, path, time);
            var result = getter(fileSystem, path);

            // Assert
            Assert.That(result, Is.EqualTo(time));
        }

        [Test]
        public void MockDirectory_GetCreationTime_ShouldReturnCreationTimeFromFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42),
                (fs, p, d) => fs.File.SetCreationTime(p, d),
                (fs, p) => fs.Directory.GetCreationTime(p));
        }

        [Test]
        public void MockDirectory_GetCreationTimeUtc_ShouldReturnCreationTimeUtcFromFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
                (fs, p, d) => fs.File.SetCreationTimeUtc(p, d),
                (fs, p) => fs.Directory.GetCreationTimeUtc(p));
        }

        [Test]
        public void MockDirectory_GetLastAccessTime_ShouldReturnLastAccessTimeFromFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42),
                (fs, p, d) => fs.File.SetLastAccessTime(p, d),
                (fs, p) => fs.Directory.GetLastAccessTime(p));
        }

        [Test]
        public void MockDirectory_GetLastAccessTimeUtc_ShouldReturnLastAccessTimeUtcFromFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
                (fs, p, d) => fs.File.SetLastAccessTimeUtc(p, d),
                (fs, p) => fs.Directory.GetLastAccessTimeUtc(p));
        }

        [Test]
        public void MockDirectory_GetLastWriteTime_ShouldReturnLastWriteTimeFromFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42),
                (fs, p, d) => fs.File.SetLastWriteTime(p, d),
                (fs, p) => fs.Directory.GetLastWriteTime(p));
        }

        [Test]
        public void MockDirectory_GetLastWriteTimeUtc_ShouldReturnLastWriteTimeUtcFromFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
                (fs, p, d) => fs.File.SetLastWriteTimeUtc(p, d),
                (fs, p) => fs.Directory.GetLastWriteTimeUtc(p));
        }

        [Test]
        public void MockDirectory_SetCreationTime_ShouldSetCreationTimeOnFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42),
                (fs, p, d) => fs.Directory.SetCreationTime(p, d),
                (fs, p) => fs.File.GetCreationTime(p));
        }

        [Test]
        public void MockDirectory_SetCreationTimeUtc_ShouldSetCreationTimeUtcOnFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
                (fs, p, d) => fs.Directory.SetCreationTimeUtc(p, d),
                (fs, p) => fs.File.GetCreationTimeUtc(p));
        }

        [Test]
        public void MockDirectory_SetLastAccessTime_ShouldSetLastAccessTimeOnFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42),
                (fs, p, d) => fs.Directory.SetLastAccessTime(p, d),
                (fs, p) => fs.File.GetLastAccessTime(p));
        }

        [Test]
        public void MockDirectory_SetLastAccessTimeUtc_ShouldSetLastAccessTimeUtcOnFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
                (fs, p, d) => fs.Directory.SetLastAccessTimeUtc(p, d),
                (fs, p) => fs.File.GetLastAccessTimeUtc(p));
        }

        [Test]
        public void MockDirectory_SetLastWriteTime_ShouldSetLastWriteTimeOnFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42),
                (fs, p, d) => fs.Directory.SetLastWriteTime(p, d),
                (fs, p) => fs.File.GetLastWriteTime(p));
        }

        [Test]
        public void MockDirectory_SetLastWriteTimeUtc_ShouldSetLastWriteTimeUtcOnFile()
        {
            ExecuteTimeAttributeTest(
                new DateTime(2010, 6, 4, 13, 26, 42, DateTimeKind.Utc),
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
        public void MockDirectory_CreateDirectory_WithConflictingFile_ShouldThrowIOException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\foo\bar.txt"), new MockFileData("Demo text content"));
            
            // Act
            TestDelegate action = () => fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\foo\bar.txt"));

            // Assert
            Assert.Throws<IOException>(action);
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

        [Test]
        public void MockDirectory_CreateDirectory_ShouldThrowIfIllegalCharacterInPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\foo.txt"), new MockFileData("Demo text content") }
            });

            // Act
            TestDelegate createDelegate = () => fileSystem.Directory.CreateDirectory(XFS.Path(@"c:\bar_?_"));

            // Assert
            Assert.Throws<ArgumentException>(createDelegate);
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

#if FEATURE_CREATE_TEMP_SUBDIRECTORY
        [Test]
        public void MockDirectory_CreateTempSubdirectory_ShouldCreateSubdirectoryInTempDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.Directory.CreateTempSubdirectory();

            // Assert
            Assert.IsTrue(fileSystem.Directory.Exists(result.FullName));
            Assert.IsTrue(result.FullName.Contains(Path.GetTempPath()));
        }

        [Test]
        public void MockDirectory_CreateTempSubdirectoryWithPrefix_ShouldCreateDirectoryWithGivenPrefixInTempDirectory()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.Directory.CreateTempSubdirectory("foo-");

            // Assert
            Assert.IsTrue(fileSystem.Directory.Exists(result.FullName));
            Assert.IsTrue(Path.GetFileName(result.FullName).StartsWith("foo-"));
            Assert.IsTrue(result.FullName.Contains(Path.GetTempPath()));
        }
#endif

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

            Assert.That(ex.Message, Is.EqualTo($"'{XFS.Path("c:\\baz")}' does not exist or could not be found."));
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
            fileSystem.DirectoryInfo.New(XFS.Path(@"c:\bar")).Delete(true);

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
            Assert.That(entries.Count(), Is.EqualTo(2));
            Assert.That(entries.First(), Is.EqualTo(testDir));
            Assert.That(entries.Last(), Is.EqualTo(testPath));
        }

        [Test]
        public void MockDirectory_GetFileSystemEntries_ShouldNotReturnSubDirectory_WithSearchOption()
        {
            string testPath = XFS.Path(@"c:\foo\bar.txt");
            string testDir = XFS.Path(@"c:\foo\bar");
            string testSubDir = XFS.Path(@"c:\foo\bar\baz");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { testPath, new MockFileData("Demo text content") },
                { testSubDir,  new MockDirectoryData() },
            });

            var entries = fileSystem.Directory.GetFileSystemEntries(XFS.Path(@"c:\foo"), "*", SearchOption.TopDirectoryOnly).OrderBy(k => k);
            Assert.That(entries.Count(), Is.EqualTo(2));
            Assert.That(entries.First(), Is.EqualTo(testDir));
            Assert.That(entries.Last(), Is.EqualTo(testPath));
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
            Assert.That(entries.Count(), Is.EqualTo(1));
            Assert.That(entries.First(), Is.EqualTo(testPath));
        }

        [Test]
        public void MockDirectory_GetFiles_Returns_Files_WithRelativePath()
        {
            // arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            string directory = XFS.Path(@"C:\foo");

            fileSystem.Directory.SetCurrentDirectory(directory);
            fileSystem.AddFile(XFS.Path(@"C:\test.txt"), new MockFileData("Some ASCII text."));

            Assert.That(fileSystem.Directory.GetFiles(XFS.Path(@"..\")).Length, Is.EqualTo(1)); // Assert with relative path
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

            Assert.That(fileSystem.Directory.GetDirectoryRoot(XFS.Path(@"C:\foo\bar")), Is.EqualTo(XFS.Path("C:\\")));
        }

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
                Assert.That(drives.Length, Is.EqualTo(1));
                Assert.IsTrue(drives.Contains("/"));
            }
            else
            {
                Assert.That(drives.Length, Is.EqualTo(2));
                Assert.IsTrue(drives.Contains(@"C:\"));
                Assert.IsTrue(drives.Contains(@"D:\"));
            }
        }

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
            Assert.That(directories.Count(), Is.EqualTo(2));
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
            var actualResult = fileSystem.Directory.GetDirectories(XFS.Path(@"C:\Folder\"), "*.foo");

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
            Assert.That(actualResult, Is.EqualTo(new[] { XFS.Path(relativeDirPath + @"\child") }));
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
            var actualResult = fileSystem.Directory.GetDirectories(XFS.Path(@"C:\Folder\"), "*.foo", SearchOption.AllDirectories);

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
            Assert.That(directories.Count(), Is.EqualTo(2));
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
            var actualResult = fileSystem.Directory.EnumerateDirectories(XFS.Path(@"C:\Folder\"), "*.foo");

            // Assert
            Assert.That(actualResult, Is.EquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo") }));
        }

#if FEATURE_ENUMERATION_OPTIONS
        [Test]
        public void MockDirectory_EnumerateDirectories_WithEnumerationOptionsTopDirectories_ShouldOnlyReturnTopDirectories()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\foo.foo"));
            fileSystem.AddDirectory(XFS.Path(@"C:\Folder\.foo\.foo"));
            fileSystem.AddFile(XFS.Path(@"C:\Folder\.foo\bar"), new MockFileData(string.Empty));

            var enumerationOptions = new EnumerationOptions
            {
                RecurseSubdirectories = false
            };

            // Act
            var actualResult = fileSystem.Directory.EnumerateDirectories(XFS.Path(@"C:\Folder\"), "*.foo", enumerationOptions);

            // Assert
            Assert.That(actualResult, Is.EquivalentTo(new[] { XFS.Path(@"C:\Folder\.foo"), XFS.Path(@"C:\Folder\foo.foo") }));
        }
#endif
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
            var actualResult = fileSystem.Directory.EnumerateDirectories(XFS.Path(@"C:\Folder\"), "*.foo", SearchOption.AllDirectories);

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
        
        [TestCaseSource(nameof(GetPrefixTestPaths))]
        public void MockDirectory_EnumerateDirectories_ShouldReturnPathsPrefixedWithQueryPath(
            string queryPath, string expectedPath)
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("Folder/SubFolder");
            
            var actualResult = fileSystem.Directory.EnumerateDirectories(queryPath);
            
            Assert.That(actualResult, Is.EqualTo(new[] { expectedPath }));
        }
        
        private static IEnumerable<object[]> GetPrefixTestPaths()
        {
            var sep = Path.DirectorySeparatorChar;
            yield return new object[] { "Folder", $"Folder{sep}SubFolder" };
            yield return new object[] { $"Folder{sep}", $"Folder{sep}SubFolder" };
            yield return new object[] { $"Folder{sep}..{sep}.{sep}Folder", $"Folder{sep}..{sep}.{sep}Folder{sep}SubFolder" };
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

        [TestCaseSource(nameof(GetPathsForMoving))]
        public void MockDirectory_Move_ShouldMoveDirectories(string sourceDirName, string destDirName, string filePathOne, string filePathTwo)
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(sourceDirName + filePathOne) , new MockFileData("aaa") },
                { XFS.Path(sourceDirName + filePathTwo) , new MockFileData("bbb") },
            });

            // Act
            fileSystem.DirectoryInfo.New(sourceDirName).MoveTo(destDirName);

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
        public void MockDirectory_Move_ShouldMoveDirectoryAttributes()
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

            var sourceDirectoryInfo = fileSystem.DirectoryInfo.New(sourceDirName);
            sourceDirectoryInfo.Attributes |= FileAttributes.System;

            // Act
            fileSystem.DirectoryInfo.New(sourceDirName).MoveTo(destDirName);

            // Assert
            var destDirectoryInfo = fileSystem.DirectoryInfo.New(destDirName);
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

            var subDirectoryInfo = fileSystem.DirectoryInfo.New(sourceSubDirName);
            subDirectoryInfo.Attributes |= FileAttributes.ReadOnly;

            var sourceDirectoryInfo = fileSystem.DirectoryInfo.New(sourceDirName);

            // Act
            fileSystem.DirectoryInfo.New(sourceDirName).MoveTo(destDirName);

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(sourceSubDirName));
            Assert.IsTrue(fileSystem.FileExists(destSubDirName));
        }

        [Test]
        public void MockDirectory_Move_ShouldOnlyMoveDirAndFilesWithinDir()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {XFS.Path(@"c:\source\dummy"), new MockDirectoryData()},
                {XFS.Path(@"c:\source\dummy\content.txt"), new MockFileData(new byte[] {0})},
                {XFS.Path(@"c:\source\dummy.txt"), new MockFileData(new byte[] {0})},
                {XFS.Path(@"c:\source\dummy2"), new MockDirectoryData()},
                {XFS.Path(@"c:\destination"), new MockDirectoryData()},
            });

            // Act
            fileSystem.Directory.Move(XFS.Path(@"c:\source\dummy"), XFS.Path(@"c:\destination\dummy"));

            // Assert
            Assert.That(fileSystem.FileExists(XFS.Path(@"c:\source\dummy.txt")), Is.True);
            Assert.That(fileSystem.Directory.Exists(XFS.Path(@"c:\source\dummy2")), Is.True);
        }

        [Test]
        public void MockDirectory_GetCurrentDirectory_ShouldReturnValueFromFileSystemConstructor()
        {
            string directory = XFS.Path(@"D:\folder1\folder2");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>(), directory);

            var actual = fileSystem.Directory.GetCurrentDirectory();

            Assert.That(actual, Is.EqualTo(directory));
        }

        [Test]
        public void MockDirectory_GetCurrentDirectory_ShouldReturnDefaultPathWhenNotSet()
        {
            string directory = XFS.Path(@"C:\");

            var fileSystem = new MockFileSystem();

            var actual = fileSystem.Directory.GetCurrentDirectory();

            Assert.That(actual, Is.EqualTo(directory));
        }

        [Test]
        public void MockDirectory_SetCurrentDirectory_ShouldChangeCurrentDirectory()
        {
            string directory = XFS.Path(@"D:\folder1\folder2");
            var fileSystem = new MockFileSystem();

            // Precondition
            Assert.AreNotEqual(directory, fileSystem.Directory.GetCurrentDirectory());

            fileSystem.Directory.SetCurrentDirectory(directory);

            Assert.That(fileSystem.Directory.GetCurrentDirectory(), Is.EqualTo(directory));
        }

        [Test]
        public void MockDirectory_SetCurrentDirectory_WithRelativePath_ShouldUseFullPath()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.SetCurrentDirectory(".");

            var result = fileSystem.Directory.GetCurrentDirectory();

            Assert.IsTrue(fileSystem.Path.IsPathRooted(result));
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
            Assert.That(parent.FullName, Is.EqualTo("/"));
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
            Assert.That(actualResult.FullName, Is.EqualTo(expectedResult));
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
        public void MockDirectory_Move_ShouldThrowADirectoryNotFoundExceptionIfDestinationDirectoryDoesNotExist()
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
        public void MockDirectory_Move_ShouldThrowAnIOExceptionIfDestinationDirectoryExists()
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
                XFS.Path(@"c:\a\d"),
                XFS.Path(@"c:\a\a\a.txt"),
                XFS.Path(@"c:\a\a\b.txt"),
                XFS.Path(@"c:\a\a\c.gif"),
                XFS.Path(@"c:\a\a\d")
            };

            // Act
            var result = fileSystem.Directory.EnumerateFiles(XFS.Path(@"c:\a"), "*", SearchOption.AllDirectories);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

#if FEATURE_ENUMERATION_OPTIONS
        [Test]
        public void MockDirectory_EnumerateFiles_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndEnumerationOptionsIsAllDirectories()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            IEnumerable<string> expected = new[]
            {
                XFS.Path(@"c:\a\a.txt"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\c.txt"),
                XFS.Path(@"c:\a\d"),
                XFS.Path(@"c:\a\a\a.txt"),
                XFS.Path(@"c:\a\a\b.txt"),
                XFS.Path(@"c:\a\a\c.gif"),
                XFS.Path(@"c:\a\a\d")
            };

            var enumerationOptions = new EnumerationOptions
            {
                RecurseSubdirectories = true
            };

            // Act
            var result = fileSystem.Directory.EnumerateFiles(XFS.Path(@"c:\a"), "*", enumerationOptions);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

#endif

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
            var someContent = new MockFileData(String.Empty);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), someContent },
                { XFS.Path(@"c:\a\a.txt"), someContent },
                { XFS.Path(@"c:\a\b\b.txt"), someContent },
                { XFS.Path(@"c:\a\c\c.txt"), someContent },
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
            var someContent = new MockFileData(String.Empty);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), someContent },
                { XFS.Path(@"c:\a\b\b.txt"), someContent },
                { XFS.Path(@"c:\c\b\b.txt"), someContent },
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
                XFS.Path(@"c:\a\d"),
                XFS.Path(@"c:\a\a\a.txt"),
                XFS.Path(@"c:\a\a\b.txt"),
                XFS.Path(@"c:\a\a\c.gif"),
                XFS.Path(@"c:\a\a\d"),
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

#if FEATURE_ENUMERATION_OPTIONS
        [Test]
        public void MockDirectory_EnumerateFileSystemEntries_ShouldReturnAllFilesBelowPathWhenPatternIsWildcardAndEnumerationOptionsIsAllDirectories()
        {
            // Arrange
            var fileSystem = SetupFileSystem();
            IEnumerable<string> expected = new[]
            {
                XFS.Path(@"c:\a\a.txt"),
                XFS.Path(@"c:\a\b.gif"),
                XFS.Path(@"c:\a\c.txt"),
                XFS.Path(@"c:\a\d"),
                XFS.Path(@"c:\a\a\a.txt"),
                XFS.Path(@"c:\a\a\b.txt"),
                XFS.Path(@"c:\a\a\c.gif"),
                XFS.Path(@"c:\a\a\d"),
                XFS.Path(@"c:\a\a")
            };

            var enumerationOptions = new EnumerationOptions
            {
                RecurseSubdirectories = true
            };

            // Act
            var result = fileSystem.Directory.EnumerateFileSystemEntries(XFS.Path(@"c:\a"), "*", enumerationOptions);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }
#endif

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
        [SupportedOSPlatform("windows")]
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

        private static IEnumerable<TestCaseData> Failing_DirectoryMoveFromToPaths
        {
            get
            {
                var testTargetDirs = new[]
                {
                    @"c:\temp2\fd\df", @"c:\temp2\fd\", @"c:\temp2\fd\..\fd", @"c:\temp2\fd", @".\..\temp2\fd\df",
                    @".\..\temp2\fd\df\..", @".\..\temp2\fd\df\..\", @"..\temp2\fd\", @".\temp2\fd", @"temp2\fd",
                    @"c:\temp3\exists2\d3", @"c:\temp4\exists"
                };

                var testSourceDirs = new[] { @"c:\temp\exists\foldertomove", @"c:\temp3\exists", @"c:\temp3" };

                return
                    from s in testSourceDirs
                    from t in testTargetDirs
                    select new TestCaseData(XFS.Path(s), XFS.Path(t));
            }
        }

        [Test]
        [TestCaseSource(nameof(Failing_DirectoryMoveFromToPaths))]
        public void Move_Directory_Throws_When_Target_Directory_Parent_Does_Not_Exist(
            string sourceDirName,
            string targetDirName)
        {
            // Arange
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(sourceDirName);

            // Act
            Assert.Throws<DirectoryNotFoundException>(() =>
                fileSystem.Directory.Move(sourceDirName, targetDirName));

            // Assert
            Assert.IsFalse(fileSystem.Directory.Exists(targetDirName));
            Assert.IsTrue(fileSystem.Directory.Exists(sourceDirName));
        }

        private static IEnumerable<TestCaseData> Success_DirectoryMoveFromToPaths
        {
            get
            {
                var testTargetDirs = new[]
                {
                    @"c:\temp2\", @"c:\temp2", @"c:\temp2\..\temp2", @".\..\temp2", @".\..\temp2\..\temp2",
                    @".\..\temp2\fd\df\..\..", @".\..\temp2\fd\df\..\..\", @"..\temp2", @".\temp2", @"\temp2", @"temp2",
                };

                var testSourceDirs = new[] { @"c:\temp3\exists\foldertomove", @"c:\temp3\exists", @"c:\temp4" };

                return
                    from s in testSourceDirs
                    from t in testTargetDirs
                    select new TestCaseData(XFS.Path(s), XFS.Path(t));
            }
        }

        [Test]
        [TestCaseSource(nameof(Success_DirectoryMoveFromToPaths))]
        public void Move_Directory_DoesNotThrow_When_Target_Directory_Parent_Exists(
            string sourceDirName,
            string targetDirName)
        {
            // Arange
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(sourceDirName);

            // Act
            Assert.DoesNotThrow(() =>
                fileSystem.Directory.Move(sourceDirName, targetDirName));

            // Assert
            Assert.IsTrue(fileSystem.Directory.Exists(targetDirName));
            Assert.IsFalse(fileSystem.Directory.Exists(sourceDirName));
        }
    }
}
