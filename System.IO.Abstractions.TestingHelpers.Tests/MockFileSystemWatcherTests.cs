using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSystemWatcherTests
    {
        [Test]
        public void MockFileSystemWatcher_OnCreated_Listen()
        {
            var fs = new MockFileSystem();
            fs.AddDirectory(@"C:\");
            var count = 0;

            using (var watcher = fs.FileSystemWatcher.FromPath(@"C:\"))
            {
                watcher.Created += (sender, e) => count++;
                fs.File.Create(@"C:\test.txt").Close();
            }

            Thread.Sleep(500); // TODO: make this unnecessary
            Assert.AreEqual(1, count);
        }

        [Test]
        public void MockFileSystemWatcher_OnCreated_UnderSpecificRoot()
        {
            var fs = new MockFileSystem();
            fs.AddDirectory(@"C:\root");
            var count = 0;

            using (var watcher = fs.FileSystemWatcher.FromPath(@"C:\root"))
            {
                watcher.Created += (sender, e) => count++;
                fs.File.Create(@"C:\test.txt").Close();
                fs.File.Create(@"C:\root\test.txt").Close();
            }

            Thread.Sleep(500); // TODO: make this unnecessary
            Assert.AreEqual(1, count);
        }

        [Test]
        public void MockFileSystemWatcher_WaitForChanged_SpecificChangeType()
        {
            var fs = new MockFileSystem();
            fs.AddDirectory(@"C:\root");
            var count = 0;

            using (var watcher = fs.FileSystemWatcher.FromPath(@"C:\root"))
            {
                var task = Task.Factory
                    .StartNew(() => watcher.WaitForChanged(WatcherChangeTypes.Deleted, 1000))
                    .ContinueWith(_ => count++);
                fs.File.Create(@"C:\root\test.txt").Close();
                fs.File.Move(@"C:\root\test.txt", @"C:\root\other.txt");
                Thread.Sleep(500); // TODO: make this unnecessary
                Assert.AreEqual(0, count);
                fs.File.Delete(@"C:\root\other.txt");
            }

            Thread.Sleep(500); // TODO: make this unnecessary
            Assert.AreEqual(1, count);
        }
    }
}
