using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockFileGetAccessControlTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_GetAccessControl_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            if (MockUnixSupport.IsUnixPlatform()) 
            {
                Assert.Inconclusive("Unix does not support ACLs.");
            }
            
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.File.GetAccessControl(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_GetAccessControl_ShouldThrowFileNotFoundExceptionIfFileDoesNotExistInMockData()
        {
            if (MockUnixSupport.IsUnixPlatform()) 
            {
                Assert.Inconclusive("Unix does not support ACLs.");
            }
            
            // Arrange
            var fileSystem = new MockFileSystem();
            var expectedFileName = XFS.Path(@"c:\a.txt");

            // Act
            TestDelegate action = () => fileSystem.File.GetAccessControl(expectedFileName);

            // Assert
            var exception = Assert.Throws<FileNotFoundException>(action);
            Assert.That(exception.FileName, Is.EqualTo(expectedFileName));
        }

        [Test]
        public void MockFile_GetAccessControl_ShouldReturnAccessControlOfFileData()
        {
            if (MockUnixSupport.IsUnixPlatform()) 
            {
                Assert.Inconclusive("Unix does not support ACLs.");
            }
            
            // Arrange
            var expectedFileSecurity = new FileSecurity();
            expectedFileSecurity.SetAccessRuleProtection(false, false);

            var filePath = XFS.Path(@"c:\a.txt");
            var fileData = new MockFileData("Test content")
            {
                AccessControl = expectedFileSecurity,
            };

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                { filePath, fileData }
            });

            // Act
            var fileSecurity = fileSystem.File.GetAccessControl(filePath);

            // Assert
            Assert.That(fileSecurity, Is.EqualTo(expectedFileSecurity));
        }
    }
}
