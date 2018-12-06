using CL.Core.Model;
using CL.Core.Native;
using CL.Core.Samples.Mandelbrot;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CL.Core.Samples.Console
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var api = new NativeOpenClApi();

            var factory = new PlatformFactory(api);
            var platforms = factory.GetPlatforms();

            foreach (var platform in platforms.Where(p => p.Devices.Count == 1))
            {
                System.Console.WriteLine($"{platform.Id} - {platform.Vendor}");

                var ctx = platform.CreateContext(platform.Devices);
                ctx.Notification += CtxOnNotification;

                const int width = 31_760/2;
                const int height = 16_320/2;

                var device = ctx.Devices.Single();

                var image = await MandelbrotCalculator.CalculateAsync(ctx, device, width, height);

                SaveBitmap("mandelbrot", width, height, image);

                ctx.Dispose();
            }

            System.Console.WriteLine("--- Finished ---");
            System.Console.ReadLine();
        }

        public static unsafe void SaveBitmap(string fileName, int width, int height, ReadOnlyMemory<byte> imageData)
        {
            var handle = imageData.Pin();

            using (var image = new Bitmap(width, height, width,
                PixelFormat.Format8bppIndexed, (IntPtr)handle.Pointer))
            {
                image.Save(Path.ChangeExtension(fileName, ".jpg"));
            }
            handle.Dispose();
        }


        private static void CtxOnNotification(object sender, ContextNotificationEventArgs e)
        {
            var ctx = (Context)sender;
            System.Console.WriteLine($"ERROR: {ctx.Id} - {e.Message}");

        }
    }
}