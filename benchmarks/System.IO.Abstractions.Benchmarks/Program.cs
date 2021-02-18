namespace System.IO.Abstractions.Benchmarks
{
    using BenchmarkDotNet.Running;
    using System.Reflection;

    class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
