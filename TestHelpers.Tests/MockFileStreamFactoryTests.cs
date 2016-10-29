using System.Collections.Generic;
using NUnit.Framework; 

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileStreamFactoryTests
    {
        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.Append)]
        public void MockFileStreamFactory_Create_string_FileMode__ShouldReturnStreamForExistingFile(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b\c.txt", new MockFileData("Demo text content") },
            });

            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Act
            var result = fileStreamFactory.Create(@"c:\a.txt", fileMode);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.Append)]
        public void MockFileStreamFactory_Create_string_FileMode__ShouldReturnStreamForNonExistingFile(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b\c.txt", new MockFileData("Demo text content") },
            });

            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Act
            var result = fileStreamFactory.Create(@"c:\foo.txt", fileMode);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}