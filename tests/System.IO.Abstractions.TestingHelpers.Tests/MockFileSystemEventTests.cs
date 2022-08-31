using System.Collections.Generic;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSystemEventTests
    {
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
        public void OnFileChanging_WithFileEvent_ShouldCallOnFileChangingWithFullFilePath()
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
        public void OnFileChanging_WithDirectoryEvent_ShouldNotBeCalled()
        {
            var basePath = Path.GetFullPath("/foo/bar");
            var directoryName = "test-directory";
            bool isCalled = false;
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { directoryName, new MockFileData("some content") }
            }, basePath)
                .OnFileChanging(f => isCalled = true);

            _ = fs.Directory.CreateDirectory(directoryName);

            Assert.That(isCalled, Is.False);
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
        public void OnDirectoryChanging_WithDirectoryEvent_ShouldCallOnDirectoryChangingWithFullDirectoryPath()
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
        public void OnDirectoryChanging_WithFileEvent_ShouldNotBeCalled()
        {
            var basePath = Path.GetFullPath("/foo/bar");
            var fileName = "test-directory";
            bool isCalled = false;
            var fs = new MockFileSystem(null, basePath)
                .OnDirectoryChanging(f => isCalled = true);

            fs.File.WriteAllText(fileName, "some content");

            Assert.That(isCalled, Is.False);
        }

        [Test]
        public void File_WriteAllText_NewFile_ShouldTriggerOnFileChangingWithCreatedType()
        {
            var fileName = "foo.txt";
            MockFileEvent.FileEventType? receivedEventType = null;
            var fs = new MockFileSystem()
                .OnFileChanging(f => receivedEventType = f.EventType);

            fs.File.WriteAllText(fileName, "some content");

            Assert.That(receivedEventType, Is.EqualTo(MockFileEvent.FileEventType.Created));
        }

        [Test]
        public void File_WriteAllText_ExistingFile_ShouldTriggerOnFileChangingWithUpdatedType()
        {
            var fileName = "foo.txt";
            MockFileEvent.FileEventType? receivedEventType = null;
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { fileName, new MockFileData("some content") }
            }).OnFileChanging(f => receivedEventType = f.EventType);

            fs.File.WriteAllText(fileName, "some content");

            Assert.That(receivedEventType, Is.EqualTo(MockFileEvent.FileEventType.Updated));
        }

        [Test]
        public void File_Delete_ShouldTriggerOnFileChangingWithDeletedType()
        {
            var fileName = "foo.txt";
            MockFileEvent.FileEventType? receivedEventType = null;
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { fileName, new MockFileData("some content") }
            }).OnFileChanging(f => receivedEventType = f.EventType);

            fs.File.Delete(fileName);

            Assert.That(receivedEventType, Is.EqualTo(MockFileEvent.FileEventType.Deleted));
        }

        [Test]
        public void File_Move_ShouldTriggerOnFileChangingWithDeletedAndCreatedTypes()
        {
            var fileName = "foo.txt";
            var receivedEventTypes = new List<MockFileEvent.FileEventType>();
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { fileName, new MockFileData("some content") }
            }).OnFileChanging(f => receivedEventTypes.Add(f.EventType));

            fs.File.Move(fileName, "bar.txt");

            Assert.That(receivedEventTypes, Contains.Item(MockFileEvent.FileEventType.Deleted));
            Assert.That(receivedEventTypes, Contains.Item(MockFileEvent.FileEventType.Created));
        }

        [Test]
        public void Directory_CreateDirectory_ShouldCallOnDirectoryChangingWithFullDirectoryPath()
        {
            var directoryName = "test-directory";
            MockDirectoryEvent.DirectoryEventType? receivedEventType = null;
            var fs = new MockFileSystem()
                .OnDirectoryChanging(f => receivedEventType = f.EventType);

            fs.Directory.CreateDirectory(directoryName);

            Assert.That(receivedEventType, Is.EqualTo(MockDirectoryEvent.DirectoryEventType.Created));
        }

        [Test]
        public void Directory_DeleteDirectory_ShouldCallOnDirectoryChangingWithFullDirectoryPath()
        {
            var directoryName = "test-directory";
            MockDirectoryEvent.DirectoryEventType? receivedEventType = null;
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { directoryName, new MockDirectoryData() }
            }).OnDirectoryChanging(f => receivedEventType = f.EventType);

            fs.Directory.Delete(directoryName);

            Assert.That(receivedEventType, Is.EqualTo(MockDirectoryEvent.DirectoryEventType.Deleted));
        }

        [Test]
        public void Directory_Move_ShouldTriggerOnDirectoryChangingWithDeletedAndCreatedTypes()
        {
            var fileName = "foo.txt";
            var receivedEventTypes = new List<MockDirectoryEvent.DirectoryEventType>();
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { fileName, new MockDirectoryData() }
            }).OnDirectoryChanging(f => receivedEventTypes.Add(f.EventType));

            fs.Directory.Move(fileName, "bar.txt");

            Assert.That(receivedEventTypes, Contains.Item(MockDirectoryEvent.DirectoryEventType.Deleted));
            Assert.That(receivedEventTypes, Contains.Item(MockDirectoryEvent.DirectoryEventType.Created));
        }
    }
}
