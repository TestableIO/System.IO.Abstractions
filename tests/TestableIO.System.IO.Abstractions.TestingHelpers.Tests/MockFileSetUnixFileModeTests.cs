#if FEATURE_UNIX_FILE_MODE
using System.Runtime.Versioning;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using NUnit.Framework;

    using XFS = MockUnixSupport;

    [UnsupportedOSPlatform("windows")]
    [UnixOnly("This feature is not supported on Windows.")]
    public class MockFileSetUnixFileModeTests
    {
        [Test]
        public async Task MockFile_SetUnixFileMode_ShouldSetSpecifiedAccessMode([Values] UnixFileMode unixFileMode)
        {
            // Arrange
            var mockFileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"C:\something\some.txt"), mockFileData }
            });

            // Act
            fileSystem.File.SetUnixFileMode(XFS.Path(@"C:\something\some.txt"), unixFileMode);

            // Assert
            await That(mockFileData.UnixMode).IsEqualTo(unixFileMode);
        }
        
        [TestCase(UnixFileMode.UserRead | UnixFileMode.GroupRead | UnixFileMode.OtherRead)]
        [TestCase(UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute)]
        [TestCase(UnixFileMode.UserExecute | UnixFileMode.OtherWrite | UnixFileMode.GroupRead)]
        public async Task MockFile_SetUnixFileMode_ShouldSetSpecifiedAccessModeFlags(UnixFileMode unixFileMode)
        {
            // Arrange
            var mockFileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"C:\something\some.txt"), mockFileData }
            });

            // Act
            fileSystem.File.SetUnixFileMode(XFS.Path(@"C:\something\some.txt"), unixFileMode);

            // Assert
            await That(mockFileData.UnixMode).IsEqualTo(unixFileMode);
        }
    }
}
#endif