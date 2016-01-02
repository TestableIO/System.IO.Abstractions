﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;
using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileWriteAllLinesTests
    {
        private class TestDataForWriteAllLines
        {
            public static readonly string Path = XFS.Path(@"c:\something\demo.txt");

            public static IEnumerable ForDifferentEncoding
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
                    yield return new TestCaseData(fileSystem, writeEnumberable, expectedContent)
                        .SetName("WriteAllLines(string, IEnumerable<string>)");
                    yield return new TestCaseData(fileSystem, writeEnumberableUtf32, expectedContent)
                        .SetName("WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)");

                    // string[]
                    yield return new TestCaseData(fileSystem, writeArray, expectedContent)
                        .SetName("WriteAllLines(string, string[])");
                    yield return new TestCaseData(fileSystem, writeArrayUtf32, expectedContent)
                        .SetName("WriteAllLines(string, string[], Encoding.UTF32)");
                }
            }

            public static IEnumerable ForIllegalPath
            {
                get
                {
                    const string illegalPath = "<<<";
                    return GetCasesForArgumentChecking(illegalPath);
                }
            }

            public static IEnumerable ForNullPath
            {
                get
                {
                    const string illegalPath = null;
                    return GetCasesForArgumentChecking(illegalPath);
                }
            }

            private static IEnumerable GetCasesForArgumentChecking(string path)
            {
                var fileSystem = new MockFileSystem();
                var fileContentEnumerable = new List<string>();
                var fileContentArray = fileContentEnumerable.ToArray();
                Action writeEnumberable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                Action writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);

                // IEnumerable
                yield return new TestCaseData(writeEnumberable)
                    .SetName("WriteAllLines(string, IEnumerable<string>)");
                yield return new TestCaseData(writeEnumberableUtf32)
                    .SetName("WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)");

                // string[]
                yield return new TestCaseData(writeArray)
                    .SetName("WriteAllLines(string, string[])");
                yield return new TestCaseData(writeArrayUtf32)
                    .SetName("WriteAllLines(string, string[], Encoding.UTF32)");
            }

            private static IEnumerable ForNullEncoding
            {
                get
                {
                    var fileSystem = new MockFileSystem();
                    var fileContentEnumerable = new List<string>();
                    var fileContentArray = fileContentEnumerable.ToArray();
                    Action writeEnumberableNull = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable, null);
                    Action writeArrayNull = () => fileSystem.File.WriteAllLines(Path, fileContentArray, null);

                    // IEnumerable
                    yield return new TestCaseData(writeEnumberableNull)
                        .SetName("WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)");

                    // string[]
                    yield return new TestCaseData(writeArrayNull)
                        .SetName("WriteAllLines(string, string[], Encoding.UTF32)");
                }
            }

            public static IEnumerable ForPathIsDirectory
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
                    yield return new TestCaseData(writeEnumberable, path)
                        .SetName("WriteAllLines(string, IEnumerable<string>)");
                    yield return new TestCaseData(writeEnumberableUtf32, path)
                        .SetName("WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)");

                    // string[]
                    yield return new TestCaseData(writeArray, path)
                        .SetName("WriteAllLines(string, string[])");
                    yield return new TestCaseData(writeArrayUtf32, path)
                        .SetName("WriteAllLines(string, string[], Encoding.UTF32)");
                }
            }

            public static IEnumerable ForFileIsReadOnly
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
                    yield return new TestCaseData(writeEnumberable, path)
                        .SetName("WriteAllLines(string, IEnumerable<string>)");
                    yield return new TestCaseData(writeEnumberableUtf32, path)
                        .SetName("WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)");

                    // string[]
                    yield return new TestCaseData(writeArray, path)
                        .SetName("WriteAllLines(string, string[])");
                    yield return new TestCaseData(writeArrayUtf32, path)
                        .SetName("WriteAllLines(string, string[], Encoding.UTF32)");
                }
            }

            public static IEnumerable ForContentsIsNull
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
                    yield return new TestCaseData(writeEnumberable)
                        .SetName("WriteAllLines(string, IEnumerable<string>)");
                    yield return new TestCaseData(writeEnumberableUtf32)
                        .SetName("WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)");

                    // string[]
                    yield return new TestCaseData(writeArray)
                        .SetName("WriteAllLines(string, string[])");
                    yield return new TestCaseData(writeArrayUtf32)
                        .SetName("WriteAllLines(string, string[], Encoding.UTF32)");
                }
            }
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForDifferentEncoding")]
        public void MockFile_WriteAllLinesGeneric_ShouldWriteTheCorrectContent(IMockFileDataAccessor fileSystem, Action action, string expectedContent)
        {
            // Arrange
            // is done in the test case source

            // Act
            action();

            // Assert
            var actualContent = fileSystem.GetFile(TestDataForWriteAllLines.Path).TextContents;
            Assert.Equal(expectedContent, actualContent);
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForNullPath")]
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

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForNullEncoding")]
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

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForIllegalPath")]
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

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForPathIsDirectory")]
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

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForFileIsReadOnly")]
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

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForContentsIsNull")]
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
