#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Abstractions
{
    partial class FileBase
    {
        /// <inheritdoc cref="File.AppendAllLinesAsync(string,IEnumerable{string},CancellationToken)"/>
        public abstract Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.AppendAllLinesAsync(string,IEnumerable{string},Encoding,CancellationToken)"/>
        public abstract Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken);


        /// <inheritdoc cref="File.AppendAllTextAsync(string,string,CancellationToken)"/>
        public abstract Task AppendAllTextAsync(String path, String contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.AppendAllTextAsync(string,string,Encoding,CancellationToken)"/>
        public abstract Task AppendAllTextAsync(String path, String contents, Encoding encoding, CancellationToken cancellationToken);
        /// <inheritdoc cref="File.ReadAllBytesAsync"/>
        public abstract Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken);
        /// <inheritdoc cref="File.ReadAllLinesAsync(string,CancellationToken)"/>
        public abstract Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.ReadAllLinesAsync(string,Encoding,CancellationToken)"/>
        public abstract Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken);

        ///<inheritdoc cref="File.ReadAllTextAsync(string,CancellationToken)"/>
        public abstract Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken);

        ///<inheritdoc cref="File.ReadAllTextAsync(string,Encoding,CancellationToken)"/>
        public abstract Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken);
        /// <inheritdoc cref="File.WriteAllBytesAsync"/>
        public abstract Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.WriteAllLinesAsync(string,IEnumerable{string},CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.WriteAllLinesAsync(string,IEnumerable{string},Encoding,CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.WriteAllLinesAsync(string,string[],CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, string[] contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.WriteAllLinesAsync(string,string[],Encoding,CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.WriteAllTextAsync(string,string,CancellationToken)"/>
        public abstract Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.WriteAllTextAsync(string,string,Encoding,CancellationToken)"/>
        public abstract Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken);
    }
}

#endif