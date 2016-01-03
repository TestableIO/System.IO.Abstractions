namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;


    using Text;
    using Xunit;
    using XFS = MockUnixSupport;

    public class MockFileReadLinesTests {
        [Fact]
        public void MockFile_ReadLines_ShouldReturnOriginalTextData()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo\r\ntext\ncontent\rvalue") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadLines(XFS.Path(@"c:\something\demo.txt"));

            // Assert
            Assert.Equal(
                new[] { "Demo", "text", "content", "value" },
                result);
        }

        [Fact]
        public void MockFile_ReadLines_ShouldReturnOriginalDataWithCustomEncoding()
        {
            // Arrange
            string text = "Hello\r\nthere\rBob\nBob!";
            var encodedText = Encoding.BigEndianUnicode.GetBytes(text);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData(encodedText) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadLines(XFS.Path(@"c:\something\demo.txt"), Encoding.BigEndianUnicode);

            // Assert
            Assert.Equal(
                new [] { "Hello", "there", "Bob", "Bob!" },
                result);
        }
    }
}
