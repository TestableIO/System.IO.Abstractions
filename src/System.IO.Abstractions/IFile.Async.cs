#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace System.IO.Abstractions
{
    partial interface IFile
    {
        Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken));
        /// <inheritdoc cref="File.AppendAllLinesAsync(string,IEnumerable{string},Encoding,CancellationToken)"/>
        Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken));
        /// <inheritdoc cref="File.AppendAllText(string,string)"/>

        /// <inheritdoc cref="File.AppendAllTextAsync(string,string,CancellationToken)"/>
        Task AppendAllTextAsync(String path, String contents, CancellationToken cancellationToken = default(CancellationToken));
        /// <inheritdoc cref="File.AppendAllTextAsync(string,string,Encoding,CancellationToken)"/>
        Task AppendAllTextAsync(String path, String contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken));
        /// <inheritdoc cref="File.ReadAllBytesAsync"/>
        Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default(CancellationToken));

        /// <inheritdoc cref="File.ReadAllLinesAsync(string,CancellationToken)"/>
        Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default(CancellationToken));
        /// <inheritdoc cref="File.ReadAllLinesAsync(string,Encoding,CancellationToken)"/>
        Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken));

        ///<inheritdoc cref="File.ReadAllTextAsync(string,CancellationToken)"/>
        Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default(CancellationToken));
        ///<inheritdoc cref="File.ReadAllTextAsync(string,Encoding,CancellationToken)"/>
        Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken));
        /// <inheritdoc cref="File.WriteAllLinesAsync(string,IEnumerable{string},CancellationToken)"/>
        Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken));
        /// <inheritdoc cref="File.WriteAllLinesAsync(string,IEnumerable{string},Encoding,CancellationToken)"/>
        Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken));
        /// <inheritdoc cref="File.WriteAllLinesAsync(string,string[],CancellationToken)"/>
        Task WriteAllLinesAsync(string path, string[] contents, CancellationToken cancellationToken = default(CancellationToken));
        /// <inheritdoc cref="File.WriteAllLinesAsync(string,string[],Encoding,CancellationToken)"/>
        Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken));

        /// <inheritdoc cref="File.WriteAllTextAsync(string,string,CancellationToken)"/>
        Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default(CancellationToken));
        /// <inheritdoc cref="File.WriteAllTextAsync(string,string,Encoding,CancellationToken)"/>
        Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken));
 
    /// <inheritdoc cref="File.WriteAllBytesAsync"/>
        Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default(CancellationToken));
       }
}

#endif