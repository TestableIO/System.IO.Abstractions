﻿#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Abstractions
{
    partial class FileBase
    {
        /// <inheritdoc cref="IFile.AppendAllLinesAsync(string,IEnumerable{string},CancellationToken)"/>
        public abstract Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="IFile.AppendAllLinesAsync(string,IEnumerable{string},Encoding,CancellationToken)"/>
        public abstract Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken);


        /// <inheritdoc cref="IFile.AppendAllTextAsync(string,string,CancellationToken)"/>
        public abstract Task AppendAllTextAsync(String path, String contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="IFile.AppendAllTextAsync(string,string,Encoding,CancellationToken)"/>
        public abstract Task AppendAllTextAsync(String path, String contents, Encoding encoding, CancellationToken cancellationToken);
        /// <inheritdoc cref="IFile.ReadAllBytesAsync"/>
        public abstract Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken);
        /// <inheritdoc cref="IFile.ReadAllLinesAsync(string,CancellationToken)"/>
        public abstract Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken);

        /// <inheritdoc cref="IFile.ReadAllLinesAsync(string,Encoding,CancellationToken)"/>
        public abstract Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken);

        ///<inheritdoc cref="IFile.ReadAllTextAsync(string,CancellationToken)"/>
        public abstract Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken);

        ///<inheritdoc cref="IFile.ReadAllTextAsync(string,Encoding,CancellationToken)"/>
        public abstract Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken);

#if FEATURE_READ_LINES_ASYNC
        ///<inheritdoc cref="IFile.ReadLinesAsync(string,Encoding,CancellationToken)"/>
        public abstract IAsyncEnumerable<string> ReadLinesAsync(string path,
            CancellationToken cancellationToken = default);

        ///<inheritdoc cref="IFile.ReadLinesAsync(string,Encoding,CancellationToken)"/>
        public abstract IAsyncEnumerable<string> ReadLinesAsync(string path, Encoding encoding,
            CancellationToken cancellationToken = default);
#endif

        /// <inheritdoc cref="IFile.WriteAllBytesAsync"/>
        public abstract Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken);

        /// <inheritdoc cref="IFile.WriteAllLinesAsync(string,IEnumerable{string},CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="IFile.WriteAllLinesAsync(string,IEnumerable{string},Encoding,CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken);
        
        /// <inheritdoc cref="IFile.WriteAllTextAsync(string,string,CancellationToken)"/>
        public abstract Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="IFile.WriteAllTextAsync(string,string,Encoding,CancellationToken)"/>
        public abstract Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken);
    }
}

#endif