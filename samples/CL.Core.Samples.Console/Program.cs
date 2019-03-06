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
            if (args.Length != 1)
                throw new ArgumentException($"Illegal number of arguments. Expected: 1, Got {args.Length}");

            var sizeFactor = uint.Parse(args[0]);
            var width = 1_980 * sizeFactor;
            var height = 1_020 * sizeFactor;

            var api = new NativeOpenClApi();

            var factory = new PlatformFactory(api);
            var platforms = factory.GetPlatforms().ToArray();
            var platform = platforms[0];

            System.Console.WriteLine($"{platform.Id} - {platform.Vendor}");

            var ctx = platform.CreateContext(platform.Devices);
            ctx.Notification += CtxOnNotification;

            var device = ctx.Devices.First();

            //var image = MandelbrotCalculator.Calculate(ctx, device, width, height);
            var image = await MandelbrotCalculator.CalculateAsync(ctx, device, width, height);

            SaveBitmap("mandelbrot", (int)width, (int)height, image.ToArray());

            ctx.Dispose();

            System.Console.WriteLine("--- Finished ---");
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