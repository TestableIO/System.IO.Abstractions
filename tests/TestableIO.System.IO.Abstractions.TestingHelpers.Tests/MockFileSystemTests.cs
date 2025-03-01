using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests;

[TestFixture]
public class MockFileSystemTests
{
    [Test]
    public async Task MockFileSystem_GetFile_ShouldReturnNullWhenFileIsNotRegistered()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"c:\something\demo.txt", new MockFileData("Demo\r\ntext\ncontent\rvalue") },
            { @"c:\something\other.gif", new MockFileData("gif content") }
        });

        var result = fileSystem.GetFile(@"c:\something\else.txt");

        await That(result).IsNull();
    }

    [Test]
    public async Task MockFileSystem_GetFile_ShouldReturnFileRegisteredInConstructor()
    {
        var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"c:\something\demo.txt", file1 },
            { @"c:\something\other.gif", new MockFileData("gif content") }
        });

        var result = fileSystem.GetFile(@"c:\something\demo.txt");

        await That(result).IsEqualTo(file1);
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.CaseInsensitivity)]
    public async Task MockFileSystem_GetFile_ShouldReturnFileRegisteredInConstructorWhenPathsDifferByCase()
    {
        var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"c:\something\demo.txt", file1 },
            { @"c:\something\other.gif", new MockFileData("gif content") }
        });

        var result = fileSystem.GetFile(@"c:\SomeThing\DEMO.txt");

        await That(result).IsEqualTo(file1);
    }

    [Test]
    [UnixOnly(UnixSpecifics.CaseSensitivity)]
    public async Task MockFileSystem_GetFile_ShouldNotReturnFileRegisteredInConstructorWhenPathsDifferByCase()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { "/something/demo.txt", new MockFileData("Demo\r\ntext\ncontent\rvalue") },
            { "/something/other.gif", new MockFileData("gif content") }
        });

        var result = fileSystem.GetFile("/SomeThing/DEMO.txt");

        await That(result).IsNull();
    }

    [Test]
    public async Task MockFileSystem_AddFile_ShouldHandleUnnormalizedSlashes()
    {
        var path = XFS.Path(@"c:\d1\d2\file.txt");
        var alternatePath = XFS.Path("c:/d1/d2/file.txt");
        var alternateParentPath = XFS.Path("c://d1//d2/");
        var fs = new MockFileSystem();
        fs.AddFile(path, new MockFileData("Hello"));

        var fileCount = fs.Directory.GetFiles(alternateParentPath).Length;
        var fileExists = fs.File.Exists(alternatePath);

        await That(fileCount).IsEqualTo(1);
        await That(fileExists).IsTrue();
    }

    [Test]
    public async Task MockFileSystem_AddFile_ShouldHandleNullFileDataAsEmpty()
    {
        var path = XFS.Path(@"c:\something\nullish.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, null }
        });

        var result = fileSystem.File.ReadAllText(path);

        await That(result).IsEmpty().Because("Null MockFileData should be allowed for and result in an empty file.");
    }

    [Test]
    public async Task MockFileSystem_AddFile_ShouldReplaceExistingFile()
    {
        var path = XFS.Path(@"c:\some\file.txt");
        const string existingContent = "Existing content";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { path, new MockFileData(existingContent) }
        });
        await That(fileSystem.GetFile(path).TextContents).IsEqualTo(existingContent);

        const string newContent = "New content";
        fileSystem.AddFile(path, new MockFileData(newContent));

        await That(fileSystem.GetFile(path).TextContents).IsEqualTo(newContent);
    }

    [Test]
    public async Task MockFileSystem_AddEmptyFile_ShouldBeEmpty()
    {
        var path = XFS.Path(@"c:\some\file.txt");
        var fileSystem = new MockFileSystem();

        fileSystem.AddEmptyFile(path);

        await That(fileSystem.GetFile(path).TextContents).IsEqualTo("");
    }

    [Test]
    public async Task MockFileSystem_AddEmptyFile_ShouldExist()
    {
        var fileSystem = new MockFileSystem();
        var path = fileSystem.FileInfo.New(XFS.Path(@"c:\some\file.txt"));

        fileSystem.AddEmptyFile(path);

        await That(path.Exists).IsTrue();
    }

    [Test]
    public async Task MockFileSystem_AddFile_ShouldExist()
    {
        var fileSystem = new MockFileSystem();
        var path = fileSystem.FileInfo.New(XFS.Path(@"c:\some\file.txt"));

        fileSystem.AddFile(path, new MockFileData("stuff"));

        await That(path.Exists).IsTrue();
    }

    [Test]
    public async Task MockFileSystem_AddDirectory_ShouldExist()
    {
        var fileSystem = new MockFileSystem();
        var path = fileSystem.DirectoryInfo.New(XFS.Path(@"c:\thedir"));

        fileSystem.AddDirectory(path);

        await That(path.Exists).IsTrue();
    }
        
#if !NET9_0_OR_GREATER
        [Test]
        public async Task MockFileSystem_ByDefault_IsSerializable()
        {
            var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", file1 },
                { @"c:\something\other.gif", new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });
            var memoryStream = new MemoryStream();

#pragma warning disable SYSLIB0011
            var serializer = new Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            serializer.Serialize(memoryStream, fileSystem);
#pragma warning restore SYSLIB0011

            await That(memoryStream).HasLength().GreaterThan(0).Because("Length didn't increase after serialization task.");
        }
#endif

    [Test]
    public async Task MockFileSystem_AddDirectory_ShouldCreateDirectory()
    {
        string baseDirectory = XFS.Path(@"C:\Test");
        var fileSystem = new MockFileSystem();

        fileSystem.AddDirectory(baseDirectory);

        await That(fileSystem.Directory.Exists(baseDirectory)).IsTrue();
    }

    [Test]
    public async Task MockFileSystem_AddDirectory_ShouldThrowExceptionIfDirectoryIsReadOnly()
    {
        string baseDirectory = XFS.Path(@"C:\Test");
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(baseDirectory, new MockFileData(string.Empty));
        fileSystem.File.SetAttributes(baseDirectory, FileAttributes.ReadOnly);

        Action action = () => fileSystem.AddDirectory(baseDirectory);

        await That(action).Throws<UnauthorizedAccessException>();
    }

    [Test]
    public async Task MockFileSystem_AddDrive_ShouldExist()
    {
        string name = @"D:\";
        var fileSystem = new MockFileSystem();
        fileSystem.AddDrive(name, new MockDriveData());

        var actualResults = fileSystem.DriveInfo.GetDrives().Select(d => d.Name);

        await That(actualResults).Contains(name);
    }

    [Test]
    public async Task MockFileSystem_DriveInfo_ShouldNotThrowAnyException()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\Test"));

        var actualResults = fileSystem.DriveInfo.GetDrives();

        await That(actualResults).IsNotNull();
    }

    [Test]
    public async Task MockFileSystem_AddFile_ShouldMatchCapitalization_PerfectMatch()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\test"));
        fileSystem.AddDirectory(XFS.Path(@"C:\LOUD"));

        fileSystem.AddFile(XFS.Path(@"C:\test\file.txt"), "foo");
        fileSystem.AddFile(XFS.Path(@"C:\LOUD\file.txt"), "foo");
        fileSystem.AddDirectory(XFS.Path(@"C:\test\SUBDirectory"));
        fileSystem.AddDirectory(XFS.Path(@"C:\LOUD\SUBDirectory"));

        await That(fileSystem.AllFiles.ToList()).Contains(XFS.Path(@"C:\test\file.txt"));
        await That(fileSystem.AllFiles.ToList()).Contains(XFS.Path(@"C:\LOUD\file.txt"));
        await That(fileSystem.AllDirectories.ToList()).Contains(XFS.Path(@"C:\test\SUBDirectory"));
        await That(fileSystem.AllDirectories.ToList()).Contains(XFS.Path(@"C:\LOUD\SUBDirectory"));
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.CaseInsensitivity)]
    public async Task MockFileSystem_AddFile_ShouldMatchCapitalization_PartialMatch()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\test\subtest"));
        fileSystem.AddDirectory(XFS.Path(@"C:\LOUD\SUBLOUD"));

        fileSystem.AddFile(XFS.Path(@"C:\test\SUBTEST\file.txt"), "foo");
        fileSystem.AddFile(XFS.Path(@"C:\LOUD\subloud\file.txt"), "foo");
        fileSystem.AddDirectory(XFS.Path(@"C:\test\SUBTEST\SUBDirectory"));
        fileSystem.AddDirectory(XFS.Path(@"C:\LOUD\subloud\SUBDirectory"));

        await That(fileSystem.AllFiles.ToList()).Contains(XFS.Path(@"C:\test\subtest\file.txt"));
        await That(fileSystem.AllFiles.ToList()).Contains(XFS.Path(@"C:\LOUD\SUBLOUD\file.txt"));
        await That(fileSystem.AllDirectories.ToList()).Contains(XFS.Path(@"C:\test\subtest\SUBDirectory"));
        await That(fileSystem.AllDirectories.ToList()).Contains(XFS.Path(@"C:\LOUD\SUBLOUD\SUBDirectory"));
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.CaseInsensitivity)]
    public async Task MockFileSystem_AddFile_ShouldMatchCapitalization_PartialMatch_FurtherLeft()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(@"C:\test\subtest"));
        fileSystem.AddDirectory(XFS.Path(@"C:\LOUD\SUBLOUD"));

        fileSystem.AddFile(XFS.Path(@"C:\test\SUBTEST\new\file.txt"), "foo");
        fileSystem.AddFile(XFS.Path(@"C:\LOUD\subloud\new\file.txt"), "foo");
        fileSystem.AddDirectory(XFS.Path(@"C:\test\SUBTEST\new\SUBDirectory"));
        fileSystem.AddDirectory(XFS.Path(@"C:\LOUD\subloud\new\SUBDirectory"));

        await That(fileSystem.AllFiles.ToList()).Contains(XFS.Path(@"C:\test\subtest\new\file.txt"));
        await That(fileSystem.AllFiles.ToList()).Contains(XFS.Path(@"C:\LOUD\SUBLOUD\new\file.txt"));
        await That(fileSystem.AllDirectories.ToList()).Contains(XFS.Path(@"C:\test\subtest\new\SUBDirectory"));
        await That(fileSystem.AllDirectories.ToList()).Contains(XFS.Path(@"C:\LOUD\SUBLOUD\new\SUBDirectory"));
    }

    [Test]
    public async Task MockFileSystem_AddFile_InitializesMockFileDataFileVersionInfoIfNull()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        fileSystem.AddFile(XFS.Path(@"C:\file.txt"), string.Empty);

        // Assert
        IFileVersionInfo fileVersionInfo = fileSystem.FileVersionInfo.GetVersionInfo(XFS.Path(@"C:\file.txt"));
        await That(fileVersionInfo).IsNotNull();
        await That(fileVersionInfo.FileName).IsEqualTo(XFS.Path(@"C:\file.txt"));
    }

    [Test]
    public async Task MockFileSystem_AddFileFromEmbeddedResource_ShouldAddTheFile()
    {
        var fileSystem = new MockFileSystem();

        fileSystem.AddFileFromEmbeddedResource(XFS.Path(@"C:\TestFile.txt"), Assembly.GetExecutingAssembly(), "System.IO.Abstractions.TestingHelpers.Tests.TestFiles.TestFile.txt");
        var result = fileSystem.GetFile(XFS.Path(@"C:\TestFile.txt"));

        await That(result.Contents).IsEqualTo(new UTF8Encoding().GetBytes("This is a test file."));
    }

    [Test]
    public async Task MockFileSystem_AddFilesFromEmbeddedResource_ShouldAddAllTheFiles()
    {
        var fileSystem = new MockFileSystem();

        fileSystem.AddFilesFromEmbeddedNamespace(XFS.Path(@"C:\"), Assembly.GetExecutingAssembly(), "System.IO.Abstractions.TestingHelpers.Tests.TestFiles");

        await That(fileSystem.AllFiles.ToList()).Contains(XFS.Path(@"C:\TestFile.txt"));
        await That(fileSystem.AllFiles.ToList()).Contains(XFS.Path(@"C:\SecondTestFile.txt"));
    }

    [Test]
    public async Task MockFileSystem_MoveDirectory_MovesDirectoryWithoutRenamingFiles()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(XFS.Path(@"C:\dir1\dir1\dir1.txt"), string.Empty);

        fileSystem.MoveDirectory(XFS.Path(@"C:\dir1"), XFS.Path(@"C:\dir2"));

        var expected = new[] { XFS.Path(@"C:\dir2\dir1\dir1.txt") };
        await That(fileSystem.AllFiles).IsEqualTo(expected);
    }

    [Test]
    public async Task MockFileSystem_MoveDirectoryAndFile_ShouldMoveCorrectly()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(XFS.Path(@"C:\source\project.txt"), string.Empty);
        fileSystem.AddFile(XFS.Path(@"C:\source\subdir\other.txt"), string.Empty);

        fileSystem.Directory.Move(XFS.Path(@"C:\source"), XFS.Path(@"C:\target"));
        fileSystem.File.Move(XFS.Path(@"C:\target\project.txt"), XFS.Path(@"C:\target\proj.txt"));

        var expected = new[] { XFS.Path(@"C:\target\proj.txt"), XFS.Path(@"C:\target\subdir\other.txt") };
        await That(fileSystem.AllFiles).IsEqualTo(expected);
    }

    [Test]
    public async Task MockFileSystem_RemoveFile_RemovesFiles()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(@"C:\file.txt", new MockFileData("Content"));

        fileSystem.RemoveFile(@"C:\file.txt");

        await That(fileSystem.FileExists(@"C:\file.txt")).IsFalse();
    }

    [Test]
    public async Task MockFileSystem_RemoveFile_ThrowsUnauthorizedAccessExceptionIfFileIsReadOnly()
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

        Action action = () => fileSystem.RemoveFile(path);

        await That(action).Throws<UnauthorizedAccessException>();
    }

    [Test]
    public async Task MockFileSystem_AllNodes_ShouldReturnAllNodes()
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

        await That(result).IsEqualTo(expectedNodes);
    }

    [Test]
    [TestCase(@"C:\path")]
    [TestCase(@"C:\path\")]
    public async Task MockFileSystem_AddDirectory_TrailingSlashAllowedButNotRequired(string path)
    {
        var fileSystem = new MockFileSystem();
        var path2 = XFS.Path(path);

        fileSystem.AddDirectory(path2);

        await That(fileSystem.FileExists(path2)).IsTrue();
    }

    [Test]
    public async Task MockFileSystem_GetFiles_ThrowsArgumentExceptionForInvalidCharacters()
    {
        // Arrange
        const string path = @"c:\";
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(XFS.Path(path));

        // Act
        Action getFilesWithInvalidCharacterInPath = () => fileSystem.Directory.GetFiles($"{path}{'\0'}.txt");

        // Assert
        await That(getFilesWithInvalidCharacterInPath).Throws<ArgumentException>();
    }

    [Test]
    [TestCase(null)]
    [TestCase(@"C:\somepath")]
    public async Task MockFileSystem_DefaultState_CurrentDirectoryExists(string currentDirectory)
    {
        var fs = new MockFileSystem(null, XFS.Path(currentDirectory));

        var actualCurrentDirectory = fs.DirectoryInfo.New(".");

        await That(actualCurrentDirectory.Exists).IsTrue();
    }

    [Test]
    public async Task MockFileSystem_Constructor_ThrowsForNonRootedCurrentDirectory()
    {
        await That(() =>
                new MockFileSystem(null, "non-rooted")
            ).Throws<ArgumentException>()
            .WithParamName("currentDirectory");
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockFileSystem_Constructor_ShouldSupportDifferentRootDrives()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            [@"c:\"] = new MockDirectoryData(),
            [@"z:\"] = new MockDirectoryData(),
            [@"d:\"] = new MockDirectoryData(),
        });

        var cExists = fileSystem.Directory.Exists(@"c:\");
        var zExists = fileSystem.Directory.Exists(@"z:\");
        var dExists = fileSystem.Directory.Exists(@"d:\");

        await That(fileSystem).IsNotNull();
        await That(cExists).IsTrue();
        await That(zExists).IsTrue();
        await That(dExists).IsTrue();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockFileSystem_Constructor_ShouldAddDifferentDrivesIfNotExist()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            [@"d:\foo\bar\"] = new MockDirectoryData(),
        });

        var drivesInfo = fileSystem.DriveInfo.GetDrives();
        var fooExists = fileSystem.Directory.Exists(@"d:\foo\");
        var barExists = fileSystem.Directory.Exists(@"d:\foo\bar\");

        await That(drivesInfo.Any(d => string.Equals(d.Name, @"D:\", StringComparison.InvariantCultureIgnoreCase))).IsTrue();
        await That(fooExists).IsTrue();
        await That(barExists).IsTrue();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockFileSystem_Constructor_ShouldNotDuplicateDrives()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            [@"d:\foo\bar\"] = new MockDirectoryData(),
            [@"d:\"] = new MockDirectoryData()
        });

        var drivesInfo = fileSystem.DriveInfo.GetDrives();

        await That(drivesInfo.Where(d => string.Equals(d.Name, @"D:\", StringComparison.InvariantCultureIgnoreCase))).HasCount().EqualTo(1);
    }

    [Test]
    public async Task MockFileSystem_DefaultState_DefaultTempDirectoryExists()
    {
        var tempDirectory = XFS.Path(@"C:\temp");

        var mockFileSystem = new MockFileSystem();
        var mockFileSystemOverload = new MockFileSystem(null, string.Empty);

        await That(mockFileSystem.Directory.Exists(tempDirectory)).IsTrue();
        await That(mockFileSystemOverload.Directory.Exists(tempDirectory)).IsTrue();
    }

    [Test]
    public async Task MockFileSystem_FileSystemWatcher_Can_Be_Overridden()
    {
        var path = XFS.Path(@"C:\root");
        var fileSystem = new TestFileSystem(new TestFileSystemWatcherFactory());
        var watcher = fileSystem.FileSystemWatcher.New(path);
        await That(watcher.Path).IsEqualTo(path);
    }

    [Test]
    public async Task MockFileSystem_DeleteDirectoryRecursive_WithReadOnlyFile_ShouldThrowUnauthorizedException()
    {
        string baseDirectory = XFS.Path(@"C:\Test");
        string textFile = XFS.Path(@"C:\Test\file.txt");

        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(baseDirectory, new MockFileData(string.Empty));
        fileSystem.AddFile(textFile, new MockFileData("Content"));
        fileSystem.File.SetAttributes(textFile, FileAttributes.ReadOnly);

        Action action = () => fileSystem.Directory.Delete(baseDirectory, true);

        await That(action).Throws<UnauthorizedAccessException>();
        await That(fileSystem.File.Exists(textFile)).IsTrue();
        await That(fileSystem.Directory.Exists(baseDirectory)).IsTrue();
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

#if FEATURE_FILE_SYSTEM_WATCHER_WAIT_WITH_TIMESPAN
        public override IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, TimeSpan timeout)
        {
            _ = changeType;
            _ = timeout;
            return default(IWaitForChangedResult);
        }
#endif
    }
}