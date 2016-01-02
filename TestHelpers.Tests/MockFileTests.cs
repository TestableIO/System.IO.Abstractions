using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Xunit;
    using Xunit.Extensions;
    using XFS = MockUnixSupport;

    public class MockFileTests
    {
        [Fact]
        public void MockFile_GetSetCreationTime_ShouldPersist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            var file = new MockFile(fileSystem);

            // Act
            var creationTime = new DateTime(2010, 6, 4, 13, 26, 42);
            file.SetCreationTime(path, creationTime);
            var result = file.GetCreationTime(path);

            // Assert
            Assert.Equal(creationTime, result);
        }

        [Fact]
        public void MockFile_SetCreationTimeUtc_ShouldAffectCreationTime()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            var file = new MockFile(fileSystem);

            // Act
            var creationTime = new DateTime(2010, 6, 4, 13, 26, 42);
            file.SetCreationTimeUtc(path, creationTime.ToUniversalTime());
            var result = file.GetCreationTime(path);

            // Assert
            Assert.Equal(creationTime, result);
        }

        [Fact]
        public void MockFile_SetCreationTime_ShouldAffectCreationTimeUtc()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            var file = new MockFile(fileSystem);

            // Act
            var creationTime = new DateTime(2010, 6, 4, 13, 26, 42);
            file.SetCreationTime(path, creationTime);
            var result = file.GetCreationTimeUtc(path);

            // Assert
            Assert.Equal(creationTime.ToUniversalTime(), result);
        }

        [Fact]
        public void MockFile_GetSetLastAccessTime_ShouldPersist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            var file = new MockFile(fileSystem);

            // Act
            var lastAccessTime = new DateTime(2010, 6, 4, 13, 26, 42);
            file.SetLastAccessTime(path, lastAccessTime);
            var result = file.GetLastAccessTime(path);

            // Assert
            Assert.Equal(lastAccessTime, result);
        }

        [Fact]
        public void MockFile_SetLastAccessTimeUtc_ShouldAffectLastAccessTime()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            var file = new MockFile(fileSystem);

            // Act
            var lastAccessTime = new DateTime(2010, 6, 4, 13, 26, 42);
            file.SetLastAccessTimeUtc(path, lastAccessTime.ToUniversalTime());
            var result = file.GetLastAccessTime(path);

            // Assert
            Assert.Equal(lastAccessTime, result);
        }

        [Fact]
        public void MockFile_SetLastAccessTime_ShouldAffectLastAccessTimeUtc()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            var file = new MockFile(fileSystem);

            // Act
            var lastAccessTime = new DateTime(2010, 6, 4, 13, 26, 42);
            file.SetLastAccessTime(path, lastAccessTime);
            var result = file.GetLastAccessTimeUtc(path);

            // Assert
            Assert.Equal(lastAccessTime.ToUniversalTime(), result);
        }

        [Fact]
        public void MockFile_GetSetLastWriteTime_ShouldPersist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            var file = new MockFile(fileSystem);

            // Act
            var lastWriteTime = new DateTime(2010, 6, 4, 13, 26, 42);
            file.SetLastWriteTime(path, lastWriteTime);
            var result = file.GetLastWriteTime(path);

            // Assert
            Assert.Equal(lastWriteTime, result);
        }

        static void ExecuteDefaultValueTest(Func<MockFile, string, DateTime> getDateValue)
        {
            var expected = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc);
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var file = new MockFile(fileSystem);

            var actual = getDateValue(file, path);

            Assert.Equal(expected, actual.ToUniversalTime());
        }

        [Fact]
        public void MockFile_GetLastWriteTimeOfNonExistantFile_ShouldReturnDefaultValue()
        {
            ExecuteDefaultValueTest((f, p) => f.GetLastWriteTime(p));
        }

        [Fact]
        public void MockFile_GetLastWriteTimeUtcOfNonExistantFile_ShouldReturnDefaultValue() {
            ExecuteDefaultValueTest((f, p) => f.GetLastWriteTimeUtc(p));
        }

        [Fact]
        public void MockFile_GetLastAccessTimeUtcOfNonExistantFile_ShouldReturnDefaultValue() {
            ExecuteDefaultValueTest((f, p) => f.GetLastAccessTimeUtc(p));
        }

        [Fact]
        public void MockFile_GetLastAccessTimeOfNonExistantFile_ShouldReturnDefaultValue() {
            ExecuteDefaultValueTest((f, p) => f.GetLastAccessTime(p));
        }

        [Fact]
        public void MockFile_GetAttributeOfNonExistantFileButParentDirectoryExists_ShouldThrowOneFileNotFoundException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            // Act
            Action action = () => fileSystem.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));

            // Assert
            Assert.Throws<FileNotFoundException>(action);
        }

        [Fact]
        public void MockFile_GetAttributeOfNonExistantFile_ShouldThrowOneDirectoryNotFoundException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action);
        }

        [Fact]
        public void MockFile_GetAttributeOfExistingFile_ShouldReturnCorrectValue()
        {
            var filedata = new MockFileData("test")
            {
                Attributes = FileAttributes.Hidden
            };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"),  filedata }
            });

            var attributes = fileSystem.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));
            Assert.Equal(FileAttributes.Hidden, attributes);
        }

        [Fact]
        public void MockFile_GetAttributeOfExistingUncDirectory_ShouldReturnCorrectValue()
        {
            var filedata = new MockFileData("test");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"\\share\folder\demo.txt"), filedata }
            });

            var attributes = fileSystem.File.GetAttributes(XFS.Path(@"\\share\folder"));
            Assert.Equal(FileAttributes.Directory, attributes);
        }

        [Fact]
        public void MockFile_GetAttributeWithEmptyParameter_ShouldThrowOneArgumentException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.GetAttributes(string.Empty);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.Message, Is.StringStarting("The path is not of a legal form."));
        }

        [Fact]
        public void MockFile_GetAttributeWithIllegalParameter_ShouldThrowOneArgumentException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => fileSystem.File.GetAttributes(string.Empty);

            // Assert
            // Note: The actual type of the exception differs from the documentation.
            //       According to the documentation it should be of type NotSupportedException.
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void MockFile_GetCreationTimeOfNonExistantFile_ShouldReturnDefaultValue() {
            ExecuteDefaultValueTest((f, p) => f.GetCreationTime(p));
        }

        [Fact]
        public void MockFile_GetCreationTimeUtcOfNonExistantFile_ShouldReturnDefaultValue() {
            ExecuteDefaultValueTest((f, p) => f.GetCreationTimeUtc(p));
        }

        [Fact]
        public void MockFile_SetLastWriteTimeUtc_ShouldAffectLastWriteTime()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            var file = new MockFile(fileSystem);

            // Act
            var lastWriteTime = new DateTime(2010, 6, 4, 13, 26, 42);
            file.SetLastWriteTimeUtc(path, lastWriteTime.ToUniversalTime());
            var result = file.GetLastWriteTime(path);

            // Assert
            Assert.Equal(lastWriteTime, result);
        }

        [Fact]
        public void MockFile_SetLastWriteTime_ShouldAffectLastWriteTimeUtc()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });
            var file = new MockFile(fileSystem);

            // Act
            var lastWriteTime = new DateTime(2010, 6, 4, 13, 26, 42);
            file.SetLastWriteTime(path, lastWriteTime);
            var result = file.GetLastWriteTimeUtc(path);

            // Assert
            Assert.Equal(lastWriteTime.ToUniversalTime(), result);
        }

        [Fact]
        public void MockFile_ReadAllBytes_ShouldReturnOriginalByteData()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllBytes(XFS.Path(@"c:\something\other.gif"));

            // Assert
            CollectionAssert.Equal(
                new byte[] { 0x21, 0x58, 0x3f, 0xa9 },
                result);
        }

        [Fact]
        public void MockFile_ReadAllText_ShouldReturnOriginalTextData()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllText(XFS.Path(@"c:\something\demo.txt"));

            // Assert
            Assert.Equal(
                "Demo text content",
                result);
        }

        [Fact]
        public void MockFile_ReadAllText_ShouldReturnOriginalDataWithCustomEncoding()
        {
            // Arrange
            string text = "Hello there!";
            var encodedText = Encoding.BigEndianUnicode.GetBytes(text);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData(encodedText) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllText(XFS.Path(@"c:\something\demo.txt"), Encoding.BigEndianUnicode);

            // Assert
            Assert.Equal(text, result);
        }

        public static IEnumerable<object[]> EncodingsForReadAllText
        {
            get
            {
                // little endian
                yield return new object[] { new UTF32Encoding(false, true, true) };

                // big endian
                yield return new object[] { new UTF32Encoding(true, true, true) };
                yield return new object[] { new UTF8Encoding(true, true) };

                yield return new object[] { new ASCIIEncoding() };
            }
        }

        [Theory]
        [MemberData("EncodingsForReadAllText")]
        public void MockFile_ReadAllText_ShouldReturnTheOriginalContentWhenTheFileContainsDifferentEncodings(Encoding encoding)
        {
            // Arrange
            string text = "Hello there!";
            var encodedText = encoding.GetPreamble().Concat(encoding.GetBytes(text)).ToArray();
            var path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { path, new MockFileData(encodedText) }
                });

            // Act
            var actualText = fileSystem.File.ReadAllText(path);

            // Assert
            Assert.Equal(text, actualText);
        }

        [Fact]
        public void MockFile_ReadAllBytes_ShouldReturnDataSavedByWriteAllBytes()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };

            // Act
            fileSystem.File.WriteAllBytes(path, fileContent);

            // Assert
            Assert.Equal(
                fileContent,
                fileSystem.File.ReadAllBytes(path));
        }

#if DNX451
        [Fact]
        public void MockFile_OpenWrite_ShouldCreateNewFiles() {
            string filePath = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "this is some content";
            var fileSystem = new MockFileSystem();

            var bytes = new UTF8Encoding(true).GetBytes(fileContent);
            var stream = fileSystem.File.OpenWrite(filePath);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            Assert.True(fileSystem.FileExists(filePath));
            Assert.Equal(fileSystem.GetFile(filePath).TextContents, fileContent);
        }

        [Fact]
        public void MockFile_OpenWrite_ShouldOverwriteExistingFiles()
        {
            string filePath = XFS.Path(@"c:\something\demo.txt");
            string startFileContent = "this is some content";
            string endFileContent = "this is some other content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {filePath, new MockFileData(startFileContent)}
            });

            var bytes = new UTF8Encoding(true).GetBytes(endFileContent);
            var stream = fileSystem.File.OpenWrite(filePath);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            Assert.True(fileSystem.FileExists(filePath));
            Assert.Equal(fileSystem.GetFile(filePath).TextContents, endFileContent);
        }
#endif

        [Fact]
        public void MockFile_Delete_ShouldRemoveFileFromFileSystem()
        {
            string fullPath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { fullPath, new MockFileData("Demo text content") }
            });

            var file = new MockFile(fileSystem);

            file.Delete(fullPath);

            Assert.False(fileSystem.FileExists(fullPath));
        }

        [Fact]
        public void MockFile_Delete_Should_RemoveFiles()
        {
            string filePath = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "this is some content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { { filePath, new MockFileData(fileContent) } });
            Assert.Equal(1, fileSystem.AllFiles.Count());
            fileSystem.File.Delete(filePath);
            Assert.Equal(0, fileSystem.AllFiles.Count());
        }

        [Fact]
        public void MockFile_Delete_No_File_Does_Nothing()
        {
            string filePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.File.Delete(filePath);
        }

        [Fact]
        public void MockFile_AppendText_AppendTextToanExistingFile()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here. ") }
            });

            var stream = filesystem.File.AppendText(filepath);

            stream.Write("Me too!");
            stream.Flush();
#if DNX451
            stream.Close();
#endif

            var file = filesystem.GetFile(filepath);
            Assert.Equal("I'm here. Me too!", file.TextContents);
        }

        [Fact]
        public void MockFile_AppendText_CreatesNewFileForAppendToNonExistingFile()
        {
            string filepath = XFS.Path(@"c:\something\doesnt\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            var stream = filesystem.File.AppendText(filepath);

            stream.Write("New too!");
            stream.Flush();
#if DNX451
            stream.Close();
#endif

            var file = filesystem.GetFile(filepath);
            Assert.Equal("New too!", file.TextContents);
            Assert.True(filesystem.FileExists(filepath));
        }

#if DNX451
        [Fact]
        public void Serializable_works()
        {
            //Arrange
            MockFileData data = new MockFileData("Text Contents");

            //Act
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, data);

            //Assert
            Assert.True(true);
        }

        [Fact]
        public void Serializable_can_deserialize()
        {
            //Arrange
            string textContentStr = "Text Contents";

            //Act
            MockFileData data = new MockFileData(textContentStr);

            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, data);

            stream.Seek(0, SeekOrigin.Begin);

            MockFileData deserialized = (MockFileData)formatter.Deserialize(stream);

            //Assert
            Assert.Equal(textContentStr, deserialized.TextContents);
        }

        [Fact]
        public void MockFile_Encrypt_ShouldEncryptTheFile()
        {
            // Arrange
            const string Content = "Demo text content";
            var fileData = new MockFileData(Content);
            var filePath = XFS.Path(@"c:\a.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {filePath, fileData }
            });

            // Act
            fileSystem.File.Encrypt(filePath);

            string newcontents;
            using (var newfile = fileSystem.File.OpenText(filePath))
            {
                newcontents = newfile.ReadToEnd();
            }

            // Assert
            Assert.NotEqual(Content, newcontents);
        }

        [Fact]
        public void MockFile_Decrypt_ShouldDecryptTheFile()
        {
            // Arrange
            const string Content = "Demo text content";
            var fileData = new MockFileData(Content);
            var filePath = XFS.Path(@"c:\a.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {filePath, fileData }
            });

            // Act
            fileSystem.File.Decrypt(filePath);

            string newcontents;
            using (var newfile = fileSystem.File.OpenText(filePath))
            {
                newcontents = newfile.ReadToEnd();
            }

            // Assert
            Assert.NotEqual(Content, newcontents);
        }
#endif
    }
}
