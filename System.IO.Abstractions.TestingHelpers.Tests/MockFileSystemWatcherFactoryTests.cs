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
        public void MockFileSystemWatcherFactory_FromPath_ShouldThrowNotImplementedException()
        {
            var path = XFS.Path(@"y:\test");
            var factory = new MockFileSystemWatcherFactory();
            Assert.Throws<NotImplementedException>(() => factory.FromPath(path));
        }
    }
}
