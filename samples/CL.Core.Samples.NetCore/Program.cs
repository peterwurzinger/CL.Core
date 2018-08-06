using System;
using CL.Core.API;
using CL.Core.Native;

namespace CL.Core.Samples.NetCore
{
    public class Program
    {
        public static void Main()
        {
            var nativeDeviceInterop = new NativeDeviceInfoInterop();
            var nativePlatformInfoInterop = new NativePlatformInfoInterop();



            ICommandQueueInterop cmdinterop = new NativeCommandQueueInterop();
            var factory = new PatformFactory(nativePlatformInfoInterop, nativeDeviceInterop);
            var platforms = factory.GetPlatforms();
            foreach (var platform in platforms)
            {
                
                Console.WriteLine($"{platform.Id} - {platform.Vendor}");
            }
        }
    }
}
