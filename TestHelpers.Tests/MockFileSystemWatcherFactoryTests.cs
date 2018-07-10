using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSystemWatcherFactoryTests
    {
        [Test]
        public void MockFileSystemWatcherFactory_CreateNew_ShouldReturnNonNullMockWatcher()
        {
            // Arrange
            var factory = new MockFileSystemWatcherFactory();

            // Act
            var result = factory.CreateNew();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void MockFileSystemWatcherFactory_FromPath_ShouldReturnNonNullMockWatcher()
        {
            // Arrange
            var factory = new MockFileSystemWatcherFactory();

            // Act
            var result = factory.FromPath(@"y:\test");

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void MockFileSystemWatcherFactory_FromPath_ShouldReturnWatcherForSpecifiedPath()
        {
            // Arrange
            const string path = @"z:\test";
            var factory = new MockFileSystemWatcherFactory();

            // Act
            var result = factory.FromPath(path);

            // Assert
            Assert.AreEqual(path, result.Path);
        }
    }
}
