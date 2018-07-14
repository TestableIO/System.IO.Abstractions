using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        public FileSystemWatcherBase CreateNew()
        {
            return new MockFileSystemWatcher();
        }

        public FileSystemWatcherBase FromPath(string path)
        {
            return new MockFileSystemWatcher {Path = path};
        }
    }
}
