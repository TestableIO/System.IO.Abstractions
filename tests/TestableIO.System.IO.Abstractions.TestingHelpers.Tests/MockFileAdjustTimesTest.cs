using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Text;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileAdjustTimesTest
    {
        [Test]
        public void MockFile_AfterAppendAllText_ShouldUpdateLastAccessAndLastWriteTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.AppendAllText("foo.txt", "xyz");

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(updateTime));
        }

        [Test]
        public void MockFile_AfterCopy_ShouldUpdateCreationAndLastAccessTimeOfDestination()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.Copy("foo.txt", "bar.txt");

            var actualSourceCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualDestinationCreationTime = fileSystem.File.GetCreationTimeUtc("bar.txt");
            var actualSourceLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualDestinationLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("bar.txt");
            var actualSourceLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");
            var actualDestinationLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("bar.txt");

            Assert.That(actualSourceCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualDestinationCreationTime, Is.EqualTo(updateTime));
            Assert.That(actualSourceLastAccessTime, Is.EqualTo(creationTime));
            Assert.That(actualDestinationLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualSourceLastWriteTime, Is.EqualTo(creationTime));
            Assert.That(actualDestinationLastWriteTime, Is.EqualTo(creationTime));
        }

        [Test]
        public void MockFile_AfterMove_ShouldUpdateLastAccessTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.Move("foo.txt", "bar.txt");

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("bar.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("bar.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("bar.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(creationTime));
        }

        [TestCase(FileMode.Open, FileAccess.ReadWrite)]
        [TestCase(FileMode.OpenOrCreate, FileAccess.Write)]
        [TestCase(FileMode.Append, FileAccess.Write)]
        public void MockFile_AfterOpen_WithWriteAccess_ShouldUpdateLastAccessAndLastWriteTime(FileMode fileMode, FileAccess fileAccess)
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.Open("foo.txt", fileMode, fileAccess);

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(updateTime));
        }

        [TestCase(FileMode.Open, FileAccess.Read)]
        [TestCase(FileMode.OpenOrCreate, FileAccess.Read)]
        public void MockFile_AfterOpen_WithReadOnlyAccess_ShouldUpdateLastAccessTime(FileMode fileMode, FileAccess fileAccess)
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.Open("foo.txt", fileMode, fileAccess);

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(creationTime));
        }

        [Test]
        public void MockFile_AfterReadAllBytes_ShouldUpdateLastAccessTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.ReadAllBytes("foo.txt");

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(creationTime));
        }

        [Test]
        public void MockFile_AfterReadAllLines_ShouldUpdateLastAccessTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.ReadAllLines("foo.txt");

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(creationTime));
        }

        [Test]
        public void MockFile_AfterReadAllText_ShouldUpdateLastAccessTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.ReadAllText("foo.txt");

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(creationTime));
        }

        [Test]
        public void MockFile_AfterSetAttributes_ShouldUpdateLastAccessTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.SetAttributes("foo.txt", FileAttributes.Hidden);

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(creationTime));
        }

        [Test]
        [SupportedOSPlatform("windows")]
        [WindowsOnly(WindowsSpecifics.AccessControlLists)]
        public void MockFile_AfterSetAccessControl_ShouldUpdateLastAccessTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.SetAccessControl("foo.txt", new FileSecurity());

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(creationTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(creationTime));
        }

        [Test]
        public void MockFile_AfterWriteAllBytes_ShouldUpdateLastAccessAndLastWriteTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.WriteAllBytes("foo.txt", Encoding.UTF8.GetBytes("xyz"));

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(updateTime));
        }

        [Test]
        public void MockFile_AfterWriteAllText_ShouldUpdateLastAccessAndLastWriteTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            fileSystem.File.WriteAllText("foo.txt", "xyz");

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(updateTime));
        }

        [Test]
        public void MockFileStream_OpenRead_ShouldUpdateLastAccessTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            _ = fileSystem.File.OpenRead("foo.txt");

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(creationTime));
        }

        [Test]
        public void MockFileStream_OpenWrite_ShouldUpdateLastAccessAndLastWriteTime()
        {
            var creationTime = DateTime.UtcNow.AddDays(10);
            var updateTime = creationTime.AddDays(10);
            var fileSystem = new MockFileSystem()
                .MockTime(() => creationTime);
            fileSystem.File.WriteAllText("foo.txt", "abc");
            fileSystem.MockTime(() => updateTime);
            _ = fileSystem.File.OpenWrite("foo.txt");

            var actualCreationTime = fileSystem.File.GetCreationTimeUtc("foo.txt");
            var actualLastAccessTime = fileSystem.File.GetLastAccessTimeUtc("foo.txt");
            var actualLastWriteTime = fileSystem.File.GetLastWriteTimeUtc("foo.txt");

            Assert.That(actualCreationTime, Is.EqualTo(creationTime));
            Assert.That(actualLastAccessTime, Is.EqualTo(updateTime));
            Assert.That(actualLastWriteTime, Is.EqualTo(updateTime));
        }
    }
}
