using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;
    public class MockFileInfoTests
    {
        [Fact]
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
            Assert.True(result);
        }

        [Fact]
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
            Assert.False(result);
        }

        [Fact]
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
            Assert.Equal(fileContent.Length, result);
        }

        [Fact]
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
            Assert.Equal(XFS.Path(@"c:\foo.txt"), ex.FileName);
        }

        [Fact]
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
            Assert.Equal(creationTime.ToUniversalTime(), result);
        }

        [Fact]
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
            Assert.Equal(newUtcTime, fileInfo.CreationTimeUtc);
        }


        [Fact]
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
            Assert.Equal(creationTime, result);
        }

        [Fact]
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
            Assert.Equal(newTime, fileInfo.CreationTime);
        }

        [Fact]
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
            Assert.Equal(FileAttributes.ReadOnly, fileData.Attributes & FileAttributes.ReadOnly);
        }

        [Fact]
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
            Assert.NotEqual(FileAttributes.ReadOnly, fileData.Attributes & FileAttributes.ReadOnly);
        }

        [Fact]
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
            Assert.Equal("Demo text contentThis should be at the end\r\n", newcontents);
        }

        [Fact]
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
            Assert.Equal("ABCDEtext content", newcontents);
        }

#if NET45
        [Fact]
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
            Assert.NotEqual("Demo text content", newcontents);
        }

        [Fact]
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
            Assert.Equal("Demo text content", newcontents);
        }
#endif

        [Fact]
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
            Assert.Equal(lastAccessTime.ToUniversalTime(), result);
        }

        [Fact]
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
            Assert.Equal(newUtcTime, fileInfo.LastAccessTimeUtc);
        }

        [Fact]
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
            Assert.Equal(lastWriteTime.ToUniversalTime(), result);
        }

        [Fact]
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
            Assert.Equal(newUtcTime, fileInfo.LastWriteTime);
        }

        [Fact]
        public void MockFileInfo_GetExtension_ShouldReturnExtension()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            // Act
            var result = fileInfo.Extension;

            // Assert
            Assert.Equal(".txt", result);
        }

        [Fact]
        public void MockFileInfo_GetExtensionWithoutExtension_ShouldReturnEmptyString()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a"));

            // Act
            var result = fileInfo.Extension;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void MockFileInfo_GetDirectoryName_ShouldReturnCompleteDirectoryPath()
        {
            // Arrange
            var fileInfo = new MockFileInfo(new MockFileSystem(), XFS.Path(@"c:\temp\level1\level2\file.txt"));

            // Act
            var result = fileInfo.DirectoryName;

            Assert.Equal(XFS.Path(@"c:\temp\level1\level2"), result);
        }

        [Fact]
        public void MockFileInfo_GetDirectory_ShouldReturnDirectoryInfoWithCorrectPath()
        {
            // Arrange
            var fileInfo = new MockFileInfo(new MockFileSystem(), XFS.Path(@"c:\temp\level1\level2\file.txt"));

            // Act
            var result = fileInfo.Directory;

            Assert.Equal(XFS.Path(@"c:\temp\level1\level2"), result.FullName);
        }

        [Fact]
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

            Assert.Equal(new byte[] { 1, 2 }, result);
        }

        [Fact]
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

            Assert.Equal(@"line 1\r\nline 2", result);
        }

        [Fact]
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

            Assert.Equal(fileInfo.DirectoryName, destinationFolder);
            Assert.Equal(fileInfo.FullName, destination);
        }
    }
}