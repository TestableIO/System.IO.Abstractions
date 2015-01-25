using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockFileTests
    {
        [Test]
        public void MockFile_AppendAllText_ShouldPersistNewText()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllText(path, "+ some text");

            // Assert
            Assert.AreEqual(
                "Demo text content+ some text",
                file.ReadAllText(path));
        }

        [Test]
        public void MockFile_AppendAllText_ShouldCreateIfNotExist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            var path2 = XFS.Path(@"c:\something\demo2.txt");
            fileSystem.File.AppendAllText(path2, "some text");
            var path3 = XFS.Path(@"c:\something\demo3.txt");
            fileSystem.File.AppendAllText(path3, "some text", Encoding.Unicode);

            // Assert
            Assert.AreEqual(
                "some text",
                fileSystem.File.ReadAllText(path2));
            Assert.AreEqual(
                "some text",
                fileSystem.File.ReadAllText(path3, Encoding.Unicode));
        }

        [Test]
        public void MockFile_AppendAllText_ShouldFailIfNotExistButDirectoryAlsoNotExist()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            // Act
            path = XFS.Path(@"c:\something2\demo.txt");

            // Assert
            Exception ex;
            ex = Assert.Throws<DirectoryNotFoundException>(() => fileSystem.File.AppendAllText(path, "some text"));
            Assert.That(ex.Message, Is.EqualTo(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path)));

            ex = Assert.Throws<DirectoryNotFoundException>(() => fileSystem.File.AppendAllText(path, "some text", Encoding.Unicode));
            Assert.That(ex.Message, Is.EqualTo(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path)));
        }

        [Test]
        public void MockFile_AppendAllText_ShouldPersistNewTextWithCustomEncoding()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("Demo text content") }
            });

            var file = new MockFile(fileSystem);

            // Act
            file.AppendAllText(path, "+ some text", Encoding.BigEndianUnicode);

            // Assert
            var expected = new byte[]
            {
                68, 101, 109, 111, 32, 116, 101, 120, 116, 32, 99, 111, 110, 116,
                101, 110, 255, 253, 0, 43, 0, 32, 0, 115, 0, 111, 0, 109, 0, 101,
                0, 32, 0, 116, 0, 101, 0, 120, 0, 116
            };

            if (XFS.IsUnixPlatform())
            {
                // Remove EOF on mono
                expected = new byte[]
                {
                    68, 101, 109, 111, 32, 116, 101, 120, 116, 32, 99, 111, 110, 116,
                    101, 110, 0, 43, 0, 32, 0, 115, 0, 111, 0, 109, 0, 101,
                    0, 32, 0, 116, 0, 101, 0, 120, 0, 116
                };
            }

            CollectionAssert.AreEqual(
                expected,
                file.ReadAllBytes(path));
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
        public void MockFile_GetAttributeOfNonExistantFile_ShouldReturnDefaultValue() {
            var fileSystem = new MockFileSystem();
            var attributes = fileSystem.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));
            Assert.That(attributes, Is.EqualTo(FileAttributes.Normal));
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
                { XFS.Path(@"c:\something\demo.txt"),  filedata},
            });

            var attributes = fileSystem.File.GetAttributes(XFS.Path(@"c:\something\demo.txt"));
            Assert.That(attributes, Is.EqualTo(FileAttributes.Hidden));
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
        public void MockFile_Exists_ShouldReturnTrueForSamePath()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.Exists(XFS.Path(@"c:\something\other.gif"));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MockFile_Exists_ShouldReturnTrueForPathVaryingByCase()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.Exists(XFS.Path(@"c:\SomeThing\Other.gif"));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MockFile_Exists_ShouldReturnFalseForEntirelyDifferentPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.Exists(XFS.Path(@"c:\SomeThing\DoesNotExist.gif"));

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MockFile_Exists_ShouldReturnFalseForNullPath()
        {
            var file = new MockFile(new MockFileSystem());

            Assert.That(file.Exists(null), Is.False);
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
        public void MockFile_ReadAllLines_ShouldReturnOriginalTextData()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData("Demo\r\ntext\ncontent\rvalue") },
                { XFS.Path(@"c:\something\other.gif"), new MockFileData(new byte[] { 0x21, 0x58, 0x3f, 0xa9 }) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllLines(XFS.Path(@"c:\something\demo.txt"));

            // Assert
            CollectionAssert.AreEqual(
                new[] { "Demo", "text", "content", "value" },
                result);
        }

        [Test]
        public void MockFile_ReadAllLines_ShouldReturnOriginalDataWithCustomEncoding()
        {
            // Arrange
            string text = "Hello\r\nthere\rBob\nBob!";
            var encodedText = Encoding.BigEndianUnicode.GetBytes(text);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\something\demo.txt"), new MockFileData(encodedText) }
            });

            var file = new MockFile(fileSystem);

            // Act
            var result = file.ReadAllLines(XFS.Path(@"c:\something\demo.txt"), Encoding.BigEndianUnicode);

            // Assert
            CollectionAssert.AreEqual(
                new [] { "Hello", "there", "Bob", "Bob!" },
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
        
        [Test]
        public void MockFile_ReadAllBytes_ShouldReturnDataSavedByWriteAllBytes()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };
            
            // Act
            fileSystem.File.WriteAllBytes(path, fileContent);

            // Assert
            Assert.AreEqual(
                fileContent,
                fileSystem.File.ReadAllBytes(path));
        }

        [Test]
        public void MockFile_WriteAllBytes_ShouldWriteDataToMemoryFileSystem()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };

            // Act
            fileSystem.File.WriteAllBytes(path, fileContent);

            // Assert
            Assert.AreEqual(
                fileContent,
                fileSystem.GetFile(path).Contents);
        }

        [Test]
        public void MockFile_WriteAllText_ShouldWriteTextFileToMemoryFileSystem()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "Hello there!";
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.File.WriteAllText(path, fileContent);

            // Assert
            Assert.AreEqual(
                fileContent,
                fileSystem.GetFile(path).TextContents);
        }

        [Test]
        public void MockFile_WriteAllText_ShouldOverriteAnExistingFile()
        {
            // http://msdn.microsoft.com/en-us/library/ms143375.aspx

            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.File.WriteAllText(path, "foo");
            fileSystem.File.WriteAllText(path, "bar");

            // Assert
            Assert.AreEqual("bar", fileSystem.GetFile(path).TextContents);
        }

        private IEnumerable<Encoding> GetEncodings()
        {
            return new List<Encoding>()
                {
                    Encoding.ASCII,
                    Encoding.BigEndianUnicode,
                    Encoding.Default,
                    Encoding.UTF32,
                    Encoding.UTF7,
                    Encoding.UTF8,
                    Encoding.Unicode
                };
        }

        [TestCaseSource("GetEncodings")]
        public void MockFile_WriteAllText_Encoding_ShouldWriteTextFileToMemoryFileSystem(Encoding encoding)
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "Hello there! Dzięki.";
            var fileSystem = new MockFileSystem();

            // Act
            fileSystem.File.WriteAllText(path, fileContent, encoding);

            // Assert
            Assert.AreEqual(
                encoding.GetString(encoding.GetBytes(fileContent)),
                fileSystem.GetFile(path).TextContents);
        }

        [Test]
        public void MockFile_Move_ShouldMoveFileWithinMemoryFileSystem()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string sourceFileContent = "this is some content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFilePath, new MockFileData(sourceFileContent)},
                {XFS.Path(@"c:\somethingelse\dummy.txt"), new MockFileData(new byte[] {0})}
            });

            string destFilePath = XFS.Path(@"c:\somethingelse\demo1.txt");

            fileSystem.File.Move(sourceFilePath, destFilePath);

            Assert.That(fileSystem.FileExists(destFilePath), Is.True);
            Assert.That(fileSystem.GetFile(destFilePath).TextContents, Is.EqualTo(sourceFileContent));
            Assert.That(fileSystem.FileExists(sourceFilePath), Is.False);
        }

        [Test]
        public void MockFile_Move_ShouldThrowIOExceptionWhenTargetAlreadyExists() 
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string sourceFileContent = "this is some content";
            string destFilePath = XFS.Path(@"c:\somethingelse\demo1.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
              {
                  {sourceFilePath, new MockFileData(sourceFileContent)},
                  {destFilePath, new MockFileData(sourceFileContent)}
              });

            var exception = Assert.Throws<IOException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.That(exception.Message, Is.EqualTo("A file can not be created if it already exists."));
        }

        [Test]
        public void MockFile_Move_ShouldThrowArgumentNullExceptionWhenSourceIsNull_Message() 
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(()=>fileSystem.File.Move(null, destFilePath));

            Assert.That(exception.Message, Is.StringStarting("Value can not be null."));
        }

        [Test]
        public void MockFile_Move_ShouldThrowArgumentNullExceptionWhenSourceIsNull_ParamName() {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Move(null, destFilePath));

            Assert.That(exception.ParamName, Is.EqualTo("sourceFileName"));
        }

        [TestCase('>')]
        [TestCase('<')]
        [TestCase('"')]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenSourceContainsInvalidChars_Message(char invalidChar) 
        {
            if (XFS.IsUnixPlatform())
            {
                Assert.Pass("Path.GetInvalidChars() does not return anything on Mono");
                return;
            }

            var sourceFilePath = XFS.Path(@"c:\something\demo.txt") + invalidChar;
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.That(exception.Message, Is.StringStarting("Illegal characters in path."));
        }

        [TestCase('>')]
        [TestCase('<')]
        [TestCase('"')]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenSourceContainsInvalidChars_ParamName(char invalidChar) {
            if (XFS.IsUnixPlatform())
            {
                Assert.Pass("Path.GetInvalidChars() does not return anything on Mono");
                return;
            }

            var sourceFilePath = XFS.Path(@"c:\something\demo.txt") + invalidChar;
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.That(exception.ParamName, Is.EqualTo("sourceFileName"));
        }

        [TestCase('>')]
        [TestCase('<')]
        [TestCase('"')]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenTargetContainsInvalidChars_Message(char invalidChar) 
        {
            if (XFS.IsUnixPlatform())
            {
                Assert.Pass("Path.GetInvalidChars() does not return anything on Mono");
                return;
            }

            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var destFilePath = XFS.Path(@"c:\something\demo.txt") + invalidChar;
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.That(exception.Message, Is.StringStarting("Illegal characters in path."));
        }

        [TestCase('>')]
        [TestCase('<')]
        [TestCase('"')]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenTargetContainsInvalidChars_ParamName(char invalidChar) {
            if (XFS.IsUnixPlatform())
            {
                Assert.Pass("Path.GetInvalidChars() does not return anything on Mono");
                return;
            }

            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var destFilePath = XFS.Path(@"c:\something\demo.txt") + invalidChar;
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.That(exception.ParamName, Is.EqualTo("destFileName"));
        }

        [Test]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenSourceIsEmpty_Message() 
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(string.Empty, destFilePath));

            Assert.That(exception.Message, Is.StringStarting("An empty file name is invalid."));
        }

        [Test]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenSourceIsEmpty_ParamName() {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(string.Empty, destFilePath));

            Assert.That(exception.ParamName, Is.EqualTo("sourceFileName"));
        }
        [Test]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenSourceIsStringOfBlanks() 
        {
            string sourceFilePath = "   ";
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.That(exception.Message, Is.EqualTo("The path has an invalid format."));
        }

        [Test]
        public void MockFile_Move_ShouldThrowArgumentNullExceptionWhenTargetIsNull_Message() 
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Move(sourceFilePath, null));

            Assert.That(exception.Message, Is.StringStarting("Value can not be null."));
        }

        [Test]
        public void MockFile_Move_ShouldThrowArgumentNullExceptionWhenTargetIsNull_ParamName() {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Move(sourceFilePath, null));

            Assert.That(exception.ParamName, Is.EqualTo("destFileName"));
        }

        [Test]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenTargetIsStringOfBlanks() 
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = "   ";
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.That(exception.Message, Is.EqualTo("The path has an invalid format."));
        }

        [Test]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenTargetIsEmpty_Message() 
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, string.Empty));

            Assert.That(exception.Message, Is.StringStarting("An empty file name is invalid."));
        }

        [Test]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenTargetIsEmpty_ParamName() {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, string.Empty));

            Assert.That(exception.ParamName, Is.EqualTo("destFileName"));
        }
        [Test]
        public void MockFile_Move_ShouldThrowFileNotFoundExceptionWhenSourceDoesNotExist_Message() 
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = XFS.Path(@"c:\something\demo1.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<FileNotFoundException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.That(exception.Message, Is.EqualTo("The file \"" + XFS.Path("c:\\something\\demo.txt") + "\" could not be found."));
        }

        [Test]
        public void MockFile_Move_ShouldThrowFileNotFoundExceptionWhenSourceDoesNotExist_FileName() {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = XFS.Path(@"c:\something\demo1.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<FileNotFoundException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.That(exception.FileName, Is.EqualTo(XFS.Path(@"c:\something\demo.txt")));
        }

        [Test]
        public void MockFile_Move_ShouldThrowDirectoryNotFoundExceptionWhenSourcePathDoesNotExist_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = XFS.Path(@"c:\somethingelse\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
              {
                  {sourceFilePath, new MockFileData(new byte[] {0})}
              });

            //var exists = fileSystem.Directory.Exists(XFS.Path(@"c:\something"));
            //exists = fileSystem.Directory.Exists(XFS.Path(@"c:\something22"));

            var exception = Assert.Throws<DirectoryNotFoundException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));
            //Message = "Could not find a part of the path."
            Assert.That(exception.Message, Is.EqualTo(XFS.Path(@"Could not find a part of the path.")));
        }

        [Test]
        public void MockFile_OpenWrite_ShouldCreateNewFiles() {
            string filePath = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "this is some content";
            var fileSystem = new MockFileSystem();

            var bytes = new UTF8Encoding(true).GetBytes(fileContent);
            var stream = fileSystem.File.OpenWrite(filePath);
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
            var stream = fileSystem.File.OpenWrite(filePath);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            Assert.That(fileSystem.FileExists(filePath), Is.True);
            Assert.That(fileSystem.GetFile(filePath).TextContents, Is.EqualTo(endFileContent));
        }

        [Test]
        public void MockFile_Copy_ShouldOverwriteFileWhenOverwriteFlagIsTrue()
        {
            string sourceFileName = XFS.Path(@"c:\source\demo.txt");
            var sourceContents = new MockFileData("Source content");
            string destFileName = XFS.Path(@"c:\destination\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFileName, sourceContents},
                {destFileName, new MockFileData("Destination content")}
            });

            fileSystem.File.Copy(sourceFileName, destFileName, true);

            var copyResult = fileSystem.GetFile(destFileName);
            Assert.AreEqual(copyResult.Contents, sourceContents.Contents);
        }

        [Test]
        public void MockFile_Copy_ShouldCreateFileAtNewDestination()
        {
            string sourceFileName = XFS.Path(@"c:\source\demo.txt");
            var sourceContents = new MockFileData("Source content");
            string destFileName = XFS.Path(@"c:\source\demo_copy.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFileName, sourceContents}
            });

            fileSystem.File.Copy(sourceFileName, destFileName, false);

            var copyResult = fileSystem.GetFile(destFileName);
            Assert.AreEqual(copyResult.Contents, sourceContents.Contents);
        }

        [Test]
        public void MockFile_Copy_ShouldThrowExceptionWhenFileExistsAtDestination()
        {
            string sourceFileName = XFS.Path(@"c:\source\demo.txt");
            var sourceContents = new MockFileData("Source content");
            string destFileName = XFS.Path(@"c:\destination\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFileName, sourceContents},
                {destFileName, new MockFileData("Destination content")}
            });

            Assert.Throws<IOException>(() => fileSystem.File.Copy(sourceFileName, destFileName), XFS.Path(@"The file c:\destination\demo.txt already exists."));
        }

        [TestCase(@"c:\source\demo.txt", @"c:\source\doesnotexist\demo.txt")]
        [TestCase(@"c:\source\demo.txt", @"c:\doesnotexist\demo.txt")]
        public void MockFile_Copy_ShouldThrowExceptionWhenFolderInDestinationDoesNotExist(string sourceFilePath, string destFilePath)
        {
            string sourceFileName = XFS.Path(sourceFilePath);
            string destFileName = XFS.Path(destFilePath);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFileName, MockFileData.NullObject}
            });

            Assert.Throws<DirectoryNotFoundException>(() => fileSystem.File.Copy(sourceFileName, destFileName), string.Format(CultureInfo.InvariantCulture, @"Could not find a part of the path '{0}'.", destFilePath));
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
        public void Mockfile_Create_ShouldCreateNewStream()
        {
            string fullPath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var sut = new MockFile(fileSystem);

            Assert.That(fileSystem.FileExists(fullPath), Is.False);

            sut.Create(fullPath).Close();

            Assert.That(fileSystem.FileExists(fullPath), Is.True);
        }

        [Test]
        public void Mockfile_Create_CanWriteToNewStream()
        {
            string fullPath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var data = new UTF8Encoding(false).GetBytes("Test string");

            var sut = new MockFile(fileSystem);
            using (var stream = sut.Create(fullPath))
            {
                stream.Write(data, 0, data.Length);
            }

            var mockFileData = fileSystem.GetFile(fullPath);
            var fileData = mockFileData.Contents;

            Assert.That(fileData, Is.EqualTo(data));
        }

        [Test]
        public void Mockfile_Create_OverwritesExistingFile()
        {
            string path = XFS.Path(@"c:\some\file.txt");
            var fileSystem = new MockFileSystem();

            var mockFile = new MockFile(fileSystem);

            // Create a file
            using (var stream = mockFile.Create(path))
            {
                var contents = new UTF8Encoding(false).GetBytes("Test 1");
                stream.Write(contents, 0, contents.Length);
            }

            // Create new file that should overwrite existing file
            var expectedContents = new UTF8Encoding(false).GetBytes("Test 2");
            using (var stream = mockFile.Create(path))
            {
                stream.Write(expectedContents, 0, expectedContents.Length);
            }

            var actualContents = fileSystem.GetFile(path).Contents;

            Assert.That(actualContents, Is.EqualTo(expectedContents));
        }

        [Test]
        public void Mockfile_Create_ThrowsWhenPathIsReadOnly()
        {
            string path = XFS.Path(@"c:\something\read-only.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { { path, new MockFileData("Content") } });
            var mockFile = new MockFile(fileSystem);
            
            mockFile.SetAttributes(path, FileAttributes.ReadOnly);
         
            var exception =  Assert.Throws<UnauthorizedAccessException>(() => mockFile.Create(path).Close());
            Assert.That(exception.Message, Is.EqualTo(string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path)));
        }

        [Test]
        public void MockFile_Delete_Should_RemoveFiles()
        {
            string filePath = XFS.Path(@"c:\something\demo.txt");
            string fileContent = "this is some content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { { filePath, new MockFileData(fileContent) } });
            Assert.AreEqual(1, fileSystem.AllFiles.Count());
            fileSystem.File.Delete(filePath);
            Assert.AreEqual(0, fileSystem.AllFiles.Count());
        }

        [Test]
        public void MockFile_Delete_No_File_Does_Nothing()
        {
            string filePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            fileSystem.File.Delete(filePath);
        }

        [Test]
        public void MockFile_Open_ThrowsOnCreateNewWithExistingFile()
        {
            string filepath = XFS.Path(@"c:\something\already\exists.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData> 
            {
                { filepath, new MockFileData("I'm here") }
            });

            Assert.Throws<IOException>(() => filesystem.File.Open(filepath, FileMode.CreateNew));
        }

        [Test]
        public void MockFile_Open_ThrowsOnOpenWithMissingFile()
        {
            string filepath = XFS.Path(@"c:\something\doesnt\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            Assert.Throws<FileNotFoundException>(() => filesystem.File.Open(filepath, FileMode.Open));
        }

        [Test]
        public void MockFile_Open_ThrowsOnTruncateWithMissingFile()
        {
            string filepath = XFS.Path(@"c:\something\doesnt\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            Assert.Throws<FileNotFoundException>(() => filesystem.File.Open(filepath, FileMode.Truncate));
        }

        [Test]
        public void MockFile_Open_CreatesNewFileFileOnCreate()
        {
            string filepath = XFS.Path(@"c:\something\doesnt\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            var stream = filesystem.File.Open(filepath, FileMode.Create);

            Assert.That(filesystem.File.Exists(filepath), Is.True);
            Assert.That(stream.Position, Is.EqualTo(0));
            Assert.That(stream.Length, Is.EqualTo(0));
        }

        [Test]
        public void MockFile_Open_CreatesNewFileFileOnCreateNew()
        {
            string filepath = XFS.Path(@"c:\something\doesnt\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            var stream = filesystem.File.Open(filepath, FileMode.CreateNew);

            Assert.That(filesystem.File.Exists(filepath), Is.True);
            Assert.That(stream.Position, Is.EqualTo(0));
            Assert.That(stream.Length, Is.EqualTo(0));
        }

        [Test]
        public void MockFile_Open_OpensExistingFileOnAppend()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") }
            });

            var stream = filesystem.File.Open(filepath, FileMode.Append);
            var file = filesystem.GetFile(filepath);
            
            Assert.That(stream.Position, Is.EqualTo(file.Contents.Length));
            Assert.That(stream.Length, Is.EqualTo(file.Contents.Length));

            stream.Seek(0, SeekOrigin.Begin);

            byte[] data;
            using (var br = new BinaryReader(stream))
                data = br.ReadBytes((int)stream.Length);

            CollectionAssert.AreEqual(file.Contents, data);
        }

        [Test]
        public void MockFile_Open_OpensExistingFileOnTruncate()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") }
            });

            var stream = filesystem.File.Open(filepath, FileMode.Truncate);
            var file = filesystem.GetFile(filepath);

            Assert.That(stream.Position, Is.EqualTo(0));
            Assert.That(stream.Length, Is.EqualTo(0));
            Assert.That(file.Contents.Length, Is.EqualTo(0));
        }

        [Test]
        public void MockFile_Open_OpensExistingFileOnOpen()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") }
            });

            var stream = filesystem.File.Open(filepath, FileMode.Open);
            var file = filesystem.GetFile(filepath);

            Assert.That(stream.Position, Is.EqualTo(0));
            Assert.That(stream.Length, Is.EqualTo(file.Contents.Length));

            byte[] data;
            using (var br = new BinaryReader(stream))
                data = br.ReadBytes((int)stream.Length);

            CollectionAssert.AreEqual(file.Contents, data);
        }

        [Test]
        public void MockFile_Open_OpensExistingFileOnOpenOrCreate()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") }
            });

            var stream = filesystem.File.Open(filepath, FileMode.OpenOrCreate);
            var file = filesystem.GetFile(filepath);

            Assert.That(stream.Position, Is.EqualTo(0));
            Assert.That(stream.Length, Is.EqualTo(file.Contents.Length));

            byte[] data;
            using (var br = new BinaryReader(stream))
                data = br.ReadBytes((int)stream.Length);

            CollectionAssert.AreEqual(file.Contents, data);
        }

        [Test]
        public void MockFile_Open_CreatesNewFileOnOpenOrCreate()
        {
            string filepath = XFS.Path(@"c:\something\doesnt\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            var stream = filesystem.File.Open(filepath, FileMode.OpenOrCreate);

            Assert.That(filesystem.File.Exists(filepath), Is.True);
            Assert.That(stream.Position, Is.EqualTo(0));
            Assert.That(stream.Length, Is.EqualTo(0));
        }

        [Test]
        public void MockFile_Open_OverwritesExistingFileOnCreate()
        {
            string filepath = XFS.Path(@"c:\something\doesnt\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") }
            });

            var stream = filesystem.File.Open(filepath, FileMode.Create);
            var file = filesystem.GetFile(filepath);

            Assert.That(stream.Position, Is.EqualTo(0));
            Assert.That(stream.Length, Is.EqualTo(0));
            Assert.That(file.Contents.Length, Is.EqualTo(0));
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
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
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

            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
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
            fileSystem.File.Encrypt(filePath);

            string newcontents;
            using (var newfile = fileSystem.File.OpenText(filePath))
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
            fileSystem.File.Decrypt(filePath);

            string newcontents;
            using (var newfile = fileSystem.File.OpenText(filePath))
            {
                newcontents = newfile.ReadToEnd();
            }

            // Assert
            Assert.AreNotEqual(Content, newcontents);
        }
    }
}
