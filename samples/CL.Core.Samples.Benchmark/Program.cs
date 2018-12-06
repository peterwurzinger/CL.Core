using BenchmarkDotNet.Running;

namespace CL.Core.Samples.Benchmark
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<MandelbrotBenchmark>();
        }

    }
}
