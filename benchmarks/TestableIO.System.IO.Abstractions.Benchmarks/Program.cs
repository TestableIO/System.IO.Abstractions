namespace System.IO.Abstractions.Benchmarks;

using BenchmarkDotNet.Running;

static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}