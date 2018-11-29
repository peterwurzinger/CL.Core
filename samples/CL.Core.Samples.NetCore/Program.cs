using CL.Core.Model;
using CL.Core.Native;
using System;
using System.Diagnostics;
using System.IO;
using CL.Core.API;

namespace CL.Core.Samples.NetCore
{
    public static class Program
    {
        public static void Main()
        {
            var api = new NativeOpenClApi();

            var factory = new PlatformFactory(api);
            var platforms = factory.GetPlatforms();

            foreach (var platform in platforms)
            {
                Console.WriteLine($"{platform.Id} - {platform.Vendor}");

                var ctx = platform.CreateContext(platform.Devices);
                ctx.Notification += CtxOnNotification;

                var sources = File.ReadAllText("ExampleProgram.cl");
                var program = ctx.CreateProgram(sources);
                program.BuildAsync(ctx.Devices).Wait();

                //var fromBinaries = ctx.CreateProgram(program.Builds.AsBinariesDictionary());
                //fromBinaries.BuildAsync(ctx.Devices).Wait();

                var kernel = program.CreateKernel("SAXPY");

                const long workSize = 100_000_000;

                var x = new float[workSize];
                var rnd = new Random();
                for (var i = 0; i < x.Length; i++)
                {
                    x[i] = (float)rnd.NextDouble() * 100;
                }

                var xBuffer = ctx.CreateBuffer<float>().ByAllocation((uint)workSize).AsReadWrite();
                var yBuffer = ctx.CreateBuffer<float>().ByAllocation((uint)workSize).AsReadOnly();

                var xSub = xBuffer.CreateSubBuffer(new BufferRegion(0, 16)).AsReadOnly();

                kernel.SetMemoryArgument(0, xBuffer);
                kernel.SetMemoryArgument(1, yBuffer);
                kernel.SetArgument(2, 300f);

                var watch = new Stopwatch();
                foreach (var device in platform.Devices)
                {
                    Console.WriteLine($"\t{device.Name}");
                    watch.Reset();
                    watch.Start();

                    var cq = ctx.CreateCommandQueue(device, false, false);

                    xBuffer.Write(cq, x);

                    var executionEvent = kernel.Execute(cq, new GlobalWorkParameters((uint)x.Length));

                    cq.Flush();

                    executionEvent.WaitComplete();

                    var readBuffer = yBuffer.Read(cq);
                    cq.Finish();
                    watch.Stop();
                    Console.WriteLine($"Wrote, multiplied and read back {workSize} items in {watch.Elapsed.TotalMilliseconds}ms");
                }

                ctx.Dispose();
            }

            Console.WriteLine("--- Finished ---");
            Console.ReadLine();
        }

        private static void CtxOnNotification(object sender, ContextNotificationEventArgs e)
        {
            var ctx = (Context)sender;
            Console.WriteLine($"ERROR: {ctx.Id} - {e.Message}");

        }
    }
}
