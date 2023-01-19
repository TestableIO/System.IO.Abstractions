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
        /// <inheritdoc />
        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken)) =>
            AppendAllLinesAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        /// <inheritdoc />
        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            AppendAllLines(path, contents, encoding);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default(CancellationToken)) =>
            AppendAllTextAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);


        /// <inheritdoc />
        public override Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            AppendAllText(path, contents, encoding);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ReadAllBytes(path));
        }

        /// <inheritdoc />
        public override Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default(CancellationToken)) =>
            ReadAllLinesAsync(path, MockFileData.DefaultEncoding, cancellationToken);

        /// <inheritdoc />

        public override Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ReadAllLines(path, encoding));
        }

        /// <inheritdoc />
        public override Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken) =>
            ReadAllTextAsync(path, MockFileData.DefaultEncoding, cancellationToken);


        /// <inheritdoc />
        public override Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken)
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
        public override Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WriteAllBytes(path, bytes);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken) =>
            WriteAllLinesAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        /// <inheritdoc />
        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WriteAllLines(path, contents, encoding);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken) =>
            WriteAllTextAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        /// <inheritdoc />
        public override Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WriteAllText(path, contents, encoding);
            return Task.CompletedTask;
        }
    }
}

#endif
