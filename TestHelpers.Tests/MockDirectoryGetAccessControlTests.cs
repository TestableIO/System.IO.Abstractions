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
    [WindowsOnly(WindowsSpecifics.AccessControlLists)]
    public class MockDirectoryGetAccessControlTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockDirectory_GetAccessControl_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.Directory.GetAccessControl(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockDirectory_GetAccessControl_ShouldThrowDirectoryNotFoundExceptionIfDirectoryDoesNotExistInMockData()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var expectedDirectoryName = XFS.Path(@"c:\a");

            // Act
            TestDelegate action = () => fileSystem.Directory.GetAccessControl(expectedDirectoryName);

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action);
        }

        [Test]
        public void MockDirectory_GetAccessControl_ShouldReturnAccessControlOfDirectoryData()
        {
            // Arrange
            var expectedDirectorySecurity = new DirectorySecurity();
            expectedDirectorySecurity.SetAccessRuleProtection(false, false);

            var filePath = XFS.Path(@"c:\a\");
            var fileData = new MockDirectoryData()
            {
                AccessControl = expectedDirectorySecurity,
            };

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                { filePath, fileData }
            });

            // Act
            var directorySecurity = fileSystem.Directory.GetAccessControl(filePath);

            // Assert
            Assert.That(directorySecurity, Is.EqualTo(expectedDirectorySecurity));
        }
    }
}
