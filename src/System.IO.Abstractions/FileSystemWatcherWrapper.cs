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
        public FileSystemWatcherWrapper(IFileSystem fileSystem)
            : this(fileSystem, new FileSystemWatcher())
        {
            // do nothing
        }

        /// <inheritdoc />
        public FileSystemWatcherWrapper(IFileSystem fileSystem, string path)
            : this(fileSystem, new FileSystemWatcher(path))
        {
            // do nothing
        }

        /// <inheritdoc />
        public FileSystemWatcherWrapper(IFileSystem fileSystem, string path, string filter)
            : this(fileSystem, new FileSystemWatcher(path, filter))
        {
            // do nothing
        }

        /// <inheritdoc />
        public FileSystemWatcherWrapper(IFileSystem fileSystem, FileSystemWatcher watcher)
            : base(fileSystem)
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
        public override IFileSystemWatcher.IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
        {
            return new WaitForChangedResultWrapper(watcher.WaitForChanged(changeType));
        }

        /// <inheritdoc />
        public override IFileSystemWatcher.IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
        {
            return new WaitForChangedResultWrapper(watcher.WaitForChanged(changeType, timeout));
        }

        /// <inheritdoc />
        public override IContainer Container => watcher.Container;

        private readonly struct WaitForChangedResultWrapper
            : IFileSystemWatcher.IWaitForChangedResult
        {
            private readonly WaitForChangedResult instance;

            public WaitForChangedResultWrapper(WaitForChangedResult instance)
            {
                this.instance = instance;
            }

            /// <inheritdoc cref="IFileSystemWatcher.IWaitForChangedResult.ChangeType" />
            public WatcherChangeTypes ChangeType
                => instance.ChangeType;

            /// <inheritdoc cref="IFileSystemWatcher.IWaitForChangedResult.Name" />
            public string Name
                => instance.Name;

            /// <inheritdoc cref="IFileSystemWatcher.IWaitForChangedResult.OldName" />
            public string OldName
                => instance.OldName;

            /// <inheritdoc cref="IFileSystemWatcher.IWaitForChangedResult.TimedOut" />
            public bool TimedOut
                => instance.TimedOut;
        }
    }
}
