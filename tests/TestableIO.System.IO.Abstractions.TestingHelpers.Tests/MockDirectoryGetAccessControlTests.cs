using NUnit.Framework;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Runtime.Versioning;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;
    [TestFixture]
    [WindowsOnly(WindowsSpecifics.AccessControlLists)]
    [SupportedOSPlatform("windows")]
    public class MockDirectoryGetAccessControlTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public async Task MockDirectory_GetAccessControl_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.Directory.GetAccessControl(path);

            // Assert
            var exception = await That(action).Throws<ArgumentException>();
            await That(exception.ParamName).IsEqualTo("path");
        }

        [Test]
        public async Task MockDirectory_GetAccessControl_ShouldThrowDirectoryNotFoundExceptionIfDirectoryDoesNotExistInMockData()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var expectedDirectoryName = XFS.Path(@"c:\a");

            // Act
            Action action = () => fileSystem.Directory.GetAccessControl(expectedDirectoryName);

            // Assert
            await That(action).Throws<DirectoryNotFoundException>();
        }

        [Test]
        public async Task MockDirectory_GetAccessControl_ShouldReturnAccessControlOfDirectoryData()
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
            await That(directorySecurity).IsEqualTo(expectedDirectorySecurity);
        }
    }
}
