using System.ComponentModel;

namespace System.IO.Abstractions
{
    [Serializable]
    public class FileSystemWatcherWrapper : FileSystemWatcherBase
    {
        [NonSerialized]
        private readonly FileSystemWatcher watcher;

        public FileSystemWatcherWrapper()
            : this(new FileSystemWatcher())
        {
            // do nothing
        }

        public FileSystemWatcherWrapper(string path)
            : this(new FileSystemWatcher(path))
        {
            // do nothing
        }

        public FileSystemWatcherWrapper(string path, string filter)
            : this(new FileSystemWatcher(path, filter))
        {
            // do nothing
        }

        public FileSystemWatcherWrapper(FileSystemWatcher watcher)
        {
            if (watcher == null)
            {
                throw new ArgumentNullException("watcher");
            }

            this.watcher = watcher;
            this.watcher.Created += OnCreated;
            this.watcher.Changed += OnChanged;
            this.watcher.Deleted += OnDeleted;
            this.watcher.Error += OnError;
            this.watcher.Renamed += OnRenamed;
        }

        public override bool IncludeSubdirectories
        {
            get { return watcher.IncludeSubdirectories; }
            set { watcher.IncludeSubdirectories = value; }
        }

        public override bool EnableRaisingEvents
        {
            get { return watcher.EnableRaisingEvents; }
            set { watcher.EnableRaisingEvents = value; }
        }

        public override string Filter
        {
            get { return watcher.Filter; }
            set { watcher.Filter = value; }
        }

        public override int InternalBufferSize
        {
            get { return watcher.InternalBufferSize; }
            set { watcher.InternalBufferSize = value; }
        }

        public override NotifyFilters NotifyFilter
        {
            get { return watcher.NotifyFilter; }
            set { watcher.NotifyFilter = value; }
        }

        public override string Path
        {
            get { return watcher.Path; }
            set { watcher.Path = value; }
        }

        public override ISite Site
        {
            get { return watcher.Site; }
            set { watcher.Site = value; }
        }

        public override ISynchronizeInvoke SynchronizingObject
        {
            get { return watcher.SynchronizingObject; }
            set { watcher.SynchronizingObject = value; }
        }

        public override void BeginInit()
        {
            watcher.BeginInit();
        }

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

        public override void EndInit()
        {
            watcher.EndInit();
        }

        public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
        {
            return watcher.WaitForChanged(changeType);
        }

        public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
        {
            return watcher.WaitForChanged(changeType, timeout);
        }
    }
}
