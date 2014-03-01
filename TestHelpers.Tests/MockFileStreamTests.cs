using System.Collections.Generic;

using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileStreamTests
    {
        [Test]
        public void MockFileStream_Flush_WritesByteToFile()
        {
            // Arrange
            const string filepath = @"c:\something\foo.txt";
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var cut = new MockFileStream(filesystem, filepath);

            // Act
            cut.WriteByte(255);
            cut.Flush();

            // Assert
            CollectionAssert.AreEqual(new byte[]{255}, filesystem.GetFile(filepath).Contents);
        }
    }
}
