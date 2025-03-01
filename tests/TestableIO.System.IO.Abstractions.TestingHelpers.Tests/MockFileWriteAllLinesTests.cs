using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests;

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
                Action writeEnumerable = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable);
                Action writeEnumerableUtf32 = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(Path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(Path, fileContentArray, Encoding.UTF32);
                var expectedContent = string.Format(CultureInfo.InvariantCulture,
                    "first line{0}second line{0}third line{0}fourth and last line{0}", Environment.NewLine);

                // IEnumerable
                yield return new TestCaseData(fileSystem, writeEnumerable, expectedContent)
                    .SetName("WriteAllLines(string, IEnumerable<string>)");
                yield return new TestCaseData(fileSystem, writeEnumerableUtf32, expectedContent)
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
            Action writeEnumerable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
            Action writeEnumerableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
            Action writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
            Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);

            // IEnumerable
            yield return new TestCaseData(writeEnumerable)
                .SetName("WriteAllLines(string, IEnumerable<string>) input: " + path);
            yield return new TestCaseData(writeEnumerableUtf32)
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
                Action writeEnumerableNull = () => fileSystem.File.WriteAllLines(Path, fileContentEnumerable, null);
                Action writeArrayNull = () => fileSystem.File.WriteAllLines(Path, fileContentArray, null);

                // IEnumerable
                yield return new TestCaseData(writeEnumerableNull)
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
                Action writeEnumerable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                Action writeEnumerableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);

                // IEnumerable
                yield return new TestCaseData(writeEnumerable, path)
                    .SetName("WriteAllLines(string, IEnumerable<string>)");
                yield return new TestCaseData(writeEnumerableUtf32, path)
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
                Action writeEnumerable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                Action writeEnumerableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);

                // IEnumerable
                yield return new TestCaseData(writeEnumerable, path)
                    .SetName("WriteAllLines(string, IEnumerable<string>)");
                yield return new TestCaseData(writeEnumerableUtf32, path)
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
                Action writeEnumerable = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable);
                Action writeEnumerableUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentEnumerable, Encoding.UTF32);
                Action writeArray = () => fileSystem.File.WriteAllLines(path, fileContentArray);
                Action writeArrayUtf32 = () => fileSystem.File.WriteAllLines(path, fileContentArray, Encoding.UTF32);
                // ReSharper restore ExpressionIsAlwaysNull

                // IEnumerable
                yield return new TestCaseData(writeEnumerable)
                    .SetName("WriteAllLines(string, IEnumerable<string>)");
                yield return new TestCaseData(writeEnumerableUtf32)
                    .SetName("WriteAllLines(string, IEnumerable<string>, Encoding.UTF32)");

                // string[]
                yield return new TestCaseData(writeArray)
                    .SetName("WriteAllLines(string, string[])");
                yield return new TestCaseData(writeArrayUtf32)
                    .SetName("WriteAllLines(string, string[], Encoding.UTF32)");
            }
        }

#if FEATURE_ASYNC_FILE
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
            Func<Task> writeEnumberable = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable);
            Func<Task> writeEnumberableUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable, Encoding.UTF32);
            Func<Task> writeArray = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray);
            Func<Task> writeArrayUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray, Encoding.UTF32);

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
                Func<Task> writeEnumberableNull = () => fileSystem.File.WriteAllLinesAsync(Path, fileContentEnumerable, null);
                Func<Task> writeArrayNull = () => fileSystem.File.WriteAllLinesAsync(Path, fileContentArray, null);

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
                Func<Task> writeEnumberable = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable);
                Func<Task> writeEnumberableUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable, Encoding.UTF32);
                Func<Task> writeArray = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray);
                Func<Task> writeArrayUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray, Encoding.UTF32);

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
                Func<Task> writeEnumberable = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable);
                Func<Task> writeEnumberableUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable, Encoding.UTF32);
                Func<Task> writeArray = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray);
                Func<Task> writeArrayUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray, Encoding.UTF32);

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
                Func<Task> writeEnumberable = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable);
                Func<Task> writeEnumberableUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentEnumerable, Encoding.UTF32);
                Func<Task> writeArray = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray);
                Func<Task> writeArrayUtf32 = () => fileSystem.File.WriteAllLinesAsync(path, fileContentArray, Encoding.UTF32);
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

        [Test]
        public async Task MockFile_WriteAllLinesAsync_ShouldThrowOperationCanceledExceptionIfCancelled()
        {
            // Arrange
            const string path = "test.txt";
            var fileSystem = new MockFileSystem();

            // Act
            async Task Act() =>
                await fileSystem.File.WriteAllLinesAsync(
                    path,
                    new[] { "line 1", "line 2" },
                    new CancellationToken(canceled: true));
            await That(Act).Throws<OperationCanceledException>();

            // Assert
            await That(fileSystem.File.Exists(path)).IsFalse();
        }
#endif

    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForDifferentEncoding))]
    public async Task MockFile_WriteAllLinesGeneric_ShouldWriteTheCorrectContent(IMockFileDataAccessor fileSystem, Action action, string expectedContent)
    {
        // Arrange
        // is done in the test case source

        // Act
        action();

        // Assert
        var actualContent = fileSystem.GetFile(TestDataForWriteAllLines.Path).TextContents;
        await That(actualContent).IsEqualTo(expectedContent);
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForNullPath))]
    public async Task MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfPathIsNull(Action action)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case source

        // Assert
        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.Message).StartsWith("Value cannot be null.");
        await That(exception.ParamName).StartsWith("path");
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), "ForNullEncoding")]
    public async Task MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfEncodingIsNull(Action action)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case source

        // Assert
        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.Message).StartsWith("Value cannot be null.");
        await That(exception.ParamName).StartsWith("encoding");
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForIllegalPath))]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentExceptionIfPathContainsIllegalCharacters(Action action)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case source

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.Message).IsEqualTo("Illegal characters in path.");
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForPathIsDirectory))]
    public async Task MockFile_WriteAllLinesGeneric_ShouldThrowAnUnauthorizedAccessExceptionIfPathIsOneDirectory(Action action, string path)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case source

        // Assert
        var exception = await That(action).Throws<UnauthorizedAccessException>();
        var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
        await That(exception.Message).IsEqualTo(expectedMessage);
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForFileIsReadOnly))]
    public async Task MockFile_WriteAllLinesGeneric_ShouldThrowOneUnauthorizedAccessExceptionIfFileIsReadOnly(Action action, string path)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case source

        // Assert
        var exception = await That(action).Throws<UnauthorizedAccessException>();
        var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
        await That(exception.Message).IsEqualTo(expectedMessage);
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForContentsIsNull))]
    public async Task MockFile_WriteAllLinesGeneric_ShouldThrowAnArgumentNullExceptionIfContentsIsNull(Action action)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case sourceForContentsIsNull

        // Assert
        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.Message).StartsWith("Value cannot be null.");
        await That(exception.ParamName).IsEqualTo("contents");
    }

#if FEATURE_ASYNC_FILE
    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForDifferentEncodingAsync))]
    public async Task MockFile_WriteAllLinesAsyncGeneric_ShouldWriteTheCorrectContent(IMockFileDataAccessor fileSystem, Action action, string expectedContent)
    {
        // Arrange
        // is done in the test case source

        // Act
        action();

        // Assert
        var actualContent = fileSystem.GetFile(TestDataForWriteAllLines.Path).TextContents;
        await That(actualContent).IsEqualTo(expectedContent);
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForNullPathAsync))]
    public async Task MockFile_WriteAllLinesAsyncGeneric_ShouldThrowAnArgumentNullExceptionIfPathIsNull(Func<Task> action)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case source

        // Assert
        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.Message).StartsWith("Value cannot be null.");
        await That(exception.ParamName).StartsWith("path");
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), "ForNullEncodingAsync")]
    public async Task MockFile_WriteAllLinesAsyncGeneric_ShouldThrowAnArgumentNullExceptionIfEncodingIsNull(Func<Task> action)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case source

        // Assert
        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.Message).StartsWith("Value cannot be null.");
        await That(exception.ParamName).StartsWith("encoding");
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForIllegalPathAsync))]
    [WindowsOnly(WindowsSpecifics.StrictPathRules)]
    public async Task MockFile_WriteAllLinesAsyncGeneric_ShouldThrowAnArgumentExceptionIfPathContainsIllegalCharacters(Func<Task> action)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case source

        // Assert
        var exception = await That(action).Throws<ArgumentException>();
        await That(exception.Message).IsEqualTo("Illegal characters in path.");
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForPathIsDirectoryAsync))]
    public async Task MockFile_WriteAllLinesAsyncGeneric_ShouldThrowAnUnauthorizedAccessExceptionIfPathIsOneDirectory(Func<Task> action, string path)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case source

        // Assert
        var exception = await That(action).Throws<UnauthorizedAccessException>();
        var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
        await That(exception.Message).IsEqualTo(expectedMessage);
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForFileIsReadOnlyAsync))]
    public async Task MockFile_WriteAllLinesAsyncGeneric_ShouldThrowOneUnauthorizedAccessExceptionIfFileIsReadOnly(Func<Task> action, string path)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case source

        // Assert
        var exception = await That(action).Throws<UnauthorizedAccessException>();
        var expectedMessage = string.Format(CultureInfo.InvariantCulture, "Access to the path '{0}' is denied.", path);
        await That(exception.Message).IsEqualTo(expectedMessage);
    }

    [TestCaseSource(typeof(TestDataForWriteAllLines), nameof(TestDataForWriteAllLines.ForContentsIsNullAsync))]
    public async Task MockFile_WriteAllLinesAsyncGeneric_ShouldThrowAnArgumentNullExceptionIfContentsIsNull(Func<Task> action)
    {
        // Arrange
        // is done in the test case source

        // Act
        // is done in the test case sourceForContentsIsNull

        // Assert
        var exception = await That(action).Throws<ArgumentNullException>();
        await That(exception.Message).StartsWith("Value cannot be null.");
        await That(exception.ParamName).IsEqualTo("contents");
    }
#endif
}