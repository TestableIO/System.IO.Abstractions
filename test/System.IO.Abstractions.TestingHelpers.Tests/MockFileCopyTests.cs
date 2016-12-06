using FluentAssertions;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using Globalization;

    using Linq;

    using Xunit;

    using XFS = MockUnixSupport;

    public class MockFileCopyTests {
        [Fact]
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
            Assert.Equal(copyResult.Contents, sourceContents.Contents);
        }

        [Fact]
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
            Assert.Equal(copyResult.Contents, sourceContents.Contents);
        }

        [Fact]
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

            Action action = () => fileSystem.File.Copy(sourceFileName, destFileName);
            action.ShouldThrow<IOException>(XFS.Path(@"The file c:\destination\demo.txt already exists."));
        }

        [Theory]
        [InlineData(@"c:\source\demo.txt", @"c:\source\doesnotexist\demo.txt")]
        [InlineData(@"c:\source\demo.txt", @"c:\doesnotexist\demo.txt")]
        public void MockFile_Copy_ShouldThrowExceptionWhenFolderInDestinationDoesNotExist(string sourceFilePath, string destFilePath)
        {
            string sourceFileName = XFS.Path(sourceFilePath);
            string destFileName = XFS.Path(destFilePath);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourceFileName, MockFileData.NullObject}
            });

            Action action = () => fileSystem.File.Copy(sourceFileName, destFileName);
            action.ShouldThrow<DirectoryNotFoundException>(string.Format(CultureInfo.InvariantCulture, @"Could not find a part of the path '{0}'.", destFilePath));
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowArgumentNullExceptionWhenSourceIsNull_Message()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Copy(null, destFilePath));

            exception.Message.Should().StartWith("File name cannot be null.");
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowArgumentNullExceptionWhenSourceIsNull_ParamName()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Copy(null, destFilePath));

            Assert.Equal("sourceFileName", exception.ParamName);
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowNotSupportedExceptionWhenSourceFileNameContainsInvalidChars_Message()
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
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

                exception.Message.Should().Be("Illegal characters in path.", string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowNotSupportedExceptionWhenSourcePathContainsInvalidChars_Message()
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
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

                exception.Message.Should().Be("Illegal characters in path.", string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowNotSupportedExceptionWhenTargetPathContainsInvalidChars_Message()
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
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

                exception.Message.Should().Be("Illegal characters in path.", exception.Message,
                    string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowNotSupportedExceptionWhenTargetFileNameContainsInvalidChars_Message()
        {
            if (XFS.IsUnixPlatform())
            {
                Assert.True(true, "Path.GetInvalidChars() does not return anything on Mono");
            }

            var sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            foreach (var invalidChar in fileSystem.Path.GetInvalidFileNameChars().Where(x => x != fileSystem.Path.DirectorySeparatorChar))
            {
                var destFilePath = XFS.Path(@"c:\something\demo.txt") + invalidChar;

                var exception =
                    Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

                exception.Message.Should().Be("Illegal characters in path.",
                    string.Format("Testing char: [{0:c}] \\{1:X4}", invalidChar, (int)invalidChar));
            }
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceIsEmpty_Message()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(string.Empty, destFilePath));

            exception.Message.Should().StartWith("Empty file name is not legal.");
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceIsEmpty_ParamName()
        {
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(string.Empty, destFilePath));

            Assert.Equal("sourceFileName", exception.ParamName);
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenSourceIsStringOfBlanks()
        {
            string sourceFilePath = "   ";
            string destFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

            exception.Message.Should().StartWith("The path is not of a legal form.");
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowArgumentNullExceptionWhenTargetIsNull_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Copy(sourceFilePath, null));

            exception.Message.Should().StartWith("File name cannot be null.");
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowArgumentNullExceptionWhenTargetIsNull_ParamName()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentNullException>(() => fileSystem.File.Copy(sourceFilePath, null));

            Assert.Equal("destFileName", exception.ParamName);
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenTargetIsStringOfBlanks()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            string destFilePath = "   ";
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, destFilePath));

            exception.Message.Should().StartWith("The path is not of a legal form.");
        }

        [Fact]
        public void MockFile_Copy_ShouldThrowArgumentExceptionWhenTargetIsEmpty_Message()
        {
            string sourceFilePath = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();

            var exception = Assert.Throws<ArgumentException>(() => fileSystem.File.Copy(sourceFilePath, string.Empty));

            exception.Message.Should().StartWith("Empty file name is not legal.");
        }
    }
}