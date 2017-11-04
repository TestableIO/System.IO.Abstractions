[![Build status](https://ci.appveyor.com/api/projects/status/em172apw1v5k70vq/branch/master?svg=true)](https://ci.appveyor.com/project/tathamoddie/system-io-abstractions/branch/master)

Just like System.Web.Abstractions, but for System.IO. Yay for testable IO access!

NuGet only:

    Install-Package System.IO.Abstractions

and/or:

    Install-Package System.IO.Abstractions.TestingHelpers

At the core of the library is IFileSystem and FileSystem. Instead of calling methods like `File.ReadAllText` directly, use `IFileSystem.File.ReadAllText`. We have exactly the same API, except that ours is injectable and testable.

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
FileInfo SomeBadApiMethodThatReturnsFileInfo()
{
    return new FileInfo("a");
}

void MyFancyMethod()
{
    var testableFileInfo = (FileInfoBase)SomeBadApiMethodThatReturnsFileInfo();
    ...
}
```
