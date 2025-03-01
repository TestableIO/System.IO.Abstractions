using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    [WindowsOnly(WindowsSpecifics.AccessControlLists)]
    [SupportedOSPlatform("windows")]
    public class MockFileSetAccessControlTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public async Task MockFile_SetAccessControl_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileSecurity = new FileSecurity();

            // Act
            Action action = () => fileSystem.File.SetAccessControl(path, fileSecurity);

            // Assert
            var exception = await That(action).Throws<ArgumentException>();
            await That(exception.ParamName).IsEqualTo("path");
        }

        [Test]
        public async Task MockFile_SetAccessControl_ShouldThrowFileNotFoundExceptionIfFileDoesNotExistInMockData()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var expectedFileName = XFS.Path(@"c:\a.txt");
            var fileSecurity = new FileSecurity();

            // Act
            Action action = () => fileSystem.File.SetAccessControl(expectedFileName, fileSecurity);

            // Assert
            var exception = await That(action).Throws<FileNotFoundException>();
            await That(exception.FileName).IsEqualTo(expectedFileName);
        }

        [Test]
        public async Task MockFile_SetAccessControl_ShouldSetAccessControlOfFileData()
        {
            // Arrange
            var filePath = XFS.Path(@"c:\a.txt");
            var fileData = new MockFileData("Test content");

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                { filePath, fileData }
            });

            // Act
            var expectedAccessControl = new FileSecurity();
            expectedAccessControl.SetAccessRuleProtection(false, false);
            fileSystem.File.SetAccessControl(filePath, expectedAccessControl);

            // Assert
            var accessControl = fileSystem.File.GetAccessControl(filePath);
            await That(accessControl).IsEqualTo(expectedAccessControl);
        }
    }
}
