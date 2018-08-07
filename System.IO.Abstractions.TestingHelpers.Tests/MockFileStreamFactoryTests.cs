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
        public void MockFileStreamFactory_CreateForExistingFile_ShouldReturnStream(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\existing.txt", MockFileData.NullObject }
            });

            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Act
            var result = fileStreamFactory.Create(@"c:\existing.txt", fileMode);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.Append)]
        public void MockFileStreamFactory_CreateForNonExistingFile_ShouldReturnStream(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Act
            var result = fileStreamFactory.Create(@"c:\not_existing.txt", fileMode);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}