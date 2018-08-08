using System.ComponentModel;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockFileSystemWatcher : FileSystemWatcherBase
    {
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
