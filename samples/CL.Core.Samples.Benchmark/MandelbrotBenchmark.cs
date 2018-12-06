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
        private const int Width = 15_840;
        private const int Height = 8_160;

        [Benchmark]
        public ReadOnlyMemory<byte> Compute()
        {
            var api = new NativeOpenClApi();
            var factory = new PlatformFactory(api);
            var platform = factory.GetPlatforms().First();
            var ctx = platform.CreateContext(platform.Devices);
            var device = ctx.Devices.Single();

            //Using synchronous calculation, since the MemoryDiagnoser is only able to fetch memory allocations by one thread
            var result = MandelbrotCalculator.Calculate(ctx, device, Width, Height);

            ctx.Dispose();
            return result;
        }

    }
}
