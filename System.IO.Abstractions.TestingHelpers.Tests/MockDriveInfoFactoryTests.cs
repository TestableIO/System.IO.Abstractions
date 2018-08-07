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
        public void MockDriveInfoFactory_GetDrives_ShouldReturnDrives()
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
            Assert.That(actualNames, Is.EquivalentTo(new[] { @"C:\", @"Z:\", @"D:\" }));
        }

        [Test]
        public void MockDriveInfoFactory_GetDrives_ShouldReturnDrivesWithNoDuplicates()
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
            Assert.That(actualNames, Is.EquivalentTo(new[] { @"C:\", @"Z:\", @"D:\" }));
        }

        [Test]
        public void MockDriveInfoFactory_GetDrives_ShouldReturnOnlyLocalDrives()
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
            Assert.That(actualNames, Is.EquivalentTo(new[] { @"C:\", @"Z:\", @"D:\" }));
        }

        [Test]
        public void MockDriveInfoFactory_FromDriveName_WithDriveShouldReturnDrive()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var factory = new MockDriveInfoFactory(fileSystem);

            // Act
            var actualResult = factory.FromDriveName(@"Z:\");

            // Assert
            Assert.That(actualResult.Name, Is.EquivalentTo(@"Z:\"));
        }

        [Test]
        public void MockDriveInfoFactory_FromDriveName_WithPathShouldReturnDrive()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var factory = new MockDriveInfoFactory(fileSystem);

            // Act
            var actualResult = factory.FromDriveName(@"Z:\foo\bar\");

            // Assert
            Assert.That(actualResult.Name, Is.EquivalentTo(@"Z:\"));
        }
    }
}
