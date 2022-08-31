using System.Collections.Generic;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSystemEventTests
    {
        [Test]
        public void OnFileChanging_WriteAllText_ShouldBeCalledWithFullFilePath()
        {
            var basePath = Path.GetFullPath("/foo/bar");
            var fileName = "foo.txt";
            var expectedPath = Path.Combine(basePath, fileName);
            var calledPath = string.Empty;
            var fs = new MockFileSystem(null, basePath)
                .OnFileChanging(f => calledPath = f.Path);

            fs.File.WriteAllText(fileName, "some content");

            Assert.That(calledPath, Is.EqualTo(expectedPath));
        }

        [Test]
        public void OnFileChanging_SetExceptionToThrow_ShouldThrowExceptionAndNotCreateFile()
        {
            var basePath = Path.GetFullPath("/foo/bar");
            var fileName = "foo.txt";
            var exception = new Exception("the file should not be created");
            var expectedPath = Path.Combine(basePath, fileName);
            var fs = new MockFileSystem(null, basePath)
                .OnFileChanging(f => f.ExceptionToThrow = exception);

            var receivedException = Assert.Throws<Exception>(() => fs.File.WriteAllText(fileName, "some content"));
            var result = fs.File.Exists(expectedPath);

            Assert.That(receivedException, Is.EqualTo(exception));
            Assert.That(result, Is.False);
        }

        [Test]
        public void OnFileChanging_ReadAllText_ShouldNotBeCalled()
        {
            var basePath = Path.GetFullPath("/foo/bar");
            var fileName = "foo.txt";
            bool isCalled = false;
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { fileName, new MockFileData("some content") }
            }, basePath)
                .OnFileChanging(f => isCalled = true);

            _ = fs.File.ReadAllText(fileName);

            Assert.That(isCalled, Is.False);
        }

        [Test]
        public void OnDirectoryChanging_WriteAllText_ShouldBeCalledWithFullDirectoryPath()
        {
            var basePath = Path.GetFullPath("/foo/bar");
            var directoryName = "test-directory";
            var expectedPath = Path.Combine(basePath, directoryName);
            var calledPath = string.Empty;
            var fs = new MockFileSystem(null, basePath)
                .OnDirectoryChanging(f => calledPath = f.Path);

            fs.Directory.CreateDirectory(directoryName);

            Assert.That(calledPath, Is.EqualTo(expectedPath));
        }

        [Test]
        public void OnDirectoryChanging_SetExceptionToThrow_ShouldThrowExceptionAndNotCreateDirectory()
        {
            var basePath = Path.GetFullPath("/foo/bar");
            var directoryName = "test-directory";
            var exception = new Exception("the directory should not be created");
            var expectedPath = Path.Combine(basePath, directoryName);
            var fs = new MockFileSystem(null, basePath)
                .OnDirectoryChanging(f => f.ExceptionToThrow = exception);

            var receivedException = Assert.Throws<Exception>(() => fs.Directory.CreateDirectory(directoryName));
            var result = fs.Directory.Exists(expectedPath);

            Assert.That(receivedException, Is.EqualTo(exception));
            Assert.That(result, Is.False);
        }

        [Test]
        public void OnDirectoryChanging_ReadAllText_ShouldNotBeCalled()
        {
            var basePath = Path.GetFullPath("/foo/bar");
            var directoryName = "test-directory";
            bool isCalled = false;
            var fs = new MockFileSystem(null, basePath)
                .OnDirectoryChanging(f => isCalled = true);

            _ = fs.Directory.Exists(directoryName);

            Assert.That(isCalled, Is.False);
        }
    }
}
