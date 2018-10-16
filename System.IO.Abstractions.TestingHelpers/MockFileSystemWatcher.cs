using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading.Tasks;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockFileSystemWatcher : FileSystemWatcherBase
    {
        private bool running = true;
        private readonly Task task;

        public MockFileSystemWatcher(ConcurrentQueue<FileSystemEventArgs> queue)
        {
            task = Task.Factory.StartNew(() =>
            {
                while (running)
                {
                    // TODO: filter on root path
                    if (queue.TryDequeue(out var e))
                    {
                        if (e.ChangeType.HasFlag(WatcherChangeTypes.Created))
                        {
                            OnCreated(this, e);
                        }

                        if (e.ChangeType.HasFlag(WatcherChangeTypes.Deleted))
                        {
                            OnDeleted(this, e);
                        }

                        if (e.ChangeType.HasFlag(WatcherChangeTypes.Renamed))
                        {
                            OnRenamed(this, (RenamedEventArgs)e);
                        }

                        if (e.ChangeType.HasFlag(WatcherChangeTypes.Changed))
                        {
                            OnChanged(this, e);
                        }
                    }
                }
            });
        }

        public override bool IncludeSubdirectories { get; set; }
        public override bool EnableRaisingEvents { get; set; }
        public override string Filter { get; set; }
        public override int InternalBufferSize { get; set; }
        public override NotifyFilters NotifyFilter { get; set; }
        public override string Path { get; set; }
#if NET40
        public override ISite Site { get; set; }
        public override ISynchronizeInvoke SynchronizingObject { get; set; }
#endif

#if NET40
        public override void BeginInit()
        {
        }

        public override void EndInit()
        {
        }
#endif

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            running = false;
            task.Wait();
        }

        // TODO: notify threads blocked waiting for event

        public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
        {
            throw new NotImplementedException();
        }

        public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
        {
            throw new NotImplementedException();
        }
    }
}
