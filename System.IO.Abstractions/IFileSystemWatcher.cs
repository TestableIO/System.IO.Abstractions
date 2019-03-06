namespace System.IO.Abstractions
{
    public interface IFileSystemWatcher
    {
        bool EnableRaisingEvents { get; set; }
        string Filter { get; set; }
        bool IncludeSubdirectories { get; set; }
        int InternalBufferSize { get; set; }
        NotifyFilters NotifyFilter { get; set; }
        string Path { get; set; }

        event FileSystemEventHandler Changed;
        event FileSystemEventHandler Created;
        event FileSystemEventHandler Deleted;
        event ErrorEventHandler Error;
        event RenamedEventHandler Renamed;

        WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType);
        WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout);
    }
}