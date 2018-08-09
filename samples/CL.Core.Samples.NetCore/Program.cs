using CL.Core.Native;
using System;
using CL.Core.Model;

namespace CL.Core.Samples.NetCore
{
    public class Program
    {
        public static void Main()
        {
            var nativeDeviceInterop = new NativeDeviceInfoInterop();
            var nativeContextInterop = new NativeContextInterop();
            var nativePlatformInfoInterop = new NativePlatformInfoInterop();
            var nativeCommandQueueInterop = new NativeCommandQueueInterop();
            
            var factory = new PatformFactory(nativePlatformInfoInterop, nativeDeviceInterop);
            var platforms = factory.GetPlatforms();
            foreach (var platform in platforms)
            {
                var ctx = new Context(nativeContextInterop, platform.Devices);

                foreach (var device in platform.Devices)
                {
                    var cq = ctx.CreateCommandQueue(device, false, false, nativeCommandQueueInterop);
                    Console.WriteLine(cq.Id);
                }

                ctx.Dispose();
                Console.WriteLine($"{platform.Id} - {platform.Vendor}");
            }
        }
    }
}
