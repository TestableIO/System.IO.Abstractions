using System.Collections.Generic;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockDirectoryInfoTests
    {
        [Test]
        public void MockDirectoryInfo_GetExtension_ShouldReturnEmptyString()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var directoryInfo = new MockDirectoryInfo(fileSystem, @"c:\temp");

            // Act
            var result = directoryInfo.Extension;

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void MockDirectoryInfo_GetExtensionWithTrailingSlash_ShouldReturnEmptyString()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var directoryInfo = new MockDirectoryInfo(fileSystem, @"c:\temp\");

            // Act
            var result = directoryInfo.Extension;

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestCase(@"c:\temp\folder", true)]
        [TestCase(@"c:\temp\folder\notExistant", false)]
        public void MockDirectoryInfo_Exists(string path, bool expected) 
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> 
            {
                {@"c:\temp\folder\file.txt", new MockFileData("Hello World")}
            });
            var directoryInfo = new MockDirectoryInfo(fileSystem, path);

            var result = directoryInfo.Exists;

            Assert.That(result, Is.EqualTo(expected));
        }
  
        [Test]
        public void MockDirectoryInfo_FullName_ShouldReturnFullNameIncludingTrailingPathDelimiter() 
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {
                        @"c:\temp\folder\file.txt",
                        new MockFileData("Hello World")
                }
            });
            var directoryInfo = new MockDirectoryInfo(fileSystem, @"c:\temp\folder");

            var result = directoryInfo.FullName;

            Assert.That(result, Is.EqualTo(@"c:\temp\folder\"));
        }
    }
}