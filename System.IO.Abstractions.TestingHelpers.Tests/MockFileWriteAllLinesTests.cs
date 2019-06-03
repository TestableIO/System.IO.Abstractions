using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

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
                    fileSystem.AddDirectory(XFS.Path(@"c:\something"));
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
                TestDelegate writeEnumberable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                TestDelegate writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                TestDelegate writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                TestDelegate writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);

                // IEnumerable
                yield return new TestCaseData(writeEnumberable)
                    .SetName("WriteAllLines(string, IEnumerable<string>) input: " + path);
                yield return new TestCaseData(writeEnumberableUtf32)
                    .SetName("WriteAllLines(string, IEnumerable<string>, Encoding.UTF32) input: " + path);

                // string[]
                yield return new TestCaseData(writeArray)
                    .SetName("WriteAllLines(string, string[]) input: " + path);
                yield return new TestCaseData(writeArrayUtf32)
                    .SetName("WriteAllLines(string, string[], Encoding.UTF32) input: " + path);
            }

            private static IEnumerable ForNullEncoding
            {
                get
                {
                    var fileSystem = new MockFileSystem();
                    var fileContentEnumerable = new List<string>();
                    var fileContentArray = fileContentEnumerable.ToArray();
                    TestDelegate writeEnumberableNull = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable, null);
                    TestDelegate writeArrayNull = () => fileSystem.File.WriteAllLines(Path, fileContentArray, null);

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
                    TestDelegate writeEnumberable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                    TestDelegate writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                    TestDelegate writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                    TestDelegate writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);

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
                    TestDelegate writeEnumberable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                    TestDelegate writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                    TestDelegate writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                    TestDelegate writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);

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
                    fileSystem.AddDirectory(XFS.Path(@"c:\something"));
                    fileSystem.AddFile(path, mockFileData);
                    List<string> fileContentEnumerable = null;
                    string[] fileContentArray = null;

                    // ReSharper disable ExpressionIsAlwaysNull
                    TestDelegate writeEnumberable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                    TestDelegate writeEnumberableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                    TestDelegate writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                    TestDelegate writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);
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

#if NETCOREAPP2_0
            public static IEnumerable ForDifferentEncodingAsync
            {
                get
                {
                    var fileSystem = new MockFileSystem();
                    fileSystem.AddDirectory(XFS.Path(@"c:\something"));
                    var fileContentEnumerable = new List<string> { "first line", "second line", "third line", "fourth and last line" };
                    var fileContentArray = fileContentEnumerable.ToArray();
                    Action writeEnumberable = () => fileSystem.File.WriteAllLinesAsync(Path, fileContentEnumerable);
                    Action writeEnumberableUtf32 = () => fileSystem.File.WriteAllLinesAsync(Path, fileContentEnumerable, Encoding.UTF32);
                    Action writeArray = () => fileSystem.File.WriteAllLinesAsync(Path, fileContentArray);
                    Action writeArrayUtf32 = () => fileSystem.File.WriteAllLinesAsync(Path, fileContentArray, Encoding.UTF32);
                    var expectedContent = string.Format(CultureInfo.InvariantCulture,
                        "first line{0}second line{0}third line{0}fourth and last line{0}", Environment.NewLine);

                    // IEnumerable
                    yield return new TestCaseData(fileSystem, writeEnumberable, expectedContent)
                        .SetName("WriteAllLinesAsync(string, IEnumerable<string>)");
                    yield return new TestCaseData(fileSystem, writeEnumberableUtf32, expectedContent)
                        .SetName("WriteAllLinesAsync(string, IEnumerable<string>, Encoding.UTF32)");

                    // string[]
                    yield return new TestCaseData(fileSystem, writeArray, expectedContent)
                        .SetName("WriteAllLinesAsync(string, string[])");
                    yield return new TestCaseData(fileSystem, writeArrayUtf32, expectedContent)
                        .SetName("WriteAllLinesAsync(string, string[], Encoding.UTF32)");
                }
            }

            public static IEnumerable ForIllegalPathAsync
            {
                get
                {
                    const string illegalPath = "<<<";
                    return GetCasesForArgumentCheckingAsync(illegalPath);
                }
            }

            public static IEnumerable ForNullPathAsync
            {
                get
                {
                    const string illegalPath = null;
                    return GetCasesForArgumentCheckingAsync(illegalPath);
                }
            }

            private static IEnumerable GetCasesForArgumentCheckingAsync(string path)
            {
                var fileSystem = new MockFileSystem();
                var fileContentEnumerable = new List<string>();
                var fileContentArray = fileContentEnumerable.ToArray();
                AsyncTestDelegate writeEnumberable = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable);
                AsyncTestDelegate writeEnumberableUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable, Encoding.UTF32);
                AsyncTestDelegate writeArray = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray);
                AsyncTestDelegate writeArrayUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray, Encoding.UTF32);

                // IEnumerable
                yield return new TestCaseData(writeEnumberable)
                    .SetName("WriteAllLinesAsync(string, IEnumerable<string>) input: " + path);
                yield return new TestCaseData(writeEnumberableUtf32)
                    .SetName("WriteAllLinesAsync(string, IEnumerable<string>, Encoding.UTF32) input: " + path);

                // string[]
                yield return new TestCaseData(writeArray)
                    .SetName("WriteAllLinesAsync(string, string[]) input: " + path);
                yield return new TestCaseData(writeArrayUtf32)
                    .SetName("WriteAllLinesAsync(string, string[], Encoding.UTF32) input: " + path);
            }

            private static IEnumerable ForNullEncodingAsync
            {
                get
                {
                    var fileSystem = new MockFileSystem();
                    var fileContentEnumerable = new List<string>();
                    var fileContentArray = fileContentEnumerable.ToArray();
                    AsyncTestDelegate writeEnumberableNull = () => fileSystem.File.WriteAllLinesAsync(Path, fileContentEnumerable, null);
                    AsyncTestDelegate writeArrayNull = () => fileSystem.File.WriteAllLinesAsync(Path, fileContentArray, null);

                    // IEnumerable
                    yield return new TestCaseData(writeEnumberableNull)
                        .SetName("WriteAllLinesAsync(string, IEnumerable<string>, Encoding.UTF32)");

                    // string[]
                    yield return new TestCaseData(writeArrayNull)
                        .SetName("WriteAllLinesAsync(string, string[], Encoding.UTF32)");
                }
            }

            public static IEnumerable ForPathIsDirectoryAsync
            {
                get
                {
                    var fileSystem = new MockFileSystem();
                    var path = XFS.Path(@"c:\something");
                    fileSystem.Directory.CreateDirectory(path);
                    var fileContentEnumerable = new List<string>();
                    var fileContentArray = fileContentEnumerable.ToArray();
                    AsyncTestDelegate writeEnumberable = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable);
                    AsyncTestDelegate writeEnumberableUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable, Encoding.UTF32);
                    AsyncTestDelegate writeArray = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray);
                    AsyncTestDelegate writeArrayUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray, Encoding.UTF32);

                    // IEnumerable
                    yield return new TestCaseData(writeEnumberable, path)
                        .SetName("WriteAllLinesAsync(string, IEnumerable<string>)");
                    yield return new TestCaseData(writeEnumberableUtf32, path)
                        .SetName("WriteAllLinesAsync(string, IEnumerable<string>, Encoding.UTF32)");

                    // string[]
                    yield return new TestCaseData(writeArray, path)
                        .SetName("WriteAllLinesAsync(string, string[])");
                    yield return new TestCaseData(writeArrayUtf32, path)
                        .SetName("WriteAllLinesAsync(string, string[], Encoding.UTF32)");
                }
            }

            public static IEnumerable ForFileIsReadOnlyAsync
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
                    AsyncTestDelegate writeEnumberable = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable);
                    AsyncTestDelegate writeEnumberableUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable, Encoding.UTF32);
                    AsyncTestDelegate writeArray = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray);
                    AsyncTestDelegate writeArrayUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray, Encoding.UTF32);

                    // IEnumerable
                    yield return new TestCaseData(writeEnumberable, path)
                        .SetName("WriteAllLinesAsync(string, IEnumerable<string>)");
                    yield return new TestCaseData(writeEnumberableUtf32, path)
                        .SetName("WriteAllLinesAsync(string, IEnumerable<string>, Encoding.UTF32)");

                    // string[]
                    yield return new TestCaseData(writeArray, path)
                        .SetName("WriteAllLinesAsync(string, string[])");
                    yield return new TestCaseData(writeArrayUtf32, path)
                        .SetName("WriteAllLinesAsync(string, string[], Encoding.UTF32)");
                }
            }

            public static IEnumerable ForContentsIsNullAsync
            {
                get
                {
                    var fileSystem = new MockFileSystem();
                    var path = XFS.Path(@"c:\something\file.txt");
                    var mockFileData = new MockFileData(string.Empty);
                    mockFileData.Attributes = FileAttributes.ReadOnly;
                    fileSystem.AddDirectory(XFS.Path(@"c:\something"));
                    fileSystem.AddFile(path, mockFileData);
                    List<string> fileContentEnumerable = null;
                    string[] fileContentArray = null;

                    // ReSharper disable ExpressionIsAlwaysNull
                    AsyncTestDelegate writeEnumberable = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable);
                    AsyncTestDelegate writeEnumberableUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable, Encoding.UTF32);
                    AsyncTestDelegate writeArray = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray);
                    AsyncTestDelegate writeArrayUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray, Encoding.UTF32);
                    // ReSharper restore ExpressionIsAlwaysNull

                    // IEnumerable
                    yield return new TestCaseData(writeEnumberable)
                        .SetName("WriteAllLinesAsync(string, IEnumerable<string>)");
                    yield return new TestCaseData(writeEnumberableUtf32)
                        .SetName("WriteAllLinesAsync(string, IEnumerable<string>, Encoding.UTF32)");

                    // string[]
                    yield return new TestCaseData(writeArray)
                        .SetName("WriteAllLinesAsync(string, string[])");
                    yield return new TestCaseData(writeArrayUtf32)
                        .SetName("WriteAllLinesAsync(string, string[], Encoding.UTF32)");
                }
            }
#endif

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
            Assert.That(actualContent, Is.EqualTo(expectedContent));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForNullPath")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfPathIsNull(TestDelegate action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.That(exception.Message, Does.StartWith("Value cannot be null."));
            Assert.That(exception.ParamName, Does.StartWith("path"));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForNullEncoding")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfEncodingIsNull(TestDelegate action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.That(exception.Message, Does.StartWith("Value cannot be null."));
            Assert.That(exception.ParamName, Does.StartWith("encoding"));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForIllegalPath")]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentExceptionIfPathContainsIllegalCharacters(TestDelegate action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.Message, Is.EqualTo("Illegal characters in path."));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForPathIsDirectory")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnUnauthorizedAccessExceptionIfPathIsOneDirectory(TestDelegate action, string path)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<UnauthorizedAccessException>(action);
            var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForFileIsReadOnly")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowOneUnauthorizedAccessExceptionIfFileIsReadOnly(TestDelegate action, string path)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.Throws<UnauthorizedAccessException>(action);
            var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForContentsIsNull")]
        public void MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfContentsIsNull(TestDelegate action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case sourceForContentsIsNull

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.That(exception.Message, Does.StartWith("Value cannot be null."));
            Assert.That(exception.ParamName, Is.EqualTo("contents"));
        }

#if NETCOREAPP2_0
        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForDifferentEncodingAsync")]
        public void MockFile_WriteAllLinesAsyncGeneric_ShouldWriteTheCorrectContent(IMockFileDataAccessor fileSystem, Action action, string expectedContent)
        {
            // Arrange
            // is done in the test case source

            // Act
            action();

            // Assert
            var actualContent = fileSystem.GetFile(TestDataForWriteAllLines.Path).TextContents;
            Assert.That(actualContent, Is.EqualTo(expectedContent));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForNullPathAsync")]
        public void MockFile_WriteAllLinesAsyncGeneric_ShouldThrowAnArgumentNullExceptionIfPathIsNull(AsyncTestDelegate action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.That(exception.Message, Does.StartWith("Value cannot be null."));
            Assert.That(exception.ParamName, Does.StartWith("path"));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForNullEncodingAsync")]
        public void MockFile_WriteAllLinesAsyncGeneric_ShouldThrowAnArgumentNullExceptionIfEncodingIsNull(AsyncTestDelegate action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.That(exception.Message, Does.StartWith("Value cannot be null."));
            Assert.That(exception.ParamName, Does.StartWith("encoding"));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForIllegalPathAsync")]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockFile_WriteAllLinesAsyncGeneric_ShouldThrowAnArgumentExceptionIfPathContainsIllegalCharacters(AsyncTestDelegate action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(action);
            Assert.That(exception.Message, Is.EqualTo("Illegal characters in path."));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForPathIsDirectoryAsync")]
        public void MockFile_WriteAllLinesAsyncGeneric_ShouldThrowAnUnauthorizedAccessExceptionIfPathIsOneDirectory(AsyncTestDelegate action, string path)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.ThrowsAsync<UnauthorizedAccessException>(action);
            var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForFileIsReadOnlyAsync")]
        public void MockFile_WriteAllLinesAsyncGeneric_ShouldThrowOneUnauthorizedAccessExceptionIfFileIsReadOnly(AsyncTestDelegate action, string path)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case source

            // Assert
            var exception = Assert.ThrowsAsync<UnauthorizedAccessException>(action);
            var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [TestCaseSource(typeof(TestDataForWriteAllLines), "ForContentsIsNullAsync")]
        public void MockFile_WriteAllLinesAsyncGeneric_ShouldThrowAnArgumentNullExceptionIfContentsIsNull(AsyncTestDelegate action)
        {
            // Arrange
            // is done in the test case source

            // Act
            // is done in the test case sourceForContentsIsNull

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.That(exception.Message, Does.StartWith("Value cannot be null."));
            Assert.That(exception.ParamName, Is.EqualTo("contents"));
        }
#endif
    }
}
