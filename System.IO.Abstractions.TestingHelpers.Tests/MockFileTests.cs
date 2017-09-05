using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockFileTests
    {
        [Test]
        public void MockFile_Constructor_ShouldThrowArgumentNullExceptionIfMockFileDataAccessorIsNull()
        {
            // Arrange
            // nothing to do

            // Act
            TestDelegate action = () => new MockFile(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
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
            Assert.AreEqual(creationTime, result);
        }

        [Test]
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
            Assert.AreEqual(creationTime, result);
        }

        [Test]
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
            Assert.AreEqual(creationTime.ToUniversalTime(), result);
        }

        [Test]
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
            Assert.AreEqual(lastAccessTime, result);
        }

        [Test]
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
            Assert.AreEqual(lastAccessTime, result);
        }

        [Test]
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
            Assert.AreEqual(lastAccessTime.ToUniversalTime(), result);
        }

        [Test]
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
            Assert.AreEqual(lastWriteTime, result);
        }

        static void ExecuteDefaultValueTest(Func<MockFile, string, DateTime> getDateValue)
        {
            var expected = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc);
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var file = new MockFile(fileSystem);

            var actual = getDateValue(file, path);

            Assert.That(actual.ToUniversalTime(), Is.EqualTo(expected));
        }

        [Test]
        public void MockFile_GetLastWriteTimeOfNonExistantFile_ShouldReturnDefaultValue()
        {
            ExecuteDefaultValueTest((f, p) => f.GetLastWriteTime(p));
        }

        [Test]
        public void MockFile_GetLastWriteTimeUtcOfNonExistantFile_ShouldReturnDefaultValue() {
            ExecuteDefaultValueTest((f, p) => f.GetLastWriteTimeUtc(p));
        }

        [Test]
        public void MockFile_GetLastAccessTimeUtcOfNonExistantFile_ShouldReturnDefaultValue() {
            ExecuteDefaultValueTest((f, p) => f.GetLastAccessTimeUtc(p));
        }

        [Test]
        public void MockFile_GetLastAccessTimeOfNonExistantFile_ShouldReturnDefaultValue() {
            ExecuteDefaultValueTest((f, p) => f.GetLastAccessTime(p));
        }

        [Test]
        public void MockFile_GetAttributeOfNonExistantFileButParentDirectoryExists_ShouldThrowOneFileNotFoundException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            // Act
            TestDelegate action = () => fileSystem.Internals.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));

            // Assert
            Assert.Throws<FileNotFoundException>(action);
        }

        [Test]
        public void MockFile_GetAttributeOfNonExistantFile_ShouldThrowOneDirectoryNotFoundException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.Internals.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));

            // Assert
            Assert.Throws<DirectoryNotFoundException>(action);
        }

        [Test]
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

            var attributes = fileSystem.Internals.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));
            Assert.That(attributes, Is.EqualTo(FileAttributes.Hidden));
        }

        [Test]
        public void MockFile_GetAttributeOfExistingUncDirectory_ShouldReturnCorrectValue()
        {
            var filedata = new MockFileData("test");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"\\share\folder\demo.txt"), filedata }
            });

            var attributes = fileSystem.Internals.File.GetAttributes(XFS.Path(@"\\share\folder"));
            Assert.That(attributes, Is.EqualTo(FileAttributes.Directory));
        }

        [Test]
        public void MockFile_GetAttributeWithEmptyParameter_ShouldThrowOneArgumentException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.Internals.File.GetAttributes(string.Empty);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.Message, Is.StringStarting("The path is not of a legal form."));
        }

        [Test]
        public void MockFile_GetAttributeWithIllegalParameter_ShouldThrowOneArgumentException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.Internals.File.GetAttributes(string.Empty);

            // Assert
            // Note: The actual type of the exception differs from the documentation.
            //       According to the documentation it should be of type NotSupportedException.
            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void MockFile_GetCreationTimeOfNonExistantFile_ShouldReturnDefaultValue() {
            ExecuteDefaultValueTest((f, p) => f.GetCreationTime(p));
        }

        [Test]
        public void MockFile_GetCreationTimeUtcOfNonExistantFile_ShouldReturnDefaultValue() {
            ExecuteDefaultValueTest((f, p) => f.GetCreationTimeUtc(p));
        }

        [Test]
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
            Assert.AreEqual(lastWriteTime, result);
        }

        [Test]
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
            Assert.AreEqual(lastWriteTime.ToUniversalTime(), result);
        }

        [Test]
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
            CollectionAssert.AreEqual(
                new byte[] { 0x21, 0x58, 0x3f, 0xa9 },
                result);
        }

        [Test]
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
            Assert.AreEqual(
                "Demo text content",
                result);
        }

        [Test]
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
            Assert.AreEqual(text, result);
        }

        private IEnumerable<Encoding> GetEncodingsForReadAllText()
        {
            // little endian
            yield return new UTF32Encoding(false, true, true);

            // big endian
            yield return new UTF32Encoding(true, true, true);
            yield return new UTF8Encoding(true, true);

            yield return new ASCIIEncoding();
        }

        [TestCaseSource("GetEncodingsForReadAllText")]
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
            var actualText = fileSystem.Internals.File.ReadAllText(path);

            // Assert
            Assert.AreEqual(text, actualText);
        }

        [Test]
        public void MockFile_ReadAllBytes_ShouldReturnDataSavedByWriteAllBytes()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };
            fileSystem.AddDirectory(@"c:\something");

            // Act
            fileSystem.Internals.File.WriteAllBytes(path, fileContent);

            // Assert
            Assert.AreEqual(
                fileContent,
                fileSystem.Internals.File.ReadAllBytes(path));
        }

        [Test]
        public void MockFile_OpenWrite_ShouldCreateNewFiles() {
            string filePath = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "this is some content";
            var fileSystem = new MockFileSystem();

            var bytes = new UTF8Encoding(true).GetBytes(fileContent);
            var stream = fileSystem.Internals.File.OpenWrite(filePath);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            Assert.That(fileSystem.FileExists(filePath), Is.True);
            Assert.That(fileSystem.GetFile(filePath).TextContents, Is.EqualTo(fileContent));
        }

        [Test]
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
            var stream = fileSystem.Internals.File.OpenWrite(filePath);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            Assert.That(fileSystem.FileExists(filePath), Is.True);
            Assert.That(fileSystem.GetFile(filePath).TextContents, Is.EqualTo(endFileContent));
        }

        [Test]
        public void MockFile_Delete_ShouldRemoveFileFromFileSystem()
        {
            string fullPath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { fullPath, new MockFileData("Demo text content") }
            });

            var file = new MockFile(fileSystem);

            file.Delete(fullPath);

            Assert.That(fileSystem.FileExists(fullPath), Is.False);
        }

        [Test]
        public void MockFile_Delete_Should_RemoveFiles()
        {
            string filePath = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "this is some content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { { filePath, new MockFileData(fileContent) } });
            Assert.AreEqual(1, fileSystem.AllFiles.Count());
            fileSystem.Internals.File.Delete(filePath);
            Assert.AreEqual(0, fileSystem.AllFiles.Count());
        }

        [Test]
        public void MockFile_Delete_No_File_Does_Nothing()
        {
            string filePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.Internals.File.Delete(filePath);
        }

        [Test]
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
            stream.Close();

            var file = filesystem.GetFile(filepath);
            Assert.That(file.TextContents, Is.EqualTo("I'm here. Me too!"));
        }

        [Test]
        public void MockFile_AppendText_CreatesNewFileForAppendToNonExistingFile()
        {
            string filepath = XFS.Path(@"c:\something\doesnt\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            var stream = filesystem.File.AppendText(filepath);

            stream.Write("New too!");
            stream.Flush();
            stream.Close();

            var file = filesystem.GetFile(filepath);
            Assert.That(file.TextContents, Is.EqualTo("New too!"));
            Assert.That(filesystem.FileExists(filepath));
        }

        [Test]
        public void Serializable_works()
        {
            //Arrange
            MockFileData data = new MockFileData("Text Contents");

            //Act
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, data);

            //Assert
            Assert.Pass();
        }

        [Test]
        public void Serializable_can_deserialize()
        {
            //Arrange
            string textContentStr = "Text Contents";

            //Act
            MockFileData data = new MockFileData(textContentStr);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, data);

            stream.Seek(0, SeekOrigin.Begin);

            MockFileData deserialized = (MockFileData)formatter.Deserialize(stream);

            //Assert
            Assert.That(deserialized.TextContents, Is.EqualTo(textContentStr));
        }

        [Test]
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
            fileSystem.Internals.File.Encrypt(filePath);

            string newcontents;
            using (var newfile = fileSystem.Internals.File.OpenText(filePath))
            {
                newcontents = newfile.ReadToEnd();
            }

            // Assert
            Assert.AreNotEqual(Content, newcontents);
        }

        [Test]
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
            fileSystem.Internals.File.Decrypt(filePath);

            string newcontents;
            using (var newfile = fileSystem.Internals.File.OpenText(filePath))
            {
                newcontents = newfile.ReadToEnd();
            }

            // Assert
            Assert.AreNotEqual(Content, newcontents);
        }
    }
}
