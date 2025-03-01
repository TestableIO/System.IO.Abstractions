using NUnit.Framework;
using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileVersionInfoFactoryTests
    {
        [Test]
        public async Task MockFileVersionInfoFactory_GetVersionInfo_ShouldReturnTheFileVersionInfoOfTheMockFileData()
        {
            // Arrange
            var fileVersionInfo = new MockFileVersionInfo(@"c:\a.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") { FileVersionInfo = fileVersionInfo } }
            });

            // Act
            var result = fileSystem.FileVersionInfo.GetVersionInfo(@"c:\a.txt");

            // Assert
            await That(result).IsEqualTo(fileVersionInfo);
        }

        [Test]
        public async Task MockFileVersionInfoFactory_GetVersionInfo_ShouldThrowFileNotFoundExceptionIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b\c.txt", new MockFileData("Demo text content") },
            });

            // Act
            Action code = () => fileSystem.FileVersionInfo.GetVersionInfo(@"c:\foo.txt");

            // Assert
            await That(code).Throws<FileNotFoundException>();
        }
    }
}
