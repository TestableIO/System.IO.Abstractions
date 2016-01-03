using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;
using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileWriteAllLinesTests
    {
        public static readonly string Path = XFS.Path(@"c:\something\demo.txt");

        public static IEnumerable<object[]> ForDifferentEncoding
        {
            get
            {
                var fileSystem = new MockFileSystem();
                var fileContentEnumerable = new List<string> { "first line", "second line", "third line", "fourth and last line" };
                var fileContentArray = fileContentEnumerable.ToArray();
                Action writeEnumberable = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable);
                Action writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(Path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(Path, fileContentArray, Encoding.UTF32);
                var expectedContent = string.Format(CultureInfo.InvariantCulture,
                    "first line{0}second line{0}third line{0}fourth and last line{0}", Environment.NewLine);

                // IEnumerable
                yield return new object[] { fileSystem, writeEnumberable, expectedContent };
                yield return new object[] { fileSystem, writeEnumberableUtf32, expectedContent };

                // string[]
                yield return new object[] { fileSystem, writeArray, expectedContent };
                yield return new object[] { fileSystem, writeArrayUtf32, expectedContent };
            }
        }

        public static IEnumerable<object[]> ForIllegalPath
        {
            get
            {
                const string illegalPath = "<<<";
                return GetCasesForArgumentChecking(illegalPath);
            }
        }

        public static IEnumerable<object[]> ForNullPath
        {
            get
            {
                const string illegalPath = null;
                return GetCasesForArgumentChecking(illegalPath);
            }
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
            yield return new object[] { writeEnumberable };
            yield return new object[] { writeEnumberableUtf32 };

            // string[]
            yield return new object[] { writeArray };
            yield return new object[] { writeArrayUtf32 };
        }

        private static IEnumerable<object[]> ForNullEncoding
        {
            get
            {
                var fileSystem = new MockFileSystem();
                var fileContentEnumerable = new List<string>();
                var fileContentArray = fileContentEnumerable.ToArray();
                Action writeEnumberableNull = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable, null);
                Action writeArrayNull = () => fileSystem.File.WriteAllLines(Path, fileContentArray, null);

                // IEnumerable
                yield return new object[] { writeEnumberableNull };

                // string[]
                yield return new object[] { writeArrayNull };
            }
        }

        public static IEnumerable<object[]> ForPathIsDirectory
        {
            get
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
                yield return new object[] { writeEnumberable, path };
                yield return new object[] { writeEnumberableUtf32, path };

                // string[]
                yield return new object[] { writeArray, path };
                yield return new object[] { writeArrayUtf32, path };
            }
        }

        public static IEnumerable<object[]> ForFileIsReadOnly
        {
            get
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
                yield return new object[] { writeEnumberable, path };
                yield return new object[] { writeEnumberableUtf32, path };

                // string[]
                yield return new object[] { writeArray, path };
                yield return new object[] { writeArrayUtf32, path };
            }
        }

        public static IEnumerable<object[]> ForContentsIsNull
        {
            get
            {
                var fileSystem = new MockFileSystem();
                var path = XFS.Path(@"c:\something\file.txt");
                var mockFileData = new MockFileData(string.Empty);
                mockFileData.Attributes = FileAttributes.ReadOnly;
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
                yield return new object[] { writeEnumberable };
                yield return new object[] { writeEnumberableUtf32 };

                // string[]
                yield return new object[] { writeArray };
                yield return new object[] { writeArrayUtf32 };
            }
        }

        [MemberData("ForDifferentEncoding")]
        public void MockFile_WriteAllLinesGeneric_ShouldWriteTheCorrectContent(IMockFileDataAccessor fileSystem, Action action, string expectedContent)
        {
            // Arrange
            // is done in the test case source

            // Act
            action();

            // Assert
            var actualContent = fileSystem.GetFile(Path).TextContents;
            Assert.Equal(expectedContent, actualContent);
        }

        [MemberData("ForNullPath")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfPathIsNull(Action action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.StartsWith("Value cannot be null.", exception.Message);
            Assert.StartsWith("path", exception.ParamName);
        }

        [MemberData("ForNullEncoding")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfEncodingIsNull(Action action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.StartsWith("Value cannot be null.", exception.Message);
            Assert.StartsWith("encoding", exception.ParamName);
        }

        [MemberData("ForIllegalPath")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentExceptionIfPathContainsIllegalCharacters(Action action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("Illegal characters in path.", exception.Message);
        }

        [MemberData("ForPathIsDirectory")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnUnauthorizedAccessExceptionIfPathIsOneDirectory(Action action, string path)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<UnauthorizedAccessException>(action);
            var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
            Assert.Equal(expectedMessage, exception.Message);
        }

        [MemberData("ForFileIsReadOnly")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowOneUnauthorizedAccessExceptionIfFileIsReadOnly(Action action, string path)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<UnauthorizedAccessException>(action);
            var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
            Assert.Equal(expectedMessage, exception.Message);
        }

        [MemberData("ForContentsIsNull")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfContentsIsNull(Action action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case sourceForContentsIsNull

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.StartsWith("Value cannot be null.", exception.Message);
            Assert.Equal("contents", exception.ParamName);
        }
    }
}
