using System.IO.Abstractions.Benchmarks.Support;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace System.IO.Abstractions.Benchmarks;

//[SimpleJob(launchCount: 3, warmupCount: 10, targetCount: 30)]
[RPlotExporter]
[MemoryDiagnoser]
[Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class FileSystemAbstractionBenchmarks
{
    /// <summary>
    /// FileSupport type to avoid counting object initialisation on the benchmark
    /// </summary>
    private readonly FileSupport _fileSupport;
    private readonly DirectorySupport _directorySupport;

    private readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    public FileSystemAbstractionBenchmarks()
    {
        // Initialize file support
        _fileSupport = new FileSupport();
        _directorySupport = new DirectorySupport();
    }

    #region File IsFile
    [Benchmark]
    public void FileExists_DotNet() => FileSupportStatic.IsFile(_path);

    [Benchmark]
    public void FileExists_Abstraction() => _fileSupport.IsFile(_path);
    #endregion

    #region Directory Exists
    [Benchmark]
    public void DirectoryExists_DotNet() => DirectorySupportStatic.Exists(_path);

    [Benchmark]
    public void DirectoryExists_Abstraction() => _directorySupport.Exists(_path);
    #endregion
}