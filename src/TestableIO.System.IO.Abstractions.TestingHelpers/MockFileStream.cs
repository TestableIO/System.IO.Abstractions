using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Linq;

namespace System.IO.Abstractions.TestingHelpers;

/// <inheritdoc />
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
public class MockFileStream : FileSystemStream, IFileSystemAclSupport
{
    /// <summary>
    ///     Wrapper around a <see cref="Stream" /> with no backing store, which
    ///     is used as a replacement for a <see cref="FileSystemStream" />. As such
    ///     it implements the same properties and methods as a <see cref="FileSystemStream" />.
    /// </summary>
    public new static FileSystemStream Null { get; } = new NullFileSystemStream();

    private class NullFileSystemStream : FileSystemStream
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NullFileSystemStream" />.
        /// </summary>
        public NullFileSystemStream() : base(Null, ".", true)
        {
                
        }
    }

    private readonly IMockFileDataAccessor mockFileDataAccessor;
    private readonly string path;
    private readonly FileAccess access = FileAccess.ReadWrite;
    private readonly FileOptions options;
    private readonly MockFileData fileData;
    private bool disposed;
    
    // Version-based content synchronization to support shared file access
    // Tracks the version of shared content we last synchronized with
    private long lastKnownContentVersion;
    
    // Tracks whether this stream has local modifications that haven't been flushed to shared storage
    // This prevents us from overwriting shared content unnecessarily and helps preserve unflushed changes during refresh
    private bool hasUnflushedWrites;
    
    // Maximum file size for content comparison optimization
    private const int MaxContentComparisonSize = 4096;
    

    /// <inheritdoc />
    public MockFileStream(
        IMockFileDataAccessor mockFileDataAccessor,
        string path,
        FileMode mode,
        FileAccess access = FileAccess.ReadWrite,
        FileOptions options = FileOptions.None)
        : base(new MemoryStream(),
            path == null ? null : Path.GetFullPath(path),
            (options & FileOptions.Asynchronous) != 0)

    {
        ThrowIfInvalidModeAccess(mode, access);

        this.mockFileDataAccessor = mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));
        this.path = path;
        this.options = options;

        if (mockFileDataAccessor.FileExists(path))
        {
            if (mode.Equals(FileMode.CreateNew))
            {
                throw CommonExceptions.FileAlreadyExists(path);
            }

            fileData = mockFileDataAccessor.GetFile(path);
            fileData.CheckFileAccess(path, access);

            var timeAdjustments = GetTimeAdjustmentsForFileStreamWhenFileExists(mode, access);
            mockFileDataAccessor.AdjustTimes(fileData, timeAdjustments);
            
            // For Truncate mode, clear the file contents first
            if (mode == FileMode.Truncate)
            {
                fileData.Contents = new byte[0];
            }
            
            var existingContents = fileData.Contents;
            var keepExistingContents =
                existingContents?.Length > 0 &&
                mode != FileMode.Truncate && mode != FileMode.Create;
            if (keepExistingContents)
            {
                base.Write(existingContents, 0, existingContents.Length);
                base.Seek(0, mode == FileMode.Append
                    ? SeekOrigin.End
                    : SeekOrigin.Begin);
            }
            lastKnownContentVersion = fileData.ContentVersion;
        }
        else
        {
            var directoryPath = mockFileDataAccessor.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryPath) && !mockFileDataAccessor.Directory.Exists(directoryPath))
            {
                throw CommonExceptions.CouldNotFindPartOfPath(path);
            }

            if (mode.Equals(FileMode.Open) || mode.Equals(FileMode.Truncate))
            {
                throw CommonExceptions.FileNotFound(path);
            }

            fileData = new MockFileData(new byte[] { });
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.CreationTime | TimeAdjustments.LastAccessTime);
            mockFileDataAccessor.AddFile(path, fileData);
            lastKnownContentVersion = fileData.ContentVersion;
        }

        this.access = access;
    }

    private static void ThrowIfInvalidModeAccess(FileMode mode, FileAccess access)
    {
        if (mode == FileMode.Append)
        {
            if (access == FileAccess.Read)
            {
                throw CommonExceptions.InvalidAccessCombination(mode, access);
            }

            if (access != FileAccess.Write)
            {
                throw CommonExceptions.AppendAccessOnlyInWriteOnlyMode();
            }
        }

        if (!access.HasFlag(FileAccess.Write) &&
            (mode == FileMode.Truncate || mode == FileMode.CreateNew ||
             mode == FileMode.Create || mode == FileMode.Append))
        {
            throw CommonExceptions.InvalidAccessCombination(mode, access);
        }
    }

    /// <inheritdoc />
    public override bool CanRead => access.HasFlag(FileAccess.Read);

    /// <inheritdoc />
    public override bool CanWrite => access.HasFlag(FileAccess.Write);

    /// <inheritdoc />
    public override long Length
    {
        get
        {
            RefreshFromSharedContentIfNeeded();
            return base.Length;
        }
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
    {
        mockFileDataAccessor.AdjustTimes(fileData,
            TimeAdjustments.LastAccessTime);
        RefreshFromSharedContentIfNeeded();
        var result = base.Read(buffer, offset, count);
        return result;
    }

#if FEATURE_SPAN
    /// <inheritdoc />
    public override int Read(Span<byte> buffer)
    {
        mockFileDataAccessor.AdjustTimes(fileData,
            TimeAdjustments.LastAccessTime);
        RefreshFromSharedContentIfNeeded();
        var result = base.Read(buffer);
        return result;
    }
#endif

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }
        InternalFlush();
        base.Dispose(disposing);
        OnClose();
        disposed = true;
    }

    /// <inheritdoc cref="FileSystemStream.EndWrite(IAsyncResult)" />
    public override void EndWrite(IAsyncResult asyncResult)
    {
        if (!CanWrite)
        {
            throw new NotSupportedException("Stream does not support writing.");
        }
        base.EndWrite(asyncResult);
    }

    /// <inheritdoc />
    public override void SetLength(long value)
    {
        if (!CanWrite)
        {
            throw new NotSupportedException("Stream does not support writing.");
        }

        // Mark that we have changes that need to be flushed to shared storage
        hasUnflushedWrites = true;
        base.SetLength(value);
    }

    /// <inheritdoc cref="FileSystemStream.Write(byte[], int, int)" />
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!CanWrite)
        {
            throw new NotSupportedException("Stream does not support writing.");
        }
        RefreshFromSharedContentIfNeeded();
        mockFileDataAccessor.AdjustTimes(fileData,
            TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
        // Mark that we now have local changes that need to be flushed
        hasUnflushedWrites = true;
        base.Write(buffer, offset, count);
    }

#if FEATURE_SPAN
        /// <inheritdoc />
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            if (!CanWrite)
            {
                throw new NotSupportedException("Stream does not support writing.");
            }
            RefreshFromSharedContentIfNeeded();
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
            hasUnflushedWrites = true;
            base.Write(buffer);
        }
#endif

    /// <inheritdoc cref="FileSystemStream.WriteAsync(byte[], int, int, CancellationToken)" />
    public override Task WriteAsync(byte[] buffer, int offset, int count,
        CancellationToken cancellationToken)
    {
        if (!CanWrite)
        {
            throw new NotSupportedException("Stream does not support writing.");
        }
        RefreshFromSharedContentIfNeeded();
        mockFileDataAccessor.AdjustTimes(fileData,
            TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
        hasUnflushedWrites = true;
        return base.WriteAsync(buffer, offset, count, cancellationToken);
    }

#if FEATURE_SPAN
        /// <inheritdoc />
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer,
                                             CancellationToken cancellationToken = new())
        {
            if (!CanWrite)
            {
                throw new NotSupportedException("Stream does not support writing.");
            }
            RefreshFromSharedContentIfNeeded();
            mockFileDataAccessor.AdjustTimes(fileData,
                TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
            hasUnflushedWrites = true;
            return base.WriteAsync(buffer, cancellationToken);
        }
#endif

    /// <inheritdoc cref="FileSystemStream.WriteByte(byte)" />
    public override void WriteByte(byte value)
    {
        if (!CanWrite)
        {
            throw new NotSupportedException("Stream does not support writing.");
        }
        RefreshFromSharedContentIfNeeded();
        mockFileDataAccessor.AdjustTimes(fileData,
            TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime);
        hasUnflushedWrites = true;
        base.WriteByte(value);
    }

    /// <inheritdoc />
    public override void Flush()
    {
        InternalFlush();
    }

    /// <inheritdoc />
    public override void Flush(bool flushToDisk)
        => InternalFlush();

    /// <inheritdoc />
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        InternalFlush();
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IFileSystemAclSupport.GetAccessControl()" />
    [SupportedOSPlatform("windows")]
    public object GetAccessControl()
    {
        return GetMockFileData().AccessControl;
    }

    /// <inheritdoc cref="IFileSystemAclSupport.GetAccessControl(IFileSystemAclSupport.AccessControlSections)" />
    [SupportedOSPlatform("windows")]
    public object GetAccessControl(IFileSystemAclSupport.AccessControlSections includeSections)
    {
        return GetMockFileData().AccessControl;
    }

    /// <inheritdoc cref="IFileSystemAclSupport.SetAccessControl(object)" />
    [SupportedOSPlatform("windows")]
    public void SetAccessControl(object value)
    {
        GetMockFileData().AccessControl = value as FileSecurity;
    }

    private MockFileData GetMockFileData()
    {
        return mockFileDataAccessor.GetFile(path)
               ?? throw CommonExceptions.FileNotFound(path);
    }

    /// <summary>
    /// Fast path optimization: only refresh if the content version has actually changed.
    /// This avoids expensive content synchronization when we're already up to date.
    /// </summary>
    private void RefreshFromSharedContentIfNeeded()
    {
        if (mockFileDataAccessor.FileExists(path))
        {
            var mockFileData = mockFileDataAccessor.GetFile(path);
            
            // Quick version check - if versions match, we're already synchronized
            if (mockFileData.ContentVersion == lastKnownContentVersion)
            {
                return; // Fast exit - no work needed
            }
            
            // Version changed, indicating another stream modified the file
            RefreshFromSharedContent(mockFileData);
        }
    }

    /// <summary>
    /// Synchronizes this stream's content with the shared file data to support FileShare.ReadWrite scenarios.
    /// 
    /// When multiple MockFileStream instances open the same file with FileShare.ReadWrite, they need to see
    /// each other's changes. This method implements a version-based synchronization mechanism where:
    /// 
    /// 1. Each MockFileData has a ContentVersion that increments when content changes
    /// 2. Each stream tracks the lastKnownContentVersion it has synchronized with
    /// 3. Before reads/writes/length checks, streams refresh to get the latest shared content
    /// 4. During refresh, any unflushed local changes are preserved and merged with shared content
    /// 
    /// This solves GitHub issue #1131 where shared file content was not visible between streams.
    /// </summary>
    private void RefreshFromSharedContent(MockFileData mockFileData = null)
    {
        if (mockFileDataAccessor.FileExists(path))
        {
            mockFileData ??= mockFileDataAccessor.GetFile(path);
            
            // Only refresh if the shared content has been modified since we last synced
            // This prevents unnecessary work and maintains performance
            if (mockFileData.ContentVersion != lastKnownContentVersion)
            {
                long currentPosition = Position;
                
                // Preserve unflushed content if necessary
                var (preservedContent, preservedLength) = PreserveUnflushedContent();
                
                var sharedContent = mockFileData.Contents;
                
                // Check if content is already the same (optimization for metadata-only changes)
                if (IsContentIdentical(sharedContent))
                {
                    lastKnownContentVersion = mockFileData.ContentVersion;
                    return;
                }
                
                // Merge shared content with any preserved unflushed writes
                MergeWithSharedContent(sharedContent, preservedContent, preservedLength);
                
                // Restore position, but ensure it's within bounds of the new content
                Position = Math.Min(currentPosition, base.Length);
                
                // Update our version to match what we just synchronized with
                // This prevents unnecessary refreshes until the shared content changes again
                lastKnownContentVersion = mockFileData.ContentVersion;
            }
        }
    }

    /// <summary>
    /// Preserves unflushed content from the current stream before refreshing from shared content.
    /// </summary>
    /// <returns>A tuple containing the preserved content and its length.</returns>
    private (byte[] content, long length) PreserveUnflushedContent()
    {
        if (!hasUnflushedWrites)
        {
            return (null, 0);
        }

        // Save our current stream content to preserve unflushed writes
        var preservedLength = base.Length;
        var preservedContent = new byte[preservedLength];
        var originalPosition = Position;
        Position = 0;
        var totalBytesRead = 0;
        while (totalBytesRead < preservedLength)
        {
            var bytesRead = base.Read(preservedContent, totalBytesRead, (int)(preservedLength - totalBytesRead));
            if (bytesRead == 0)
            {
                break;
            }
            totalBytesRead += bytesRead;
        }
        Position = originalPosition;
        
        return (preservedContent, preservedLength);
    }

    /// <summary>
    /// Checks if the current content is identical to the shared content (optimization).
    /// </summary>
    /// <param name="sharedContent">The shared content to compare against.</param>
    /// <returns>True if the content is identical, false otherwise.</returns>
    private bool IsContentIdentical(byte[] sharedContent)
    {
        // Performance optimization: if we have no unflushed writes and the shared content
        // is the same length as our current content, we might not need to do expensive copying
        if (hasUnflushedWrites || sharedContent?.Length != base.Length)
        {
            return false;
        }

        // Quick content comparison for common case where only metadata changed
        if (sharedContent.Length > 0 && sharedContent.Length <= MaxContentComparisonSize) // Only check small files
        {
            var currentPos = Position;
            Position = 0;
            var currentContent = new byte[base.Length];
            var bytesRead = base.Read(currentContent, 0, (int)base.Length);
            Position = currentPos;
            
            return bytesRead == sharedContent.Length && 
                   currentContent.Take(bytesRead).SequenceEqual(sharedContent);
        }

        return false; // Don't compare large files
    }

    /// <summary>
    /// Merges the shared content with any preserved unflushed writes.
    /// </summary>
    /// <param name="sharedContent">The shared content from MockFileData.</param>
    /// <param name="preservedContent">Any preserved unflushed content.</param>
    /// <param name="preservedLength">The length of the preserved content.</param>
    private void MergeWithSharedContent(byte[] sharedContent, byte[] preservedContent, long preservedLength)
    {
        // Start with shared content as the base - this gives us the latest changes from other streams
        base.SetLength(0);
        Position = 0;
        if (sharedContent != null && sharedContent.Length > 0)
        {
            base.Write(sharedContent, 0, sharedContent.Length);
        }
        
        // If we had unflushed writes, we need to overlay them on the shared content
        // This ensures our local changes take precedence over shared content
        if (hasUnflushedWrites && preservedContent != null)
        {
            // Optimization: if preserved content is same length or longer, just use it directly
            if (preservedLength >= (sharedContent?.Length ?? 0))
            {
                base.SetLength(0);
                Position = 0;
                base.Write(preservedContent, 0, (int)preservedLength);
            }
            else
            {
                // Need to merge: ensure stream is large enough
                if (base.Length < preservedLength)
                {
                    base.SetLength(preservedLength);
                }
                
                // Apply our preserved content on top of the shared content
                Position = 0;
                base.Write(preservedContent, 0, (int)preservedLength);
            }
        }
    }

    /// <summary>
    /// Flushes this stream's content to the shared file data, implementing proper synchronization for FileShare.ReadWrite.
    /// 
    /// This method is called by Flush(), FlushAsync(), and Dispose() to ensure local changes are persisted
    /// to the shared MockFileData that other streams can see. Key aspects:
    /// 
    /// 1. Only flushes if we have unflushed writes (performance optimization)
    /// 2. Refreshes from shared content first to merge any changes from other streams
    /// 3. Reads the entire stream content and updates the shared MockFileData.Contents
    /// 4. Updates version tracking to stay synchronized
    /// 5. Clears the hasUnflushedWrites flag
    /// 
    /// This ensures that when multiple streams share a file, all changes are properly coordinated.
    /// </summary>
    private void InternalFlush()
    {
        if (mockFileDataAccessor.FileExists(path) && hasUnflushedWrites)
        {
            var mockFileData = mockFileDataAccessor.GetFile(path);
            
            // Before flushing, ensure we have the latest shared content merged with our unflushed writes
            // This is critical to prevent overwriting changes made by other streams
            RefreshFromSharedContentIfNeeded();
            
            // Save current position to restore later
            var position = Position;
            
            // Read the entire stream content to flush to shared storage
            Seek(0, SeekOrigin.Begin);
            var data = new byte[base.Length];
            var totalBytesRead = 0;
            
            // Use a loop to ensure we read everything (handles partial reads)
            while (totalBytesRead < base.Length)
            {
                var bytesRead = base.Read(data, totalBytesRead, (int)(base.Length - totalBytesRead));
                if (bytesRead == 0)
                {
                    break; // End of stream
                }
                totalBytesRead += bytesRead;
            }
            
            // Restore original position
            Seek(position, SeekOrigin.Begin);
            
            // Update the shared content - this is what makes changes visible to other streams
            // Setting Contents will increment the ContentVersion, notifying other streams to refresh
            mockFileData.Contents = data;
            
            // Update our version tracking to match what we just wrote
            lastKnownContentVersion = mockFileData.ContentVersion;
            
            // Clear the flag since we've now flushed all pending changes
            hasUnflushedWrites = false;
        }
    }

    private void OnClose()
    {
        if (options.HasFlag(FileOptions.DeleteOnClose) && mockFileDataAccessor.FileExists(path))
        {
            mockFileDataAccessor.RemoveFile(path);
        }

        if (options.HasFlag(FileOptions.Encrypted) && mockFileDataAccessor.FileExists(path))
        {
#pragma warning disable CA1416 // Ignore SupportedOSPlatform for testing helper encryption
            mockFileDataAccessor.FileInfo.New(path).Encrypt();
#pragma warning restore CA1416
        }
    }

    private TimeAdjustments GetTimeAdjustmentsForFileStreamWhenFileExists(FileMode mode, FileAccess access)
    {
        switch (mode)
        {
            case FileMode.Append:
            case FileMode.CreateNew:
                if (access.HasFlag(FileAccess.Read))
                {
                    return TimeAdjustments.LastAccessTime;
                }
                return TimeAdjustments.None;
            case FileMode.Create:
            case FileMode.Truncate:
                if (access.HasFlag(FileAccess.Write))
                {
                    return TimeAdjustments.LastAccessTime | TimeAdjustments.LastWriteTime;
                }
                return TimeAdjustments.LastAccessTime;
            case FileMode.Open:
            case FileMode.OpenOrCreate:
            default:
                return TimeAdjustments.None;
        }
    }
}