using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests;

[TestFixture]
[WindowsOnly(WindowsSpecifics.AccessControlLists)]
[SupportedOSPlatform("windows")]
public class MockDirectoryInfoAccessControlTests
{
    [Test]
    public async Task MockDirectoryInfo_GetAccessControl_ShouldReturnAccessControlOfDirectoryData()
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

        var directorInfo = fileSystem.DirectoryInfo.New(filePath);

        // Act
        var directorySecurity = directorInfo.GetAccessControl();

        // Assert
        await That(directorySecurity).IsEqualTo(expectedDirectorySecurity);
    }

    [Test]
    public async Task MockDirectoryInfo_SetAccessControl_ShouldSetAccessControlOfDirectoryData()
    {
        // Arrange
        var filePath = XFS.Path(@"c:\a\");
        var fileData = new MockDirectoryData();

        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { filePath, fileData }
        });

        var directorInfo = fileSystem.DirectoryInfo.New(filePath);

        // Act
        var expectedAccessControl = new DirectorySecurity();
        expectedAccessControl.SetAccessRuleProtection(false, false);
        directorInfo.SetAccessControl(expectedAccessControl);

        // Assert
        var accessControl = directorInfo.GetAccessControl();
        await That(accessControl).IsEqualTo(expectedAccessControl);
    }
}