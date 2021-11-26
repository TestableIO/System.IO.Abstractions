using System;
using System.IO;
using System.IO.Abstractions.Benchmarks.Support;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace System.IO.Abstractions.Benchmarks
{
    //[SimpleJob(launchCount: 3, warmupCount: 10, targetCount: 30)]
    [RPlotExporter]
    [MemoryDiagnoser]
    [Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class FileSystemAbstractionBenchmarks
    {
        #region Members
        /// <summary>
        /// FileSupport type to avoid counting object initialisation on the benchmark
        /// </summary>
        private FileSupport _fileSupport;
        private DirectorySupport _directorySupport;
        #endregion

        #region CTOR's
        public FileSystemAbstractionBenchmarks()
        {
            // Initialize file support
            _fileSupport = new FileSupport();
            _directorySupport = new DirectorySupport();
        }
        #endregion

        #region File IsFile
        [Benchmark]
        public void FileExists_DotNet() => FileSupportStatic.IsFile(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

        [Benchmark]
        public void FileExists_Abstraction() => _fileSupport.IsFile(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        #endregion

        #region Directory Exists
        [Benchmark]
        public void DirectoryExists_DotNet() => DirectorySupportStatic.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

        [Benchmark]
        public void DirectoryExists_Abstraction() => _directorySupport.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        #endregion
    }
}
