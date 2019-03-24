using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions
{
    [Serializable]
    public class FileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        public IFileSystemWatcher CreateNew()
        {
            return new FileSystemWatcherWrapper();
        }

        public IFileSystemWatcher FromPath(string path)
        {
            return new FileSystemWatcherWrapper(path);
        }
    }
}
