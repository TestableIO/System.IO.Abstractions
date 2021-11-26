#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace System.IO.Abstractions
{
    partial class FileWrapper
    {
        /// <inheritdoc />
        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken)
        {
            return File.AppendAllLinesAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc />
        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken)
        {
            return File.AppendAllLinesAsync(path, contents, encoding, cancellationToken);
        }

        /// <inheritdoc />
        public override Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken)
        {
            return File.AppendAllTextAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc />
        public override Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken)
        {
            return File.AppendAllTextAsync(path, contents, encoding, cancellationToken);
        }

        /// <inheritdoc />
        public override Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken)
        {
            return File.ReadAllBytesAsync(path, cancellationToken);
        }

        /// <inheritdoc />
        public override Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken)
        {
            return File.ReadAllLinesAsync(path, cancellationToken);
        }

        /// <inheritdoc />
        public override Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken)
        {
            return File.ReadAllLinesAsync(path, encoding, cancellationToken);
        }

        /// <inheritdoc />
        public override Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken)
        {
            return File.ReadAllTextAsync(path, cancellationToken);
        }

        /// <inheritdoc />
        public override Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken)
        {
            return File.ReadAllTextAsync(path, encoding, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
        {
            return File.WriteAllBytesAsync(path, bytes, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken)
        {
            return File.WriteAllLinesAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken)
        {
            return File.WriteAllLinesAsync(path, contents, encoding, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAllLinesAsync(string path, string[] contents, CancellationToken cancellationToken)
        {
            return File.WriteAllLinesAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding, CancellationToken cancellationToken)
        {
            return File.WriteAllLinesAsync(path, contents, encoding, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken)
        {
            return File.WriteAllTextAsync(path, contents, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken)
        {
            return File.WriteAllTextAsync(path, contents, encoding, cancellationToken);
        }
    }
}
#endif
