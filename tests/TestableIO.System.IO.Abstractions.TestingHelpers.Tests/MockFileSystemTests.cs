using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSystemTests
    {
        [Test]
        public void MockFileSystem_GetFile_ShouldReturnNullWhenFileIsNotRegistered()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", new MockFileData("Demo\r\ntext\ncontent\rvalue") },
                { @"c:\something\other.gif", new MockFileData("gif content") }
            });

            var result = fileSystem.GetFile(@"c:\something\else.txt");

            Assert.IsNull(result);
        }

        [Test]
        public void MockFileSystem_GetFile_ShouldReturnFileRegisteredInConstructor()
        {
            var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", file1 },
                { @"c:\something\other.gif", new MockFileData("gif content") }
            });

            var result = fileSystem.GetFile(@"c:\something\demo.txt");

            Assert.AreEqual(file1, result);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.CaseInsensitivity)]
        public void MockFileSystem_GetFile_ShouldReturnFileRegisteredInConstructorWhenPathsDifferByCase()
        {
            var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", file1 },
                { @"c:\something\other.gif", new MockFileData("gif content") }
            });

            var result = fileSystem.GetFile(@"c:\SomeThing\DEMO.txt");

            Assert.AreEqual(file1, result);
        }

        [Test]
        [UnixOnly(UnixSpecifics.CaseSensitivity)]
        public void MockFileSystem_GetFile_ShouldNotReturnFileRegisteredInConstructorWhenPathsDifferByCase()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { "/something/demo.txt", new MockFileData("Demo\r\ntext\ncontent\rvalue") },
                { "/something/other.gif", new MockFileData("gif content") }
            });

            var result = fileSystem.GetFile("/SomeThing/DEMO.txt");

            Assert.IsNull(result);
        }

        [Test]
        public void MockFileSystem_AddFile_ShouldHandleUnnormalizedSlashes()
        {
            var path = XFS.Path(@"c:\d1\d2\file.txt");
            var alternatePath = XFS.Path("c:/d1/d2/file.txt");
            var alternateParentPath = XFS.Path("c://d1//d2/");
            var fs = new MockFileSystem();
            fs.AddFile(path, new MockFileData("Hello"));

            var fileCount = fs.Directory.GetFiles(alternateParentPath).Length;
            var fileExists = fs.File.Exists(alternatePath);

            Assert.AreEqual(1, fileCount);
            Assert.IsTrue(fileExists);
        }

        [Test]
        public void MockFileSystem_AddFile_ShouldHandleNullFileDataAsEmpty()
        {
            var path = XFS.Path(@"c:\something\nullish.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, null }
            });

            var result = fileSystem.File.ReadAllText(path);

            Assert.IsEmpty(result, "Null MockFileData should be allowed for and result in an empty file.");
        }

        [Test]
        public void MockFileSystem_AddFile_ShouldRepaceExistingFile()
        {
            var path = XFS.Path(@"c:\some\file.txt");
            const string existingContent = "Existing content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData(existingContent) }
            });
            Assert.That(fileSystem.GetFile(path).TextContents, Is.EqualTo(existingContent));

            const string newContent = "New content";
            fileSystem.AddFile(path, new MockFileData(newContent));

            Assert.That(fileSystem.GetFile(path).TextContents, Is.EqualTo(newContent));
        }

        [Test]
        public void MockFileSystem_ByDefault_IsSerializable()
        {
            var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", file1 },
                { @"c:\something\other.gif", new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });
            var memoryStream = new MemoryStream();

            var serializer = new Runtime.Serialization.Formatters.Binary.BinaryFormatter();

#pragma warning disable SYSLIB0011
            serializer.Serialize(memoryStream, fileSystem);
#pragma warning restore SYSLIB0011

            Assert.That(memoryStream.Length > 0, "Length didn't increase after serialization task.");
        }

        [Test]
        public void MockFileSystem_AddDirectory_ShouldCreateDirectory()
        {
            string baseDirectory = XFS.Path(@"C:\Test");
            var fileSystem = new MockFileSystem();

            fileSystem.AddDirectory(baseDirectory);

            Assert.IsTrue(fileSystem.Directory.Exists(baseDirectory));
        }

        [Test]
        public void MockFileSystem_AddDirectory_ShouldThrowExceptionIfDirectoryIsReadOnly()
        {
            string baseDirectory = XFS.Path(@"C:\Test");
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(baseDirectory, new MockFileData(string.Empty));
            fileSystem.File.SetAttributes(baseDirectory, FileAttributes.ReadOnly);

            TestDelegate action = () => fileSystem.AddDirectory(baseDirectory);

            Assert.Throws<UnauthorizedAccessException>(action);
        }

        [Test]
        public void MockFileSystem_DriveInfo_ShouldNotThrowAnyException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Test"));

            var actualResults = fileSystem.DriveInfo.GetDrives();

            Assert.IsNotNull(actualResults);
        }

        [Test]
        public void MockFileSystem_AddFile_ShouldMatchCapitalization_PerfectMatch()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\test"));
            fileSystem.AddDirectory(XFS.Path(@"C:\LOUD"));

            fileSystem.AddFile(XFS.Path(@"C:\test\file.txt"), "foo");
            fileSystem.AddFile(XFS.Path(@"C:\LOUD\file.txt"), "foo");
            fileSystem.AddDirectory(XFS.Path(@"C:\test\SUBDirectory"));
            fileSystem.AddDirectory(XFS.Path(@"C:\LOUD\SUBDirectory"));

            Assert.Contains(XFS.Path(@"C:\test\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(XFS.Path(@"C:\LOUD\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(XFS.Path(@"C:\test\SUBDirectory"), fileSystem.AllDirectories.ToList());
            Assert.Contains(XFS.Path(@"C:\LOUD\SUBDirectory"), fileSystem.AllDirectories.ToList());
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.CaseInsensitivity)]
        public void MockFileSystem_AddFile_ShouldMatchCapitalization_PartialMatch()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\test\subtest"));
            fileSystem.AddDirectory(XFS.Path(@"C:\LOUD\SUBLOUD"));

            fileSystem.AddFile(XFS.Path(@"C:\test\SUBTEST\file.txt"), "foo");
            fileSystem.AddFile(XFS.Path(@"C:\LOUD\subloud\file.txt"), "foo");
            fileSystem.AddDirectory(XFS.Path(@"C:\test\SUBTEST\SUBDirectory"));
            fileSystem.AddDirectory(XFS.Path(@"C:\LOUD\subloud\SUBDirectory"));

            Assert.Contains(XFS.Path(@"C:\test\subtest\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(XFS.Path(@"C:\LOUD\SUBLOUD\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(XFS.Path(@"C:\test\subtest\SUBDirectory"), fileSystem.AllDirectories.ToList());
            Assert.Contains(XFS.Path(@"C:\LOUD\SUBLOUD\SUBDirectory"), fileSystem.AllDirectories.ToList());
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.CaseInsensitivity)]
        public void MockFileSystem_AddFile_ShouldMatchCapitalization_PartialMatch_FurtherLeft()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\test\subtest"));
            fileSystem.AddDirectory(XFS.Path(@"C:\LOUD\SUBLOUD"));

            fileSystem.AddFile(XFS.Path(@"C:\test\SUBTEST\new\file.txt"), "foo");
            fileSystem.AddFile(XFS.Path(@"C:\LOUD\subloud\new\file.txt"), "foo");
            fileSystem.AddDirectory(XFS.Path(@"C:\test\SUBTEST\new\SUBDirectory"));
            fileSystem.AddDirectory(XFS.Path(@"C:\LOUD\subloud\new\SUBDirectory"));

            Assert.Contains(XFS.Path(@"C:\test\subtest\new\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(XFS.Path(@"C:\LOUD\SUBLOUD\new\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(XFS.Path(@"C:\test\subtest\new\SUBDirectory"), fileSystem.AllDirectories.ToList());
            Assert.Contains(XFS.Path(@"C:\LOUD\SUBLOUD\new\SUBDirectory"), fileSystem.AllDirectories.ToList());
        }

        [Test]
        public void MockFileSystem_AddFileFromEmbeddedResource_ShouldAddTheFile()
        {
            var fileSystem = new MockFileSystem();

            fileSystem.AddFileFromEmbeddedResource(XFS.Path(@"C:\TestFile.txt"), Assembly.GetExecutingAssembly(), "System.IO.Abstractions.TestingHelpers.Tests.TestFiles.TestFile.txt");
            var result = fileSystem.GetFile(XFS.Path(@"C:\TestFile.txt"));

            Assert.AreEqual(new UTF8Encoding().GetBytes("This is a test file."), result.Contents);
        }

        [Test]
        public void MockFileSystem_AddFilesFromEmbeddedResource_ShouldAddAllTheFiles()
        {
            var fileSystem = new MockFileSystem();

            fileSystem.AddFilesFromEmbeddedNamespace(XFS.Path(@"C:\"), Assembly.GetExecutingAssembly(), "System.IO.Abstractions.TestingHelpers.Tests.TestFiles");

            Assert.Contains(XFS.Path(@"C:\TestFile.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(XFS.Path(@"C:\SecondTestFile.txt"), fileSystem.AllFiles.ToList());
        }

        [Test]
        public void MockFileSystem_MoveDirectory_MovesDirectoryWithoutRenamingFiles()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"C:\dir1\dir1\dir1.txt"), string.Empty);

            fileSystem.MoveDirectory(XFS.Path(@"C:\dir1"), XFS.Path(@"C:\dir2"));

            var expected = new[] { XFS.Path(@"C:\dir2\dir1\dir1.txt") };
            CollectionAssert.AreEquivalent(expected, fileSystem.AllFiles);
        }

        [Test]
        public void MockFileSystem_MoveDirectoryAndFile_ShouldMoveCorrectly()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"C:\source\project.txt"), string.Empty);
            fileSystem.AddFile(XFS.Path(@"C:\source\subdir\other.txt"), string.Empty);

            fileSystem.Directory.Move(XFS.Path(@"C:\source"), XFS.Path(@"C:\target"));
            fileSystem.File.Move(XFS.Path(@"C:\target\project.txt"), XFS.Path(@"C:\target\proj.txt"));

            var expected = new[] { XFS.Path(@"C:\target\proj.txt"), XFS.Path(@"C:\target\subdir\other.txt") };
            CollectionAssert.AreEquivalent(expected, fileSystem.AllFiles);
        }

        [Test]
        public void MockFileSystem_RemoveFile_RemovesFiles()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(@"C:\file.txt", new MockFileData("Content"));

            fileSystem.RemoveFile(@"C:\file.txt");

            Assert.False(fileSystem.FileExists(@"C:\file.txt"));
        }

        [Test]
        public void MockFileSystem_RemoveFile_ThrowsUnauthorizedAccessExceptionIfFileIsReadOnly()
        {
            var path = XFS.Path(@"C:\file.txt");
            var readOnlyFile = new MockFileData("")
            {
                Attributes = FileAttributes.ReadOnly
            };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, readOnlyFile },
            });

            TestDelegate action = () => fileSystem.RemoveFile(path);

            Assert.Throws<UnauthorizedAccessException>(action);
        }

        [Test]
        public void MockFileSystem_AllNodes_ShouldReturnAllNodes()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), string.Empty },
                { XFS.Path(@"c:\something\other.gif"), string.Empty },
                { XFS.Path(@"d:\foobar\"), new MockDirectoryData() },
                { XFS.Path(@"d:\foo\bar"), new MockDirectoryData( )}
            });
            var expectedNodes = new[]
            {
                XFS.Path(@"c:\something\demo.txt"),
                XFS.Path(@"c:\something\other.gif"),
                XFS.Path(@"d:\foobar"),
                XFS.Path(@"d:\foo\bar"),
                XFS.Path(@"C:\temp")
            };

            var result = fileSystem.AllNodes;

            Assert.AreEqual(expectedNodes, result);
        }

        [Test]
        [TestCase(@"C:\path")]
        [TestCase(@"C:\path\")]
        public void MockFileSystem_AddDirectory_TrailingSlashAllowedButNotRequired(string path)
        {
            var fileSystem = new MockFileSystem();
            var path2 = XFS.Path(path);

            fileSystem.AddDirectory(path2);

            Assert.IsTrue(fileSystem.FileExists(path2));
        }

        [Test]
        public void MockFileSystem_GetFiles_ThrowsArgumentExceptionForInvalidCharacters()
        {
            // Arrange
            const string path = @"c:\";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(path));

            // Act
            TestDelegate getFilesWithInvalidCharacterInPath = () => fileSystem.Directory.GetFiles($"{path}{'\0'}.txt");

            // Assert
            Assert.Throws<ArgumentException>(getFilesWithInvalidCharacterInPath);
        }

        [Test]
        [TestCase(null)]
        [TestCase(@"C:\somepath")]
        public void MockFileSystem_DefaultState_CurrentDirectoryExists(string currentDirectory)
        {
            var fs = new MockFileSystem(null, XFS.Path(currentDirectory));

            var actualCurrentDirectory = fs.DirectoryInfo.New(".");

            Assert.IsTrue(actualCurrentDirectory.Exists);
        }

        [Test]
        public void MockFileSystem_Constructor_ThrowsForNonRootedCurrentDirectory()
        {
            var ae = Assert.Throws<ArgumentException>(() =>
                new MockFileSystem(null, "non-rooted")
            );
            Assert.AreEqual("currentDirectory", ae.ParamName);
        }

        [Test]
        public void MockFileSystem_DefaultState_DefaultTempDirectoryExists()
        {
            var tempDirectory = XFS.Path(@"C:\temp");

            var mockFileSystem = new MockFileSystem();
            var mockFileSystemOverload = new MockFileSystem(null, string.Empty);

            Assert.IsTrue(mockFileSystem.Directory.Exists(tempDirectory));
            Assert.IsTrue(mockFileSystemOverload.Directory.Exists(tempDirectory));
        }

        [Test]
        public void MockFileSystem_FileSystemWatcher_Can_Be_Overridden()
        {
            var path = XFS.Path(@"C:\root");
            var fileSystem = new TestFileSystem(new TestFileSystemWatcherFactory());
            var watcher = fileSystem.FileSystemWatcher.New(path);
            Assert.AreEqual(path, watcher.Path);
        }

        [Test]
        public void MockFileSystem_DeleteDirectoryRecursive_WithReadOnlyFile_ShouldThrowUnauthorizedException()
        {
            string baseDirectory = XFS.Path(@"C:\Test");
            string textFile = XFS.Path(@"C:\Test\file.txt");

            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(baseDirectory, new MockFileData(string.Empty));
            fileSystem.AddFile(textFile, new MockFileData("Content"));
            fileSystem.File.SetAttributes(textFile, FileAttributes.ReadOnly);

            TestDelegate action = () => fileSystem.Directory.Delete(baseDirectory, true);

            Assert.Throws<UnauthorizedAccessException>(action);
            Assert.True(fileSystem.File.Exists(textFile));
            Assert.True(fileSystem.Directory.Exists(baseDirectory));
        }

        private class TestFileSystem : MockFileSystem
        {
            private readonly IFileSystemWatcherFactory fileSystemWatcherFactory;

            public TestFileSystem(IFileSystemWatcherFactory fileSystemWatcherFactory)
            {
                this.fileSystemWatcherFactory = fileSystemWatcherFactory;
            }

            public override IFileSystemWatcherFactory FileSystemWatcher => fileSystemWatcherFactory;
        }

        private class TestFileSystemWatcherFactory : IFileSystemWatcherFactory
        {
            public IFileSystemWatcher CreateNew() => New();
            public IFileSystemWatcher CreateNew(string path) => New(path);
            public IFileSystemWatcher CreateNew(string path, string filter) => New(path, filter);
            public IFileSystemWatcher New()
                => new TestFileSystemWatcher(null);

            public IFileSystemWatcher New(string path)
                => new TestFileSystemWatcher(path);

            public IFileSystemWatcher New(string path, string filter)
                => new TestFileSystemWatcher(path, filter);

            public IFileSystemWatcher Wrap(FileSystemWatcher fileSystemWatcher)
                => new TestFileSystemWatcher(fileSystemWatcher.Path, fileSystemWatcher.Filter);

            public IFileSystemWatcher FromPath(string path) => new TestFileSystemWatcher(path);
            public IFileSystem FileSystem => null!;
        }

        private class TestFileSystemWatcher : FileSystemWatcherBase
        {
            public TestFileSystemWatcher(string path) => Path = path;

            public TestFileSystemWatcher(string path, string filter)
            {
                Path = path;
                Filter = filter;
            }

            public override string Path { get; set; }
            public override IFileSystem FileSystem { get; }
            public override bool IncludeSubdirectories { get; set; }
            public override IContainer Container { get; }
            public override bool EnableRaisingEvents { get; set; }
            public override string Filter { get; set; }
            public override int InternalBufferSize { get; set; }
            public override NotifyFilters NotifyFilter { get; set; }
            public override ISite Site { get; set; }
            public override ISynchronizeInvoke SynchronizingObject { get; set; }
#if FEATURE_FILE_SYSTEM_WATCHER_FILTERS
            public override Collection<string> Filters { get; }
#endif
            public override void BeginInit() { }
            public override void EndInit() { }
            public override IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
            {
                _ = changeType;
                return default(IWaitForChangedResult);
            }

            public override IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
            {
                _ = changeType;
                _ = timeout;
                return default(IWaitForChangedResult);
            }

#if FEATURE_FILESYSTEM_NET7
            public override IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, TimeSpan timeout)
            {
                _ = changeType;
                _ = timeout;
                return default(IWaitForChangedResult);
            }
#endif
        }
    }
}
