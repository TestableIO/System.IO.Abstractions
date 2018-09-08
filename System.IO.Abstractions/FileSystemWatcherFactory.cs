using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions
{
    [Serializable]
    public class FileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        public FileSystemWatcherBase CreateNew()
        {
            return new FileSystemWatcherWrapper();
        }

        public FileSystemWatcherBase FromPath(string path)
        {
            return new FileSystemWatcherWrapper(path);
        }
    }
}
