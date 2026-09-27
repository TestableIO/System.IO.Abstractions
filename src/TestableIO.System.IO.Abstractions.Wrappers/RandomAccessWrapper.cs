#if FEATURE_RANDOM_ACCESS
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions;

/// <inheritdoc />
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
public class RandomAccessWrapper : IRandomAccess
{
    /// <summary>
    /// Wrapper class for calling static methods of <see cref="System.IO.RandomAccess"/>
    /// </summary>
    public RandomAccessWrapper(IFileSystem fileSystem)
    {
        FileSystem = fileSystem;
    }

    /// <inheritdoc />
    public IFileSystem FileSystem { get; }

#if FEATURE_RANDOM_ACCESS_FLUSH_TO_DISK
    /// <inheritdoc />
    public void FlushToDisk(SafeFileHandle handle)
        => RandomAccess.FlushToDisk(handle);
#endif

    /// <inheritdoc />
    public long GetLength(SafeFileHandle handle)
        => RandomAccess.GetLength(handle);

    /// <inheritdoc />
    public int Read(SafeFileHandle handle, Span<byte> buffer, long fileOffset)
        => RandomAccess.Read(handle, buffer, fileOffset);

    /// <inheritdoc />
    public long Read(SafeFileHandle handle, IReadOnlyList<Memory<byte>> buffers, long fileOffset)
        => RandomAccess.Read(handle, buffers, fileOffset);

    /// <inheritdoc />
    public ValueTask<int> ReadAsync(SafeFileHandle handle, Memory<byte> buffer, long fileOffset,
        CancellationToken cancellationToken = default)
        => RandomAccess.ReadAsync(handle, buffer, fileOffset, cancellationToken);

    /// <inheritdoc />
    public ValueTask<long> ReadAsync(SafeFileHandle handle, IReadOnlyList<Memory<byte>> buffers,
        long fileOffset, CancellationToken cancellationToken = default)
        => RandomAccess.ReadAsync(handle, buffers, fileOffset, cancellationToken);

#if FEATURE_RANDOM_ACCESS_FLUSH_TO_DISK
    /// <inheritdoc />
    public void SetLength(SafeFileHandle handle, long length)
        => RandomAccess.SetLength(handle, length);
#endif

    /// <inheritdoc />
    public void Write(SafeFileHandle handle, ReadOnlySpan<byte> buffer, long fileOffset)
        => RandomAccess.Write(handle, buffer, fileOffset);

    /// <inheritdoc />
    public void Write(SafeFileHandle handle, IReadOnlyList<ReadOnlyMemory<byte>> buffers,
        long fileOffset)
        => RandomAccess.Write(handle, buffers, fileOffset);

    /// <inheritdoc />
    public ValueTask WriteAsync(SafeFileHandle handle, ReadOnlyMemory<byte> buffer,
        long fileOffset, CancellationToken cancellationToken = default)
        => RandomAccess.WriteAsync(handle, buffer, fileOffset, cancellationToken);

    /// <inheritdoc />
    public ValueTask WriteAsync(SafeFileHandle handle, IReadOnlyList<ReadOnlyMemory<byte>> buffers,
        long fileOffset, CancellationToken cancellationToken = default)
        => RandomAccess.WriteAsync(handle, buffers, fileOffset, cancellationToken);
}
#endif
