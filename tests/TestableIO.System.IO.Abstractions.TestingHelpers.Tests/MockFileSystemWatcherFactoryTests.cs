﻿using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSystemWatcherFactoryTests
    {
        [Test]
        public void MockFileSystemWatcherFactory_CreateNew_ShouldThrowNotImplementedException()
        {
            var factory = new MockFileSystemWatcherFactory(new MockFileSystem());
            Assert.Throws<NotImplementedException>(() => factory.New());
        }

        [Test]
        public void MockFileSystemWatcherFactory_CreateNewWithPath_ShouldThrowNotImplementedException()
        {
            var path = XFS.Path(@"y:\test");
            var factory = new MockFileSystemWatcherFactory(new MockFileSystem());
            Assert.Throws<NotImplementedException>(() => factory.New(path));
        }

        [Test]
        public void MockFileSystemWatcherFactory_CreateNewWithPathAndFilter_ShouldThrowNotImplementedException()
        {
            var path = XFS.Path(@"y:\test");
            var filter = "*.txt";
            var factory = new MockFileSystemWatcherFactory(new MockFileSystem());
            Assert.Throws<NotImplementedException>(() => factory.New(path, filter));
        }

        [Test]
        public void MockFileSystemWatcherFactory_FromPath_ShouldThrowNotImplementedException()
        {
            var path = XFS.Path(@"y:\test");
            var factory = new MockFileSystemWatcherFactory(new MockFileSystem());
            Assert.Throws<NotImplementedException>(() => factory.New(path));
        }

        [Test]
        public void MockFileSystemWatcherFactory_Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new MockFileSystem();

            var result = fileSystem.FileSystemWatcher.Wrap(null);

            Assert.That(result, Is.Null);
        }
    }
}
