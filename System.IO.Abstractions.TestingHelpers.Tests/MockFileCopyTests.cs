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
        public void MockFile_Copy_ShouldThrowArgumentNullExceptionWhenSourceIsNull_Message()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Copy(null, destFilePath));

            Assert.That(exception.Message, Does.StartWith("File name cannot be null."));
        }

        [Test]
        public void MockFile_Copy_ShouldThrowArgumentNullExceptionWhenSourceIsNull_ParamName()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Copy(null, destFilePath));

            Assert.That(exception.ParamName, Is.EqualTo("sourceFileName"));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceFileNameContainsInvalidChars_Message()
        {
            var destFilePath = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem();
            var excludeChars = Shared.SpecialInvalidPathChars(fileSystem);

            foreach (var invalidChar in fileSystem.Path.GetInvalidFileNameChars().Except(excludeChars))
            {
                var sourceFilePath = @"c:\something\demo.txt" + invalidChar;

                var exception =
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

                Assert.That(exception.Message, Is.EqualTo("Illegal characters in path."),
                    string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenSourcePathContainsInvalidChars_Message()
        {
            var destFilePath = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem();

            foreach (var invalidChar in fileSystem.Path.GetInvalidPathChars())
            {
                var sourceFilePath = @"c:\some" + invalidChar + @"thing\demo.txt";

                var exception =
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

                Assert.That(exception.Message, Is.EqualTo("Illegal characters in path."),
                    string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenTargetPathContainsInvalidChars_Message()
        {
            var sourceFilePath = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem();

            foreach (var invalidChar in fileSystem.Path.GetInvalidPathChars())
            {
                var destFilePath = @"c:\some" + invalidChar + @"thing\demo.txt";

                var exception =
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

                Assert.That(exception.Message, Is.EqualTo("Illegal characters in path."),
                    string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenTargetFileNameContainsInvalidChars_Message()
        {
            var sourceFilePath = @"c:\something\demo.txt";
            var fileSystem = new MockFileSystem();
            var excludeChars = Shared.SpecialInvalidPathChars(fileSystem);

            foreach (var invalidChar in fileSystem.Path.GetInvalidFileNameChars().Except(excludeChars))
            {
                var destFilePath = @"c:\something\demo.txt" + invalidChar;

                var exception =
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

                Assert.That(exception.Message, Is.EqualTo("Illegal characters in path."),
                    string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void MockFile_Copy_ShouldThrowNotSupportedExceptionWhenSourcePathContainsInvalidUseOfDriveSeparator()
        {
            var badSourcePath = @"C::\something\demo.txt";
            var destinationPath = @"C:\elsewhere\demo.txt";
            var fileSystem = new MockFileSystem();

            TestDelegate action = () => fileSystem.File.Copy(badSourcePath, destinationPath);

            Assert.Throws<NotSupportedException>(action);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void MockFile_Copy_ShouldThrowNotSupportedExceptionWhenSourcePathContainsInvalidDriveLetter()
        {
            var badSourcePath = @"0:\something\demo.txt";
            var destinationPath = @"C:\elsewhere\demo.txt";
            var fileSystem = new MockFileSystem();

            TestDelegate action = () => fileSystem.File.Copy(badSourcePath, destinationPath);

            Assert.Throws<NotSupportedException>(action);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void MockFile_Copy_ShouldThrowNotSupportedExceptionWhenDestinationPathContainsInvalidUseOfDriveSeparator()
        {
            var sourcePath = @"C:\something\demo.txt";
            var badDestinationPath = @"C:\elsewhere:\demo.txt";
            var fileSystem = new MockFileSystem();

            TestDelegate action = () => fileSystem.File.Copy(sourcePath, badDestinationPath);

            Assert.Throws<NotSupportedException>(action);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void MockFile_Copy_ShouldThrowNotSupportedExceptionWhenDestinationPathContainsInvalidDriveLetter()
        {
            var sourcePath = @"C:\something\demo.txt";
            var badDestinationPath = @"^:\elsewhere\demo.txt";
            var fileSystem = new MockFileSystem();

            TestDelegate action = () => fileSystem.File.Copy(sourcePath, badDestinationPath);

            Assert.Throws<NotSupportedException>(action);
        }

        [Test]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceIsEmpty_Message()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(string.Empty, destFilePath));

            Assert.That(exception.Message, Does.StartWith("Empty file name is not legal."));
        }

        [Test]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceIsEmpty_ParamName()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(string.Empty, destFilePath));

            Assert.That(exception.ParamName, Is.EqualTo("sourceFileName"));
        }

        [Test]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceIsStringOfBlanks()
        {
            string sourceFilePath = "   ";
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

            Assert.That(exception.Message, Does.StartWith("The path is not of a legal form."));
        }

        [Test]
        public void MockFile_Copy_ShouldThrowArgumentNullExceptionWhenTargetIsNull_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Copy(sourceFilePath, null));

            Assert.That(exception.Message, Does.StartWith("File name cannot be null."));
        }

        [Test]
        public void MockFile_Copy_ShouldThrowArgumentNullExceptionWhenTargetIsNull_ParamName()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Copy(sourceFilePath, null));

            Assert.That(exception.ParamName, Is.EqualTo("destFileName"));
        }

        [Test]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenTargetIsStringOfBlanks()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = "   ";
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

            Assert.That(exception.Message, Does.StartWith("The path is not of a legal form."));
        }

        [Test]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenTargetIsEmpty_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, string.Empty));

            Assert.That(exception.Message, Does.StartWith("Empty file name is not legal."));
        }

        [Test]
        public void MockFile_Copy_ShouldThrowFileNotFoundExceptionWhenSourceDoesNotExist()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            TestDelegate action = () => fileSystem.File.Copy(sourceFilePath, XFS.Path(@"c:\something\demo2.txt"));

            Assert.Throws<FileNotFoundException>(action);
        }

        [Test]
        public void MockFile_Copy_ShouldThrowFileNotFoundExceptionWhenSourceDoesNotExist_EvenWhenCopyingToItself()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            TestDelegate action = () => fileSystem.File.Copy(sourceFilePath, XFS.Path(@"c:\something\demo.txt"));

            Assert.Throws<FileNotFoundException>(action);
        }
    }
}