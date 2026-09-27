#if FEATURE_RANDOM_ACCESS
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions.TestingHelpers;

/// <summary>
/// Placeholder for <see cref="IRandomAccess"/>: every member throws a <see cref="NotSupportedException"/>.
/// </summary>
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
public class MockRandomAccess : IRandomAccess
{
    /// <inheritdoc />
    public MockRandomAccess(IFileSystem fileSystem)
    {
        FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    /// <inheritdoc />
    public IFileSystem FileSystem { get; }

#if FEATURE_RANDOM_ACCESS_FLUSH_TO_DISK
    /// <inheritdoc />
    public void FlushToDisk(SafeFileHandle handle)
        => throw CommonExceptions.RandomAccessNotSupported();
#endif

    /// <inheritdoc />
    public long GetLength(SafeFileHandle handle)
        => throw CommonExceptions.RandomAccessNotSupported();

    /// <inheritdoc />
    public int Read(SafeFileHandle handle, Span<byte> buffer, long fileOffset)
        => throw CommonExceptions.RandomAccessNotSupported();

    /// <inheritdoc />
    public long Read(SafeFileHandle handle, IReadOnlyList<Memory<byte>> buffers, long fileOffset)
        => throw CommonExceptions.RandomAccessNotSupported();

    /// <inheritdoc />
    public ValueTask<int> ReadAsync(SafeFileHandle handle, Memory<byte> buffer, long fileOffset,
        CancellationToken cancellationToken = default)
        => throw CommonExceptions.RandomAccessNotSupported();

    /// <inheritdoc />
    public ValueTask<long> ReadAsync(SafeFileHandle handle, IReadOnlyList<Memory<byte>> buffers,
        long fileOffset, CancellationToken cancellationToken = default)
        => throw CommonExceptions.RandomAccessNotSupported();

#if FEATURE_RANDOM_ACCESS_FLUSH_TO_DISK
    /// <inheritdoc />
    public void SetLength(SafeFileHandle handle, long length)
        => throw CommonExceptions.RandomAccessNotSupported();
#endif

    /// <inheritdoc />
    public void Write(SafeFileHandle handle, ReadOnlySpan<byte> buffer, long fileOffset)
        => throw CommonExceptions.RandomAccessNotSupported();

    /// <inheritdoc />
    public void Write(SafeFileHandle handle, IReadOnlyList<ReadOnlyMemory<byte>> buffers,
        long fileOffset)
        => throw CommonExceptions.RandomAccessNotSupported();

    /// <inheritdoc />
    public ValueTask WriteAsync(SafeFileHandle handle, ReadOnlyMemory<byte> buffer,
        long fileOffset, CancellationToken cancellationToken = default)
        => throw CommonExceptions.RandomAccessNotSupported();

    /// <inheritdoc />
    public ValueTask WriteAsync(SafeFileHandle handle, IReadOnlyList<ReadOnlyMemory<byte>> buffers,
        long fileOffset, CancellationToken cancellationToken = default)
        => throw CommonExceptions.RandomAccessNotSupported();
}
#endif
