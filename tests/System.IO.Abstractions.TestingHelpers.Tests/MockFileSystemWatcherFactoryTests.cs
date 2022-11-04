using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSystemWatcherFactoryTests
    {
        [Test]
        public void MockFileSystemWatcherFactory_CreateNew_ShouldThrowNotImplementedException()
        {
            var factory = new MockFileSystemWatcherFactory();
            Assert.Throws<NotImplementedException>(() => factory.CreateNew());
        }

        [Test]
        public void MockFileSystemWatcherFactory_CreateNewWithPath_ShouldThrowNotImplementedException()
        {
            var path = XFS.Path(@"y:\test");
            var factory = new MockFileSystemWatcherFactory();
            Assert.Throws<NotImplementedException>(() => factory.CreateNew(path));
        }

        [Test]
        public void MockFileSystemWatcherFactory_CreateNewWithPathAndFilter_ShouldThrowNotImplementedException()
        {
            var path = XFS.Path(@"y:\test");
            var filter = "*.txt";
            var factory = new MockFileSystemWatcherFactory();
            Assert.Throws<NotImplementedException>(() => factory.CreateNew(path, filter));
        }

        [Test]
        public void MockFileSystemWatcherFactory_FromPath_ShouldThrowNotImplementedException()
        {
            var path = XFS.Path(@"y:\test");
            var factory = new MockFileSystemWatcherFactory();
            Assert.Throws<NotImplementedException>(() => factory.New(path));
        }
    }
}
