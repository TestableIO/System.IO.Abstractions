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

        [Test]
        public void MockDriveInfo_AvailableFreeSpace_ShouldReturnAvailableFreeSpaceOfDriveInMemoryFileSystem()
        {
            // Arrange
            var availableFreeSpace = 1024L;
            var driveData = new MockDriveData { AvailableFreeSpace = availableFreeSpace };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var mockDriveInfo = new MockDriveInfo(fileSystem, "C:");

            // Act
            var result = mockDriveInfo.AvailableFreeSpace;

            // Assert
            Assert.That(result, Is.EqualTo(availableFreeSpace));
        }

        [Test]
        public void MockDriveInfo_DriveFormat_ShouldReturnDriveFormatOfDriveInMemoryFileSystem()
        {
            // Arrange
            var driveFormat = "NTFS";
            var driveData = new MockDriveData { DriveFormat = driveFormat };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var mockDriveInfo = new MockDriveInfo(fileSystem, "C:");

            // Act
            var result = mockDriveInfo.DriveFormat;

            // Assert
            Assert.That(result, Is.EqualTo(driveFormat));
        }

        [Test]
        public void MockDriveInfo_DriveType_ShouldReturnDriveTypeOfDriveInMemoryFileSystem()
        {
            // Arrange
            var driveType = DriveType.Fixed;
            var driveData = new MockDriveData { DriveType = driveType };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var mockDriveInfo = new MockDriveInfo(fileSystem, "C:");

            // Act
            var result = mockDriveInfo.DriveType;

            // Assert
            Assert.That(result, Is.EqualTo(driveType));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void MockDriveInfo_IsReady_ShouldReturnIsReadyOfDriveInMemoryFileSystem(bool isReady)
        {
            // Arrange
            var driveData = new MockDriveData { IsReady = isReady };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var mockDriveInfo = new MockDriveInfo(fileSystem, "C:");

            // Act
            var result = mockDriveInfo.IsReady;

            // Assert
            Assert.That(result, Is.EqualTo(isReady));
        }

        [Test]
        public void MockDriveInfo_TotalFreeSpace_ShouldReturnTotalFreeSpaceOfDriveInMemoryFileSystem()
        {
            // Arrange
            var totalFreeSpace = 4096L;
            var driveData = new MockDriveData { TotalFreeSpace = totalFreeSpace };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var mockDriveInfo = new MockDriveInfo(fileSystem, "C:");

            // Act
            var result = mockDriveInfo.TotalFreeSpace;

            // Assert
            Assert.That(result, Is.EqualTo(totalFreeSpace));
        }

        [Test]
        public void MockDriveInfo_TotalSize_ShouldReturnTotalSizeOfDriveInMemoryFileSystem()
        {
            // Arrange
            var totalSize = 8192L;
            var driveData = new MockDriveData { TotalSize = totalSize };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var mockDriveInfo = new MockDriveInfo(fileSystem, "C:");

            // Act
            var result = mockDriveInfo.TotalSize;

            // Assert
            Assert.That(result, Is.EqualTo(totalSize));
        }

        [Test]
        public void MockDriveInfo_VolumeLabel_ShouldReturnVolumeLabelOfDriveInMemoryFileSystem()
        {
            // Arrange
            var volumeLabel = "Windows";
            var driveData = new MockDriveData { VolumeLabel = volumeLabel };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var mockDriveInfo = new MockDriveInfo(fileSystem, "C:");

            // Act
            var result = mockDriveInfo.VolumeLabel;

            // Assert
            Assert.That(result, Is.EqualTo(volumeLabel));
        }
    }
}
