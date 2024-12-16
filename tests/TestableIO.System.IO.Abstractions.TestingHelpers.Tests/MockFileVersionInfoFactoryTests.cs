using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileVersionInfoFactoryTests
    {
        [Test]
        public void MockFileVersionInfoFactory_GetVersionInfo_ShouldReturnTheFileVersionInfoOfTheMockFileData()
        {
            // Arrange
            var fileVersionInfo = new MockFileVersionInfo();
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") { FileVersionInfo = fileVersionInfo } }
            });

            // Act
            var result = fileSystem.FileVersionInfo.GetVersionInfo(@"c:\a.txt");

            // Assert
            Assert.That(result, Is.EqualTo(fileVersionInfo));
        }

        [Test]
        public void MockFileVersionInfoFactory_GetVersionInfo_ShouldThrowFileNotFoundExceptionIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\a.txt", new MockFileData("Demo text content") },
                { @"c:\a\b\c.txt", new MockFileData("Demo text content") },
            });

            // Act
            TestDelegate code = () => fileSystem.FileVersionInfo.GetVersionInfo(@"c:\foo.txt");
            
            // Assert
            Assert.Throws<FileNotFoundException>(code);
        }
    }
}
