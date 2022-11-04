#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace System.IO.Abstractions
{
    partial interface IFile
    {
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

        /// <inheritdoc cref="File.WriteAllBytesAsync(string, byte[], CancellationToken)" />
        Task WriteAllBytesAsync(string path,
            byte[] bytes,
            CancellationToken cancellationToken = default);

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
    }
}

#endif