using CL.Core.Model;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace CL.Core.Samples.Mandelbrot
{
    public static class MandelbrotCalculator
    {
        public static async Task<IReadOnlyCollection<byte>> CalculateAsync(Context ctx, Device device, uint width, uint height)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "~", "Mandelbrot/Mandelbrot.cl");
            var sources = File.ReadAllText(path);
            var program = ctx.CreateProgram(sources);
            await program.BuildAsync(ctx.Devices);
            var queue = ctx.CreateCommandQueue(device, false, false);

            var imageBuffer = ctx.CreateBuffer<byte>().ByAllocation(width * height).AsWriteOnly();

            var mandelbrotKernel = program.CreateKernel("render");
            mandelbrotKernel.SetMemoryArgument(0, imageBuffer);

            var executionEvent = mandelbrotKernel.Execute(queue, new GlobalWorkParameters(width), new GlobalWorkParameters(height));

            var readTask = imageBuffer.ReadAsync(queue);
            queue.Finish();
            await executionEvent.WaitCompleteAsync();

            return await readTask;
        }

        public static IReadOnlyCollection<byte> Calculate(Context ctx, Device device, uint width, uint height)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "~", "Mandelbrot/Mandelbrot.cl");
            var sources = File.ReadAllText(path);
            var program = ctx.CreateProgram(sources);
            program.BuildAsync(ctx.Devices).Wait();
            var queue = ctx.CreateCommandQueue(device, false, false);

            var imageBuffer = ctx.CreateBuffer<byte>().ByAllocation(width * height).AsWriteOnly();

            var mandelbrotKernel = program.CreateKernel("render");
            mandelbrotKernel.SetMemoryArgument(0, imageBuffer);

            var executionEvent = mandelbrotKernel.Execute(queue, new GlobalWorkParameters(width), new GlobalWorkParameters(height));

            var readEvent = imageBuffer.Read(queue);
            queue.Finish();

            executionEvent.WaitComplete();

            return readEvent;
        }

    }
}
