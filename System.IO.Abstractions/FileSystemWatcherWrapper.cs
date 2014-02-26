using System.ComponentModel;

namespace System.IO.Abstractions
{
    [Serializable]
    public class FileSystemWatcherWrapper : FileSystemWatcherBase
    {
        [NonSerialized]
        private readonly FileSystemWatcher watcher;

        public FileSystemWatcherWrapper()
        {
            watcher = new FileSystemWatcher();
            SetupEvents();
        }

        public FileSystemWatcherWrapper(string path)
        {
            watcher = new FileSystemWatcher(path);
            SetupEvents();
        }

        public FileSystemWatcherWrapper(string path, string filter)
        {
            watcher = new FileSystemWatcher(path, filter);
            SetupEvents();
        }

        public FileSystemWatcherWrapper(FileSystemWatcher watcher)
        {
            this.watcher = watcher;
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

        public override void Dispose()
        {
            watcher.Dispose();
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

        private void SetupEvents()
        {
            watcher.Created += OnCreated;
            watcher.Changed += OnChanged;
            watcher.Deleted += OnDeleted;
            watcher.Error += OnError;
            watcher.Renamed += OnRenamed;
        }
    }
}
