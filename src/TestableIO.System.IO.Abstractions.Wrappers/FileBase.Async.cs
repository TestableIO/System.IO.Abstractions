#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Abstractions
{
    partial class FileBase
    {
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.AppendAllBytesAsync(string,byte[],CancellationToken)"/>
        public abstract Task AppendAllBytesAsync(string path, byte[] bytes,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IFile.AppendAllBytesAsync(string,ReadOnlyMemory{byte},CancellationToken)"/>
        public abstract Task AppendAllBytesAsync(string path, ReadOnlyMemory<byte> bytes,
            CancellationToken cancellationToken = default);
#endif

        /// <inheritdoc cref="IFile.AppendAllLinesAsync(string,IEnumerable{string},CancellationToken)"/>
        public abstract Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IFile.AppendAllLinesAsync(string,IEnumerable{string},Encoding,CancellationToken)"/>
        public abstract Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default);


        /// <inheritdoc cref="IFile.AppendAllTextAsync(string,string,CancellationToken)"/>
        public abstract Task AppendAllTextAsync(String path, String contents, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IFile.AppendAllTextAsync(string,string,Encoding,CancellationToken)"/>
        public abstract Task AppendAllTextAsync(String path, String contents, Encoding encoding, CancellationToken cancellationToken = default);

#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.AppendAllTextAsync(string,ReadOnlyMemory{char},CancellationToken)"/>
        public abstract Task AppendAllTextAsync(string path, ReadOnlyMemory<char> contents,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IFile.AppendAllTextAsync(string,ReadOnlyMemory{char},Encoding,CancellationToken)"/>
        public abstract Task AppendAllTextAsync(string path, ReadOnlyMemory<char> contents, Encoding encoding,
            CancellationToken cancellationToken = default);
#endif

        /// <inheritdoc cref="IFile.ReadAllBytesAsync"/>
        public abstract Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default);
        /// <inheritdoc cref="IFile.ReadAllLinesAsync(string,CancellationToken)"/>
        public abstract Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IFile.ReadAllLinesAsync(string,Encoding,CancellationToken)"/>
        public abstract Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);

        ///<inheritdoc cref="IFile.ReadAllTextAsync(string,CancellationToken)"/>
        public abstract Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);

        ///<inheritdoc cref="IFile.ReadAllTextAsync(string,Encoding,CancellationToken)"/>
        public abstract Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);

#if FEATURE_READ_LINES_ASYNC
        ///<inheritdoc cref="IFile.ReadLinesAsync(string,Encoding,CancellationToken)"/>
        public abstract IAsyncEnumerable<string> ReadLinesAsync(string path,
            CancellationToken cancellationToken = default);

        ///<inheritdoc cref="IFile.ReadLinesAsync(string,Encoding,CancellationToken)"/>
        public abstract IAsyncEnumerable<string> ReadLinesAsync(string path, Encoding encoding,
            CancellationToken cancellationToken = default);
#endif

        /// <inheritdoc cref="IFile.WriteAllBytesAsync(string,byte[],CancellationToken)"/>
        public abstract Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default);

#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.WriteAllBytesAsync(string,ReadOnlyMemory{byte},CancellationToken)"/>
        public abstract Task WriteAllBytesAsync(string path, ReadOnlyMemory<byte> bytes,
            CancellationToken cancellationToken = default);
#endif

        /// <inheritdoc cref="IFile.WriteAllLinesAsync(string,IEnumerable{string},CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IFile.WriteAllLinesAsync(string,IEnumerable{string},Encoding,CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default);
        
        /// <inheritdoc cref="IFile.WriteAllTextAsync(string,string,CancellationToken)"/>
        public abstract Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IFile.WriteAllTextAsync(string,string,Encoding,CancellationToken)"/>
        public abstract Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default);

#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.WriteAllTextAsync(string,ReadOnlyMemory{char},CancellationToken)"/>
        public abstract Task WriteAllTextAsync(string path, ReadOnlyMemory<char> contents,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IFile.WriteAllTextAsync(string,ReadOnlyMemory{char},Encoding,CancellationToken)"/>
        public abstract Task WriteAllTextAsync(string path, ReadOnlyMemory<char> contents, Encoding encoding,
            CancellationToken cancellationToken = default);
#endif
    }
}

#endif