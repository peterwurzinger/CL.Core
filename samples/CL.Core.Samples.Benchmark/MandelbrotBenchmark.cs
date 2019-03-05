using BenchmarkDotNet.Attributes;
using CL.Core.Native;
using CL.Core.Samples.Mandelbrot;
using System;
using System.Linq;

namespace CL.Core.Samples.Benchmark
{
    [MemoryDiagnoser]
    [RyuJitX64Job]
    public class MandelbrotBenchmark
    {
        [Params(1,2,4,8)]
        public uint SizeFactor { get; set; }

        private const uint Width = 1_980;
        private const uint Height = 1_020;

        private uint ActualWidth => Width * SizeFactor;
        private uint ActualHeight => Height * SizeFactor;

        [Benchmark]
        public ReadOnlyMemory<byte> Compute()
        {
            var api = new NativeOpenClApi();
            var factory = new PlatformFactory(api);
            var platform = factory.GetPlatforms().First(f => f.Devices.Count == 1);
            var ctx = platform.CreateContext(platform.Devices);
            var device = ctx.Devices.Single();

            //Using synchronous calculation, since the MemoryDiagnoser is only able to fetch memory allocations by one thread
            var result = MandelbrotCalculator.Calculate(ctx, device, ActualWidth, ActualHeight);

            ctx.Dispose();
            return result;
        }

    }
}
