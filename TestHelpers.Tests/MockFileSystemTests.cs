using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
#if NET40
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
#endif

        [Test]
        public void MockFileSystem_AddDirectory_ShouldCreateDirectory()
        {
            // Arrange
            string baseDirectory = MockUnixSupport.Path(@"C:\Test");
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.AddDirectory(baseDirectory);

            // Assert
            Assert.IsTrue(fileSystem.Directory.Exists(baseDirectory));
        }

        [Test]
        public void MockFileSystem_AddDirectory_ShouldThrowExceptionIfDirectoryIsReadOnly()
        {
            // Arrange
            string baseDirectory = MockUnixSupport.Path(@"C:\Test");
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(baseDirectory, new MockFileData(string.Empty));
            fileSystem.File.SetAttributes(baseDirectory, FileAttributes.ReadOnly);

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
            fileSystem.AddFile(MockUnixSupport.Path(@"C:\test\file.txt"), "foo");
            fileSystem.AddFile(MockUnixSupport.Path(@"C:\LOUD\file.txt"), "foo");
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\test\SUBDirectory"));
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\LOUD\SUBDirectory"));

            // Assert
            Assert.Contains(MockUnixSupport.Path(@"C:\test\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(MockUnixSupport.Path(@"C:\LOUD\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(MockUnixSupport.Path(@"C:\test\SUBDirectory\"), fileSystem.AllDirectories.ToList());
            Assert.Contains(MockUnixSupport.Path(@"C:\LOUD\SUBDirectory\"), fileSystem.AllDirectories.ToList());
        }

        [Test]
        public void MockFileSystem_AddFile_ShouldMatchCapitalization_PartialMatch()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\test\subtest"));
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\LOUD\SUBLOUD"));

            // Act
            fileSystem.AddFile(MockUnixSupport.Path(@"C:\test\SUBTEST\file.txt"), "foo");
            fileSystem.AddFile(MockUnixSupport.Path(@"C:\LOUD\subloud\file.txt"), "foo");
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\test\SUBTEST\SUBDirectory"));
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\LOUD\subloud\SUBDirectory"));

            // Assert
            Assert.Contains(MockUnixSupport.Path(@"C:\test\subtest\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(MockUnixSupport.Path(@"C:\LOUD\SUBLOUD\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(MockUnixSupport.Path(@"C:\test\subtest\SUBDirectory\"), fileSystem.AllDirectories.ToList());
            Assert.Contains(MockUnixSupport.Path(@"C:\LOUD\SUBLOUD\SUBDirectory\"), fileSystem.AllDirectories.ToList());
        }

        [Test]
        public void MockFileSystem_AddFile_ShouldMatchCapitalization_PartialMatch_FurtherLeft()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\test\subtest"));
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\LOUD\SUBLOUD"));

            // Act
            fileSystem.AddFile(MockUnixSupport.Path(@"C:\test\SUBTEST\new\file.txt"), "foo");
            fileSystem.AddFile(MockUnixSupport.Path(@"C:\LOUD\subloud\new\file.txt"), "foo");
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\test\SUBTEST\new\SUBDirectory"));
            fileSystem.AddDirectory(MockUnixSupport.Path(@"C:\LOUD\subloud\new\SUBDirectory"));

            // Assert
            Assert.Contains(MockUnixSupport.Path(@"C:\test\subtest\new\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(MockUnixSupport.Path(@"C:\LOUD\SUBLOUD\new\file.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(MockUnixSupport.Path(@"C:\test\subtest\new\SUBDirectory\"), fileSystem.AllDirectories.ToList());
            Assert.Contains(MockUnixSupport.Path(@"C:\LOUD\SUBLOUD\new\SUBDirectory\"), fileSystem.AllDirectories.ToList());
        }

        [Test]
        public void MockFileSystem_AddFileFromEmbeddedResource_ShouldAddTheFile()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.AddFileFromEmbeddedResource(MockUnixSupport.Path(@"C:\TestFile.txt"), Assembly.GetExecutingAssembly(), "System.IO.Abstractions.TestingHelpers.Tests.TestFiles.TestFile.txt");
            var result = fileSystem.GetFile(MockUnixSupport.Path(@"C:\TestFile.txt"));

            // Assert
            Assert.AreEqual(new UTF8Encoding().GetBytes("This is a test file."), result.Contents);
        }

        [Test]
        public void MockFileSystem_AddFilesFromEmbeddedResource_ShouldAddAllTheFiles()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            //Act
            fileSystem.AddFilesFromEmbeddedNamespace(MockUnixSupport.Path(@"C:\"), Assembly.GetExecutingAssembly(), "System.IO.Abstractions.TestingHelpers.Tests.TestFiles");

            Assert.Contains(MockUnixSupport.Path(@"C:\TestFile.txt"), fileSystem.AllFiles.ToList());
            Assert.Contains(MockUnixSupport.Path(@"C:\SecondTestFile.txt"), fileSystem.AllFiles.ToList());
        }
    }
}
