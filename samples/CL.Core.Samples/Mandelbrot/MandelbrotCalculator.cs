using CL.Core.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CL.Core.Samples.Mandelbrot
{
    public static class MandelbrotCalculator
    {
        public static async Task<ReadOnlyMemory<byte>> Calculate(Context ctx, Device device, uint width, uint height)
        {
            var sources = File.ReadAllText("./Mandelbrot/Mandelbrot.cl");
            var program = ctx.CreateProgram(sources);
            await program.BuildAsync(ctx.Devices);

            var queue = ctx.CreateCommandQueue(device, false, false);

            var imageBuffer = ctx.CreateBuffer<byte>().ByAllocation(width * height).AsWriteOnly();

            var mandelbrotKernel = program.CreateKernel("render");
            mandelbrotKernel.SetMemoryArgument(0, imageBuffer);

            var executionEvent = mandelbrotKernel.Execute(queue, new GlobalWorkParameters(width), new GlobalWorkParameters(height));
            await executionEvent.WaitCompleteAsync();

            var readEvent = imageBuffer.ReadAsync(queue);
            queue.Finish();
            
            return await readEvent.WaitCompleteAsync();
        }

    }
}
