using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.Benchmarks
{
    [RPlotExporter]
    [MemoryDiagnoser]
    public class MockFileSystemBenchmarks
    {
        private readonly Dictionary<string, MockFileData> testData = CreateTestData();

        private Dictionary<string, MockFileData> CreateTestData()
        {
            var filesCount = 100000;
            var maxDirectoryDepth = 8;
            return Enumerable.Range(0, filesCount).ToDictionary(
                i => XFS.Path(@$"C:\{string.Join(@"\", Enumerable.Range(0, i % maxDirectoryDepth + 1).Select(i => i.ToString()))}\{i}.bin"),
                i => new MockFileData(i.ToString()));
        }

        [Benchmark]
        public MockFileSystem MockFileSystem_Constructor() => new MockFileSystem(testData);
    }
}
