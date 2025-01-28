#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Abstractions.TestingHelpers
{
    partial class MockFile
    {
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.AppendAllBytesAsync(string,byte[],CancellationToken)"/>
        public override Task AppendAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            AppendAllBytes(path, bytes);
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="IFile.AppendAllBytesAsync(string,ReadOnlyMemory{byte},CancellationToken)"/>
        public override Task AppendAllBytesAsync(string path, ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken = default)
        {
            return AppendAllBytesAsync(path, bytes.ToArray(), cancellationToken);
        }
#endif
        /// <inheritdoc />
        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default) =>
            AppendAllLinesAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        /// <inheritdoc />
        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            AppendAllLines(path, contents, encoding);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default) =>
            AppendAllTextAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);


        /// <inheritdoc />
        public override Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            AppendAllText(path, contents, encoding);
            return Task.CompletedTask;
        }
        
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.AppendAllTextAsync(string,ReadOnlyMemory{char},CancellationToken)"/>
        public override Task AppendAllTextAsync(string path, ReadOnlyMemory<char> contents, CancellationToken cancellationToken = default)
        {
            return AppendAllTextAsync(path, contents.ToString(), cancellationToken);
        }

        /// <inheritdoc cref="IFile.AppendAllTextAsync(string,ReadOnlyMemory{char},Encoding,CancellationToken)"/>
        public override Task AppendAllTextAsync(string path, ReadOnlyMemory<char> contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return AppendAllTextAsync(path, contents.ToString(), encoding, cancellationToken);
        }
#endif

        /// <inheritdoc />
        public override Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ReadAllBytes(path));
        }

        /// <inheritdoc />
        public override Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default) =>
            ReadAllLinesAsync(path, MockFileData.DefaultEncoding, cancellationToken);

        /// <inheritdoc />

        public override Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ReadAllLines(path, encoding));
        }

        /// <inheritdoc />
        public override Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default) =>
            ReadAllTextAsync(path, MockFileData.DefaultEncoding, cancellationToken);


        /// <inheritdoc />
        public override Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ReadAllText(path, encoding));
        }

#if FEATURE_READ_LINES_ASYNC
        /// <inheritdoc />
        public override IAsyncEnumerable<string> ReadLinesAsync(string path, CancellationToken cancellationToken = default) =>
            ReadLinesAsync(path, MockFileData.DefaultEncoding, cancellationToken);

        /// <inheritdoc />
        public override async IAsyncEnumerable<string> ReadLinesAsync(string path, Encoding encoding,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var lines = await ReadAllLinesAsync(path, encoding, cancellationToken);
            foreach (var line in lines)
                yield return line;
        }
#endif

        /// <inheritdoc />
        public override Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WriteAllBytes(path, bytes);
            return Task.CompletedTask;
        }
        
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.WriteAllBytesAsync(string,ReadOnlyMemory{byte},CancellationToken)"/>
        public override Task WriteAllBytesAsync(string path, ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken = default)
        {
            return WriteAllBytesAsync(path, bytes.ToArray(), cancellationToken);
        }
#endif

        /// <inheritdoc />
        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default) =>
            WriteAllLinesAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        /// <inheritdoc />
        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WriteAllLines(path, contents, encoding);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default) =>
            WriteAllTextAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        /// <inheritdoc />
        public override Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WriteAllText(path, contents, encoding);
            return Task.CompletedTask;
        }
        
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="IFile.WriteAllTextAsync(string,ReadOnlyMemory{char},CancellationToken)"/>
        public override Task WriteAllTextAsync(string path, ReadOnlyMemory<char> contents, CancellationToken cancellationToken = default)
        {
            return WriteAllTextAsync(path, contents.ToString(), cancellationToken);
        }

        /// <inheritdoc cref="IFile.WriteAllTextAsync(string,ReadOnlyMemory{char},Encoding,CancellationToken)"/>
        public override Task WriteAllTextAsync(string path, ReadOnlyMemory<char> contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return WriteAllTextAsync(path, contents.ToString(), encoding, cancellationToken);
        }
#endif
    }
}

#endif
