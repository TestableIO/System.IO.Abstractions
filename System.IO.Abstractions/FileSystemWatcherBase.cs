using System.ComponentModel;

namespace System.IO.Abstractions
{
    [Serializable]
    public abstract class FileSystemWatcherBase : IDisposable
    {
        public abstract bool IncludeSubdirectories { get; set; }
        public abstract bool EnableRaisingEvents { get; set; }
        public abstract string Filter { get; set; }
        public abstract int InternalBufferSize { get; set; }
        public abstract NotifyFilters NotifyFilter { get; set; }
        public abstract string Path { get; set; }
#if NET40
        public abstract ISite Site { get; set; }
        public abstract ISynchronizeInvoke SynchronizingObject { get; set; }
#endif
        public virtual event FileSystemEventHandler Changed;
        public virtual event FileSystemEventHandler Created;
        public virtual event FileSystemEventHandler Deleted;
        public virtual event ErrorEventHandler Error;
        public virtual event RenamedEventHandler Renamed;
#if NET40
        public abstract void BeginInit();
#endif
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

#if NET40
        public abstract void EndInit();
#endif
        public abstract WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType);
        public abstract WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout);

        public static implicit operator FileSystemWatcherBase(FileSystemWatcher watcher)
        {
            if (watcher == null)
            {
                throw new ArgumentNullException("watcher");
            }

            return new FileSystemWatcherWrapper(watcher);
        }

        public virtual void Dispose(bool disposing)
        {
            // do nothing
        }

        protected void OnCreated(object sender, FileSystemEventArgs args)
        {
            Created?.Invoke(sender, args);
        }

        protected void OnChanged(object sender, FileSystemEventArgs args)
        {
            Changed?.Invoke(sender, args);
        }

        protected void OnDeleted(object sender, FileSystemEventArgs args)
        {
            Deleted?.Invoke(sender, args);
        }

        protected void OnRenamed(object sender, RenamedEventArgs args)
        {
            Renamed?.Invoke(sender, args);
        }

        protected void OnError(object sender, ErrorEventArgs args)
        {
            Error?.Invoke(sender, args);
        }
    }
}
