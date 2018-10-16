using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockFileSystemWatcher : FileSystemWatcherBase
    {
        private readonly CancellationTokenSource cancel;
        private readonly Task task;
        private readonly Dictionary<WatcherChangeTypes, WaitableRef<FileSystemEventArgs>> waiters;

        public MockFileSystemWatcher(IMockFileDataAccessor mockFileDataAccessor, string root = null)
        {
            Path = root;
            cancel = new CancellationTokenSource();
            waiters = new Dictionary<WatcherChangeTypes, WaitableRef<FileSystemEventArgs>>();
            var queue = mockFileDataAccessor.Listen();
            var pathBase = mockFileDataAccessor.Path;

            void ConsumeEvents()
            {
                while (!cancel.IsCancellationRequested)
                {
                    if (queue.TryDequeue(out var e) &&
                        (root == null || pathBase.GetFullPath(e.FullPath).StartsWith(root)))
                    {
                        lock (waiters)
                        {
                            var keysToRemove = new List<WatcherChangeTypes>();

                            foreach (var keyChangeType in waiters.Keys)
                            {
                                // if event has all the change type
                                // flags as dictionary entry
                                if ((keyChangeType & e.ChangeType) == keyChangeType)
                                {
                                    foreach (var waiter in waiters)
                                    {
                                        waiter.Value.Send(e);
                                    }

                                    keysToRemove.Add(keyChangeType);
                                }
                            }

                            foreach (var keyToRemove in keysToRemove)
                            {
                                waiters.Remove(keyToRemove);
                            }
                        }

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
            }

            task = Task.Factory.StartNew(ConsumeEvents);
        }

        public override bool IncludeSubdirectories { get; set; }
        public override bool EnableRaisingEvents { get; set; }
        public override string Filter { get; set; }
        public override int InternalBufferSize { get; set; }
        public override NotifyFilters NotifyFilter { get; set; }
        public override string Path { get; set; }
#if NET40
        public override ComponentModel.ISite Site { get; set; }
        public override ComponentModel.ISynchronizeInvoke SynchronizingObject { get; set; }

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
            cancel.Cancel();
            task.Wait();
        }

        public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType) =>
            WaitForChanged(changeType, int.MaxValue);

        public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
        {
            var waiter = new WaitableRef<FileSystemEventArgs>();

            lock (waiters)
            {
                waiters.Add(changeType, waiter);
            }

            try
            {
                var e = waiter.Wait(timeout);

                return new WaitForChangedResult
                {
                    ChangeType = e.ChangeType,
                    Name = e.Name,
                    OldName = (e as RenamedEventArgs)?.OldName
                };
            }
            catch (TimeoutException)
            {
                return new WaitForChangedResult
                {
                    TimedOut = true
                };
            }
        }
    }
}
