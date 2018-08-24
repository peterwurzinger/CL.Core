using CL.Core.Native;
using System;
using CL.Core.Model;

namespace CL.Core.Samples.NetCore
{
    public class Program
    {
        public static void Main()
        {
            var api = new NativeOpenClApi();
            
            var factory = new PatformFactory(api);
            var platforms = factory.GetPlatforms();
            foreach (var platform in platforms)
            {
                var ctx = new Context(api, platform.Devices);

                foreach (var device in platform.Devices)
                {
                    var cq = ctx.CreateCommandQueue(device, false, false);
                    Console.WriteLine(cq.Id);
                }

                ctx.Dispose();
                Console.WriteLine($"{platform.Id} - {platform.Vendor}");
            }
        }
    }
}
