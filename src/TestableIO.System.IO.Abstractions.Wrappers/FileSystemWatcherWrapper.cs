using System.ComponentModel;

namespace System.IO.Abstractions
{
    /// <inheritdoc />
#if !NET8_0_OR_GREATER
    [Serializable]
#endif
    public class FileSystemWatcherWrapper : FileSystemWatcherBase
    {
#if !NET8_0_OR_GREATER
        [NonSerialized]
#endif
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
        {
            FileSystem = fileSystem;
            this.watcher = watcher ?? throw new ArgumentNullException(nameof(watcher));
            this.watcher.Created += OnCreated;
            this.watcher.Changed += OnChanged;
            this.watcher.Deleted += OnDeleted;
            this.watcher.Error += OnError;
            this.watcher.Renamed += OnRenamed;
        }

        /// <inheritdoc />
        public override IFileSystem FileSystem { get; }

        /// <inheritdoc />
        public override bool IncludeSubdirectories
        {
            get { return watcher.IncludeSubdirectories; }
            set { watcher.IncludeSubdirectories = value; }
        }

        /// <inheritdoc />
        public override IContainer Container
            => watcher.Container;

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
        public override IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
        {
            return new WaitForChangedResultWrapper(watcher.WaitForChanged(changeType));
        }

        /// <inheritdoc />
        public override IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
        {
            return new WaitForChangedResultWrapper(watcher.WaitForChanged(changeType, timeout));
        }

#if FEATURE_FILE_SYSTEM_WATCHER_WAIT_WITH_TIMESPAN
        /// <inheritdoc />
        public override IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, TimeSpan timeout)
        {
            return new WaitForChangedResultWrapper(watcher.WaitForChanged(changeType, timeout));
        }
#endif

        private readonly struct WaitForChangedResultWrapper
            : IWaitForChangedResult, IEquatable<WaitForChangedResultWrapper>
        {
            private readonly WaitForChangedResult _instance;

            public WaitForChangedResultWrapper(WaitForChangedResult instance)
            {
                _instance = instance;
            }

            /// <inheritdoc cref="IWaitForChangedResult.ChangeType" />
            public WatcherChangeTypes ChangeType
                => _instance.ChangeType;

            /// <inheritdoc cref="IWaitForChangedResult.Name" />
            public string Name
                => _instance.Name;

            /// <inheritdoc cref="IWaitForChangedResult.OldName" />
            public string OldName
                => _instance.OldName;

            /// <inheritdoc cref="IWaitForChangedResult.TimedOut" />
            public bool TimedOut
                => _instance.TimedOut;

            /// <inheritdoc cref="IEquatable{WaitForChangedResultWrapper}.Equals(WaitForChangedResultWrapper)" />
            public bool Equals(WaitForChangedResultWrapper other)
            {
                return _instance.Equals(other._instance);
            }

            /// <inheritdoc cref="object.Equals(object)" />
            public override bool Equals(object obj)
            {
                return obj is WaitForChangedResultWrapper other
                       && Equals(other);
            }

            /// <inheritdoc cref="object.GetHashCode()" />
            public override int GetHashCode()
            {
                return _instance.GetHashCode();
            }
        }
    }
}
