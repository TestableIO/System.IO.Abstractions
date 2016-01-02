namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;
    using Xunit;

    using XFS = MockUnixSupport;

    public class MockFileExistsTests {
        [Fact]
        public void MockFile_Exists_ShouldReturnTrueForSamePath()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.Exists(XFS.Path(@"c:\something\other.gif"));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void MockFile_Exists_ShouldReturnTrueForPathVaryingByCase()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.Exists(XFS.Path(@"c:\SomeThing\Other.gif"));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void MockFile_Exists_ShouldReturnFalseForEntirelyDifferentPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.Exists(XFS.Path(@"c:\SomeThing\DoesNotExist.gif"));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void MockFile_Exists_ShouldReturnFalseForNullPath()
        {
            var file = new MockFile(new MockFileSystem());

            Assert.False(file.Exists(null));
        }

        [Fact]
        public void MockFile_Exists_ShouldReturnFalseForDirectories()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.Exists(XFS.Path(@"c:\SomeThing\"));

            // Assert
            Assert.False(result);
        }
    }
}