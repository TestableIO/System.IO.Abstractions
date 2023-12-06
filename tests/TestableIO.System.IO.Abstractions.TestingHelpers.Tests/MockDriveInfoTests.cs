using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public class MockDriveInfoTests
    {
        [TestCase(@"c:")]
        [TestCase(@"c:\")]
        public void MockDriveInfo_Constructor_ShouldInitializeLocalWindowsDrives(string driveName)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\Test"));
            var path = XFS.Path(driveName);

            // Act
            var driveInfo = new MockDriveInfo(fileSystem, path);

            // Assert
            Assert.That(driveInfo.Name, Is.EqualTo(@"c:\"));
        }

        [Test]
        public void MockDriveInfo_Constructor_ShouldInitializeLocalWindowsDrives_SpecialForWindows()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\Test"));

            // Act
            var driveInfo = new MockDriveInfo(fileSystem, "c");

            // Assert
            Assert.That(driveInfo.Name, Is.EqualTo(@"c:\"));
        }

        [TestCase(@"\\unc\share")]
        [TestCase(@"\\unctoo")]
        public void MockDriveInfo_Constructor_ShouldThrowExceptionIfUncPath(string driveName)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => new MockDriveInfo(fileSystem, XFS.Path(driveName));

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void MockDriveInfo_RootDirectory_ShouldReturnTheDirectoryBase()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\Test"));
            var driveInfo = new MockDriveInfo(fileSystem, "c:");
            var expectedDirectory = XFS.Path(@"c:\");

            // Act
            var actualDirectory = driveInfo.RootDirectory;

            // Assert
            Assert.That(actualDirectory.FullName, Is.EqualTo(expectedDirectory));
        }

        [TestCase("c:", "c:\\")]
        [TestCase("C:", "C:\\")]
        [TestCase("d:", "d:\\")]
        [TestCase("e:", "e:\\")]
        [TestCase("f:", "f:\\")]
        public void MockDriveInfo_ToString_ShouldReturnTheDrivePath(string path, string expectedPath)
        {
            // Arrange
            var directoryPath = XFS.Path(path);

            // Act
            var mockDriveInfo = new MockDriveInfo(new MockFileSystem(), directoryPath);

            // Assert
            Assert.That(mockDriveInfo.ToString(), Is.EqualTo(expectedPath));
        }
    }
}
