using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions
{
    public interface IFileSystemWatcherFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemWatcherBase"/> class, which acts as a wrapper for a FileSystemWatcher
        /// </summary>
        /// <returns></returns>
        IFileSystemWatcher CreateNew();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemWatcherBase"/> class, which acts as a wrapper for a FileSystemWatcher
        /// </summary>
        /// <param name="path">Path to generate the FileSystemWatcherBase for</param>
        /// <returns></returns>
        IFileSystemWatcher FromPath(string path);
    }
}
