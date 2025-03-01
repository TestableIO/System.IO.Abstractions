using NUnit.Framework;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Runtime.Versioning;

namespace System.IO.Abstractions.TestingHelpers.Tests;

using XFS = MockUnixSupport;
[TestFixture]
[WindowsOnly(WindowsSpecifics.AccessControlLists)]
[SupportedOSPlatform("windows")]
public class MockFileGetAccessControlTests
{
    [TestCase(" ")]
    [TestCase("   ")]
    public async Task MockFile_GetAccessControl_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        Action action = () => fileSystem.File.GetAccessControl(path);

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.ParamName).IsEqualTo("path");
    }

    [Test]
    public async Task MockFile_GetAccessControl_ShouldThrowFileNotFoundExceptionIfFileDoesNotExistInMockData()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var expectedFileName = XFS.Path(@"c:\a.txt");

        // Act
        Action action = () => fileSystem.File.GetAccessControl(expectedFileName);

        // Assert
        await That(action).Throws<FileNotFoundException>();
    }

    [Test]
    public async Task MockFile_GetAccessControl_ShouldReturnAccessControlOfFileData()
    {
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
        await That(fileSecurity).IsEqualTo(expectedFileSecurity);
    }
}