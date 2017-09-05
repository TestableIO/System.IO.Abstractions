using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSystemTests
    {
        [Test]
        public void MockFileSystem_GetFile_ShouldReturnNullWhenFileIsNotRegistered()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", new MockFileData("Demo\r\ntext\ncontent\rvalue") },
                { @"c:\something\other.gif", new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            // Act
            var result = fileSystem.GetFile(@"c:\something\else.txt");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void MockFileSystem_GetFile_ShouldReturnFileRegisteredInConstructor()
        {
            // Arrange
            var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", file1 },
                { @"c:\something\other.gif", new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            // Act
            var result = fileSystem.GetFile(@"c:\something\demo.txt");

            // Assert
            Assert.AreEqual(file1, result);
        }

        [Test]
        public void MockFileSystem_GetFile_ShouldReturnFileRegisteredInConstructorWhenPathsDifferByCase()
        {
            // Arrange
            var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", file1 },
                { @"c:\something\other.gif", new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            // Act
            var result = fileSystem.GetFile(@"c:\SomeThing\DEMO.txt");

            // Assert
            Assert.AreEqual(file1, result);
        }

        [Test]
        public void MockFileSystem_AddFile_ShouldRepaceExistingFile()
        {
            const string path = @"c:\some\file.txt";
            const string existingContent = "Existing content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData(existingContent) }
            });
            Assert.That(fileSystem.GetFile(path).TextContents, Is.EqualTo(existingContent));

            const string newContent = "New content";
            fileSystem.AddFile(path, new MockFileData(newContent));

            Assert.That(fileSystem.GetFile(path).TextContents, Is.EqualTo(newContent));
        }

        [Test]
        public void Is_Serializable()
        {
            var file1 = new MockFileData("Demo\r\ntext\ncontent\rvalue");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\something\demo.txt", file1 },
                { @"c:\something\other.gif", new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });
            var memoryStream = new MemoryStream();

            var serializer = new Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            serializer.Serialize(memoryStream, fileSystem);

            Assert.That(memoryStream.Length > 0, "Length didn't increase after serialization task.");
        }

        [Test]
        public void MockFileSystem_AddDirectory_ShouldCreateDirectory()
        {
            // Arrange
            string baseDirectory = MockUnixSupport.Path(@"C:\Test");
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.AddDirectory(baseDirectory);

            // Assert
            Assert.IsTrue(fileSystem.Internals.Directory.Exists(baseDirectory));
        }

        [Test]
        public void MockFileSystem_AddDirectory_ShouldThrowExceptionIfDirectoryIsReadOnly()
        {
            // Arrange
            string baseDirectory = MockUnixSupport.Path(@"C:\Test");
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(baseDirectory, new MockFileData(string.Empty));
            fileSystem.Internals.File.SetAttributes(baseDirectory, FileAttributes.ReadOnly);

            // Act
            TestDelegate act = () => fileSystem.AddDirectory(baseDirectory);

            // Assert
            Assert.Throws<UnauthorizedAccessException>(act);
        }

        [Test]
        public void MockFileSystem_DriveInfo_ShouldNotThrowAnyException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\Test"));
            fileSystem.AddDirectory(MockUnixSupport.Path(@"Z:\Test"));
            fileSystem.AddDirectory(MockUnixSupport.Path(@"d:\Test"));

            // Act
            var actualResults = fileSystem.DriveInfo.GetDrives();

            // Assert
            Assert.IsNotNull(actualResults);
        }

        [Test]
        public void MockFileSystem_AddFile_ShouldMatchCapitalization_PerfectMatch()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\test"));
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\LOUD"));

            // Act
            fileSystem.AddFile(@"C:\test\file.txt", "foo");
            fileSystem.AddFile(@"C:\LOUD\file.txt", "foo");
            fileSystem.AddDirectory(@"C:\test\SUBDirectory");
            fileSystem.AddDirectory(@"C:\LOUD\SUBDirectory");

            // Assert
            Assert.Contains(@"C:\test\file.txt", fileSystem.AllFiles.ToList());
            Assert.Contains(@"C:\LOUD\file.txt", fileSystem.AllFiles.ToList());
            Assert.Contains(@"C:\test\SUBDirectory\", fileSystem.AllDirectories.ToList());
            Assert.Contains(@"C:\LOUD\SUBDirectory\", fileSystem.AllDirectories.ToList());
        }

        [Test]
        public void MockFileSystem_AddFile_ShouldMatchCapitalization_PartialMatch()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\test\subtest"));
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\LOUD\SUBLOUD"));

            // Act
            fileSystem.AddFile(@"C:\test\SUBTEST\file.txt", "foo");
            fileSystem.AddFile(@"C:\LOUD\subloud\file.txt", "foo");
            fileSystem.AddDirectory(@"C:\test\SUBTEST\SUBDirectory");
            fileSystem.AddDirectory(@"C:\LOUD\subloud\SUBDirectory");

            // Assert
            Assert.Contains(@"C:\test\subtest\file.txt", fileSystem.AllFiles.ToList());
            Assert.Contains(@"C:\LOUD\SUBLOUD\file.txt", fileSystem.AllFiles.ToList());
            Assert.Contains(@"C:\test\subtest\SUBDirectory\", fileSystem.AllDirectories.ToList());
            Assert.Contains(@"C:\LOUD\SUBLOUD\SUBDirectory\", fileSystem.AllDirectories.ToList());
        }

        [Test]
        public void MockFileSystem_AddFile_ShouldMatchCapitalization_PartialMatch_FurtherLeft()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\test\subtest"));
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\LOUD\SUBLOUD"));

            // Act
            fileSystem.AddFile(@"C:\test\SUBTEST\new\file.txt", "foo");
            fileSystem.AddFile(@"C:\LOUD\subloud\new\file.txt", "foo");
            fileSystem.AddDirectory(@"C:\test\SUBTEST\new\SUBDirectory");
            fileSystem.AddDirectory(@"C:\LOUD\subloud\new\SUBDirectory");

            // Assert
            Assert.Contains(@"C:\test\subtest\new\file.txt", fileSystem.AllFiles.ToList());
            Assert.Contains(@"C:\LOUD\SUBLOUD\new\file.txt", fileSystem.AllFiles.ToList());
            Assert.Contains(@"C:\test\subtest\new\SUBDirectory\", fileSystem.AllDirectories.ToList());
            Assert.Contains(@"C:\LOUD\SUBLOUD\new\SUBDirectory\", fileSystem.AllDirectories.ToList());
        }
    }
}