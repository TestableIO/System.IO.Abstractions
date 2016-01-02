using System.Collections.Generic;
using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{

    public class MockFileInfoFactoryTests
    {
        [Fact]
        public void MockFileInfoFactory_FromFileName_ShouldReturnFileInfoForExistingFile()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b\c.txt", new MockFileData("Demo text content") },
            });
            var fileInfoFactory = new MockFileInfoFactory(fileSystem);

            // Act
            var result = fileInfoFactory.FromFileName(@"c:\a.txt");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void MockFileInfoFactory_FromFileName_ShouldReturnFileInfoForNonExistantFile()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b\c.txt", new MockFileData("Demo text content") },
            });
            var fileInfoFactory = new MockFileInfoFactory(fileSystem);

            // Act
            var result = fileInfoFactory.FromFileName(@"c:\foo.txt");

            // Assert
            Assert.NotNull(result);
        }
    }
}