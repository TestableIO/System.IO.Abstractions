namespace System.IO.Abstractions.TestingHelpers.Tests;

using Collections.Generic;
using Linq;
using NUnit.Framework;
using XFS = MockUnixSupport;

public class MockFileMoveTests
{
    [Test]
    public async Task MockFile_Move_ShouldMoveFileWithinMemoryFileSystem()
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

        await That(fileSystem.FileExists(destFilePath)).IsTrue();
        await That(fileSystem.GetFile(destFilePath).TextContents).IsEqualTo(sourceFileContent);
        await That(fileSystem.FileExists(sourceFilePath)).IsFalse();
    }

    [Test]
    public async Task MockFile_Move_WithReadOnlyAttribute_ShouldMoveFile()
    {
        var sourceFilePath = @"c:\foo.txt";
        var destFilePath = @"c:\bar.txt";
        var fileSystem = new MockFileSystem();
        fileSystem.File.WriteAllText(sourceFilePath, "this is some content");
        fileSystem.File.SetAttributes(sourceFilePath, FileAttributes.ReadOnly);

        fileSystem.File.Move(sourceFilePath, destFilePath);

        await That(fileSystem.File.Exists(destFilePath)).IsTrue();
        await That(fileSystem.File.Exists(sourceFilePath)).IsFalse();
    }

    [Test]
    public async Task MockFile_Move_SameSourceAndTargetIsANoOp()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        string sourceFileContent = "this is some content";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {sourceFilePath, new MockFileData(sourceFileContent)},
            {XFS.Path(@"c:\somethingelse\dummy.txt"), new MockFileData(new byte[] {0})}
        });

        string destFilePath = XFS.Path(@"c:\somethingelse\demo.txt");

        fileSystem.File.Move(sourceFilePath, destFilePath);

        await That(fileSystem.FileExists(destFilePath)).IsTrue();
        await That(fileSystem.GetFile(destFilePath).TextContents).IsEqualTo(sourceFileContent);
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowIOExceptionWhenTargetAlreadyExists()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        string sourceFileContent = "this is some content";
        string destFilePath = XFS.Path(@"c:\somethingelse\demo1.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {sourceFilePath, new MockFileData(sourceFileContent)},
            {destFilePath, new MockFileData(sourceFileContent)}
        });

        var exception = await That(() => fileSystem.File.Move(sourceFilePath, destFilePath)).Throws<IOException>();

        await That(exception.Message).IsEqualTo("A file can not be created if it already exists.");
    }

#if FEATURE_FILE_MOVE_WITH_OVERWRITE
    [Test]
    public async Task MockFile_MoveWithOverwrite_ShouldSucceedWhenTargetAlreadyExists()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        string sourceFileContent = "this is some content";
        string destFilePath = XFS.Path(@"c:\somethingelse\demo1.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {sourceFilePath, new MockFileData(sourceFileContent)},
            {destFilePath, new MockFileData(sourceFileContent)}
        });

        fileSystem.File.Move(sourceFilePath, destFilePath, overwrite: true);

        await That(fileSystem.File.ReadAllText(destFilePath)).IsEqualTo(sourceFileContent);
    }
#endif

    [Test]
    public async Task MockFile_Move_ShouldThrowArgumentNullExceptionWhenSourceIsNull_Message()
    {
        string destFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(null, destFilePath)).Throws<ArgumentNullException>();

        await That(exception.Message).StartsWith("File name cannot be null.");
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowArgumentNullExceptionWhenSourceIsNull_ParamName()
    {
        string destFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(null, destFilePath)).Throws<ArgumentNullException>();

        await That(exception.ParamName).IsEqualTo("sourceFileName");
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockFile_Move_ShouldThrowArgumentExceptionWhenSourceFileNameContainsInvalidChars_Message()
    {
        var destFilePath = @"c:\something\demo.txt";
        var fileSystem = new MockFileSystem();
        var excludeChars = Shared.SpecialInvalidPathChars(fileSystem);

        foreach (var invalidChar in fileSystem.Path.GetInvalidFileNameChars().Except(excludeChars))
        {
            var sourceFilePath = @"c:\something\demo.txt" + invalidChar;

            var exception =
                await That(() => fileSystem.File.Move(sourceFilePath, destFilePath)).Throws<ArgumentException>();

            await That(exception.Message).IsEqualTo("Illegal characters in path.")
                .Because(string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
        }
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockFile_Move_ShouldThrowArgumentExceptionWhenSourcePathContainsInvalidChars_Message()
    {
        var destFilePath = @"c:\something\demo.txt";
        var fileSystem = new MockFileSystem();

        foreach (var invalidChar in fileSystem.Path.GetInvalidPathChars())
        {
            var sourceFilePath = @"c:\some" + invalidChar + @"thing\demo.txt";

            var exception =
                await That(() => fileSystem.File.Move(sourceFilePath, destFilePath)).Throws<ArgumentException>();

            await That(exception.Message).IsEqualTo("Illegal characters in path.")
                .Because(string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
        }
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockFile_Move_ShouldThrowArgumentExceptionWhenTargetPathContainsInvalidChars_Message()
    {
        var sourceFilePath = @"c:\something\demo.txt";
        var fileSystem = new MockFileSystem();

        foreach (var invalidChar in fileSystem.Path.GetInvalidPathChars())
        {
            var destFilePath = @"c:\some" + invalidChar + @"thing\demo.txt";

            var exception =
                await That(() => fileSystem.File.Move(sourceFilePath, destFilePath)).Throws<ArgumentException>();

            await That(exception.Message).IsEqualTo("Illegal characters in path.")
                .Because(string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
        }
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockFile_Move_ShouldThrowArgumentExceptionWhenTargetFileNameContainsInvalidChars_Message()
    {
        var sourceFilePath = @"c:\something\demo.txt";
        var fileSystem = new MockFileSystem();
        var excludeChars = Shared.SpecialInvalidPathChars(fileSystem);

        foreach (var invalidChar in fileSystem.Path.GetInvalidFileNameChars().Except(excludeChars))
        {
            var destFilePath = @"c:\something\demo.txt" + invalidChar;

            var exception =
                await That(() => fileSystem.File.Move(sourceFilePath, destFilePath)).Throws<ArgumentException>();

            await That(exception.Message).IsEqualTo("Illegal characters in path.")
                .Because(string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
        }
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockFile_Move_ShouldThrowNotSupportedExceptionWhenSourcePathContainsInvalidUseOfDriveSeparator()
    {
        var badSourcePath = @"C::\something\demo.txt";
        var destinationPath = @"C:\elsewhere\demo.txt";
        var fileSystem = new MockFileSystem();

        Action action = () => fileSystem.File.Move(badSourcePath, destinationPath);

        await That(action).Throws<NotSupportedException>();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockFile_Move_ShouldThrowNotSupportedExceptionWhenSourcePathContainsInvalidDriveLetter()
    {
        var badSourcePath = @"0:\something\demo.txt";
        var destinationPath = @"C:\elsewhere\demo.txt";
        var fileSystem = new MockFileSystem();

        Action action = () => fileSystem.File.Move(badSourcePath, destinationPath);

        await That(action).Throws<NotSupportedException>();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockFile_Move_ShouldThrowNotSupportedExceptionWhenDestinationPathContainsInvalidUseOfDriveSeparator()
    {
        var sourcePath = @"C:\something\demo.txt";
        var badDestinationPath = @"C:\elsewhere:\demo.txt";
        var fileSystem = new MockFileSystem();

        Action action = () => fileSystem.File.Move(sourcePath, badDestinationPath);

        await That(action).Throws<NotSupportedException>();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockFile_Move_ShouldThrowNotSupportedExceptionWhenDestinationPathContainsInvalidDriveLetter()
    {
        var sourcePath = @"C:\something\demo.txt";
        var badDestinationPath = @"^:\elsewhere\demo.txt";
        var fileSystem = new MockFileSystem();

        Action action = () => fileSystem.File.Move(sourcePath, badDestinationPath);

        await That(action).Throws<NotSupportedException>();
    }

    [Test]
    [WindowsOnly(WindowsSpecifics.Drives)]
    public async Task MockFile_Move_CaseOnlyRename_ShouldChangeCase()
    {
        var fileSystem = new MockFileSystem();
        string sourceFilePath = @"c:\temp\demo.txt";
        string destFilePath = @"c:\temp\DEMO.TXT";
        string sourceFileContent = "content";
        fileSystem.File.WriteAllText(sourceFilePath, sourceFileContent);

        fileSystem.File.Move(sourceFilePath, destFilePath);

        await That(fileSystem.File.Exists(destFilePath)).IsTrue();
        await That(fileSystem.File.ReadAllText(destFilePath)).IsEqualTo(sourceFileContent);
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowArgumentExceptionWhenSourceIsEmpty_Message()
    {
        string destFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(string.Empty, destFilePath)).Throws<ArgumentException>();

        await That(exception.Message).StartsWith("Empty file name is not legal.");
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowArgumentExceptionWhenSourceIsEmpty_ParamName()
    {
        string destFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(string.Empty, destFilePath)).Throws<ArgumentException>();

        await That(exception.ParamName).IsEqualTo("sourceFileName");
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowArgumentExceptionWhenSourceIsStringOfBlanks()
    {
        string sourceFilePath = "   ";
        string destFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(sourceFilePath, destFilePath)).Throws<ArgumentException>();

        await That(exception.Message).StartsWith("The path is not of a legal form.");
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowArgumentNullExceptionWhenTargetIsNull_Message()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(sourceFilePath, null)).Throws<ArgumentNullException>();

        await That(exception.Message).StartsWith("File name cannot be null.");
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowArgumentNullExceptionWhenTargetIsNull_ParamName()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(sourceFilePath, null)).Throws<ArgumentNullException>();

        await That(exception.ParamName).IsEqualTo("destFileName");
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowArgumentExceptionWhenTargetIsStringOfBlanks()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        string destFilePath = "   ";
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(sourceFilePath, destFilePath)).Throws<ArgumentException>();

        await That(exception.Message).StartsWith("The path is not of a legal form.");
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowArgumentExceptionWhenTargetIsEmpty_Message()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(sourceFilePath, string.Empty)).Throws<ArgumentException>();

        await That(exception.Message).StartsWith("Empty file name is not legal.");
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowArgumentExceptionWhenTargetIsEmpty_ParamName()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(sourceFilePath, string.Empty)).Throws<ArgumentException>();

        await That(exception.ParamName).IsEqualTo("destFileName");
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowFileNotFoundExceptionWhenSourceDoesNotExist_Message()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        string destFilePath = XFS.Path(@"c:\something\demo1.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(sourceFilePath, destFilePath)).Throws<FileNotFoundException>();

        await That(exception.Message).IsEqualTo("Could not find file '" + XFS.Path("c:\\something\\demo.txt") + "'.");
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowFileNotFoundExceptionWhenSourceDoesNotExist_FileName()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        string destFilePath = XFS.Path(@"c:\something\demo1.txt");
        var fileSystem = new MockFileSystem();

        var exception = await That(() => fileSystem.File.Move(sourceFilePath, destFilePath)).Throws<FileNotFoundException>();

        await That(exception.FileName).IsEqualTo(XFS.Path(@"c:\something\demo.txt"));
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowDirectoryNotFoundExceptionWhenSourcePathDoesNotExist_Message()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        string destFilePath = XFS.Path(@"c:\somethingelse\demo.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {sourceFilePath, new MockFileData(new byte[] {0})}
        });

        await That(() => fileSystem.File.Move(sourceFilePath, destFilePath))
            .Throws<DirectoryNotFoundException>()
            .WithMessage(@"Could not find a part of the path*").AsWildcard();
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowExceptionWhenSourceDoesNotExist()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        Action action = () => fileSystem.File.Move(sourceFilePath, XFS.Path(@"c:\something\demo2.txt"));

        await That(action).Throws<FileNotFoundException>();
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowExceptionWhenSourceDoesNotExist_EvenWhenCopyingToItself()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        var fileSystem = new MockFileSystem();

        Action action = () => fileSystem.File.Move(sourceFilePath, XFS.Path(@"c:\something\demo.txt"));

        await That(action).Throws<FileNotFoundException>();
    }

    [Test]
    public async Task MockFile_Move_ShouldRetainMetadata()
    {
        string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
        string sourceFileContent = "this is some content";
        DateTimeOffset creationTime = DateTimeOffset.Now;
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {sourceFilePath, new MockFileData(sourceFileContent){CreationTime = creationTime}},
            {XFS.Path(@"c:\somethingelse\dummy.txt"), new MockFileData(new byte[] {0})}
        });

        string destFilePath = XFS.Path(@"c:\somethingelse\demo1.txt");

        fileSystem.File.Move(sourceFilePath, destFilePath);

        await That(fileSystem.File.GetCreationTimeUtc(destFilePath)).IsEqualTo(creationTime.UtcDateTime);
    }

    [Test]
    public async Task MockFile_Move_ShouldThrowExceptionWhenSourceFileShare_Is_Not_Delete()
    {
        string sourceFileReadDelete = XFS.Path(@"c:\something\IHaveReadDelete.txt");
        string sourceFileDelete = XFS.Path(@"c:\something\IHaveDelete.txt");
        string sourceFileRead = XFS.Path(@"c:\something\IHaveRead.txt");
        string sourceFileNone = XFS.Path(@"c:\something\IHaveNone.txt");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { sourceFileDelete, new MockFileData("") { AllowedFileShare = FileShare.Delete } },
            { sourceFileRead, new MockFileData("") { AllowedFileShare = FileShare.Read } },
            { sourceFileReadDelete, new MockFileData("") { AllowedFileShare = FileShare.Delete | FileShare.Read } },
            { sourceFileNone, new MockFileData("") { AllowedFileShare = FileShare.None } },
        });

        await AssertMoveSuccess(sourceFileReadDelete);
        await AssertMoveSuccess(sourceFileDelete);
        await AssertMoveThrowsIOException(sourceFileRead);
        await AssertMoveThrowsIOException(sourceFileNone);

        async Task AssertMoveThrowsIOException(string sourceFile)
        {
            var target = sourceFile + ".moved";
            await That(() => fileSystem.File.Move(sourceFile, target)).Throws<IOException>();
        }

        async Task AssertMoveSuccess(string sourceFile)
        {
            var target = sourceFile + ".moved";
            fileSystem.File.Move(sourceFile, target);
            await That(fileSystem.File.Exists(target)).IsTrue();
        }
    }
}