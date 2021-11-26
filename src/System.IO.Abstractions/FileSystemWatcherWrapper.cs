using System.ComponentModel;

namespace System.IO.Abstractions
{
    /// <inheritdoc />
    [Serializable]
    public class FileSystemWatcherWrapper : FileSystemWatcherBase
    {
        [NonSerialized]
        private readonly FileSystemWatcher watcher;

        /// <inheritdoc />
        public FileSystemWatcherWrapper()
            : this(new FileSystemWatcher())
        {
            // do nothing
        }

        /// <inheritdoc />
        public FileSystemWatcherWrapper(string path)
            : this(new FileSystemWatcher(path))
        {
            // do nothing
        }

        /// <inheritdoc />
        public FileSystemWatcherWrapper(string path, string filter)
            : this(new FileSystemWatcher(path, filter))
        {
            // do nothing
        }

        /// <inheritdoc />
        public FileSystemWatcherWrapper(FileSystemWatcher watcher)
        {
            this.watcher = watcher ?? throw new ArgumentNullException(nameof(watcher));
            this.watcher.Created += OnCreated;
            this.watcher.Changed += OnChanged;
            this.watcher.Deleted += OnDeleted;
            this.watcher.Error += OnError;
            this.watcher.Renamed += OnRenamed;
        }

        /// <inheritdoc />
        public override bool IncludeSubdirectories
        {
            get { return watcher.IncludeSubdirectories; }
            set { watcher.IncludeSubdirectories = value; }
        }

        /// <inheritdoc />
        public override bool EnableRaisingEvents
        {
            get { return watcher.EnableRaisingEvents; }
            set { watcher.EnableRaisingEvents = value; }
        }

        /// <inheritdoc />
        public override string Filter
        {
            get { return watcher.Filter; }
            set { watcher.Filter = value; }
        }

#if FEATURE_FILE_SYSTEM_WATCHER_FILTERS
        /// <inheritdoc />
        public override System.Collections.ObjectModel.Collection<string> Filters
        {
            get { return watcher.Filters; }
        }
#endif

        /// <inheritdoc />
        public override int InternalBufferSize
        {
            get { return watcher.InternalBufferSize; }
            set { watcher.InternalBufferSize = value; }
        }

        /// <inheritdoc />
        public override NotifyFilters NotifyFilter
        {
            get { return watcher.NotifyFilter; }
            set { watcher.NotifyFilter = value; }
        }

        /// <inheritdoc />
        public override string Path
        {
            get { return watcher.Path; }
            set { watcher.Path = value; }
        }

        /// <inheritdoc />
        public override ISite Site
        {
            get { return watcher.Site; }
            set { watcher.Site = value; }
        }

        /// <inheritdoc />
        public override ISynchronizeInvoke SynchronizingObject
        {
            get { return watcher.SynchronizingObject; }
            set { watcher.SynchronizingObject = value; }
        }

        /// <inheritdoc />
        public override void BeginInit()
        {
            watcher.BeginInit();
        }

        /// <inheritdoc />
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                watcher.Created -= OnCreated;
                watcher.Changed -= OnChanged;
                watcher.Deleted -= OnDeleted;
                watcher.Error -= OnError;
                watcher.Renamed -= OnRenamed;
                watcher.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <inheritdoc />
        public override void EndInit()
        {
            watcher.EndInit();
        }

        /// <inheritdoc />
        public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
        {
            return watcher.WaitForChanged(changeType);
        }

        /// <inheritdoc />
        public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
        {
            return watcher.WaitForChanged(changeType, timeout);
        }
    }
}
