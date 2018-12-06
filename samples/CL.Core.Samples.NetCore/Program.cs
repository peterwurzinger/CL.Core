using CL.Core.Model;
using CL.Core.Native;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace CL.Core.Samples.NetCore
{
    public static class Program
    {
        public static void Main()
        {
            var api = new NativeOpenClApi();

            var factory = new PlatformFactory(api);
            var platforms = factory.GetPlatforms();

            foreach (var platform in platforms.Where(p => p.Devices.Count == 1))
            {
                Console.WriteLine($"{platform.Id} - {platform.Vendor}");

                var ctx = platform.CreateContext(platform.Devices);
                ctx.Notification += CtxOnNotification;

                var sources = File.ReadAllText("Mandelbrot.cl");
                var program = ctx.CreateProgram(sources);
                program.BuildAsync(ctx.Devices).Wait();

                const int width = 31_680;
                const int height = 17_280;

                var device = ctx.Devices.Single();
                var queues = ctx.CreateCommandQueue(device, false, false);

                var imageBuffer = ctx.CreateBuffer<byte>().ByAllocation(width * height).AsWriteOnly();

                var mandelbrotKernel = program.CreateKernel("render");
                mandelbrotKernel.SetMemoryArgument(0, imageBuffer);

                var evt = mandelbrotKernel.Execute(queues, new GlobalWorkParameters(width), new GlobalWorkParameters(height));
                evt.WaitComplete();
                var image = imageBuffer.Read(queues);

                queues.Finish();

                SaveBitmap("mandelbrot", width, height, image);

                ctx.Dispose();
            }

            Console.WriteLine("--- Finished ---");
            Console.ReadLine();
        }

        public static unsafe void SaveBitmap(string fileName, int width, int height, ReadOnlySpan<byte> imageData)
        {
            fixed (void* ptr = imageData)
            {
                using (var image = new Bitmap(width, height, width,
                    PixelFormat.Format8bppIndexed, (IntPtr)ptr))
                {
                    image.Save(Path.ChangeExtension(fileName, ".jpg"));
                }
            }
        }

        private static void CtxOnNotification(object sender, ContextNotificationEventArgs e)
        {
            var ctx = (Context)sender;
            Console.WriteLine($"ERROR: {ctx.Id} - {e.Message}");

        }
    }
}