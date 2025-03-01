namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;
    using Globalization;
    using Linq;
    using NUnit.Framework;
    using XFS = MockUnixSupport;

    public class MockFileCopyTests
    {

        [Test]
        public async Task MockFile_Copy_ShouldOverwriteFileWhenOverwriteFlagIsTrue()
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
            await That(sourceContents.Contents).IsEqualTo(copyResult.Contents);
        }

        [Test]
        public async Task MockFile_Copy_ShouldAdjustTimestampsOnDestination()
        {
            var sourceFileName = XFS.Path(@"c:\source\demo.txt");
            var destFileName = XFS.Path(@"c:\source\demo_copy.txt");

            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(sourceFileName, "Original");
            mockFileSystem.File.Copy(sourceFileName, destFileName);

            var sourceFileInfo = mockFileSystem.FileInfo.New(sourceFileName);
            var destFileInfo = mockFileSystem.FileInfo.New(destFileName);
            await That(destFileInfo.LastWriteTime).IsEqualTo(sourceFileInfo.LastWriteTime);
            await That(DateTime.Now - destFileInfo.CreationTime).IsLessThanOrEqualTo( TimeSpan.FromSeconds(1));
            await That(destFileInfo.LastAccessTime).IsEqualTo(destFileInfo.CreationTime);
        }

        [Test]
        public async Task MockFile_Copy_ShouldCloneContents()
        {
            var sourceFileName = XFS.Path(@"c:\source\demo.txt");
            var destFileName = XFS.Path(@"c:\source\demo_copy.txt");

            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(sourceFileName, "Original");
            mockFileSystem.File.Copy(sourceFileName, destFileName);

            using (var stream = mockFileSystem.File.Open(sourceFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                var binaryWriter = new System.IO.BinaryWriter(stream);

                binaryWriter.Seek(0, SeekOrigin.Begin);
                binaryWriter.Write("Modified");
            }

            await That(mockFileSystem.File.ReadAllText(destFileName)).IsEqualTo("Original");
        }

        [Test]
        public async Task MockFile_Copy_ShouldCloneBinaryContents()
        {
            var sourceFileName = XFS.Path(@"c:\source\demo.bin");
            var destFileName = XFS.Path(@"c:\source\demo_copy.bin");

            byte[] original = new byte[] { 0xC0 };
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(sourceFileName, new MockFileData(original));
            mockFileSystem.File.Copy(sourceFileName, destFileName);

            using (var stream = mockFileSystem.File.Open(sourceFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                var binaryWriter = new System.IO.BinaryWriter(stream);

                binaryWriter.Seek(0, SeekOrigin.Begin);
                binaryWriter.Write("Modified");
            }

            await That(mockFileSystem.File.ReadAllBytes(destFileName)).IsEqualTo(original);
        }

        [Test]
        public async Task MockFile_Copy_ShouldCreateFileAtNewDestination()
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
            await That(sourceContents.Contents).IsEqualTo(copyResult.Contents);
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowExceptionWhenFileExistsAtDestination()
        {
            string sourceFileName = XFS.Path(@"c:\source\demo.txt");
            var sourceContents = new MockFileData("Source content");
            string destFileName = XFS.Path(@"c:\destination\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFileName, sourceContents},
                {destFileName, new MockFileData("Destination content")}
            });

            await That(() => fileSystem.File.Copy(sourceFileName, destFileName), XFS.Path(@"The file c:\destination\demo.txt already exists.")).Throws<IOException>();
        }

        [TestCase(@"c:\source\demo.txt", @"c:\source\doesnotexist\demo.txt")]
        [TestCase(@"c:\source\demo.txt", @"c:\doesnotexist\demo.txt")]
        public async Task MockFile_Copy_ShouldThrowExceptionWhenFolderInDestinationDoesNotExist(string sourceFilePath, string destFilePath)
        {
            string sourceFileName = XFS.Path(sourceFilePath);
            string destFileName = XFS.Path(destFilePath);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFileName, string.Empty}
            });

            await That(() => fileSystem.File.Copy(sourceFileName, destFileName), string.Format(CultureInfo.InvariantCulture, @"Could not find a part of the path '{0}'.", destFilePath)).Throws<DirectoryNotFoundException>();
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowArgumentNullExceptionWhenSourceIsNull_Message()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = await That(() => fileSystem.File.Copy(null, destFilePath)).Throws<ArgumentNullException>();

            await That(exception.Message).StartsWith("File name cannot be null.");
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowArgumentNullExceptionWhenSourceIsNull_ParamName()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = await That(() => fileSystem.File.Copy(null, destFilePath)).Throws<ArgumentNullException>();

            await That(exception.ParamName).IsEqualTo("sourceFileName");
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public async Task MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceFileNameContainsInvalidChars_Message()
        {
            var destFilePath = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem();
            var excludeChars = Shared.SpecialInvalidPathChars(fileSystem);

            foreach (var invalidChar in fileSystem.Path.GetInvalidFileNameChars().Except(excludeChars))
            {
                var sourceFilePath = @"c:\something\demo.txt" + invalidChar;

                var exception =
                    await That(() => fileSystem.File.Copy(sourceFilePath, destFilePath)).Throws<ArgumentException>();

                await That(exception.Message).IsEqualTo("Illegal characters in path.")
                    .Because(string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public async Task MockFile_Copy_ShouldThrowArgumentExceptionWhenSourcePathContainsInvalidChars_Message()
        {
            var destFilePath = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem();

            foreach (var invalidChar in fileSystem.Path.GetInvalidPathChars())
            {
                var sourceFilePath = @"c:\some" + invalidChar + @"thing\demo.txt";

                var exception =
                    await That(() => fileSystem.File.Copy(sourceFilePath, destFilePath)).Throws<ArgumentException>();

                await That(exception.Message).IsEqualTo("Illegal characters in path.")
                    .Because(string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public async Task MockFile_Copy_ShouldThrowArgumentExceptionWhenTargetPathContainsInvalidChars_Message()
        {
            var sourceFilePath = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem();

            foreach (var invalidChar in fileSystem.Path.GetInvalidPathChars())
            {
                var destFilePath = @"c:\some" + invalidChar + @"thing\demo.txt";

                var exception =
                    await That(() => fileSystem.File.Copy(sourceFilePath, destFilePath)).Throws<ArgumentException>();

                await That(exception.Message).IsEqualTo("Illegal characters in path.")
                    .Because(string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public async Task MockFile_Copy_ShouldThrowArgumentExceptionWhenTargetFileNameContainsInvalidChars_Message()
        {
            var sourceFilePath = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem();
            var excludeChars = Shared.SpecialInvalidPathChars(fileSystem);

            foreach (var invalidChar in fileSystem.Path.GetInvalidFileNameChars().Except(excludeChars))
            {
                var destFilePath = @"c:\something\demo.txt" + invalidChar;

                var exception =
                    await That(() => fileSystem.File.Copy(sourceFilePath, destFilePath)).Throws<ArgumentException>();

                await That(exception.Message).IsEqualTo("Illegal characters in path.")
                    .Because(string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public async Task MockFile_Copy_ShouldThrowNotSupportedExceptionWhenSourcePathContainsInvalidUseOfDriveSeparator()
        {
            var badSourcePath = @"C::\something\demo.txt";
            var destinationPath = @"C:\elsewhere\demo.txt";
            var fileSystem = new MockFileSystem();

            Action action = () => fileSystem.File.Copy(badSourcePath, destinationPath);

            await That(action).Throws<NotSupportedException>();
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public async Task MockFile_Copy_ShouldThrowNotSupportedExceptionWhenSourcePathContainsInvalidDriveLetter()
        {
            var badSourcePath = @"0:\something\demo.txt";
            var destinationPath = @"C:\elsewhere\demo.txt";
            var fileSystem = new MockFileSystem();

            Action action = () => fileSystem.File.Copy(badSourcePath, destinationPath);

            await That(action).Throws<NotSupportedException>();
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public async Task MockFile_Copy_ShouldThrowNotSupportedExceptionWhenDestinationPathContainsInvalidUseOfDriveSeparator()
        {
            var sourcePath = @"C:\something\demo.txt";
            var badDestinationPath = @"C:\elsewhere:\demo.txt";
            var fileSystem = new MockFileSystem();

            Action action = () => fileSystem.File.Copy(sourcePath, badDestinationPath);

            await That(action).Throws<NotSupportedException>();
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public async Task MockFile_Copy_ShouldThrowNotSupportedExceptionWhenDestinationPathContainsInvalidDriveLetter()
        {
            var sourcePath = @"C:\something\demo.txt";
            var badDestinationPath = @"^:\elsewhere\demo.txt";
            var fileSystem = new MockFileSystem();

            Action action = () => fileSystem.File.Copy(sourcePath, badDestinationPath);

            await That(action).Throws<NotSupportedException>();
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceIsEmpty_Message()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = await That(() => fileSystem.File.Copy(string.Empty, destFilePath)).Throws<ArgumentException>();

            await That(exception.Message).StartsWith("Empty file name is not legal.");
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceIsEmpty_ParamName()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = await That(() => fileSystem.File.Copy(string.Empty, destFilePath)).Throws<ArgumentException>();

            await That(exception.ParamName).IsEqualTo("sourceFileName");
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceIsStringOfBlanks()
        {
            string sourceFilePath = "   ";
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = await That(() => fileSystem.File.Copy(sourceFilePath, destFilePath)).Throws<ArgumentException>();

            await That(exception.Message).StartsWith("The path is not of a legal form.");
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowArgumentNullExceptionWhenTargetIsNull_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = await That(() => fileSystem.File.Copy(sourceFilePath, null)).Throws<ArgumentNullException>();

            await That(exception.Message).StartsWith("File name cannot be null.");
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowArgumentNullExceptionWhenTargetIsNull_ParamName()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = await That(() => fileSystem.File.Copy(sourceFilePath, null)).Throws<ArgumentNullException>();

            await That(exception.ParamName).IsEqualTo("destFileName");
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowArgumentExceptionWhenTargetIsStringOfBlanks()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = "   ";
            var fileSystem = new MockFileSystem();

            var exception = await That(() => fileSystem.File.Copy(sourceFilePath, destFilePath)).Throws<ArgumentException>();

            await That(exception.Message).StartsWith("The path is not of a legal form.");
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowArgumentExceptionWhenTargetIsEmpty_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = await That(() => fileSystem.File.Copy(sourceFilePath, string.Empty)).Throws<ArgumentException>();

            await That(exception.Message).StartsWith("Empty file name is not legal.");
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowFileNotFoundExceptionWhenSourceDoesNotExist()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            Action action = () => fileSystem.File.Copy(sourceFilePath, XFS.Path(@"c:\something\demo2.txt"));

            await That(action).Throws<FileNotFoundException>();
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowFileNotFoundExceptionWhenSourceDoesNotExist_EvenWhenCopyingToItself()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            Action action = () => fileSystem.File.Copy(sourceFilePath, XFS.Path(@"c:\something\demo.txt"));

            await That(action).Throws<FileNotFoundException>();
        }

        [Test]
        public async Task MockFile_Copy_ShouldWorkWithRelativePaths()
        {
            var sourceFile = "source_file.txt";
            var destinationFile = "destination_file.txt";
            var fileSystem = new MockFileSystem();

            fileSystem.File.Create(sourceFile).Close();
            fileSystem.File.Copy(sourceFile, destinationFile);

            await That(fileSystem.File.Exists(destinationFile)).IsTrue();
        }

        [Test]
        public async Task MockFile_Copy_ShouldThrowIOExceptionForInvalidFileShare()
        {
            string sourceFileName = XFS.Path(@"c:\source\demo.txt");
            var sourceContents = new MockFileData("Source content")
            {
                AllowedFileShare = FileShare.None
            };
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFileName, sourceContents}
            });
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            Action action = () => fileSystem.File.Copy(sourceFileName, XFS.Path(@"c:\something\demo.txt"));

            await That(action).Throws<IOException>();
        }
    }
}