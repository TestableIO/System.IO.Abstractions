using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Xunit;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileWriteAllLinesTests
    {
        private class TestDataForWriteAllLines
        {
            public static readonly string Path = XFS.Path(@"c:\something\demo.txt");

            public static IEnumerable ForDifferentEncoding()
            {
                var fileSystem = new MockFileSystem();
                fileSystem.AddDirectory(@"c:\something");
                var fileContentEnumerable = new List<string> {"first line", "second line", "third line", "fourth and last line"};
                var fileContentArray = fileContentEnumerable.ToArray();
                Action writeEnumberable = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable);
                Action writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(Path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(Path, fileContentArray, Encoding.UTF32);
                var expectedContent = string.Format(CultureInfo.InvariantCulture,
                    "first line{0}second line{0}third line{0}fourth and last line{0}", Environment.NewLine);

                // IEnumerable
                yield return
                    new object[] {fileSystem, writeEnumberable, expectedContent, "WriteAllLines(string, IEnumerable<string>)"};
                yield return
                    new object[]
                    {
                        fileSystem, writeEnumberableUtf32, expectedContent,
                        "WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)"
                    };

                // string[]
                yield return new object[] {fileSystem, writeArray, expectedContent, "WriteAllLines(string, string[])"};
                yield return
                    new object[] {fileSystem, writeArrayUtf32, expectedContent, "WriteAllLines(string, string[], Encoding.UTF32)"};
            }

            public static IEnumerable<object[]> ForIllegalPath()
            {
                const string illegalPath = "<<<";
                return GetCasesForArgumentChecking(illegalPath);
            }

            public static IEnumerable<object[]> ForNullPath()
            {
                const string illegalPath = null;
                return GetCasesForArgumentChecking(illegalPath);
            }

            private static IEnumerable<object[]> GetCasesForArgumentChecking(string path)
            {
                var fileSystem = new MockFileSystem();
                var fileContentEnumerable = new List<string>();
                var fileContentArray = fileContentEnumerable.ToArray();
                Action writeEnumberable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                Action writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);

                // IEnumerable
                yield return new object[] { writeEnumberable, "WriteAllLines(string, IEnumerable<string>) input: " + path };
                yield return new object[] { writeEnumberableUtf32, "WriteAllLines(string, IEnumerable<string>, Encoding.UTF32) input: " + path };

                // string[]
                yield return new object[] { writeArray, "WriteAllLines(string, string[]) input: " + path };
                yield return new object[] { writeArrayUtf32, "WriteAllLines(string, string[], Encoding.UTF32) input: " + path };
            }

            public static IEnumerable ForNullEncoding()
            {
                var fileSystem = new MockFileSystem();
                var fileContentEnumerable = new List<string>();
                var fileContentArray = fileContentEnumerable.ToArray();
                Action writeEnumberableNull = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable, null);
                Action writeArrayNull = () => fileSystem.File.WriteAllLines(Path, fileContentArray, null);

                // IEnumerable
                yield return new object[] {writeEnumberableNull, "WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)"};

                // string[]
                yield return new object[] {writeArrayNull, "WriteAllLines(string, string[], Encoding.UTF32)"};
            }

            public static IEnumerable ForPathIsDirectory()
            {
                var fileSystem = new MockFileSystem();
                var path = XFS.Path(@"c:\something");
                fileSystem.Directory.CreateDirectory(path);
                var fileContentEnumerable = new List<string>();
                var fileContentArray = fileContentEnumerable.ToArray();
                Action writeEnumberable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                Action writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);

                // IEnumerable
                yield return new object[] {writeEnumberable, path, "WriteAllLines(string, IEnumerable<string>)"};
                yield return
                    new object[] {writeEnumberableUtf32, path, "WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)"};

                // string[]
                yield return new object[] {writeArray, path, "WriteAllLines(string, string[])"};
                yield return new object[] {writeArrayUtf32, path, "WriteAllLines(string, string[], Encoding.UTF32)"};
            }

            public static IEnumerable ForFileIsReadOnly()
            {
                var fileSystem = new MockFileSystem();
                var path = XFS.Path(@"c:\something\file.txt");
                var mockFileData = new MockFileData(string.Empty);
                mockFileData.Attributes = FileAttributes.ReadOnly;
                fileSystem.AddFile(path, mockFileData);
                var fileContentEnumerable = new List<string>();
                var fileContentArray = fileContentEnumerable.ToArray();
                Action writeEnumberable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                Action writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);

                // IEnumerable
                yield return new object[] {writeEnumberable, path, "WriteAllLines(string, IEnumerable<string>)"};
                yield return
                    new object[] {writeEnumberableUtf32, path, "WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)"};

                // string[]
                yield return new object[] {writeArray, path, "WriteAllLines(string, string[])"};
                yield return new object[] {writeArrayUtf32, path, "WriteAllLines(string, string[], Encoding.UTF32)"};
            }

            public static IEnumerable ForContentsIsNull()
            {
                var fileSystem = new MockFileSystem();
                var path = XFS.Path(@"c:\something\file.txt");
                var mockFileData = new MockFileData(string.Empty);
                mockFileData.Attributes = FileAttributes.ReadOnly;
                fileSystem.AddDirectory(@"c:\something");
                fileSystem.AddFile(path, mockFileData);
                List<string> fileContentEnumerable = null;
                string[] fileContentArray = null;

                // ReSharper disable ExpressionIsAlwaysNull
                Action writeEnumberable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                Action writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);
                // ReSharper restore ExpressionIsAlwaysNull

                // IEnumerable
                yield return new object[] {writeEnumberable, "WriteAllLines(string, IEnumerable<string>)"};
                yield return new object[] {writeEnumberableUtf32, "WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)"};

                // string[]
                yield return new object[] {writeArray, "WriteAllLines(string, string[])"};
                yield return new object[] {writeArrayUtf32, "WriteAllLines(string, string[], Encoding.UTF32)"};
            }
        }

        [Theory]
        [MemberData(nameof(TestDataForWriteAllLines.ForDifferentEncoding), MemberType = typeof(TestDataForWriteAllLines))]
        public void MockFile_WriteAllLinesGeneric_ShouldWriteTheCorrectContent(IMockFileDataAccessor fileSystem, Action action, string expectedContent, string message)
        {
            // Arrange
            // is done in the test case source

            // Act
            action();

            // Assert
            var actualContent = fileSystem.GetFile(TestDataForWriteAllLines.Path).TextContents;
            actualContent.Should().Be(actualContent, message);
        }

        [Theory]
        [MemberData(nameof(TestDataForWriteAllLines.ForNullPath), MemberType = typeof(TestDataForWriteAllLines))]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfPathIsNull(Action action, string message)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = action.ShouldThrow<ArgumentNullException>(message).Which;
            exception.Message.Should().StartWith("Value cannot be null.", message);
            exception.ParamName.Should().StartWith("path");
        }

        [Theory]
        [MemberData(nameof(TestDataForWriteAllLines.ForNullEncoding), MemberType=typeof(TestDataForWriteAllLines))]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfEncodingIsNull(Action action, string message)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            exception.Message.Should().StartWith("Value cannot be null.", message);
            exception.ParamName.Should().StartWith("encoding");
        }

        [Theory]
        [MemberData(nameof(TestDataForWriteAllLines.ForIllegalPath), MemberType=typeof(TestDataForWriteAllLines))]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentExceptionIfPathContainsIllegalCharacters(Action action, string message)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            action.ShouldThrow<ArgumentException>(message).WithMessage("Illegal characters in path.");
        }

        [Theory]
        [MemberData(nameof(TestDataForWriteAllLines.ForPathIsDirectory), MemberType=typeof(TestDataForWriteAllLines))]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnUnauthorizedAccessExceptionIfPathIsOneDirectory(Action action, string path, string message)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<UnauthorizedAccessException>(action);
            var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
            exception.Message.Should().Be(expectedMessage, message);
        }

        [Theory]
        [MemberData(nameof(TestDataForWriteAllLines.ForFileIsReadOnly), MemberType=typeof(TestDataForWriteAllLines))]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowOneUnauthorizedAccessExceptionIfFileIsReadOnly(Action action, string path, string message)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<UnauthorizedAccessException>(action);
            var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
            exception.Message.Should().Be(expectedMessage, message);
        }

        [Theory]
        [MemberData(nameof(TestDataForWriteAllLines.ForContentsIsNull), MemberType=typeof(TestDataForWriteAllLines))]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfContentsIsNull(Action action, string message)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case sourceForContentsIsNull

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            exception.Message.Should().StartWith("Value cannot be null.", message);
            exception.ParamName.Should().StartWith("contents", message);
        }
    }
}
