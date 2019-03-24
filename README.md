[![NuGet](https://img.shields.io/nuget/v/System.IO.Abstractions.svg)](https://www.nuget.org/packages/System.IO.Abstractions)
[![Build status](https://ci.appveyor.com/api/projects/status/em172apw1v5k70vq/branch/master?svg=true)](https://ci.appveyor.com/project/tathamoddie/system-io-abstractions/branch/master) [![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=System-IO-Abstractions/System.IO.Abstractions)](https://dependabot.com)

---

Just like System.Web.Abstractions, but for System.IO. Yay for testable IO access!

NuGet only:

    Install-Package System.IO.Abstractions

and/or:

    Install-Package System.IO.Abstractions.TestingHelpers

At the core of the library is `IFileSystem` and `FileSystem`. Instead of calling methods like `File.ReadAllText` directly, use `IFileSystem.File.ReadAllText`. We have exactly the same API, except that ours is injectable and testable.

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

The library also ships with a series of test helpers to save you from having to mock out every call, for basic scenarios. They are not a complete copy of a real-life file system, but they'll get you most of the way there.

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
        Assert.AreEqual("We can't go on together. It's not me, it's you.", ex.Message);
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