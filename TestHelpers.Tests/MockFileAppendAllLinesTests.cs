using System.Collections.Generic;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileAppendAllLinesTests
    {
        [Test]
        public void MockFile_AppendAllLines_ShouldPersistNewLinesToExistingFile()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllLines(path, new[] { "line 1", "line 2", "line 3" });

            // Assert
            Assert.AreEqual(
                "Demo text contentline 1" + Environment.NewLine + "line 2" + Environment.NewLine + "line 3" + Environment.NewLine,
                file.ReadAllText(path));
        }

        [Test]
        public void MockFile_AppendAllLines_ShouldPersistNewLinesToNewFile()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\"), new MockDirectoryData() }
            });
            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllLines(path, new[] { "line 1", "line 2", "line 3" });

            // Assert
            Assert.AreEqual(
                "line 1" + Environment.NewLine + "line 2" + Environment.NewLine + "line 3" + Environment.NewLine,
                file.ReadAllText(path));
        }
    }
}
