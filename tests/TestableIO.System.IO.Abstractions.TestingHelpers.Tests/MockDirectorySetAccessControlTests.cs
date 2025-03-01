using NUnit.Framework;
using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers.Tests;

using System.Runtime.Versioning;
using Security.AccessControl;
using XFS = MockUnixSupport;

[TestFixture]
[SupportedOSPlatform("windows")]
[WindowsOnly(WindowsSpecifics.AccessControlLists)]
public class MockDirectorySetAccessControlTests
{
    [TestCase(" ")]
    [TestCase("   ")]
    public async Task MockDirectory_SetAccessControl_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var directorySecurity = new DirectorySecurity();

        // Act
        Action action = () => fileSystem.Directory.SetAccessControl(path, directorySecurity);

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.ParamName).IsEqualTo("path");
    }

    [Test]
    public async Task MockDirectory_SetAccessControl_ShouldThrowDirectoryNotFoundExceptionIfDirectoryDoesNotExistInMockData()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var expectedFileName = XFS.Path(@"c:\a\");
        var directorySecurity = new DirectorySecurity();

        // Act
        Action action = () => fileSystem.Directory.SetAccessControl(expectedFileName, directorySecurity);

        // Assert
        await That(action).Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task MockDirectory_SetAccessControl_ShouldSetAccessControlOfDirectoryData()
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
        await That(accessControl).IsEqualTo(expectedAccessControl);
    }
}