using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests;

[TestFixture]
public class MockFileSystemWatcherFactoryTests
{
    [Test]
    public async Task MockFileSystemWatcherFactory_CreateNew_ShouldThrowNotImplementedException()
    {
        var factory = new MockFileSystemWatcherFactory(new MockFileSystem());
        await That(() => factory.New()).Throws<NotImplementedException>();
    }

    [Test]
    public async Task MockFileSystemWatcherFactory_CreateNewWithPath_ShouldThrowNotImplementedException()
    {
        var path = XFS.Path(@"y:\test");
        var factory = new MockFileSystemWatcherFactory(new MockFileSystem());
        await That(() => factory.New(path)).Throws<NotImplementedException>();
    }

    [Test]
    public async Task MockFileSystemWatcherFactory_CreateNewWithPathAndFilter_ShouldThrowNotImplementedException()
    {
        var path = XFS.Path(@"y:\test");
        var filter = "*.txt";
        var factory = new MockFileSystemWatcherFactory(new MockFileSystem());
        await That(() => factory.New(path, filter)).Throws<NotImplementedException>();
    }

    [Test]
    public async Task MockFileSystemWatcherFactory_FromPath_ShouldThrowNotImplementedException()
    {
        var path = XFS.Path(@"y:\test");
        var factory = new MockFileSystemWatcherFactory(new MockFileSystem());
        await That(() => factory.New(path)).Throws<NotImplementedException>();
    }

    [Test]
    public async Task MockFileSystemWatcherFactory_Wrap_WithNull_ShouldReturnNull()
    {
        var fileSystem = new MockFileSystem();

        var result = fileSystem.FileSystemWatcher.Wrap(null);

        await That(result).IsNull();
    }
}