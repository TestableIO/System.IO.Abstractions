![System.IO.Abstractions](https://socialify.git.ci/TestableIO/System.IO.Abstractions/image?description=1&font=Source%20Code%20Pro&forks=1&issues=1&pattern=Charlie%20Brown&pulls=1&stargazers=1&theme=Dark)
[![NuGet](https://img.shields.io/nuget/v/TestableIO.System.IO.Abstractions.svg)](https://www.nuget.org/packages/TestableIO.System.IO.Abstractions)
[![Build](https://github.com/TestableIO/System.IO.Abstractions/actions/workflows/build.yml/badge.svg)](https://github.com/TestableIO/System.IO.Abstractions/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=TestableIO_System.IO.Abstractions&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=TestableIO_System.IO.Abstractions)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=TestableIO_System.IO.Abstractions&metric=coverage)](https://sonarcloud.io/summary/new_code?id=TestableIO_System.IO.Abstractions)
[![Renovate enabled](https://img.shields.io/badge/renovate-enabled-brightgreen.svg)](https://renovatebot.com/)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FTestableIO%2FSystem.IO.Abstractions.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FTestableIO%2FSystem.IO.Abstractions?ref=badge_shield)

At the core of the library is `IFileSystem` and `FileSystem`. Instead of calling methods like `File.ReadAllText` directly, use `IFileSystem.File.ReadAllText`. We have exactly the same API, except that ours is injectable and testable.

## Usage

```shell
dotnet add package TestableIO.System.IO.Abstractions.Wrappers
```

*Note: This NuGet package is also published as `System.IO.Abstractions` but we suggest to use the prefix to make clear that this is not an official .NET package.*

```csharp
public class MyComponent
{
    readonly IFileSystem fileSystem;

    // <summary>Create MyComponent with the given fileSystem implementation</summary>
    public MyComponent(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
    }
    /// <summary>Create MyComponent</summary>
    public MyComponent() : this(
        fileSystem: new FileSystem() //use default implementation which calls System.IO
    )
    {
    }

    public void Validate()
    {
        foreach (var textFile in fileSystem.Directory.GetFiles(@"c:\", "*.txt", SearchOption.TopDirectoryOnly))
        {
            var text = fileSystem.File.ReadAllText(textFile);
            if (text != "Testing is awesome.")
                throw new NotSupportedException("We can't go on together. It's not me, it's you.");
        }
    }
}
```

### Test helpers

The library also ships with a series of test helpers to save you from having to mock out every call, for basic scenarios. They are not a complete copy of a real-life file system, but they'll get you most of the way there.

```shell
dotnet add package TestableIO.System.IO.Abstractions.TestingHelpers
```

*Note: This NuGet package is also published as `System.IO.Abstractions.TestingHelpers` but we suggest to use the prefix to make clear that this is not an official .NET package.*

```csharp
[Test]
public void MyComponent_Validate_ShouldThrowNotSupportedExceptionIfTestingIsNotAwesome()
{
    // Arrange
    var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
    {
        { @"c:\myfile.txt", new MockFileData("Testing is meh.") },
        { @"c:\demo\jQuery.js", new MockFileData("some js") },
        { @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
    });
    var component = new MyComponent(fileSystem);

    try
    {
        // Act
        component.Validate();
    }
    catch (NotSupportedException ex)
    {
        // Assert
        Assert.That(ex.Message, Is.EqualTo("We can't go on together. It's not me, it's you."));
        return;
    }

    Assert.Fail("The expected exception was not thrown.");
}
```

We even support casting from the .NET Framework's untestable types to our testable wrappers:

```csharp
FileInfo SomeApiMethodThatReturnsFileInfo()
{
    return new FileInfo("a");
}

void MyFancyMethod()
{
    var testableFileInfo = (FileInfoBase)SomeApiMethodThatReturnsFileInfo();
    ...
}
```

### Mock support

Since version 4.0 the top-level APIs expose interfaces instead of abstract base classes (these still exist, though), allowing you to completely mock the file system. Here's a small example, using [Moq](https://github.com/moq/moq4):

```csharp
[Test]
public void Test1()
{
    var watcher = Mock.Of<IFileSystemWatcher>();
    var file = Mock.Of<IFile>();

    Mock.Get(file).Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
    Mock.Get(file).Setup(f => f.ReadAllText(It.IsAny<string>())).Throws<OutOfMemoryException>();

    var unitUnderTest = new SomeClassUsingFileSystemWatcher(watcher, file);

    Assert.Throws<OutOfMemoryException>(() => {
        Mock.Get(watcher).Raise(w => w.Created += null, new System.IO.FileSystemEventArgs(System.IO.WatcherChangeTypes.Created, @"C:\Some\Directory", "Some.File"));
    });

    Mock.Get(file).Verify(f => f.Exists(It.IsAny<string>()), Times.Once);

    Assert.True(unitUnderTest.FileWasCreated);
}

public class SomeClassUsingFileSystemWatcher
{
    private readonly IFileSystemWatcher _watcher;
    private readonly IFile _file;

    public bool FileWasCreated { get; private set; }

    public SomeClassUsingFileSystemWatcher(IFileSystemWatcher watcher, IFile file)
    {
        this._file = file;
        this._watcher = watcher;
        this._watcher.Created += Watcher_Created;
    }

    private void Watcher_Created(object sender, System.IO.FileSystemEventArgs e)
    {
        FileWasCreated = true;

        if(_file.Exists(e.FullPath))
        {
            var text = _file.ReadAllText(e.FullPath);
        }
    }
}
```

## Relationship with Testably.Abstractions

[`Testably.Abstractions`](https://github.com/Testably/Testably.Abstractions) is a complementary project that uses the same interfaces as TestableIO. This means **no changes to your production code are necessary** when switching between the testing libraries.

Both projects share the same maintainer, but active development and new features are primarily focused on the Testably.Abstractions project. TestableIO.System.IO.Abstractions continues to be maintained for stability and compatibility, but significant new functionality is unlikely to be added.

### When to use Testably.Abstractions vs TestableIO
- **Use TestableIO.System.IO.Abstractions** if you need:
  - Basic file system mocking capabilities
  - Direct manipulation of stored file entities (MockFileData, MockDirectoryData)
  - Established codebase with existing TestableIO integration

- **Use Testably.Abstractions** if you need:
  - Advanced testing scenarios (FileSystemWatcher, SafeFileHandles, multiple drives)
  - Additional abstractions (ITimeSystem, IRandomSystem)
  - Cross-platform file system simulation (Linux, MacOS, Windows)
  - More extensive and consistent behavior validation
  - Active development and new features

### Migrating from TestableIO
Switching from TestableIO to Testably only requires changes in your test projects:

1. Replace the NuGet package reference in your test projects:
   ```xml
   <!-- Remove -->
   <PackageReference Include="TestableIO.System.IO.Abstractions.TestingHelpers" />
   <!-- Add -->
   <PackageReference Include="Testably.Abstractions.Testing" />
   ```

2. Update your test code to use the new `MockFileSystem`:
   ```csharp
   // Before (TestableIO)
   var fileSystem = new MockFileSystem();
   fileSystem.AddDirectory("some-directory");
   fileSystem.AddFile("some-file.txt", new MockFileData("content"));

   // After (Testably)
   var fileSystem = new MockFileSystem();
   fileSystem.Directory.CreateDirectory("some-directory");
   fileSystem.File.WriteAllText("some-file.txt", "content");
   // or using fluent initialization:
   fileSystem.Initialize()
       .WithSubdirectory("some-directory")
       .WithFile("some-file.txt").Which(f => f
           .HasStringContent("content"));
   ```

Your production code using `IFileSystem` remains unchanged.

## Other related projects

-   [`System.IO.Abstractions.Extensions`](https://github.com/TestableIO/System.IO.Abstractions.Extensions)
    provides convenience functionality on top of the core abstractions.

-   [`System.IO.Abstractions.Analyzers`](https://github.com/TestableIO/System.IO.Abstractions.Analyzers)
    provides Roslyn analyzers to help use abstractions over static methods.