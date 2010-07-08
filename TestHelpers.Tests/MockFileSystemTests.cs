using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestClass]
    public class MockFileSystemTests
    {
        [TestMethod]
        public void MockFileSystem_GetFile_ShouldThrowFileNotFoundExceptionWhenFileIsNotRegistered()
        {
            // Arrange
            var fileSystem = new MockFileSystem
            (
                new MockFileData(@"c:\something\demo.txt", "Demo\r\ntext\ncontent\rvalue"),
                new MockFileData(@"c:\something\other.gif", new byte[] { 0x21, 0x58, 0x3f, 0xa9 })
            );

            try
            {
                // Act
                fileSystem.GetFile(@"c:\something\else.txt");
            }
            catch(FileNotFoundException ex)
            {
                // Assert
                Assert.AreEqual("File not found in mock file system.", ex.Message);
                Assert.AreEqual(@"c:\something\else.txt", ex.FileName);

                return;
            }

            // Assert
            Assert.Fail("Expected exception was never thrown.");
        }

        [TestMethod]
        public void MockFileSystem_GetFile_ShouldReturnFileRegisteredInConstructor()
        {
            // Arrange
            var file1 = new MockFileData(@"c:\something\demo.txt", "Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem
            (
                file1,
                new MockFileData(@"c:\something\other.gif", new byte[] { 0x21, 0x58, 0x3f, 0xa9 })
            );

            // Act
            var result = fileSystem.GetFile(@"c:\something\demo.txt");

            // Assert
            Assert.AreEqual(file1, result);
        }

        [TestMethod]
        public void MockFileSystem_GetFile_ShouldReturnFileRegisteredInConstructorWhenPathsDifferByCase()
        {
            // Arrange
            var file1 = new MockFileData(@"c:\something\demo.txt", "Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem
            (
                file1,
                new MockFileData(@"c:\something\other.gif", new byte[] { 0x21, 0x58, 0x3f, 0xa9 })
            );

            // Act
            var result = fileSystem.GetFile(@"c:\SomeThing\DEMO.txt");

            // Assert
            Assert.AreEqual(file1, result);
        }
    }
}