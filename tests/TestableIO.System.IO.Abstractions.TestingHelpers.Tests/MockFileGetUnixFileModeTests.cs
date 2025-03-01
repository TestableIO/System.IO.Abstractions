#if FEATURE_UNIX_FILE_MODE
using System.Runtime.Versioning;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using NUnit.Framework;

    using XFS = MockUnixSupport;

    [UnsupportedOSPlatform("windows")]
    [UnixOnly("This feature is not supported on Windows.")]
    public class MockFileGetUnixFileModeTests
    {
        [Test]
        public async Task MockFile_GetUnixFileMode_ShouldReturnDefaultAccessMode()
        {
            // Arrange
            var expected = UnixFileMode.UserRead | 
                           UnixFileMode.GroupRead | 
                           UnixFileMode.OtherRead | 
                           UnixFileMode.UserWrite;
            
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"C:\something\some.txt"), new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.File.GetUnixFileMode(XFS.Path(@"C:\something\some.txt"));

            // Assert
            await That(result).IsEqualTo(expected);
        }
        
        [Test]
        public async Task MockFile_GetUnixFileMode_ShouldReturnSpecifiedAccessMode([Values] UnixFileMode unixFileMode)
        {
            // Arrange
            var mockFileData = new MockFileData("Demo text content")
            {
                UnixMode = unixFileMode
            };
            
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"C:\something\some.txt"), mockFileData }
            });

            // Act
            var result = fileSystem.File.GetUnixFileMode(XFS.Path(@"C:\something\some.txt"));

            // Assert
            await That(result).IsEqualTo(unixFileMode);
        }
    }
}
#endif