using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSystemWatcherTests
    {
        [Test]
        public void MockFileSystemWatcher_ListenForFileCreated()
        {
            var fs = new MockFileSystem();
            fs.AddDirectory(@"C:\");
            var created = false;

            using (var watcher = fs.FileSystemWatcher.FromPath(@"C:\"))
            {
                watcher.Created += (sender, e) =>
                {
                    created = true;
                };
                fs.File.Create(@"C:\test.txt").Close();
            }

            Thread.Sleep(100); // TODO: make this unnecessary
            Assert.IsTrue(created);
        }
    }
}
