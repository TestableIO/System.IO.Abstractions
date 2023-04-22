using System.Collections.Generic;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileInfoFactoryTests
    {
        [Test]
        public void MockFileInfoFactory_New_ShouldReturnFileInfoForExistingFile()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b\c.txt", new MockFileData("Demo text content") },
            });
            var fileInfoFactory = new MockFileInfoFactory(fileSystem);

            // Act
            var result = fileInfoFactory.New(@"c:\a.txt");

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void MockFileInfoFactory_New_ShouldReturnFileInfoForNonExistentFile()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b\c.txt", new MockFileData("Demo text content") },
            });
            var fileInfoFactory = new MockFileInfoFactory(fileSystem);

            // Act
            var result = fileInfoFactory.New(@"c:\foo.txt");

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void MockFileInfoFactory_Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new MockFileSystem();

            var result = fileSystem.FileInfo.Wrap(null);

            Assert.IsNull(result);
        }
    }
}