namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using Globalization;

    using NUnit.Framework;

    using Text;

    using XFS = MockUnixSupport;

    public class MockFileAppendAllLinesTests
    {

        [Test]
        public void MockFile_AppendAllLines_ShouldPersistNewLines()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {path, new MockFileData("Demo text content")}
            });

            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllLines(path, new List<string> {"line 1", "line 2", "line 3"});

            // Assert
            Assert.AreEqual(
                "Demo text contentline 1line 2line 3",
                file.ReadAllText(path));
        }


    }
}