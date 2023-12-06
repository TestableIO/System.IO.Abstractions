﻿using System.Collections.Generic;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;
using System.Threading.Tasks;
using System.Threading;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileWriteAllBytesTests
    {
        [Test]
        public void MockFile_WriteAllBytes_ShouldThrowDirectoryNotFoundExceptionIfPathDoesNotExists()
        {
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\file.txt");
            var fileContent = new byte[] { 1, 2, 3, 4 };

            TestDelegate action = () => fileSystem.File.WriteAllBytes(path, fileContent);

            Assert.Throws<DirectoryNotFoundException>(action);
        }

        [Test]
        public void MockFile_WriteAllBytes_ShouldWriteDataToMemoryFileSystem()
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            fileSystem.File.WriteAllBytes(path, fileContent);

            Assert.That(fileSystem.GetFile(path).Contents, Is.EqualTo(fileContent));
        }

        [Test]
        public void MockFile_WriteAllBytes_ShouldThrowAnUnauthorizedAccessExceptionIfFileIsHidden()
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("this is hidden") },
            });
            fileSystem.File.SetAttributes(path, FileAttributes.Hidden);

            TestDelegate action = () => fileSystem.File.WriteAllBytes(path, new byte[] { 123 });

            Assert.Throws<UnauthorizedAccessException>(action, "Access to the path '{0}' is denied.", path);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockFile_WriteAllBytes_ShouldThrowAnArgumentExceptionIfContainsIllegalCharacters()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:"));

            TestDelegate action = () => fileSystem.File.WriteAllBytes(XFS.Path(@"C:\a<b.txt"), new byte[] { 123 });

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void MockFile_WriteAllBytes_ShouldThrowAnArgumentNullExceptionIfPathIsNull()
        {
            var fileSystem = new MockFileSystem();

            TestDelegate action = () => fileSystem.File.WriteAllBytes(null, new byte[] { 123 });

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void MockFile_WriteAllBytes_ShouldThrowAnArgumentNullExceptionIfBytesAreNull()
        {
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\demo.txt");

            TestDelegate action = () => fileSystem.File.WriteAllBytes(path, null);

            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.That(exception.Message, Does.StartWith("Value cannot be null."));
            Assert.That(exception.ParamName, Is.EqualTo("bytes"));
        }

        [Test]
        public void MockFile_WriteAllBytes_ShouldWriteASeparateCopyToTheFileSystem()
        {
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\file.bin");
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));
            var fileContent = new byte[] { 1, 2, 3, 4 };

            fileSystem.File.WriteAllBytes(path, fileContent);

            for(int i = 0; i < fileContent.Length; i++)
            {
                fileContent[i] += 1;
            }

            var readAgain = fileSystem.File.ReadAllBytes(path);

            Assert.That(fileContent, Is.Not.EqualTo(readAgain));
        }


#if FEATURE_ASYNC_FILE
        [Test]
        public void MockFile_WriteAllBytesAsync_ShouldThrowDirectoryNotFoundExceptionIfPathDoesNotExists()
        {
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\file.txt");
            var fileContent = new byte[] { 1, 2, 3, 4 };

            AsyncTestDelegate action = () => fileSystem.File.WriteAllBytesAsync(path, fileContent);

            Assert.ThrowsAsync<DirectoryNotFoundException>(action);
        }

        [Test]
        public void MockFile_WriteAllTextAsync_ShouldThrowOperationCanceledExceptionIfCancelled()
        {
            // Arrange
            const string path = "test.txt";
            var fileSystem = new MockFileSystem();

            // Act
            Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await fileSystem.File.WriteAllTextAsync(
                    path,
                    "content",
                    new CancellationToken(canceled: true))
            );

            // Assert
            Assert.That(fileSystem.File.Exists(path), Is.False);
        }

        [Test]
        public async Task MockFile_WriteAllBytesAsync_ShouldWriteDataToMemoryFileSystem()
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem();
            var fileContent = new byte[] { 1, 2, 3, 4 };
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            await fileSystem.File.WriteAllBytesAsync(path, fileContent);

            Assert.That(fileSystem.GetFile(path).Contents, Is.EqualTo(fileContent));
        }

        [Test]
        public void MockFile_WriteAllBytesAsync_ShouldThrowAnUnauthorizedAccessExceptionIfFileIsHidden()
        {
            string path = XFS.Path(@"c:\something\demo.txt");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { path, new MockFileData("this is hidden") },
            });
            fileSystem.File.SetAttributes(path, FileAttributes.Hidden);

            AsyncTestDelegate action = () => fileSystem.File.WriteAllBytesAsync(path, new byte[] { 123 });

            Assert.ThrowsAsync<UnauthorizedAccessException>(action, "Access to the path '{0}' is denied.", path);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.StrictPathRules)]
        public void MockFile_WriteAllBytesAsync_ShouldThrowAnArgumentExceptionIfContainsIllegalCharacters()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:"));

            AsyncTestDelegate action = () => fileSystem.File.WriteAllBytesAsync(XFS.Path(@"C:\a<b.txt"), new byte[] { 123 });

            Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Test]
        public void MockFile_WriteAllBytesAsync_ShouldThrowAnArgumentNullExceptionIfPathIsNull()
        {
            var fileSystem = new MockFileSystem();

            AsyncTestDelegate action = () => fileSystem.File.WriteAllBytesAsync(null, new byte[] { 123 });

            Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Test]
        public void MockFile_WriteAllBytesAsync_ShouldThrowAnArgumentNullExceptionIfBytesAreNull()
        {
            var fileSystem = new MockFileSystem();
            string path = XFS.Path(@"c:\something\demo.txt");

            AsyncTestDelegate action = () => fileSystem.File.WriteAllBytesAsync(path, null);

            var exception = Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.That(exception.Message, Does.StartWith("Value cannot be null."));
            Assert.That(exception.ParamName, Is.EqualTo("bytes"));
        }
#endif
    }
}
