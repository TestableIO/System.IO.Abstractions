using FluentAssertions;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using System.Collections.Generic;

    using Xunit;

    using XFS = MockUnixSupport;
    public class MockFileStreamTests
    {
        [Fact]
        public void MockFileStream_Flush_WritesByteToFile()
        {
            // Arrange
            var filepath = XFS.Path(@"c:\something\foo.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var cut = new MockFileStream(filesystem, filepath);

            // Act
            cut.WriteByte(255);
            cut.Flush();

            // Assert
            filesystem.GetFile(filepath).Contents.Should().BeSameAs(new byte[]{255});
        }

        [Fact]
        public void MockFileStream_Dispose_ShouldNotResurrectFile()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path("C:\\test");
            var directory = fileSystem.Path.GetDirectoryName(path);
            fileSystem.AddFile(path, new MockFileData("Bla"));
            var stream = fileSystem.File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete);

            var fileCount1 = fileSystem.Directory.GetFiles(directory, "*").Length;
            fileSystem.File.Delete(path);
            var fileCount2 = fileSystem.Directory.GetFiles(directory, "*").Length;
            stream.Dispose();
            var fileCount3 = fileSystem.Directory.GetFiles(directory, "*").Length;

            fileCount1.Should().Be(1, "File should have existed");
            fileCount2.Should().Be(0, "File should have been deleted");
            fileCount3.Should().Be(0, "Disposing stream should not have resurrected the file");
        }
    }
}
