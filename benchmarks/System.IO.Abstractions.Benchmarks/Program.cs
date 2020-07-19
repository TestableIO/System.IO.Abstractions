namespace System.IO.Abstractions.Benchmarks
{
    using BenchmarkDotNet.Running;

    class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<FileSystemAbstractionBenchmarks>();
        }
    }
}
