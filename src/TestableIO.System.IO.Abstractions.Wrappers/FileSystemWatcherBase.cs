using System.ComponentModel;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="FileSystemWatcher"/>
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public abstract class FileSystemWatcherBase : IFileSystemWatcher
    {
        /// <inheritdoc />
        public abstract IFileSystem FileSystem { get; }

        /// <inheritdoc cref="FileSystemWatcher.IncludeSubdirectories"/>
        public abstract bool IncludeSubdirectories { get; set; }

        /// <inheritdoc cref="ComponentModel.Container"/>
        public abstract IContainer Container { get; }

        /// <inheritdoc cref="FileSystemWatcher.EnableRaisingEvents"/>
        public abstract bool EnableRaisingEvents { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.Filter"/>
        public abstract string Filter { get; set; }

#if FEATURE_FILE_SYSTEM_WATCHER_FILTERS
        /// <inheritdoc cref="FileSystemWatcher.Filters"/>
        public abstract System.Collections.ObjectModel.Collection<string> Filters { get; }
#endif

        /// <inheritdoc cref="FileSystemWatcher.InternalBufferSize"/>
        public abstract int InternalBufferSize { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.NotifyFilter"/>
        public abstract NotifyFilters NotifyFilter { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.Path"/>
        public abstract string Path { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.Site"/>
        public abstract ISite Site { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.SynchronizingObject"/>
        public abstract ISynchronizeInvoke SynchronizingObject { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.Changed"/>
        public virtual event FileSystemEventHandler Changed;

        /// <inheritdoc cref="FileSystemWatcher.Created"/>
        public virtual event FileSystemEventHandler Created;

        /// <inheritdoc cref="FileSystemWatcher.Deleted"/>
        public virtual event FileSystemEventHandler Deleted;

        /// <inheritdoc cref="FileSystemWatcher.Error"/>
        public virtual event ErrorEventHandler Error;

        /// <inheritdoc cref="FileSystemWatcher.Renamed"/>
        public virtual event RenamedEventHandler Renamed;

        /// <inheritdoc cref="FileSystemWatcher.BeginInit"/>
        public abstract void BeginInit();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="FileSystemWatcher.EndInit"/>
        public abstract void EndInit();
        
        /// <inheritdoc cref="FileSystemWatcher.WaitForChanged(WatcherChangeTypes)"/>
        public abstract IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType);

        /// <inheritdoc cref="FileSystemWatcher.WaitForChanged(WatcherChangeTypes,int)"/>
        public abstract IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout);

#if FEATURE_FILE_SYSTEM_WATCHER_WAIT_WITH_TIMESPAN
        /// <inheritdoc cref="FileSystemWatcher.WaitForChanged(WatcherChangeTypes,TimeSpan)"/>
        public abstract IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, TimeSpan timeout);
#endif

        /// <inheritdoc />
        public static implicit operator FileSystemWatcherBase(FileSystemWatcher watcher)
        {
            if (watcher == null)
            {
                return null;
            }

            return new FileSystemWatcherWrapper(new FileSystem(), watcher);
        }

        /// <inheritdoc />
        public virtual void Dispose(bool disposing)
        {
            // do nothing
        }

        /// <inheritdoc />
        protected void OnCreated(object sender, FileSystemEventArgs args)
        {
            Created?.Invoke(sender, args);
        }

        /// <inheritdoc />
        protected void OnChanged(object sender, FileSystemEventArgs args)
        {
            Changed?.Invoke(sender, args);
        }

        /// <inheritdoc />
        protected void OnDeleted(object sender, FileSystemEventArgs args)
        {
            Deleted?.Invoke(sender, args);
        }

        /// <inheritdoc />
        protected void OnRenamed(object sender, RenamedEventArgs args)
        {
            Renamed?.Invoke(sender, args);
        }

        /// <inheritdoc />
        protected void OnError(object sender, ErrorEventArgs args)
        {
            Error?.Invoke(sender, args);
        }
    }
}
