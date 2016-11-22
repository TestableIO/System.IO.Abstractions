using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockFileInfoTests
    {
        [Test]
        public void MockFileInfo_Exists_ShouldReturnTrueIfFileExistsInMemoryFileSystem()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData("Demo text content") },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var result = fileInfo.Exists;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MockFileInfo_Exists_ShouldReturnFalseIfFileDoesNotExistInMemoryFileSystem()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData("Demo text content") },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\foo.txt"));

            // Act
            var result = fileInfo.Exists;

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockFileInfo_Length_ShouldReturnLengthOfFileInMemoryFileSystem()
        {
            // Arrange
            const string fileContent = "Demo text content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData(fileContent) },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData(fileContent) },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var result = fileInfo.Length;

            // Assert
            Assert.AreEqual(fileContent.Length, result);
        }

        [Test]
        public void MockFileInfo_Length_ShouldThrowFileNotFoundExceptionIfFileDoesNotExistInMemoryFileSystem()
        {
            // Arrange
            const string fileContent = "Demo text content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData(fileContent) },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData(fileContent) },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\foo.txt"));

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var ex = Assert.Throws<FileNotFoundException>(() => fileInfo.Length.ToString(CultureInfo.InvariantCulture));
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.AreEqual(XFS.Path(@"c:\foo.txt"), ex.FileName);
        }

        [Test]
        public void MockFileInfo_CreationTimeUtc_ShouldReturnCreationTimeUtcOfFileInMemoryFileSystem()
        {
            // Arrange
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var result = fileInfo.CreationTimeUtc;

            // Assert
            Assert.AreEqual(creationTime.ToUniversalTime(), result);
        }

        [Test]
        public void MockFileInfo_CreationTimeUtc_ShouldSetCreationTimeUtcOfFileInMemoryFileSystem()
        {
            // Arrange
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var newUtcTime = DateTime.UtcNow;
            fileInfo.CreationTimeUtc = newUtcTime;

            // Assert
            Assert.AreEqual(newUtcTime, fileInfo.CreationTimeUtc);
        }


        [Test]
        public void MockFileInfo_CreationTime_ShouldReturnCreationTimeOfFileInMemoryFileSystem()
        {
            // Arrange
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var result = fileInfo.CreationTime;

            // Assert
            Assert.AreEqual(creationTime, result);
        }

        [Test]
        public void MockFileInfo_CreationTime_ShouldSetCreationTimeOfFileInMemoryFileSystem()
        {
            // Arrange
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var newTime = DateTime.Now;
            fileInfo.CreationTime = newTime;

            // Assert
            Assert.AreEqual(newTime, fileInfo.CreationTime);
        }

        [Test]
        public void MockFileInfo_IsReadOnly_ShouldSetReadOnlyAttributeOfFileInMemoryFileSystem()
        {
            // Arrange
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            fileInfo.IsReadOnly = true;

            // Assert
            Assert.AreEqual(FileAttributes.ReadOnly, fileData.Attributes & FileAttributes.ReadOnly);
        }

        [Test]
        public void MockFileInfo_IsReadOnly_ShouldSetNotReadOnlyAttributeOfFileInMemoryFileSystem()
        {
            // Arrange
            var fileData = new MockFileData("Demo text content") {Attributes = FileAttributes.ReadOnly};
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            fileInfo.IsReadOnly = false;

            // Assert
            Assert.AreNotEqual(FileAttributes.ReadOnly, fileData.Attributes & FileAttributes.ReadOnly);
        }

        [Test]
        public void MockFileInfo_AppendText_ShouldAddTextToFileInMemoryFileSystem()
        {
            // Arrange
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            using (var file = fileInfo.AppendText())
                file.WriteLine("This should be at the end");

            string newcontents;
            using (var newfile = fileInfo.OpenText())
                newcontents = newfile.ReadToEnd();

            // Assert
            Assert.AreEqual("Demo text contentThis should be at the end\r\n", newcontents);
        }

        [Test]
        public void MockFileInfo_OpenWrite_ShouldAddDataToFileInMemoryFileSystem()
        {
            // Arrange
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));
            var bytesToAdd = new byte[] {65, 66, 67, 68, 69};

            // Act
            using (var file = fileInfo.OpenWrite())
                file.Write(bytesToAdd, 0, bytesToAdd.Length);

            string newcontents;
            using (var newfile = fileInfo.OpenText())
                newcontents = newfile.ReadToEnd();

            // Assert
            Assert.AreEqual("ABCDEtext content", newcontents);
        }

        [Test]
        public void MockFileInfo_Encrypt_ShouldReturnXorOfFileInMemoryFileSystem()
        {
            // Arrange
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            fileInfo.Encrypt();

            string newcontents;
            using (var newfile = fileInfo.OpenText())
            {
                newcontents = newfile.ReadToEnd();
            }

            // Assert
            Assert.AreNotEqual("Demo text content", newcontents);
        }

        [Test]
        public void MockFileInfo_Decrypt_ShouldReturnCorrectContentsFileInMemoryFileSystem()
        {
            // Arrange
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));
            fileInfo.Encrypt();

            // Act
            fileInfo.Decrypt();

            string newcontents;
            using (var newfile = fileInfo.OpenText())
            {
                newcontents = newfile.ReadToEnd();
            }

            // Assert
            Assert.AreEqual("Demo text content", newcontents);
        }

        [Test]
        public void MockFileInfo_LastAccessTimeUtc_ShouldReturnLastAccessTimeUtcOfFileInMemoryFileSystem()
        {
            // Arrange
            var lastAccessTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastAccessTime = lastAccessTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var result = fileInfo.LastAccessTimeUtc;

            // Assert
            Assert.AreEqual(lastAccessTime.ToUniversalTime(), result);
        }

        [Test]
        public void MockFileInfo_LastAccessTimeUtc_ShouldSetCreationTimeUtcOfFileInMemoryFileSystem()
        {
            // Arrange
            var lastAccessTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastAccessTime = lastAccessTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var newUtcTime = DateTime.UtcNow;
            fileInfo.LastAccessTimeUtc = newUtcTime;

            // Assert
            Assert.AreEqual(newUtcTime, fileInfo.LastAccessTimeUtc);
        }

        [Test]
        public void MockFileInfo_LastWriteTimeUtc_ShouldReturnLastWriteTimeUtcOfFileInMemoryFileSystem()
        {
            // Arrange
            var lastWriteTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastWriteTime = lastWriteTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var result = fileInfo.LastWriteTimeUtc;

            // Assert
            Assert.AreEqual(lastWriteTime.ToUniversalTime(), result);
        }

        [Test]
        public void MockFileInfo_LastWriteTimeUtc_ShouldSetLastWriteTimeUtcOfFileInMemoryFileSystem()
        {
            // Arrange
            var lastWriteTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastWriteTime = lastWriteTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var newUtcTime = DateTime.UtcNow;
            fileInfo.LastWriteTime = newUtcTime;

            // Assert
            Assert.AreEqual(newUtcTime, fileInfo.LastWriteTime);
        }

        [Test]
        public void MockFileInfo_GetExtension_ShouldReturnExtension()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var result = fileInfo.Extension;

            // Assert
            Assert.AreEqual(".txt", result);
        }

        [Test]
        public void MockFileInfo_GetExtensionWithoutExtension_ShouldReturnEmptyString()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a"));

            // Act
            var result = fileInfo.Extension;

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void MockFileInfo_GetDirectoryName_ShouldReturnCompleteDirectoryPath()
        {
            // Arrange
            var fileInfo = new MockFileInfo(new MockFileSystem(), XFS.Path(@"c:\temp\level1\level2\file.txt"));

            // Act
            var result = fileInfo.DirectoryName;

            Assert.AreEqual(XFS.Path(@"c:\temp\level1\level2"), result);
        }

        [Test]
        public void MockFileInfo_GetDirectory_ShouldReturnDirectoryInfoWithCorrectPath()
        {
            // Arrange
            var fileInfo = new MockFileInfo(new MockFileSystem(), XFS.Path(@"c:\temp\level1\level2\file.txt"));

            // Act
            var result = fileInfo.Directory;

            Assert.AreEqual(XFS.Path(@"c:\temp\level1\level2"), result.FullName);
        }

        [Test]
        public void MockFileInfo_OpenRead_ShouldReturnByteContentOfFile()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\temp\file.txt"), new MockFileData(new byte[] { 1, 2 }));
            var fileInfo = fileSystem.FileInfo.FromFileName(XFS.Path(@"c:\temp\file.txt"));

            // Act
            byte[] result = new byte[2];
            using (var stream = fileInfo.OpenRead())
            {
                stream.Read(result, 0, 2);
            }

            Assert.AreEqual(new byte[] { 1, 2 }, result);
        }

        [Test]
        public void MockFileInfo_OpenText_ShouldReturnStringContentOfFile()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\temp\file.txt"), new MockFileData(@"line 1\r\nline 2"));
            var fileInfo = fileSystem.FileInfo.FromFileName(XFS.Path(@"c:\temp\file.txt"));

            // Act
            string result;
            using (var streamReader = fileInfo.OpenText())
            {
                result = streamReader.ReadToEnd();
            }

            Assert.AreEqual(@"line 1\r\nline 2", result);
        }

        [Test]
        public void MockFileInfo_MoveTo_ShouldUpdateFileInfoDirectoryAndFullName()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\temp\file.txt"), new MockFileData(@"line 1\r\nline 2"));
            var fileInfo = fileSystem.FileInfo.FromFileName(XFS.Path(@"c:\temp\file.txt"));

            // Act
            string destinationFolder = XFS.Path(@"c:\temp2");
            string destination = XFS.Path(destinationFolder + @"\file.txt");
            fileSystem.AddDirectory(destination);
            fileInfo.MoveTo(destination);

            Assert.AreEqual(fileInfo.DirectoryName, destinationFolder);
            Assert.AreEqual(fileInfo.FullName, destination);
        }

        [Test]
        public void MockFileInfo_MoveTo_SameSourceAndTargetIsANoOp()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\temp\file.txt"), new MockFileData(@"line 1\r\nline 2"));
            var fileInfo = fileSystem.FileInfo.FromFileName(XFS.Path(@"c:\temp\file.txt"));

            // Act
            string destination = XFS.Path(XFS.Path(@"c:\temp\file.txt"));
            fileInfo.MoveTo(destination);

            Assert.AreEqual(fileInfo.FullName, destination);
            Assert.True(fileInfo.Exists);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void MockFileInfo_MoveTo_SameSourceAndTargetThrowsExceptionIfSourceDoesntExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileInfo = fileSystem.FileInfo.FromFileName(XFS.Path(@"c:\temp\file.txt"));

            // Act
            string destination = XFS.Path(XFS.Path(@"c:\temp\file.txt"));
            fileInfo.MoveTo(destination);

            Assert.AreEqual(fileInfo.FullName, destination);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void MockFileInfo_MoveTo_ThrowsExceptionIfSourceDoesntExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileInfo = fileSystem.FileInfo.FromFileName(XFS.Path(@"c:\temp\file.txt"));

            // Act
            string destination = XFS.Path(XFS.Path(@"c:\temp\file2.txt"));
            fileInfo.MoveTo(destination);

            Assert.AreEqual(fileInfo.FullName, destination);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void MockFileInfo_CopyTo_ThrowsExceptionIfSourceDoesntExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileInfo = fileSystem.FileInfo.FromFileName(XFS.Path(@"c:\temp\file.txt"));

            // Act
            string destination = XFS.Path(XFS.Path(@"c:\temp\file2.txt"));
            fileInfo.CopyTo(destination);

            Assert.AreEqual(fileInfo.FullName, destination);
        }
    }
}