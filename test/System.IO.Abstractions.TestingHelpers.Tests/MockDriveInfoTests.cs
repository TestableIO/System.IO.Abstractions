using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;
    public class MockDriveInfoTests
    {
        [Theory]
        [InlineData(@"c:")]
        [InlineData(@"c:\")]
        public void MockDriveInfo_Constructor_ShouldInitializeLocalWindowsDrives(string driveName)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\Test"));
            var path = XFS.Path(driveName);

            // Act
            var driveInfo = new MockDriveInfo(fileSystem, path);

            // Assert
            Assert.Equal(@"C:\", driveInfo.Name);
        }

        [Fact]
        public void MockDriveInfo_Constructor_ShouldInitializeLocalWindowsDrives_SpecialForWindows()
        {
            if (XFS.IsUnixPlatform())
            {
                Console.WriteLine("Using XFS.Path transform c into c:.");
                return;
            }

            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\Test"));

            // Act
            var driveInfo = new MockDriveInfo(fileSystem, "c");

            // Assert
            Assert.Equal(@"C:\", driveInfo.Name);
        }

        [Theory]
        [InlineData(@"\\unc\share")]
        [InlineData(@"\\unctoo")]
        public void MockDriveInfo_Constructor_ShouldThrowExceptionIfUncPath(string driveName)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => new MockDriveInfo(fileSystem, XFS.Path(driveName));

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void MockDriveInfo_RootDirectory_ShouldReturnTheDirectoryBase()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\Test"));
            var driveInfo = new MockDriveInfo(fileSystem, "c:");
            var expectedDirectory = XFS.Path(@"C:\");

            // Act
            var actualDirectory = driveInfo.RootDirectory;

            // Assert
            Assert.Equal(expectedDirectory, actualDirectory.FullName);
        }
    }
}
