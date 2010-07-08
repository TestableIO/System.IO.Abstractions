using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestClass]
    public class MockFileTests
    {
        [TestMethod]
        public void MockFile_ReadAllBytes_ShouldReturnOriginalByteData()
        {
            // Arrange
            var fileSystem = new MockFileSystem
            (
                new MockFileData(@"c:\something\demo.txt", "Demo text content"),
                new MockFileData(@"c:\something\other.gif", new byte[] {0x21, 0x58, 0x3f, 0xa9})
            );

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllBytes(@"c:\something\other.gif");

            // Assert
            CollectionAssert.AreEqual(
                new byte[] { 0x21, 0x58, 0x3f, 0xa9 },
                result);
        }

        [TestMethod]
        public void MockFile_ReadAllLines_ShouldReturnOriginalTextData()
        {
            // Arrange
            var fileSystem = new MockFileSystem
            (
                new MockFileData(@"c:\something\demo.txt", "Demo\r\ntext\ncontent\rvalue"),
                new MockFileData(@"c:\something\other.gif", new byte[] { 0x21, 0x58, 0x3f, 0xa9 })
            );

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllLines(@"c:\something\demo.txt");

            // Assert
            CollectionAssert.AreEqual(
                new[] { "Demo", "text", "content", "value" },
                result);
        }

        [TestMethod]
        public void MockFile_ReadAllText_ShouldReturnOriginalTextData()
        {
            // Arrange
            var fileSystem = new MockFileSystem
            (
                new MockFileData(@"c:\something\demo.txt", "Demo text content"),
                new MockFileData(@"c:\something\other.gif", new byte[] { 0x21, 0x58, 0x3f, 0xa9 })
            );

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllText(@"c:\something\demo.txt");

            // Assert
            Assert.AreEqual(
                "Demo text content",
                result);
        }
    }
}