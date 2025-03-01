using System;
using System.Linq;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public class MockDriveInfoFactoryTests
    {
        [Test]
        public async Task MockDriveInfoFactory_GetDrives_ShouldReturnDrives()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Test"));
            fileSystem.AddDirectory(XFS.Path(@"Z:\Test"));
            fileSystem.AddDirectory(XFS.Path(@"d:\Test"));
            var factory = new MockDriveInfoFactory(fileSystem);

            // Act
            var actualResults = factory.GetDrives();

            var actualNames = actualResults.Select(d => d.Name);

            // Assert
            await That(actualNames).IsEquivalentTo(new[] { @"C:\", @"Z:\", @"d:\" });
        }

        [Test]
        public async Task MockDriveInfoFactory_GetDrives_ShouldReturnDrivesWithNoDuplicates()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Test"));
            fileSystem.AddDirectory(XFS.Path(@"c:\Test2"));
            fileSystem.AddDirectory(XFS.Path(@"Z:\Test"));
            fileSystem.AddDirectory(XFS.Path(@"d:\Test"));
            fileSystem.AddDirectory(XFS.Path(@"d:\Test2"));
            var factory = new MockDriveInfoFactory(fileSystem);

            // Act
            var actualResults = factory.GetDrives();

            var actualNames = actualResults.Select(d => d.Name);

            // Assert
            await That(actualNames).IsEquivalentTo(new[] { @"C:\", @"Z:\", @"d:\" });
        }

        [Test]
        public async Task MockDriveInfoFactory_GetDrives_ShouldReturnOnlyLocalDrives()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Test"));
            fileSystem.AddDirectory(XFS.Path(@"Z:\Test"));
            fileSystem.AddDirectory(XFS.Path(@"d:\Test"));
            fileSystem.AddDirectory(XFS.Path(@"\\anunc\share\Zzz"));
            var factory = new MockDriveInfoFactory(fileSystem);

            // Act
            var actualResults = factory.GetDrives();

            var actualNames = actualResults.Select(d => d.Name);

            // Assert
            await That(actualNames).IsEquivalentTo(new[] { @"C:\", @"Z:\", @"d:\" });
        }

        [Test]
        public async Task MockDriveInfoFactory_New_WithDriveShouldReturnDrive()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var factory = new MockDriveInfoFactory(fileSystem);

            // Act
            var actualResult = factory.New(@"Z:\");

            // Assert
            await That(actualResult.Name).IsEquivalentTo(@"Z:\");
        }

        [Test]
        public async Task MockDriveInfoFactory_New_WithPathShouldReturnDrive()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var factory = new MockDriveInfoFactory(fileSystem);

            // Act
            var actualResult = factory.New(@"Z:\foo\bar\");

            // Assert
            await That(actualResult.Name).IsEquivalentTo(@"Z:\");
        }

        [Test]
        public async Task MockDriveInfoFactory_Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new MockFileSystem();

            var result = fileSystem.DriveInfo.Wrap(null);

            await That(result).IsNull();
        }
    }
}
