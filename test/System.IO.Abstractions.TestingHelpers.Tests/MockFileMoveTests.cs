using FluentAssertions;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using Linq;

    using Xunit;

    using XFS = MockUnixSupport;

    public class MockFileMoveTests {
        [Fact]
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

            Assert.True(fileSystem.FileExists(destFilePath));
            Assert.Equal(sourceFileContent, fileSystem.GetFile(destFilePath).TextContents);
            Assert.False(fileSystem.FileExists(sourceFilePath));
        }

        [Fact]
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

            Assert.Equal("A file can not be created if it already exists.", exception.Message);
        }

        [Fact]
        public void MockFile_Move_ShouldThrowArgumentNullExceptionWhenSourceIsNull_Message()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Move(null, destFilePath));

            exception.Message.Should().StartWith("File name cannot be null.");
        }

        [Fact]
        public void MockFile_Move_ShouldThrowArgumentNullExceptionWhenSourceIsNull_ParamName() {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Move(null, destFilePath));

            Assert.Equal("sourceFileName", exception.ParamName);
        }

        [Fact]
        public void MockFile_Move_ShouldThrowNotSupportedExceptionWhenSourceFileNameContainsInvalidChars_Message()
        {
            if (XFS.IsUnixPlatform())
            {
                Assert.True(true, "Path.GetInvalidChars() does not return anything on Mono");
            }

            var destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            foreach (var invalidChar in fileSystem.Path.GetInvalidFileNameChars().Where(x => x != fileSystem.Path.DirectorySeparatorChar))
            {
                var sourceFilePath = XFS.Path(@"c:\something\demo.txt") + invalidChar;

                var exception =
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

                exception.Message.Should().Be("Illegal characters in path.",
                    string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Fact]
        public void MockFile_Move_ShouldThrowNotSupportedExceptionWhenSourcePathContainsInvalidChars_Message()
        {
            if (XFS.IsUnixPlatform())
            {
                Assert.True(true, "Path.GetInvalidChars() does not return anything on Mono");
            }

            var destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            foreach (var invalidChar in fileSystem.Path.GetInvalidPathChars())
            {
                var sourceFilePath = XFS.Path(@"c:\some" + invalidChar + @"thing\demo.txt");

                var exception =
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

                exception.Message.Should().Be("Illegal characters in path.", 
                    string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Fact]
        public void MockFile_Move_ShouldThrowNotSupportedExceptionWhenTargetPathContainsInvalidChars_Message()
        {
            if (XFS.IsUnixPlatform())
            {
                Assert.True(true, "Path.GetInvalidChars() does not return anything on Mono");
                return;
            }

            var sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            foreach (var invalidChar in fileSystem.Path.GetInvalidPathChars())
            {
                var destFilePath = XFS.Path(@"c:\some" + invalidChar + @"thing\demo.txt");

                var exception =
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

                exception.Message.Should().Be("Illegal characters in path.",
                    string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Fact]
        public void MockFile_Move_ShouldThrowNotSupportedExceptionWhenTargetFileNameContainsInvalidChars_Message()
        {
            if (XFS.IsUnixPlatform())
            {
                Assert.True(true, "Path.GetInvalidChars() does not return anything on Mono");
                return;
            }

            var sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            foreach (var invalidChar in fileSystem.Path.GetInvalidFileNameChars().Where(x => x != fileSystem.Path.DirectorySeparatorChar))
            {
                var destFilePath = XFS.Path(@"c:\something\demo.txt") + invalidChar;

                var exception =
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

                exception.Message.Should().Be("Illegal characters in path.", 
                    string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Fact]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenSourceIsEmpty_Message()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(string.Empty, destFilePath));

            exception.Message.Should().StartWith("Empty file name is not legal.");
        }

        [Fact]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenSourceIsEmpty_ParamName() {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(string.Empty, destFilePath));

            Assert.Equal("sourceFileName", exception.ParamName);
        }

        [Fact]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenSourceIsStringOfBlanks()
        {
            string sourceFilePath = "   ";
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            exception.Message.Should().StartWith("The path is not of a legal form.");
        }

        [Fact]
        public void MockFile_Move_ShouldThrowArgumentNullExceptionWhenTargetIsNull_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Move(sourceFilePath, null));

            exception.Message.Should().StartWith("File name cannot be null.");
        }

        [Fact]
        public void MockFile_Move_ShouldThrowArgumentNullExceptionWhenTargetIsNull_ParamName() {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Move(sourceFilePath, null));

            Assert.Equal("destFileName", exception.ParamName);
        }

        [Fact]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenTargetIsStringOfBlanks()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = "   ";
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            exception.Message.Should().StartWith("The path is not of a legal form.");
        }

        [Fact]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenTargetIsEmpty_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, string.Empty));

            exception.Message.Should().StartWith("Empty file name is not legal.");
        }

        [Fact]
        public void MockFile_Move_ShouldThrowArgumentExceptionWhenTargetIsEmpty_ParamName() {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Move(sourceFilePath, string.Empty));

            Assert.Equal("destFileName", exception.ParamName);
        }

        [Fact]
        public void MockFile_Move_ShouldThrowFileNotFoundExceptionWhenSourceDoesNotExist_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = XFS.Path(@"c:\something\demo1.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<FileNotFoundException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.Equal("The file \"" + XFS.Path("c:\\something\\demo.txt" + "\" could not be found."), exception.Message);
        }

        [Fact]
        public void MockFile_Move_ShouldThrowFileNotFoundExceptionWhenSourceDoesNotExist_FileName() {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = XFS.Path(@"c:\something\demo1.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<FileNotFoundException>(() => fileSystem.File.Move(sourceFilePath, destFilePath));

            Assert.Equal(XFS.Path(@"c:\something\demo.txt"), exception.FileName);
        }

        [Fact]
        public void MockFile_Move_ShouldThrowDirectoryNotFoundExceptionWhenSourcePathDoesNotExist_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = XFS.Path(@"c:\somethingelse\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFilePath, new MockFileData(new byte[] {0})}
            });

            Action action = () => fileSystem.File.Move(sourceFilePath, destFilePath);
            action.ShouldThrow<DirectoryNotFoundException>().Where(x => x.Message.StartsWith(@"Could not find a part of the path"));
        }
    }
}