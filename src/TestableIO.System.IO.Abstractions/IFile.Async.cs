#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace System.IO.Abstractions
{
    partial interface IFile
    {
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="File.AppendAllBytesAsync(string, byte[], CancellationToken)" />
        Task AppendAllBytesAsync(string path,
            byte[] bytes,
            CancellationToken cancellationToken = default);
        
        /// <inheritdoc cref="File.AppendAllBytesAsync(string, ReadOnlyMemory{byte}, CancellationToken)" />
        Task AppendAllBytesAsync(string path,
            ReadOnlyMemory<byte> bytes,
            CancellationToken cancellationToken = default);
#endif
        
        /// <inheritdoc cref="File.AppendAllLinesAsync(string, IEnumerable{string}, CancellationToken)" />
        Task AppendAllLinesAsync(string path,
            IEnumerable<string> contents,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="File.AppendAllLinesAsync(string, IEnumerable{string}, Encoding, CancellationToken)" />
        Task AppendAllLinesAsync(string path,
            IEnumerable<string> contents,
            Encoding encoding,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="File.AppendAllTextAsync(string, string?, CancellationToken)" />
        Task AppendAllTextAsync(string path,
            string? contents,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="File.AppendAllTextAsync(string, string?, Encoding, CancellationToken)" />
        Task AppendAllTextAsync(string path,
            string? contents,
            Encoding encoding,
            CancellationToken cancellationToken = default);

#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="File.AppendAllTextAsync(string, ReadOnlyMemory{char}, CancellationToken)" />
        Task AppendAllTextAsync(string path,
            ReadOnlyMemory<char> contents,
            CancellationToken cancellationToken = default);
        
        /// <inheritdoc cref="File.AppendAllTextAsync(string, ReadOnlyMemory{char}, Encoding, CancellationToken)" />
        Task AppendAllTextAsync(string path,
            ReadOnlyMemory<char> contents,
            Encoding encoding,
            CancellationToken cancellationToken = default);
#endif
        
        /// <inheritdoc cref="File.ReadAllBytesAsync(string, CancellationToken)" />
        Task<byte[]> ReadAllBytesAsync(string path,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="File.ReadAllLinesAsync(string, CancellationToken)" />
        Task<string[]> ReadAllLinesAsync(string path,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="File.ReadAllLinesAsync(string, Encoding, CancellationToken)" />
        Task<string[]> ReadAllLinesAsync(string path,
            Encoding encoding,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="File.ReadAllTextAsync(string, CancellationToken)" />
        Task<string> ReadAllTextAsync(string path,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="File.ReadAllTextAsync(string, Encoding, CancellationToken)" />
        Task<string> ReadAllTextAsync(string path,
            Encoding encoding,
            CancellationToken cancellationToken = default);

#if FEATURE_READ_LINES_ASYNC
        /// <inheritdoc cref="File.ReadLinesAsync(string, CancellationToken)" />
        IAsyncEnumerable<string> ReadLinesAsync(string path,
            CancellationToken cancellationToken =
                default);

        /// <inheritdoc cref="File.ReadLinesAsync(string, Encoding, CancellationToken)" />
        IAsyncEnumerable<string> ReadLinesAsync(string path, Encoding encoding,
            CancellationToken cancellationToken =
                default);
#endif


        /// <inheritdoc cref="File.WriteAllBytesAsync(string, byte[], CancellationToken)" />
        Task WriteAllBytesAsync(string path,
            byte[] bytes,
            CancellationToken cancellationToken = default);

#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="File.WriteAllBytesAsync(string, ReadOnlyMemory{byte}, CancellationToken)" />
        Task WriteAllBytesAsync(string path,
            ReadOnlyMemory<byte> bytes,
            CancellationToken cancellationToken = default);
#endif

        /// <inheritdoc cref="File.WriteAllLinesAsync(string, IEnumerable{string}, CancellationToken)" />
        Task WriteAllLinesAsync(string path,
            IEnumerable<string> contents,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="File.WriteAllLinesAsync(string, IEnumerable{string}, Encoding, CancellationToken)" />
        Task WriteAllLinesAsync(string path,
            IEnumerable<string> contents,
            Encoding encoding,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="File.WriteAllTextAsync(string, string?, CancellationToken)" />
        Task WriteAllTextAsync(string path,
            string? contents,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="File.WriteAllTextAsync(string, string?, Encoding, CancellationToken)" />
        Task WriteAllTextAsync(string path,
            string? contents,
            Encoding encoding,
            CancellationToken cancellationToken = default);

#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="File.WriteAllTextAsync(string, ReadOnlyMemory{char}, CancellationToken)" />
        Task WriteAllTextAsync(string path,
            ReadOnlyMemory<char> contents,
            CancellationToken cancellationToken = default);
        
        /// <inheritdoc cref="File.WriteAllTextAsync(string, ReadOnlyMemory{char}, Encoding, CancellationToken)" />
        Task WriteAllTextAsync(string path,
            ReadOnlyMemory<char> contents,
            Encoding encoding,
            CancellationToken cancellationToken = default);
#endif
    }
}

#endif