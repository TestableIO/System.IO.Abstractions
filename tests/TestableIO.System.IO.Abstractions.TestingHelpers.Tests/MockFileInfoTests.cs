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
        public void MockFileInfo_NullPath_ThrowArgumentNullException()
        {
            var fileSystem = new MockFileSystem();

            TestDelegate action = () => new MockFileInfo(fileSystem, null);

            Assert.Throws<ArgumentNullException>(action);

        }

        [Test]
        public void MockFileInfo_Exists_ShouldReturnTrueIfFileExistsInMemoryFileSystem()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData("Demo text content") },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.Exists;

            Assert.IsTrue(result);
        }

        [Test]
        public void MockFileInfo_Exists_ShouldReturnFalseIfFileDoesNotExistInMemoryFileSystem()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData("Demo text content") },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\foo.txt"));

            var result = fileInfo.Exists;

            Assert.IsFalse(result);
        }

        [Test]
        public void MockFileInfo_Exists_ShouldReturnFalseIfPathLeadsToDirectory()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData("Demo text content") },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a\b"));

            var result = fileInfo.Exists;

            Assert.IsFalse(result);
        }

        [Test]
        public void MockFileInfo_Length_ShouldReturnLengthOfFileInMemoryFileSystem()
        {
            const string fileContent = "Demo text content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData(fileContent) },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData(fileContent) },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.Length;

            Assert.That(result, Is.EqualTo(fileContent.Length));
        }

        [Test]
        public void MockFileInfo_Length_ShouldThrowFileNotFoundExceptionIfFileDoesNotExistInMemoryFileSystem()
        {
            const string fileContent = "Demo text content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData(fileContent) },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData(fileContent) },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\foo.txt"));

            var ex = Assert.Throws<FileNotFoundException>(() => fileInfo.Length.ToString(CultureInfo.InvariantCulture));

            Assert.That(ex.FileName, Is.EqualTo(XFS.Path(@"c:\foo.txt")));
        }

        [Test]
        public void MockFileInfo_Length_ShouldThrowFileNotFoundExceptionIfPathLeadsToDirectory()
        {
            const string fileContent = "Demo text content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData(fileContent) },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a\b"));

            var ex = Assert.Throws<FileNotFoundException>(() => fileInfo.Length.ToString(CultureInfo.InvariantCulture));

            Assert.That(ex.FileName, Is.EqualTo(XFS.Path(@"c:\a\b")));
        }

        [Test]
        public void MockFileInfo_CreationTimeUtc_ShouldReturnCreationTimeUtcOfFileInMemoryFileSystem()
        {
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.CreationTimeUtc;

            Assert.That(result, Is.EqualTo(creationTime.ToUniversalTime()));
        }

        [Test]
        public void MockFileInfo_CreationTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            var result = fileInfo.CreationTimeUtc;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime));
        }

        [Test]
        public void MockFileInfo_CreationTimeUtc_ShouldSetCreationTimeUtcOfFileInMemoryFileSystem()
        {
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var newUtcTime = DateTime.UtcNow;
            fileInfo.CreationTimeUtc = newUtcTime;

            Assert.That(fileInfo.CreationTimeUtc, Is.EqualTo(newUtcTime));
        }


        [Test]
        public void MockFileInfo_CreationTime_ShouldReturnCreationTimeOfFileInMemoryFileSystem()
        {
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.CreationTime;

            Assert.That(result, Is.EqualTo(creationTime));
        }

        [Test]
        public void MockFileInfo_CreationTime_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            var result = fileInfo.CreationTime;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.LocalDateTime));
        }

        [Test]
        public void MockFileInfo_CreationTime_ShouldSetCreationTimeOfFileInMemoryFileSystem()
        {
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));
            var newTime = DateTime.Now;

            fileInfo.CreationTime = newTime;

            Assert.That(fileInfo.CreationTime, Is.EqualTo(newTime));
        }

        [Test]
        public void MockFileInfo_Attributes_ShouldReturnMinusOneForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));
            FileAttributes expected = (FileAttributes)(-1);

            Assert.That(fileInfo.Attributes, Is.EqualTo(expected));
        }

        [Test]
        public void MockFileInfo_Attributes_SetterShouldThrowFileNotFoundExceptionOnNonExistingFileOrDirectory()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            Assert.Throws<FileNotFoundException>(() => fileInfo.Attributes = FileAttributes.Hidden);
        }

        [Test]
        public void MockFileInfo_IsReadOnly_ShouldSetReadOnlyAttributeOfFileInMemoryFileSystem()
        {
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            fileInfo.IsReadOnly = true;

            Assert.That(fileData.Attributes & FileAttributes.ReadOnly, Is.EqualTo(FileAttributes.ReadOnly));
        }

        [Test]
        public void MockFileInfo_IsReadOnly_ShouldSetNotReadOnlyAttributeOfFileInMemoryFileSystem()
        {
            var fileData = new MockFileData("Demo text content") { Attributes = FileAttributes.ReadOnly };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            fileInfo.IsReadOnly = false;

            Assert.That(fileData.Attributes & FileAttributes.ReadOnly, Is.Not.EqualTo(FileAttributes.ReadOnly));
        }

        [Test]
        public void MockFileInfo_AppendText_ShouldAddTextToFileInMemoryFileSystem()
        {
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            using (var file = fileInfo.AppendText())
                file.WriteLine("This should be at the end");

            string newcontents;
            using (var newfile = fileInfo.OpenText())
            {
                newcontents = newfile.ReadToEnd();
            }

            Assert.That(newcontents, Is.EqualTo($"Demo text contentThis should be at the end{Environment.NewLine}"));
        }

        [Test]
        public void MockFileInfo_AppendText_ShouldCreateFileIfMissing()
        {
            var fileSystem = new MockFileSystem();
            var targetFile = XFS.Path(@"c:\a.txt");
            var fileInfo = new MockFileInfo(fileSystem, targetFile);

            using (var file = fileInfo.AppendText())
                file.WriteLine("This should be the contents");

            string newcontents;
            using (var newfile = fileInfo.OpenText())
            {
                newcontents = newfile.ReadToEnd();
            }

            Assert.That(fileSystem.File.Exists(targetFile), Is.True);
            Assert.That(newcontents, Is.EqualTo($"This should be the contents{Environment.NewLine}"));
        }

        [Test]
        public void MockFileInfo_OpenWrite_ShouldAddDataToFileInMemoryFileSystem()
        {
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));
            var bytesToAdd = new byte[] { 65, 66, 67, 68, 69 };


            using (var file = fileInfo.OpenWrite())
            {
                file.Write(bytesToAdd, 0, bytesToAdd.Length);
            }

            string newcontents;
            using (var newfile = fileInfo.OpenText())
            {
                newcontents = newfile.ReadToEnd();
            }

            Assert.That(newcontents, Is.EqualTo("ABCDEtext content"));
        }

        [Test]
        public void MockFileInfo_Encrypt_ShouldSetEncryptedAttributeOfFileInMemoryFileSystem()
        {
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            fileInfo.Encrypt();

            Assert.That(fileData.Attributes & FileAttributes.Encrypted, Is.EqualTo(FileAttributes.Encrypted));
        }

        [Test]
        public void MockFileInfo_Decrypt_ShouldUnsetEncryptedAttributeOfFileInMemoryFileSystem()
        {
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));
            fileInfo.Encrypt();

            fileInfo.Decrypt();

            Assert.That(fileData.Attributes & FileAttributes.Encrypted, Is.Not.EqualTo(FileAttributes.Encrypted));
        }

        [Test]
        public void MockFileInfo_LastAccessTimeUtc_ShouldReturnLastAccessTimeUtcOfFileInMemoryFileSystem()
        {
            var lastAccessTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastAccessTime = lastAccessTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.LastAccessTimeUtc;

            Assert.That(result, Is.EqualTo(lastAccessTime.ToUniversalTime()));
        }

        [Test]
        public void MockFileInfo_LastAccessTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            var result = fileInfo.LastAccessTimeUtc;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime));
        }

        [Test]
        public void MockFileInfo_LastAccessTimeUtc_ShouldSetCreationTimeUtcOfFileInMemoryFileSystem()
        {
            var lastAccessTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastAccessTime = lastAccessTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var newUtcTime = DateTime.UtcNow;
            fileInfo.LastAccessTimeUtc = newUtcTime;

            Assert.That(fileInfo.LastAccessTimeUtc, Is.EqualTo(newUtcTime));
        }

        [Test]
        public void MockFileInfo_LastWriteTime_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            var result = fileInfo.LastWriteTime;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.LocalDateTime));
        }

        [Test]
        public void MockFileInfo_LastWriteTimeUtc_ShouldReturnLastWriteTimeUtcOfFileInMemoryFileSystem()
        {
            var lastWriteTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastWriteTime = lastWriteTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.LastWriteTimeUtc;

            Assert.That(result, Is.EqualTo(lastWriteTime.ToUniversalTime()));
        }

        [Test]
        public void MockFileInfo_LastWriteTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            var result = fileInfo.LastWriteTimeUtc;

            Assert.That(result, Is.EqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime));
        }

        [Test]
        public void MockFileInfo_LastWriteTimeUtc_ShouldSetLastWriteTimeUtcOfFileInMemoryFileSystem()
        {
            var lastWriteTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastWriteTime = lastWriteTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var newUtcTime = DateTime.UtcNow;
            fileInfo.LastWriteTimeUtc = newUtcTime;

            Assert.That(fileInfo.LastWriteTimeUtc, Is.EqualTo(newUtcTime));
        }

        [Test]
        public void MockFileInfo_GetExtension_ShouldReturnExtension()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.Extension;

            Assert.That(result, Is.EqualTo(".txt"));
        }

        [Test]
        public void MockFileInfo_GetExtensionWithoutExtension_ShouldReturnEmptyString()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a"));

            var result = fileInfo.Extension;

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void MockFileInfo_GetDirectoryName_ShouldReturnCompleteDirectoryPath()
        {
            var fileInfo = new MockFileInfo(new MockFileSystem(), XFS.Path(@"c:\temp\level1\level2\file.txt"));

            var result = fileInfo.DirectoryName;

            Assert.That(result, Is.EqualTo(XFS.Path(@"c:\temp\level1\level2")));
        }

        [Test]
        public void MockFileInfo_GetDirectory_ShouldReturnDirectoryInfoWithCorrectPath()
        {
            var fileInfo = new MockFileInfo(new MockFileSystem(), XFS.Path(@"c:\temp\level1\level2\file.txt"));

            var result = fileInfo.Directory;

            Assert.That(result.FullName, Is.EqualTo(XFS.Path(@"c:\temp\level1\level2")));
        }

        [Test]
        public void MockFileInfo_OpenRead_ShouldReturnByteContentOfFile()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\temp\file.txt"), new MockFileData(new byte[] { 1, 2 }));
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));

            byte[] result = new byte[2];
            using (var stream = fileInfo.OpenRead())
            {
                stream.Read(result, 0, 2);
            }

            Assert.That(result, Is.EqualTo(new byte[] { 1, 2 }));
        }

        [Test]
        public void MockFileInfo_OpenText_ShouldReturnStringContentOfFile()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\temp\file.txt"), new MockFileData(@"line 1\r\nline 2"));
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));

            string result;
            using (var streamReader = fileInfo.OpenText())
            {
                result = streamReader.ReadToEnd();
            }

            Assert.That(result, Is.EqualTo(@"line 1\r\nline 2"));
        }

        [Test]
        public void MockFileInfo_MoveTo_NonExistentDestination_ShouldUpdateFileInfoDirectoryAndFullName()
        {
            var fileSystem = new MockFileSystem();
            var sourcePath = XFS.Path(@"c:\temp\file.txt");
            var destinationFolder = XFS.Path(@"c:\temp2");
            var destinationPath = XFS.Path(destinationFolder + @"\file.txt");
            fileSystem.AddFile(sourcePath, new MockFileData("1"));
            var fileInfo = fileSystem.FileInfo.New(sourcePath);
            fileSystem.AddDirectory(destinationFolder);

            fileInfo.MoveTo(destinationPath);

            Assert.That(fileInfo.DirectoryName, Is.EqualTo(destinationFolder));
            Assert.That(fileInfo.FullName, Is.EqualTo(destinationPath));
        }

        [Test]
        public void MockFileInfo_MoveTo_NonExistentDestinationFolder_ShouldThrowDirectoryNotFoundException()
        {
            var fileSystem = new MockFileSystem();
            var sourcePath = XFS.Path(@"c:\temp\file.txt");
            var destinationPath = XFS.Path(@"c:\temp2\file.txt");
            fileSystem.AddFile(sourcePath, new MockFileData("1"));
            var fileInfo = fileSystem.FileInfo.New(sourcePath);

            Assert.Throws<DirectoryNotFoundException>(() => fileInfo.MoveTo(destinationPath));
        }

        [Test]
        public void MockFileInfo_MoveTo_ExistingDestination_ShouldThrowExceptionAboutFileAlreadyExisting()
        {
            var fileSystem = new MockFileSystem();
            var sourcePath = XFS.Path(@"c:\temp\file.txt");
            var destinationPath = XFS.Path(@"c:\temp2\file.txt");
            fileSystem.AddFile(sourcePath, new MockFileData("1"));
            var fileInfo = fileSystem.FileInfo.New(sourcePath);
            fileSystem.AddFile(destinationPath, new MockFileData("2"));

            Assert.Throws<IOException>(() => fileInfo.MoveTo(destinationPath));
        }

        [Test]
        public void MockFileInfo_MoveTo_SameSourceAndTargetIsANoOp()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\temp\file.txt"), new MockFileData(@"line 1\r\nline 2"));
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));
            string destination = XFS.Path(XFS.Path(@"c:\temp\file.txt"));

            fileInfo.MoveTo(destination);

            Assert.That(fileInfo.FullName, Is.EqualTo(destination));
            Assert.True(fileInfo.Exists);
        }

        [Test]
        public void MockFileInfo_MoveTo_SameSourceAndTargetThrowsExceptionIfSourceDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));
            string destination = XFS.Path(XFS.Path(@"c:\temp\file.txt"));

            TestDelegate action = () => fileInfo.MoveTo(destination);

            Assert.Throws<FileNotFoundException>(action);
        }

        [Test]
        public void MockFileInfo_MoveTo_ThrowsExceptionIfSourceDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));
            string destination = XFS.Path(XFS.Path(@"c:\temp\file2.txt"));

            TestDelegate action = () => fileInfo.MoveTo(destination);

            Assert.Throws<FileNotFoundException>(action);
        }



#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        [Test]
        public void MockFileInfo_MoveToWithOverwrite_ShouldSucceedWhenTargetAlreadyExists()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string sourceFileContent = "this is some content";
            string destFilePath = XFS.Path(@"c:\somethingelse\demo1.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFilePath, new MockFileData(sourceFileContent)},
                {destFilePath, new MockFileData(sourceFileContent)}
            });

            fileSystem.FileInfo.New(sourceFilePath).MoveTo(destFilePath, overwrite: true);

            Assert.That(fileSystem.File.ReadAllText(destFilePath), Is.EqualTo(sourceFileContent));
        }
#endif

        [Test]
        public void MockFileInfo_MoveToOnlyCaseChanging_ShouldSucceed()
        {
            string sourceFilePath = XFS.Path(@"c:\temp\file.txt");
            string destFilePath = XFS.Path(@"c:\temp\FILE.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFilePath, new MockFileData("1")},
            });

            var fileInfo = fileSystem.FileInfo.New(sourceFilePath);
            fileInfo.MoveTo(destFilePath);

            Assert.That(fileInfo.FullName, Is.EqualTo(destFilePath));
            Assert.True(fileInfo.Exists);
        }

        [Test]
        public void MockFileInfo_CopyTo_ThrowsExceptionIfSourceDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));
            string destination = XFS.Path(XFS.Path(@"c:\temp\file2.txt"));

            TestDelegate action = () => fileInfo.CopyTo(destination);

            Assert.Throws<FileNotFoundException>(action);
        }

        [TestCase(@"..\..\..\c.txt")]
        [TestCase(@"c:\a\b\c.txt")]
        [TestCase(@"c:\a\c.txt")]
        [TestCase(@"c:\c.txt")]
        public void MockFileInfo_ToString_ShouldReturnOriginalFilePath(string path)
        {
            //Arrange
            var filePath = XFS.Path(path);

            //Act
            var mockFileInfo = new MockFileInfo(new MockFileSystem(), filePath);

            //Assert
            Assert.That(mockFileInfo.ToString(), Is.EqualTo(filePath));
        }


        /// <summary>
        /// Normalize, tested with Path.GetFullPath and new FileInfo().FullName;
        /// </summary>
        [TestCaseSource(nameof(New_Paths_NormalizePaths_Cases))]
        public void New_Paths_NormalizePaths(string input, string expected)
        {
            // Arrange
            var mockFs = new MockFileSystem();

            // Act
            var mockFileInfo = mockFs.FileInfo.New(input);
            var result = mockFileInfo.FullName;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        public static IEnumerable<string[]> New_Paths_NormalizePaths_Cases
        {
            get
            {
                yield return new[] { XFS.Path(@"c:\top\..\most\file"), XFS.Path(@"c:\most\file") };
                yield return new[] { XFS.Path(@"c:\top\..\most\..\dir\file"), XFS.Path(@"c:\dir\file") };
                yield return new[] { XFS.Path(@"\file"), XFS.Path(@"C:\file") };
                yield return new[] { XFS.Path(@"c:\top\../..\most\file"), XFS.Path(@"c:\most\file") };
            }
        }

        [Test]
        public void MockFileInfo_Replace_ShouldReplaceFileContents()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var path2 = XFS.Path(@"c:\temp\file2.txt");
            fileSystem.AddFile(path1, new MockFileData("1"));
            fileSystem.AddFile(path2, new MockFileData("2"));
            var fileInfo1 = fileSystem.FileInfo.New(path1);
            var fileInfo2 = fileSystem.FileInfo.New(path2);

            // Act
            fileInfo1.Replace(path2, null);

            Assert.That(fileInfo2.OpenText().ReadToEnd(), Is.EqualTo("1"));
        }

        [Test]
        public void MockFileInfo_Replace_ShouldCreateBackup()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var path2 = XFS.Path(@"c:\temp\file2.txt");
            var path3 = XFS.Path(@"c:\temp\file3.txt");
            fileSystem.AddFile(path1, new MockFileData("1"));
            fileSystem.AddFile(path2, new MockFileData("2"));
            var fileInfo1 = fileSystem.FileInfo.New(path1);
            var fileInfo3 = fileSystem.FileInfo.New(path3);

            // Act
            fileInfo1.Replace(path2, path3);

            Assert.That(fileInfo3.OpenText().ReadToEnd(), Is.EqualTo("2"));
        }

        [Test]
        public void MockFileInfo_Replace_ShouldThrowIfDirectoryOfBackupPathDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var path2 = XFS.Path(@"c:\temp\file2.txt");
            var path3 = XFS.Path(@"c:\temp\subdirectory\file3.txt");
            fileSystem.AddFile(path1, new MockFileData("1"));
            fileSystem.AddFile(path2, new MockFileData("2"));
            var fileInfo1 = fileSystem.FileInfo.New(path1);

            // Act
            Assert.Throws<DirectoryNotFoundException>(() => fileInfo1.Replace(path2, path3));
        }

        [Test]
        public void MockFileInfo_Replace_ShouldReturnDestinationFileInfo()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var path2 = XFS.Path(@"c:\temp\file2.txt");
            fileSystem.AddFile(path1, new MockFileData("1"));
            fileSystem.AddFile(path2, new MockFileData("2"));
            var fileInfo1 = fileSystem.FileInfo.New(path1);
            var fileInfo2 = fileSystem.FileInfo.New(path2);

            // Act
            var result = fileInfo1.Replace(path2, null);

            Assert.That(result.FullName, Is.EqualTo(fileInfo2.FullName));
        }

        [Test]
        public void MockFileInfo_Replace_ShouldThrowIfSourceFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var path2 = XFS.Path(@"c:\temp\file2.txt");
            fileSystem.AddFile(path2, new MockFileData("1"));
            var fileInfo = fileSystem.FileInfo.New(path1);

            Assert.Throws<FileNotFoundException>(() => fileInfo.Replace(path2, null));
        }

        [Test]
        public void MockFileInfo_Replace_ShouldThrowIfDestinationFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var path2 = XFS.Path(@"c:\temp\file2.txt");
            fileSystem.AddFile(path1, new MockFileData("1"));
            var fileInfo = fileSystem.FileInfo.New(path1);

            Assert.Throws<FileNotFoundException>(() => fileInfo.Replace(path2, null));
        }

        [Test]
        public void MockFileInfo_Exists_ShouldReturnCachedData()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var fileInfo = fileSystem.FileInfo.New(path1);

            // Act
            fileSystem.AddFile(path1, new MockFileData("1"));

            // Assert
            Assert.IsFalse(fileInfo.Exists);
        }

        [Test]
        public void MockFileInfo_Exists_ShouldUpdateCachedDataOnRefresh()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var fileInfo = fileSystem.FileInfo.New(path1);

            // Act
            fileSystem.AddFile(path1, new MockFileData("1"));
            fileInfo.Refresh();

            // Assert
            Assert.IsTrue(fileInfo.Exists);
        }

        [Test]
        public void MockFileInfo_Create_ShouldUpdateCachedDataAndReturnTrueForExists()
        {
            IFileSystem fileSystem = new MockFileSystem();
            var path = XFS.Path(@"c:\temp\file1.txt");
            IFileInfo fileInfo = fileSystem.FileInfo.New(path);

            // Act
            fileInfo.Create().Dispose();

            // Assert
            var result = fileInfo.Exists;
            Assert.IsTrue(result);
        }

        [Test]
        public void MockFileInfo_CreateText_ShouldUpdateCachedDataAndReturnTrueForExists()
        {
            IFileSystem fileSystem = new MockFileSystem();
            var path = XFS.Path(@"c:\temp\file1.txt");
            IFileInfo fileInfo = fileSystem.FileInfo.New(path);

            // Act
            fileInfo.CreateText().Dispose();

            // Assert
            Assert.IsTrue(fileInfo.Exists);
        }

        [Test]
        public void MockFileInfo_Delete_ShouldUpdateCachedDataAndReturnFalseForExists()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path(@"c:\temp\file1.txt");
            IFileInfo fileInfo = fileSystem.FileInfo.New(path);

            // Act
            fileInfo.Delete();

            // Assert
            Assert.IsFalse(fileInfo.Exists);
        }

        [Test]
        public void MockFileInfo_Delete_ShouldThrowIfFileAccessShareHasNoWriteOrDeleteAccess()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(
                    @"c:\bar\foo.txt",
                    new MockFileData("text contents") { AllowedFileShare = FileShare.None });

            var fi = fileSystem.FileInfo.New(@"c:\bar\foo.txt");

            Assert.Throws(typeof(System.IO.IOException), () => fi.Delete());
        }

        [Test]
        public void MockFileInfo_LastAccessTimeUtcWithUnspecifiedDateTimeKind_ShouldSetLastAccessTimeUtcOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                LastAccessTimeUtc = date
            };

            Assert.That(fileInfo.LastAccessTimeUtc, Is.EqualTo(date));
            Assert.That(fileInfo.LastAccessTimeUtc.Kind, Is.Not.EqualTo(DateTimeKind.Unspecified));
        }

        [Test]
        public void MockFileInfo_LastAccessTimeWithUnspecifiedDateTimeKind_ShouldSetLastAccessTimeOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                LastAccessTime = date
            };

            Assert.That(fileInfo.LastAccessTime, Is.EqualTo(date));
            Assert.That(fileInfo.LastAccessTime.Kind, Is.Not.EqualTo(DateTimeKind.Unspecified));
        }

        [Test]
        public void MockFileInfo_CreationTimeUtcWithUnspecifiedDateTimeKind_ShouldSetCreationTimeUtcOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                CreationTimeUtc = date
            };

            Assert.That(fileInfo.CreationTimeUtc, Is.EqualTo(date));
            Assert.That(fileInfo.CreationTimeUtc.Kind, Is.Not.EqualTo(DateTimeKind.Unspecified));
        }

        [Test]
        public void MockFileInfo_CreationTimeWithUnspecifiedDateTimeKind_ShouldSetCreationTimeOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                CreationTime = date
            };

            Assert.That(fileInfo.CreationTime, Is.EqualTo(date));
            Assert.That(fileInfo.CreationTime.Kind, Is.Not.EqualTo(DateTimeKind.Unspecified));
        }

        [Test]
        public void MockFileInfo_LastWriteTimeUtcWithUnspecifiedDateTimeKind_ShouldSetLastWriteTimeUtcOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                LastWriteTimeUtc = date
            };

            Assert.That(fileInfo.LastWriteTimeUtc, Is.EqualTo(date));
            Assert.That(fileInfo.LastWriteTimeUtc.Kind, Is.Not.EqualTo(DateTimeKind.Unspecified));
        }

        [Test]
        public void MockFileInfo_LastWriteTimeWithUnspecifiedDateTimeKind_ShouldSetLastWriteTimeOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                LastWriteTime = date
            };

            Assert.That(fileInfo.LastWriteTime, Is.EqualTo(date));
            Assert.That(fileInfo.LastWriteTime.Kind, Is.Not.EqualTo(DateTimeKind.Unspecified));
        }
    }
}
