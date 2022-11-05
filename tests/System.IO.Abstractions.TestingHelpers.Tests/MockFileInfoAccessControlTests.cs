﻿using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    [WindowsOnly(WindowsSpecifics.AccessControlLists)]
    [SupportedOSPlatform("windows")]
    public class MockFileInfoAccessControlTests
    {
        [Test]
        public void MockFileInfo_GetAccessControl_ShouldReturnAccessControlOfFileData()
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

            var fileInfo = fileSystem.FileInfo.New(filePath);

            // Act
            var fileSecurity = fileInfo.GetAccessControl();

            // Assert
            Assert.That(fileSecurity, Is.EqualTo(expectedFileSecurity));
        }

        [Test]
        public void MockFile_SetAccessControl_ShouldSetAccessControlOfFileData()
        {
            // Arrange
            var filePath = XFS.Path(@"c:\a.txt");
            var fileData = new MockFileData("Test content");

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                { filePath, fileData }
            });

            var fileInfo = fileSystem.FileInfo.New(filePath);

            // Act
            var expectedAccessControl = new FileSecurity();
            expectedAccessControl.SetAccessRuleProtection(false, false);
            fileInfo.SetAccessControl(expectedAccessControl);

            // Assert
            var accessControl = fileInfo.GetAccessControl();
            Assert.That(accessControl, Is.EqualTo(expectedAccessControl));
        }
    }
}
