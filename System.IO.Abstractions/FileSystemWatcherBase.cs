using System.ComponentModel;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="FileSystemWatcher"/>
    [Serializable]
    public abstract class FileSystemWatcherBase : IDisposable
    {
        /// <inheritdoc cref="FileSystemWatcher.IncludeSubdirectories"/>
        public abstract bool IncludeSubdirectories { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.EnableRaisingEvents"/>
        public abstract bool EnableRaisingEvents { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.Filter"/>
        public abstract string Filter { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.InternalBufferSize"/>
        public abstract int InternalBufferSize { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.NotifyFilter"/>
        public abstract NotifyFilters NotifyFilter { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.Path"/>
        public abstract string Path { get; set; }

#if NET40
        /// <inheritdoc cref="FileSystemWatcher.Site"/>
        public abstract ISite Site { get; set; }

        /// <inheritdoc cref="FileSystemWatcher.SynchronizingObject"/>
        public abstract ISynchronizeInvoke SynchronizingObject { get; set; }
#endif

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

#if NET40
        /// <inheritdoc cref="FileSystemWatcher.BeginInit"/>
        public abstract void BeginInit();
#endif

        /// <inheritdoc cref="FileSystemWatcher.Dispose" />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

#if NET40
        /// <inheritdoc cref="FileSystemWatcher.EndInit"/>
        public abstract void EndInit();
#endif

        /// <inheritdoc cref="FileSystemWatcher.WaitForChanged(WatcherChangeTypes)"/>
        public abstract WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType);

        /// <inheritdoc cref="FileSystemWatcher.WaitForChanged(WatcherChangeTypes,int)"/>
        public abstract WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout);

        /// <summary>
        /// Converts a standard <see cref="FileSystemWatcher"/> to a
        /// compatible <see cref="FileSystemWatcherBase"/>.
        /// </summary>
        public static implicit operator FileSystemWatcherBase(FileSystemWatcher watcher)
        {
            if (watcher == null)
            {
                throw new ArgumentNullException(nameof(watcher));
            }

            return new FileSystemWatcherWrapper(watcher);
        }

        /// <inheritdoc cref="FileSystemWatcher.Dispose(bool)" />
        public virtual void Dispose(bool disposing)
        {
            // do nothing
        }

        /// <inheritdoc cref="FileSystemWatcher.OnCreated(FileSystemEventArgs)" />
        protected void OnCreated(object sender, FileSystemEventArgs args)
        {
            Created?.Invoke(sender, args);
        }

        /// <inheritdoc cref="FileSystemWatcher.OnChanged(FileSystemEventArgs)" />
        protected void OnChanged(object sender, FileSystemEventArgs args)
        {
            Changed?.Invoke(sender, args);
        }

        /// <inheritdoc cref="FileSystemWatcher.OnDeleted(FileSystemEventArgs)" />
        protected void OnDeleted(object sender, FileSystemEventArgs args)
        {
            Deleted?.Invoke(sender, args);
        }

        /// <inheritdoc cref="FileSystemWatcher.OnRenamed(RenamedEventArgs)" />
        protected void OnRenamed(object sender, RenamedEventArgs args)
        {
            Renamed?.Invoke(sender, args);
        }

        /// <inheritdoc cref="FileSystemWatcher.OnError(ErrorEventArgs)" />
        protected void OnError(object sender, ErrorEventArgs args)
        {
            Error?.Invoke(sender, args);
        }
    }
}
