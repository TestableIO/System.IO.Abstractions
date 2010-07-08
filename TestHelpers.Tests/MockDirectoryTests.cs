using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestClass]
    public class MockDirectoryTests
    {
        [TestMethod]
        public void MockDirectory_GetCreationTime_ShouldReturnCreationTimeFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            
            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetCreationTime(path, time);
            var result = fileSystem.Directory.GetCreationTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_GetCreationTimeUtc_ShouldReturnCreationTimeUtcFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetCreationTimeUtc(path, time);
            var result = fileSystem.Directory.GetCreationTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_GetLastAccessTime_ShouldReturnLastAccessTimeFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetLastAccessTime(path, time);
            var result = fileSystem.Directory.GetLastAccessTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_GetLastAccessTimeUtc_ShouldReturnLastAccessTimeUtcFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetLastAccessTimeUtc(path, time);
            var result = fileSystem.Directory.GetLastAccessTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_GetLastWriteTime_ShouldReturnLastWriteTimeFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetLastWriteTime(path, time);
            var result = fileSystem.Directory.GetLastWriteTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_GetLastWriteTimeUtc_ShouldReturnLastWriteTimeUtcFromFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.File.SetLastWriteTimeUtc(path, time);
            var result = fileSystem.Directory.GetLastWriteTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_SetCreationTime_ShouldSetCreationTimeOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetCreationTime(path, time);
            var result = fileSystem.File.GetCreationTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_SetCreationTimeUtc_ShouldSetCreationTimeUtcOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetCreationTimeUtc(path, time);
            var result = fileSystem.File.GetCreationTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_SetLastAccessTime_ShouldSetLastAccessTimeOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetLastAccessTime(path, time);
            var result = fileSystem.File.GetLastAccessTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_SetLastAccessTimeUtc_ShouldSetLastAccessTimeUtcOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetLastAccessTimeUtc(path, time);
            var result = fileSystem.File.GetLastAccessTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_SetLastWriteTime_ShouldSetLastWriteTimeOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetLastWriteTime(path, time);
            var result = fileSystem.File.GetLastWriteTime(path);

            // Assert
            Assert.AreEqual(time, result);
        }

        [TestMethod]
        public void MockDirectory_SetLastWriteTimeUtc_ShouldSetLastWriteTimeUtcOnFile()
        {
            // Arrange
            const string path = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var time = new DateTime(2010, 6, 4, 13, 26, 42);
            fileSystem.Directory.SetLastWriteTimeUtc(path, time);
            var result = fileSystem.File.GetLastWriteTimeUtc(path);

            // Assert
            Assert.AreEqual(time, result);
        }
    }
}