using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Abstractions.TestingHelpers;

public class FileHandles
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, (FileAccess access, FileShare share)>> handles = new();

    public void TryAddHandle(string path, Guid guid, FileAccess access, FileShare share)
    {
        var pathHandles = handles.GetOrAdd(
            path, 
            _ => new ConcurrentDictionary<Guid, (FileAccess, FileShare)>());

        var requiredShare = AccessToShare(access);
        foreach (var (existingAccess, existingShare) in pathHandles.Values)
        {
            var existingRequiredShare = AccessToShare(existingAccess);
            var existingBlocksNew = (existingShare & requiredShare) != requiredShare;
            var newBlocksExisting = (share & existingRequiredShare) != existingRequiredShare;
            if (existingBlocksNew || newBlocksExisting)
            {
                throw CommonExceptions.ProcessCannotAccessFileInUse(path);
            }
        }
        
        pathHandles[guid] = (access, share);
    }

    public void RemoveHandle(string path, Guid guid)
    {
        if (handles.TryGetValue(path, out var pathHandles))
        {
            pathHandles.TryRemove(guid, out _);
            if (pathHandles.IsEmpty)
            {
                handles.TryRemove(path, out _);
            }
        }
    }

    private static FileShare AccessToShare(FileAccess access)
    {
        var share = FileShare.None;
        if (access.HasFlag(FileAccess.Read))
        {
            share |= FileShare.Read;
        }
        if (access.HasFlag(FileAccess.Write))
        {
            share |= FileShare.Write;
        }
        return share;
    }
}