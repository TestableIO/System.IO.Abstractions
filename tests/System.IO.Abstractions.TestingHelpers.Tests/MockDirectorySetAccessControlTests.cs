using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Security.AccessControl;
    using XFS = MockUnixSupport;

    [TestFixture]
    [WindowsOnly(WindowsSpecifics.AccessControlLists)]
    public class MockDirectorySetAccessControlTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockDirectory_SetAccessControl_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var directorySecurity = new DirectorySecurity();

            // Act
            TestDelegate action = () => fileSystem.Directory.SetAccessControl(path, directorySecurity);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockDirectory_SetAccessControl_ShouldThrowDirectoryNotFoundExceptionIfDirectoryDoesNotExistInMockData()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var expectedFileName = XFS.Path(@"c:\a\");
            var directorySecurity = new DirectorySecurity();

            // Act
            TestDelegate action = () => fileSystem.Directory.SetAccessControl(expectedFileName, directorySecurity);

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action);
        }

        [Test]
        public void MockDirectory_SetAccessControl_ShouldSetAccessControlOfDirectoryData()
        {
            // Arrange
            var filePath = XFS.Path(@"c:\a\");
            var fileData = new MockDirectoryData();

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                { filePath, fileData }
            });

            // Act
            var expectedAccessControl = new DirectorySecurity();
            expectedAccessControl.SetAccessRuleProtection(false, false);
            fileSystem.Directory.SetAccessControl(filePath, expectedAccessControl);

            // Assert
            var accessControl = fileSystem.Directory.GetAccessControl(filePath);
            Assert.That(accessControl, Is.EqualTo(expectedAccessControl));
        }
    }
}
