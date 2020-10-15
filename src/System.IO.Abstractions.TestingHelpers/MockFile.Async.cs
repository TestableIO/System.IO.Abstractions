#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Abstractions.TestingHelpers
{
    partial class MockFile
    {
        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken)) => 
            AppendAllLinesAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            AppendAllLines(path, contents, encoding);
            return Task.CompletedTask;
        }

        public override Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default(CancellationToken)) => 
            AppendAllTextAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        public override Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            AppendAllText(path, contents, encoding);
            return Task.CompletedTask;
        }

        public override Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ReadAllBytes(path));
        }

        public override Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default(CancellationToken)) => 
            ReadAllLinesAsync(path, MockFileData.DefaultEncoding, cancellationToken);

        public override Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ReadAllLines(path, encoding));
        }

        public override Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken) => 
            ReadAllTextAsync(path, MockFileData.DefaultEncoding, cancellationToken);

        public override Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ReadAllText(path, encoding));
        }

        public override Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WriteAllBytes(path, bytes);
            return Task.CompletedTask;
        }

        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken) => 
            WriteAllLinesAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WriteAllLines(path, contents, encoding);
            return Task.CompletedTask;
        }

        public override Task WriteAllLinesAsync(string path, string[] contents, CancellationToken cancellationToken) => 
            WriteAllLinesAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        public override Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WriteAllLines(path, contents, encoding);
            return Task.CompletedTask;
        }

        public override Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken) => 
            WriteAllTextAsync(path, contents, MockFileData.DefaultEncoding, cancellationToken);

        public override Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WriteAllText(path, contents, encoding);
            return Task.CompletedTask;
        }
    }
}

#endif