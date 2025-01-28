#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace System.IO.Abstractions
{
    partial class FileWrapper
    {
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.AppendAllBytesAsync(string,byte[],CancellationToken)"/>
        public override Task AppendAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
        {
            return File.AppendAllBytesAsync(path, bytes, cancellationToken);
        }

        /// <inheritdoc cref="IFile.AppendAllBytesAsync(string,ReadOnlyMemory{byte},CancellationToken)"/>
        public override Task AppendAllBytesAsync(string path, ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken = default)
        {
            return File.AppendAllBytesAsync(path, bytes, cancellationToken);
        }
#endif
        /// <inheritdoc />
        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
        {
            return File.AppendAllLinesAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc />
        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            return File.AppendAllLinesAsync(path, contents, encoding, cancellationToken);
        }

        /// <inheritdoc />
        public override Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        {
            return File.AppendAllTextAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc />
        public override Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            return File.AppendAllTextAsync(path, contents, encoding, cancellationToken);
        }
        
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.AppendAllTextAsync(string,ReadOnlyMemory{char},CancellationToken)"/>
        public override Task AppendAllTextAsync(string path, ReadOnlyMemory<char> contents, CancellationToken cancellationToken = default)
        {
            return File.AppendAllTextAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc cref="IFile.AppendAllTextAsync(string,ReadOnlyMemory{char},Encoding,CancellationToken)"/>
        public override Task AppendAllTextAsync(string path, ReadOnlyMemory<char> contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return File.AppendAllTextAsync(path, contents, encoding, cancellationToken);
        }
#endif

        /// <inheritdoc />
        public override Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
        {
            return File.ReadAllBytesAsync(path, cancellationToken);
        }

        /// <inheritdoc />
        public override Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default)
        {
            return File.ReadAllLinesAsync(path, cancellationToken);
        }

        /// <inheritdoc />
        public override Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
        {
            return File.ReadAllLinesAsync(path, encoding, cancellationToken);
        }

        /// <inheritdoc />
        public override Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
        {
            return File.ReadAllTextAsync(path, cancellationToken);
        }

        /// <inheritdoc />
        public override Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
        {
            return File.ReadAllTextAsync(path, encoding, cancellationToken);
        }

#if FEATURE_READ_LINES_ASYNC
        /// <inheritdoc />
        public override IAsyncEnumerable<string> ReadLinesAsync(string path,
            CancellationToken cancellationToken = default)
            => File.ReadLinesAsync(path, cancellationToken);

        /// <inheritdoc />
        public override IAsyncEnumerable<string> ReadLinesAsync(string path, Encoding encoding,
            CancellationToken cancellationToken = default)
            => File.ReadLinesAsync(path, encoding, cancellationToken);
#endif

        /// <inheritdoc />
        public override Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
        {
            return File.WriteAllBytesAsync(path, bytes, cancellationToken);
        }
        
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.WriteAllBytesAsync(string,ReadOnlyMemory{byte},CancellationToken)"/>
        public override Task WriteAllBytesAsync(string path, ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken = default)
        {
            return File.WriteAllBytesAsync(path, bytes, cancellationToken);
        }
#endif

        /// <inheritdoc />
        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
        {
            return File.WriteAllLinesAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            return File.WriteAllLinesAsync(path, contents, encoding, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        {
            return File.WriteAllTextAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            return File.WriteAllTextAsync(path, contents, encoding, cancellationToken);
        }
        
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.WriteAllTextAsync(string,ReadOnlyMemory{char},CancellationToken)"/>
        public override Task WriteAllTextAsync(string path, ReadOnlyMemory<char> contents, CancellationToken cancellationToken = default)
        {
            return File.WriteAllTextAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc cref="IFile.WriteAllTextAsync(string,ReadOnlyMemory{char},Encoding,CancellationToken)"/>
        public override Task WriteAllTextAsync(string path, ReadOnlyMemory<char> contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return File.WriteAllTextAsync(path, contents, encoding, cancellationToken);
        }
#endif
    }
}
#endif
