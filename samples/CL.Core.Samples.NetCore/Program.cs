using CL.Core.Model;
using CL.Core.Native;
using System;
using System.Diagnostics;
using System.IO;

namespace CL.Core.Samples.NetCore
{
    public class Program
    {
        public static void Main()
        {
            var api = new NativeOpenClApi();

            var factory = new PlatformFactory(api);
            var platforms = factory.GetPlatforms();

            foreach (var platform in platforms)
            {
                Console.WriteLine($"{platform.Id} - {platform.Vendor}");

                var ctx = new Context(api, platform.Devices);
                ctx.Notification += CtxOnNotification;

                var sources = File.ReadAllText("ExampleProgram.cl");
                var program = ctx.CreateProgram(sources);
                program.BuildAsync(ctx.Devices).Wait();

                var fromBinaries = ctx.CreateProgram(program.Builds.AsBinariesDictionary());

                var kernel = program.CreateKernel("SAXPY");

                const long workSize = 200_000_000;

                var x = new float[workSize];
                var rnd = new Random();
                for (var i = 0; i < x.Length; i++)
                {
                    x[i] = (float)rnd.NextDouble() * 100;
                }

                var xBuffer = ctx.CreateBuffer<float>().ByAllocation((uint)workSize).AsReadWrite();
                var yBuffer = ctx.CreateBuffer<float>().ByAllocation((uint)workSize).AsReadOnly();

                kernel.SetMemoryArgument(0, xBuffer);
                kernel.SetMemoryArgument(1, yBuffer);
                kernel.SetArgument(2, 300f);

                var watch = new Stopwatch();
                foreach (var device in platform.Devices)
                {
                    watch.Reset();
                    watch.Start();

                    var cq = ctx.CreateCommandQueue(device, false, false);

                    xBuffer.Write(cq, x);
                    var executionEvent = kernel.Execute(cq, 1, new[] { (ulong)x.Length });

                    cq.Flush();

                    executionEvent.Completion.Wait();

                    var readBuffer = yBuffer.Read(cq);
                    watch.Stop();
                    Console.WriteLine($"Wrote, multiplied and read back {workSize} items in {watch.Elapsed.TotalMilliseconds}ms");
                }

                ctx.Dispose();
                Console.WriteLine("--- Finished ---");
                Console.ReadLine();
            }
        }

        private static void CtxOnNotification(object sender, ContextNotificationEventArgs e)
        {
            var ctx = (Context)sender;
            Console.WriteLine($"ERROR: {ctx.Id} - {e.Message}");

        }
    }
}
