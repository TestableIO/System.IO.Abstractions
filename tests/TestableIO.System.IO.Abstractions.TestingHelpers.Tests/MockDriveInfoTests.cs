using NUnit.Framework;
using System.Linq;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public class MockDriveInfoTests
    {
        [TestCase(@"c:")]
        [TestCase(@"c:\")]
        public async Task MockDriveInfo_Constructor_ShouldInitializeLocalWindowsDrives(string driveName)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\Test"));
            var path = XFS.Path(driveName);

            // Act
            var driveInfo = new MockDriveInfo(fileSystem, path);

            // Assert
            await That(driveInfo.Name).IsEqualTo(@"c:\");
        }

        [Test]
        public async Task MockDriveInfo_Constructor_ShouldInitializeLocalWindowsDrives_SpecialForWindows()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\Test"));

            // Act
            var driveInfo = new MockDriveInfo(fileSystem, "c");

            // Assert
            await That(driveInfo.Name).IsEqualTo(@"c:\");
        }

        [TestCase(@"\\unc\share")]
        [TestCase(@"\\unctoo")]
        public async Task MockDriveInfo_Constructor_ShouldThrowExceptionIfUncPath(string driveName)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => new MockDriveInfo(fileSystem, XFS.Path(driveName));

            // Assert
            await That(action).Throws<ArgumentException>();
        }

        [Test]
        public async Task MockDriveInfo_RootDirectory_ShouldReturnTheDirectoryBase()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\Test"));
            var driveInfo = new MockDriveInfo(fileSystem, "c:");
            var expectedDirectory = XFS.Path(@"c:\");

            // Act
            var actualDirectory = driveInfo.RootDirectory;

            // Assert
            await That(actualDirectory.FullName).IsEqualTo(expectedDirectory);
        }

        [TestCase("c:", "c:\\")]
        [TestCase("C:", "C:\\")]
        [TestCase("d:", "d:\\")]
        [TestCase("e:", "e:\\")]
        [TestCase("f:", "f:\\")]
        public async Task MockDriveInfo_ToString_ShouldReturnTheDrivePath(string path, string expectedPath)
        {
            // Arrange
            var directoryPath = XFS.Path(path);

            // Act
            var mockDriveInfo = new MockDriveInfo(new MockFileSystem(), directoryPath);

            // Assert
            await That(mockDriveInfo.ToString()).IsEqualTo(expectedPath);
        }

        [Test]
        public async Task MockDriveInfo_AvailableFreeSpace_ShouldReturnAvailableFreeSpaceOfDriveInMemoryFileSystem()
        {
            // Arrange
            var availableFreeSpace = 1024L;
            var driveData = new MockDriveData { AvailableFreeSpace = availableFreeSpace };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var driveInfo = fileSystem.DriveInfo
                .GetDrives()
                .Single(x => x.Name == @"C:\");

            // Act
            var result = driveInfo.AvailableFreeSpace;

            // Assert
            await That(result).IsEqualTo(availableFreeSpace);
        }

        [Test]
        public async Task MockDriveInfo_DriveFormat_ShouldReturnDriveFormatOfDriveInMemoryFileSystem()
        {
            // Arrange
            var driveFormat = "NTFS";
            var driveData = new MockDriveData { DriveFormat = driveFormat };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var driveInfo = fileSystem.DriveInfo
                .GetDrives()
                .Single(x => x.Name == @"C:\");

            // Act
            var result = driveInfo.DriveFormat;

            // Assert
            await That(result).IsEqualTo(driveFormat);
        }

        [Test]
        public async Task MockDriveInfo_DriveType_ShouldReturnDriveTypeOfDriveInMemoryFileSystem()
        {
            // Arrange
            var driveType = DriveType.Fixed;
            var driveData = new MockDriveData { DriveType = driveType };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var driveInfo = fileSystem.DriveInfo
                .GetDrives()
                .Single(x => x.Name == @"C:\");

            // Act
            var result = driveInfo.DriveType;

            // Assert
            await That(result).IsEqualTo(driveType);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task MockDriveInfo_IsReady_ShouldReturnIsReadyOfDriveInMemoryFileSystem(bool isReady)
        {
            // Arrange
            var driveData = new MockDriveData { IsReady = isReady };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var driveInfo = fileSystem.DriveInfo
                .GetDrives()
                .Single(x => x.Name == @"C:\");

            // Act
            var result = driveInfo.IsReady;

            // Assert
            await That(result).IsEqualTo(isReady);
        }

        [Test]
        public async Task MockDriveInfo_TotalFreeSpace_ShouldReturnTotalFreeSpaceOfDriveInMemoryFileSystem()
        {
            // Arrange
            var totalFreeSpace = 4096L;
            var driveData = new MockDriveData { TotalFreeSpace = totalFreeSpace };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var driveInfo = fileSystem.DriveInfo
                .GetDrives()
                .Single(x => x.Name == @"C:\");

            // Act
            var result = driveInfo.TotalFreeSpace;

            // Assert
            await That(result).IsEqualTo(totalFreeSpace);
        }

        [Test]
        public async Task MockDriveInfo_TotalSize_ShouldReturnTotalSizeOfDriveInMemoryFileSystem()
        {
            // Arrange
            var totalSize = 8192L;
            var driveData = new MockDriveData { TotalSize = totalSize };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var driveInfo = fileSystem.DriveInfo
                .GetDrives()
                .Single(x => x.Name == @"C:\");

            // Act
            var result = driveInfo.TotalSize;

            // Assert
            await That(result).IsEqualTo(totalSize);
        }

        [Test]
        public async Task MockDriveInfo_VolumeLabel_ShouldReturnVolumeLabelOfDriveInMemoryFileSystem()
        {
            // Arrange
            var volumeLabel = "Windows";
            var driveData = new MockDriveData { VolumeLabel = volumeLabel };
            var fileSystem = new MockFileSystem();
            fileSystem.AddDrive("C:", driveData);
            var driveInfo = fileSystem.DriveInfo
                .GetDrives()
                .Single(x => x.Name == @"C:\");

            // Act
            var result = driveInfo.VolumeLabel;

            // Assert
            await That(result).IsEqualTo(volumeLabel);
        }
    }
}
