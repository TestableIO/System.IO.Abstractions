#if NET40
using System.ComponentModel;
#endif

namespace System.IO.Abstractions
{
    public interface IFileSystemWatcher : IDisposable
    {
        /// <inheritdoc cref="FileSystemWatcher.IncludeSubdirectories"/>
        bool IncludeSubdirectories { get; set; }
        /// <inheritdoc cref="FileSystemWatcher.EnableRaisingEvents"/>
        bool EnableRaisingEvents { get; set; }
        /// <inheritdoc cref="FileSystemWatcher.Filter"/>
        string Filter { get; set; }
        /// <inheritdoc cref="FileSystemWatcher.InternalBufferSize"/>
        int InternalBufferSize { get; set; }
        /// <inheritdoc cref="FileSystemWatcher.NotifyFilter"/>
        NotifyFilters NotifyFilter { get; set; }
        /// <inheritdoc cref="FileSystemWatcher.Path"/>
        string Path { get; set; }
#if NET40
        /// <inheritdoc cref="FileSystemWatcher.Site"/>
        ISite Site { get; set; }
        /// <inheritdoc cref="FileSystemWatcher.SynchronizingObject"/>
        ISynchronizeInvoke SynchronizingObject { get; set; }
#endif
        /// <inheritdoc cref="FileSystemWatcher.Changed"/>
        event FileSystemEventHandler Changed;
        /// <inheritdoc cref="FileSystemWatcher.Created"/>
        event FileSystemEventHandler Created;
        /// <inheritdoc cref="FileSystemWatcher.Deleted"/>
        event FileSystemEventHandler Deleted;
        /// <inheritdoc cref="FileSystemWatcher.Error"/>
        event ErrorEventHandler Error;
        /// <inheritdoc cref="FileSystemWatcher.Renamed"/>
        event RenamedEventHandler Renamed;
#if NET40
        /// <inheritdoc cref="FileSystemWatcher.BeginInit"/>
        void BeginInit();
#endif
#if NET40
        /// <inheritdoc cref="FileSystemWatcher.EndInit"/>
        void EndInit();
#endif
        /// <inheritdoc cref="FileSystemWatcher.WaitForChanged(WatcherChangeTypes)"/>
        WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType);
        /// <inheritdoc cref="FileSystemWatcher.WaitForChanged(WatcherChangeTypes,int)"/>
        WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout);
    }
}