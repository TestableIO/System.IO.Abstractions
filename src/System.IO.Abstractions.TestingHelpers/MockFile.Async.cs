#if FEATURE_ASYNC_FILE

using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Abstractions.TestingHelpers
{
    partial class MockFile
    {
        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken))
        {
            AppendAllLines(path, contents);
            return Task.CompletedTask;
        }

        public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            AppendAllLines(path, contents, encoding);
            return Task.CompletedTask;
        }

        public override Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default(CancellationToken))
        {
            AppendAllText(path, contents);
            return Task.CompletedTask;
        }

        public override Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            AppendAllText(path, contents, encoding);
            return Task.CompletedTask;
        }

        public override Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(ReadAllBytes(path));
        }

        public override Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(ReadAllLines(path));
        }

        public override Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(ReadAllLines(path, encoding));
        }

        public override Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken)
        {
            return Task.FromResult(ReadAllText(path));
        }

        public override Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken)
        {
            return Task.FromResult(ReadAllText(path, encoding));
        }

        public override Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
        {
            WriteAllBytes(path, bytes);
            return Task.CompletedTask;
        }

        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken)
        {
            WriteAllLines(path, contents);
            return Task.CompletedTask;
        }

        public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken)
        {
            WriteAllLines(path, contents, encoding);
            return Task.CompletedTask;
        }

        public override Task WriteAllLinesAsync(string path, string[] contents, CancellationToken cancellationToken)
        {
            WriteAllLines(path, contents);
            return Task.CompletedTask;
        }

        public override Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding, CancellationToken cancellationToken)
        {
            WriteAllLines(path, contents, encoding);
            return Task.CompletedTask;
        }

        public override Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken)
        {
            WriteAllText(path, contents);
            return Task.CompletedTask;
        }

        public override Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken)
        {
            WriteAllText(path, contents, encoding);
            return Task.CompletedTask;
        }
    }
}

#endif