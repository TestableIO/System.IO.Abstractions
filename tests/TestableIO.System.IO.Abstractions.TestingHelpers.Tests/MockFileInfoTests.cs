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
        public async Task MockFileInfo_NullPath_ThrowArgumentNullException()
        {
            var fileSystem = new MockFileSystem();

            Action action = () => new MockFileInfo(fileSystem, null);

            await That(action).Throws<ArgumentNullException>();

        }

        [Test]
        public async Task MockFileInfo_Exists_ShouldReturnTrueIfFileExistsInMemoryFileSystem()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData("Demo text content") },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.Exists;

            await That(result).IsTrue();
        }

        [Test]
        public async Task MockFileInfo_Exists_ShouldReturnFalseIfFileDoesNotExistInMemoryFileSystem()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData("Demo text content") },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\foo.txt"));

            var result = fileInfo.Exists;

            await That(result).IsFalse();
        }

        [Test]
        public async Task MockFileInfo_Exists_ShouldReturnFalseIfPathLeadsToDirectory()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData("Demo text content") },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a\b"));

            var result = fileInfo.Exists;

            await That(result).IsFalse();
        }

        [Test]
        public async Task MockFileInfo_Length_ShouldReturnLengthOfFileInMemoryFileSystem()
        {
            const string fileContent = "Demo text content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData(fileContent) },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData(fileContent) },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.Length;

            await That(result).IsEqualTo(fileContent.Length);
        }

        [Test]
        public async Task MockFileInfo_Length_ShouldThrowFileNotFoundExceptionIfFileDoesNotExistInMemoryFileSystem()
        {
            const string fileContent = "Demo text content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), new MockFileData(fileContent) },
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData(fileContent) },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\foo.txt"));

            var ex = await That(() => fileInfo.Length.ToString(CultureInfo.InvariantCulture)).Throws<FileNotFoundException>();

            await That(ex.FileName).IsEqualTo(XFS.Path(@"c:\foo.txt"));
        }

        [Test]
        public async Task MockFileInfo_Length_ShouldThrowFileNotFoundExceptionIfPathLeadsToDirectory()
        {
            const string fileContent = "Demo text content";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a\b\c.txt"), new MockFileData(fileContent) },
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a\b"));

            var ex = await That(() => fileInfo.Length.ToString(CultureInfo.InvariantCulture)).Throws<FileNotFoundException>();

            await That(ex.FileName).IsEqualTo(XFS.Path(@"c:\a\b"));
        }

        [Test]
        public async Task MockFileInfo_CreationTimeUtc_ShouldReturnCreationTimeUtcOfFileInMemoryFileSystem()
        {
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.CreationTimeUtc;

            await That(result).IsEqualTo(creationTime.ToUniversalTime());
        }

        [Test]
        public async Task MockFileInfo_CreationTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            var result = fileInfo.CreationTimeUtc;

            await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime);
        }

        [Test]
        public async Task MockFileInfo_CreationTimeUtc_ShouldSetCreationTimeUtcOfFileInMemoryFileSystem()
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

            await That(fileInfo.CreationTimeUtc).IsEqualTo(newUtcTime);
        }


        [Test]
        public async Task MockFileInfo_CreationTime_ShouldReturnCreationTimeOfFileInMemoryFileSystem()
        {
            var creationTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { CreationTime = creationTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.CreationTime;

            await That(result).IsEqualTo(creationTime);
        }

        [Test]
        public async Task MockFileInfo_CreationTime_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            var result = fileInfo.CreationTime;

            await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.LocalDateTime);
        }

        [Test]
        public async Task MockFileInfo_CreationTime_ShouldSetCreationTimeOfFileInMemoryFileSystem()
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

            await That(fileInfo.CreationTime).IsEqualTo(newTime);
        }

        [Test]
        public async Task MockFileInfo_Attributes_ShouldReturnMinusOneForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));
            FileAttributes expected = (FileAttributes)(-1);

            await That(fileInfo.Attributes).IsEqualTo(expected);
        }

        [Test]
        public async Task MockFileInfo_Attributes_SetterShouldThrowFileNotFoundExceptionOnNonExistingFileOrDirectory()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            await That(() => fileInfo.Attributes = FileAttributes.Hidden).Throws<FileNotFoundException>();
        }

        [Test]
        public async Task MockFileInfo_IsReadOnly_ShouldSetReadOnlyAttributeOfFileInMemoryFileSystem()
        {
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            fileInfo.IsReadOnly = true;

            await That(fileData.Attributes & FileAttributes.ReadOnly).IsEqualTo(FileAttributes.ReadOnly);
        }

        [Test]
        public async Task MockFileInfo_IsReadOnly_ShouldSetNotReadOnlyAttributeOfFileInMemoryFileSystem()
        {
            var fileData = new MockFileData("Demo text content") { Attributes = FileAttributes.ReadOnly };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            fileInfo.IsReadOnly = false;

            await That(fileData.Attributes & FileAttributes.ReadOnly).IsNotEqualTo(FileAttributes.ReadOnly);
        }

        [Test]
        public async Task MockFileInfo_AppendText_ShouldAddTextToFileInMemoryFileSystem()
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

            await That(newcontents).IsEqualTo($"Demo text contentThis should be at the end{Environment.NewLine}");
        }

        [Test]
        public async Task MockFileInfo_AppendText_ShouldCreateFileIfMissing()
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

            await That(fileSystem.File.Exists(targetFile)).IsTrue();
            await That(newcontents).IsEqualTo($"This should be the contents{Environment.NewLine}");
        }

        [Test]
        public async Task MockFileInfo_OpenWrite_ShouldAddDataToFileInMemoryFileSystem()
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

            await That(newcontents).IsEqualTo("ABCDEtext content");
        }

        [Test]
        public async Task MockFileInfo_Encrypt_ShouldSetEncryptedAttributeOfFileInMemoryFileSystem()
        {
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            fileInfo.Encrypt();

            await That(fileData.Attributes & FileAttributes.Encrypted).IsEqualTo(FileAttributes.Encrypted);
        }

        [Test]
        public async Task MockFileInfo_Decrypt_ShouldUnsetEncryptedAttributeOfFileInMemoryFileSystem()
        {
            var fileData = new MockFileData("Demo text content");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));
            fileInfo.Encrypt();

            fileInfo.Decrypt();

            await That(fileData.Attributes & FileAttributes.Encrypted).IsNotEqualTo(FileAttributes.Encrypted);
        }

        [Test]
        public async Task MockFileInfo_LastAccessTimeUtc_ShouldReturnLastAccessTimeUtcOfFileInMemoryFileSystem()
        {
            var lastAccessTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastAccessTime = lastAccessTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.LastAccessTimeUtc;

            await That(result).IsEqualTo(lastAccessTime.ToUniversalTime());
        }

        [Test]
        public async Task MockFileInfo_LastAccessTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            var result = fileInfo.LastAccessTimeUtc;

            await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime);
        }

        [Test]
        public async Task MockFileInfo_LastAccessTimeUtc_ShouldSetCreationTimeUtcOfFileInMemoryFileSystem()
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

            await That(fileInfo.LastAccessTimeUtc).IsEqualTo(newUtcTime);
        }

        [Test]
        public async Task MockFileInfo_LastWriteTime_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            var result = fileInfo.LastWriteTime;

            await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.LocalDateTime);
        }

        [Test]
        public async Task MockFileInfo_LastWriteTimeUtc_ShouldReturnLastWriteTimeUtcOfFileInMemoryFileSystem()
        {
            var lastWriteTime = DateTime.Now.AddHours(-4);
            var fileData = new MockFileData("Demo text content") { LastWriteTime = lastWriteTime };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"c:\a.txt"), fileData }
            });
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.LastWriteTimeUtc;

            await That(result).IsEqualTo(lastWriteTime.ToUniversalTime());
        }

        [Test]
        public async Task MockFileInfo_LastWriteTimeUtc_ShouldReturnDefaultTimeForNonExistingFile()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\non\existing\file.txt"));

            var result = fileInfo.LastWriteTimeUtc;

            await That(result).IsEqualTo(MockFileData.DefaultDateTimeOffset.UtcDateTime);
        }

        [Test]
        public async Task MockFileInfo_LastWriteTimeUtc_ShouldSetLastWriteTimeUtcOfFileInMemoryFileSystem()
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

            await That(fileInfo.LastWriteTimeUtc).IsEqualTo(newUtcTime);
        }

        [Test]
        public async Task MockFileInfo_GetExtension_ShouldReturnExtension()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a.txt"));

            var result = fileInfo.Extension;

            await That(result).IsEqualTo(".txt");
        }

        [Test]
        public async Task MockFileInfo_GetExtensionWithoutExtension_ShouldReturnEmptyString()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var fileInfo = new MockFileInfo(fileSystem, XFS.Path(@"c:\a"));

            var result = fileInfo.Extension;

            await That(result).IsEmpty();
        }

        [Test]
        public async Task MockFileInfo_GetDirectoryName_ShouldReturnCompleteDirectoryPath()
        {
            var fileInfo = new MockFileInfo(new MockFileSystem(), XFS.Path(@"c:\temp\level1\level2\file.txt"));

            var result = fileInfo.DirectoryName;

            await That(result).IsEqualTo(XFS.Path(@"c:\temp\level1\level2"));
        }

        [Test]
        public async Task MockFileInfo_GetDirectory_ShouldReturnDirectoryInfoWithCorrectPath()
        {
            var fileInfo = new MockFileInfo(new MockFileSystem(), XFS.Path(@"c:\temp\level1\level2\file.txt"));

            var result = fileInfo.Directory;

            await That(result.FullName).IsEqualTo(XFS.Path(@"c:\temp\level1\level2"));
        }

        [Test]
        public async Task MockFileInfo_OpenRead_ShouldReturnByteContentOfFile()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\temp\file.txt"), new MockFileData(new byte[] { 1, 2 }));
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));

            byte[] result = new byte[2];
            using (var stream = fileInfo.OpenRead())
            {
#pragma warning disable CA2022
                // ReSharper disable once MustUseReturnValue
                stream.Read(result, 0, 2);
#pragma warning restore CA2022
            }

            await That(result).IsEqualTo(new byte[] { 1, 2 });
        }

        [Test]
        public async Task MockFileInfo_OpenText_ShouldReturnStringContentOfFile()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\temp\file.txt"), new MockFileData(@"line 1\r\nline 2"));
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));

            string result;
            using (var streamReader = fileInfo.OpenText())
            {
                result = streamReader.ReadToEnd();
            }

            await That(result).IsEqualTo(@"line 1\r\nline 2");
        }

        [Test]
        public async Task MockFileInfo_MoveTo_NonExistentDestination_ShouldUpdateFileInfoDirectoryAndFullName()
        {
            var fileSystem = new MockFileSystem();
            var sourcePath = XFS.Path(@"c:\temp\file.txt");
            var destinationFolder = XFS.Path(@"c:\temp2");
            var destinationPath = XFS.Path(destinationFolder + @"\file.txt");
            fileSystem.AddFile(sourcePath, new MockFileData("1"));
            var fileInfo = fileSystem.FileInfo.New(sourcePath);
            fileSystem.AddDirectory(destinationFolder);

            fileInfo.MoveTo(destinationPath);

            await That(fileInfo.DirectoryName).IsEqualTo(destinationFolder);
            await That(fileInfo.FullName).IsEqualTo(destinationPath);
        }

        [Test]
        public async Task MockFileInfo_MoveTo_NonExistentDestinationFolder_ShouldThrowDirectoryNotFoundException()
        {
            var fileSystem = new MockFileSystem();
            var sourcePath = XFS.Path(@"c:\temp\file.txt");
            var destinationPath = XFS.Path(@"c:\temp2\file.txt");
            fileSystem.AddFile(sourcePath, new MockFileData("1"));
            var fileInfo = fileSystem.FileInfo.New(sourcePath);

            await That(() => fileInfo.MoveTo(destinationPath)).Throws<DirectoryNotFoundException>();
        }

        [Test]
        public async Task MockFileInfo_MoveTo_ExistingDestination_ShouldThrowExceptionAboutFileAlreadyExisting()
        {
            var fileSystem = new MockFileSystem();
            var sourcePath = XFS.Path(@"c:\temp\file.txt");
            var destinationPath = XFS.Path(@"c:\temp2\file.txt");
            fileSystem.AddFile(sourcePath, new MockFileData("1"));
            var fileInfo = fileSystem.FileInfo.New(sourcePath);
            fileSystem.AddFile(destinationPath, new MockFileData("2"));

            await That(() => fileInfo.MoveTo(destinationPath)).Throws<IOException>();
        }

        [Test]
        public async Task MockFileInfo_MoveTo_SameSourceAndTargetIsANoOp()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(XFS.Path(@"c:\temp\file.txt"), new MockFileData(@"line 1\r\nline 2"));
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));
            string destination = XFS.Path(XFS.Path(@"c:\temp\file.txt"));

            fileInfo.MoveTo(destination);

            await That(fileInfo.FullName).IsEqualTo(destination);
            await That(fileInfo.Exists).IsTrue();
        }

        [Test]
        public async Task MockFileInfo_MoveTo_SameSourceAndTargetThrowsExceptionIfSourceDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));
            string destination = XFS.Path(XFS.Path(@"c:\temp\file.txt"));

            Action action = () => fileInfo.MoveTo(destination);

            await That(action).Throws<FileNotFoundException>();
        }

        [Test]
        public async Task MockFileInfo_MoveTo_ThrowsExceptionIfSourceDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));
            string destination = XFS.Path(XFS.Path(@"c:\temp\file2.txt"));

            Action action = () => fileInfo.MoveTo(destination);

            await That(action).Throws<FileNotFoundException>();
        }



#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        [Test]
        public async Task MockFileInfo_MoveToWithOverwrite_ShouldSucceedWhenTargetAlreadyExists()
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

            await That(fileSystem.File.ReadAllText(destFilePath)).IsEqualTo(sourceFileContent);
        }
#endif

        [Test]
        public async Task MockFileInfo_MoveToOnlyCaseChanging_ShouldSucceed()
        {
            string sourceFilePath = XFS.Path(@"c:\temp\file.txt");
            string destFilePath = XFS.Path(@"c:\temp\FILE.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFilePath, new MockFileData("1")},
            });

            var fileInfo = fileSystem.FileInfo.New(sourceFilePath);
            fileInfo.MoveTo(destFilePath);

            await That(fileInfo.FullName).IsEqualTo(destFilePath);
            await That(fileInfo.Exists).IsTrue();
        }

        [Test]
        public async Task MockFileInfo_CopyTo_ThrowsExceptionIfSourceDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            var fileInfo = fileSystem.FileInfo.New(XFS.Path(@"c:\temp\file.txt"));
            string destination = XFS.Path(XFS.Path(@"c:\temp\file2.txt"));

            Action action = () => fileInfo.CopyTo(destination);

            await That(action).Throws<FileNotFoundException>();
        }

        [TestCase(@"..\..\..\c.txt")]
        [TestCase(@"c:\a\b\c.txt")]
        [TestCase(@"c:\a\c.txt")]
        [TestCase(@"c:\c.txt")]
        public async Task MockFileInfo_ToString_ShouldReturnOriginalFilePath(string path)
        {
            //Arrange
            var filePath = XFS.Path(path);

            //Act
            var mockFileInfo = new MockFileInfo(new MockFileSystem(), filePath);

            //Assert
            await That(mockFileInfo.ToString()).IsEqualTo(filePath);
        }


        /// <summary>
        /// Normalize, tested with Path.GetFullPath and new FileInfo().FullName;
        /// </summary>
        [TestCaseSource(nameof(New_Paths_NormalizePaths_Cases))]
        public async Task New_Paths_NormalizePaths(string input, string expected)
        {
            // Arrange
            var mockFs = new MockFileSystem();

            // Act
            var mockFileInfo = mockFs.FileInfo.New(input);
            var result = mockFileInfo.FullName;

            // Assert
            await That(result).IsEqualTo(expected);
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
        public async Task MockFileInfo_Replace_ShouldReplaceFileContents()
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

            await That(fileInfo2.OpenText().ReadToEnd()).IsEqualTo("1");
        }

        [Test]
        public async Task MockFileInfo_Replace_ShouldCreateBackup()
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

            await That(fileInfo3.OpenText().ReadToEnd()).IsEqualTo("2");
        }

        [Test]
        public async Task MockFileInfo_Replace_ShouldThrowIfDirectoryOfBackupPathDoesNotExist()
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
            await That(() => fileInfo1.Replace(path2, path3)).Throws<DirectoryNotFoundException>();
        }

        [Test]
        public async Task MockFileInfo_Replace_ShouldReturnDestinationFileInfo()
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

            await That(result.FullName).IsEqualTo(fileInfo2.FullName);
        }

        [Test]
        public async Task MockFileInfo_Replace_ShouldThrowIfSourceFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var path2 = XFS.Path(@"c:\temp\file2.txt");
            fileSystem.AddFile(path2, new MockFileData("1"));
            var fileInfo = fileSystem.FileInfo.New(path1);

            await That(() => fileInfo.Replace(path2, null)).Throws<FileNotFoundException>();
        }

        [Test]
        public async Task MockFileInfo_Replace_ShouldThrowIfDestinationFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var path2 = XFS.Path(@"c:\temp\file2.txt");
            fileSystem.AddFile(path1, new MockFileData("1"));
            var fileInfo = fileSystem.FileInfo.New(path1);

            await That(() => fileInfo.Replace(path2, null)).Throws<FileNotFoundException>();
        }

        [Test]
        public async Task MockFileInfo_Exists_ShouldReturnCachedData()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var fileInfo = fileSystem.FileInfo.New(path1);

            // Act
            fileSystem.AddFile(path1, new MockFileData("1"));

            // Assert
            await That(fileInfo.Exists).IsFalse();
        }

        [Test]
        public async Task MockFileInfo_Exists_ShouldUpdateCachedDataOnRefresh()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var path1 = XFS.Path(@"c:\temp\file1.txt");
            var fileInfo = fileSystem.FileInfo.New(path1);

            // Act
            fileSystem.AddFile(path1, new MockFileData("1"));
            fileInfo.Refresh();

            // Assert
            await That(fileInfo.Exists).IsTrue();
        }

        [Test]
        public async Task MockFileInfo_Create_ShouldUpdateCachedDataAndReturnTrueForExists()
        {
            IFileSystem fileSystem = new MockFileSystem();
            var path = XFS.Path(@"c:\temp\file1.txt");
            IFileInfo fileInfo = fileSystem.FileInfo.New(path);

            // Act
            fileInfo.Create().Dispose();

            // Assert
            var result = fileInfo.Exists;
            await That(result).IsTrue();
        }

        [Test]
        public async Task MockFileInfo_CreateText_ShouldUpdateCachedDataAndReturnTrueForExists()
        {
            IFileSystem fileSystem = new MockFileSystem();
            var path = XFS.Path(@"c:\temp\file1.txt");
            IFileInfo fileInfo = fileSystem.FileInfo.New(path);

            // Act
            fileInfo.CreateText().Dispose();

            // Assert
            await That(fileInfo.Exists).IsTrue();
        }

        [Test]
        public async Task MockFileInfo_Delete_ShouldUpdateCachedDataAndReturnFalseForExists()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path(@"c:\temp\file1.txt");
            IFileInfo fileInfo = fileSystem.FileInfo.New(path);

            // Act
            fileInfo.Delete();

            // Assert
            await That(fileInfo.Exists).IsFalse();
        }

        [Test]
        public async Task MockFileInfo_Delete_ShouldThrowIfFileAccessShareHasNoWriteOrDeleteAccess()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(
                    @"c:\bar\foo.txt",
                    new MockFileData("text contents") { AllowedFileShare = FileShare.None });

            var fi = fileSystem.FileInfo.New(@"c:\bar\foo.txt");

            await That(() => fi.Delete()).Throws<IOException>();
        }

        [Test]
        public async Task MockFileInfo_LastAccessTimeUtcWithUnspecifiedDateTimeKind_ShouldSetLastAccessTimeUtcOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                LastAccessTimeUtc = date
            };

            await That(fileInfo.LastAccessTimeUtc).IsEqualTo(date);
            await That(fileInfo.LastAccessTimeUtc.Kind).IsNotEqualTo(DateTimeKind.Unspecified);
        }

        [Test]
        public async Task MockFileInfo_LastAccessTimeWithUnspecifiedDateTimeKind_ShouldSetLastAccessTimeOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                LastAccessTime = date
            };

            await That(fileInfo.LastAccessTime).IsEqualTo(date);
            await That(fileInfo.LastAccessTime.Kind).IsNotEqualTo(DateTimeKind.Unspecified);
        }

        [Test]
        public async Task MockFileInfo_CreationTimeUtcWithUnspecifiedDateTimeKind_ShouldSetCreationTimeUtcOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                CreationTimeUtc = date
            };

            await That(fileInfo.CreationTimeUtc).IsEqualTo(date);
            await That(fileInfo.CreationTimeUtc.Kind).IsNotEqualTo(DateTimeKind.Unspecified);
        }

        [Test]
        public async Task MockFileInfo_CreationTimeWithUnspecifiedDateTimeKind_ShouldSetCreationTimeOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                CreationTime = date
            };

            await That(fileInfo.CreationTime).IsEqualTo(date);
            await That(fileInfo.CreationTime.Kind).IsNotEqualTo(DateTimeKind.Unspecified);
        }

        [Test]
        public async Task MockFileInfo_LastWriteTimeUtcWithUnspecifiedDateTimeKind_ShouldSetLastWriteTimeUtcOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                LastWriteTimeUtc = date
            };

            await That(fileInfo.LastWriteTimeUtc).IsEqualTo(date);
            await That(fileInfo.LastWriteTimeUtc.Kind).IsNotEqualTo(DateTimeKind.Unspecified);
        }

        [Test]
        public async Task MockFileInfo_LastWriteTimeWithUnspecifiedDateTimeKind_ShouldSetLastWriteTimeOfFileInFileSystem()
        {
            var date = DateTime.SpecifyKind(DateTime.Now.AddHours(-4), DateTimeKind.Unspecified);
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(@"c:\test");
            fileSystem.File.WriteAllText(@"c:\test\a.txt", "Demo text content");
            var fileInfo = new MockFileInfo(fileSystem, @"c:\test\a.txt")
            {
                LastWriteTime = date
            };

            await That(fileInfo.LastWriteTime).IsEqualTo(date);
            await That(fileInfo.LastWriteTime.Kind).IsNotEqualTo(DateTimeKind.Unspecified);
        }
    }
}
