using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="FileSystemWatcher" />
    public interface IFileSystemWatcher : IFileSystemEntity, IDisposable
    {
        /// <inheritdoc cref="Component.Container" />
        IContainer? Container { get; }

        /// <inheritdoc cref="FileSystemWatcher.EnableRaisingEvents" />
        bool EnableRaisingEvents { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.Filter" />
        string Filter { get; set; }

#if FEATURE_FILE_SYSTEM_WATCHER_FILTERS
        /// <inheritdoc cref="FileSystemWatcher.Filters" />
        Collection<string> Filters { get; }
#endif

        /// <inheritdoc cref="FileSystemWatcher.IncludeSubdirectories" />
        bool IncludeSubdirectories { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.InternalBufferSize" />
        int InternalBufferSize { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.NotifyFilter" />
        NotifyFilters NotifyFilter { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.Path" />
        string Path { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.Site" />
        ISite? Site { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.SynchronizingObject" />
        ISynchronizeInvoke? SynchronizingObject { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.Changed" />
        event FileSystemEventHandler? Changed;

        /// <inheritdoc cref="FileSystemWatcher.Created" />
        event FileSystemEventHandler? Created;

        /// <inheritdoc cref="FileSystemWatcher.Deleted" />
        event FileSystemEventHandler? Deleted;

        /// <inheritdoc cref="FileSystemWatcher.Error" />
        event ErrorEventHandler? Error;

        /// <inheritdoc cref="FileSystemWatcher.Renamed" />
        event RenamedEventHandler? Renamed;

        /// <inheritdoc cref="FileSystemWatcher.BeginInit()" />
        void BeginInit();

        /// <inheritdoc cref="FileSystemWatcher.EndInit()" />
        void EndInit();

        /// <inheritdoc cref="FileSystemWatcher.WaitForChanged(WatcherChangeTypes)" />
        IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType);

        /// <inheritdoc cref="FileSystemWatcher.WaitForChanged(WatcherChangeTypes, int)" />
        IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout);

#if FEATURE_FILE_SYSTEM_WATCHER_WAIT_WITH_TIMESPAN
        /// <inheritdoc cref="FileSystemWatcher.WaitForChanged(WatcherChangeTypes, TimeSpan)" />
        IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, TimeSpan timeout);
#endif
    }
}