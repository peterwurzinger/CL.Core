using CL.Core.Model;
using CL.Core.Native;
using System;
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
                var ctx = new Context(api, platform.Devices);
                ctx.Notification += CtxOnNotification;

                var sources = File.ReadAllText("ExampleProgram.cl");
                var program = ctx.CreateProgram(sources);

                program.BuildAsync(ctx.Devices).Wait();

                var kernel = program.CreateKernel("SAXPY");

                var x = new float[100];
                var y = new float[100];

                var xBuffer = ctx.CreateBuffer<float>().ByHostMemory(x).AsReadWrite();
                var yBuffer = ctx.CreateBuffer<float>().ByHostMemory(y).AsReadOnly();

                foreach (var device in platform.Devices)
                {
                    var cq = ctx.CreateCommandQueue(device, false, false);
                    Console.WriteLine(cq.Id);
                }

                ctx.Dispose();
                Console.WriteLine($"{platform.Id} - {platform.Vendor}");
            }
        }

        private static void CtxOnNotification(object sender, ContextNotificationEventArgs e)
        {
            var ctx = (Context)sender;
            Console.WriteLine($"ERROR: {ctx.Id} - {e.Message}");

        }
    }
}
