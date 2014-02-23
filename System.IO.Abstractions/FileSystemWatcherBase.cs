using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions
{
    [Serializable]
    public abstract class FileSystemWatcherBase : IDisposable
    {
        public abstract bool EnableRaisingEvents { get; set; }
        public abstract string Filter { get; set; }
        public abstract int InternalBufferSize { get; set; }
        public abstract NotifyFilters NotifyFilter { get; set; }
        public abstract string Path { get; set; }
        public abstract ISite Site { get; set; }
        public abstract ISynchronizeInvoke SynchronizingObject { get; set; }
        public virtual event FileSystemEventHandler Changed;
        public virtual event FileSystemEventHandler Created;
        public virtual event FileSystemEventHandler Deleted;
        public virtual event ErrorEventHandler Error;
        public virtual event RenamedEventHandler Renamed;
        public abstract void BeginInit();
        public abstract void Dispose();
        public abstract void EndInit();
        public abstract WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType);
        public abstract WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout);

        public static implicit operator FileSystemWatcherBase(FileSystemWatcher watcher)
        {
            return new FileSystemWatcherWrapper(watcher);
        }

        protected void OnCreated(object sender, FileSystemEventArgs args)
        {
            if (Created != null)
                Created(sender, args);
        }

        protected void OnChanged(object sender, FileSystemEventArgs args)
        {
            if (Changed != null)
                Changed(sender, args);
        }

        protected void OnDeleted(object sender, FileSystemEventArgs args)
        {
            if (Deleted != null)
                Deleted(sender, args);
        }

        protected void OnRenamed(object sender, RenamedEventArgs args)
        {
            if (Renamed != null)
                Renamed(sender, args);
        }

        protected void OnError(object sender, ErrorEventArgs args)
        {
            if (Error != null)
                Error(sender, args);
        }
    }
}
