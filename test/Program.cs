using ocltest.Model;
using System;
using System.Linq;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var platforms = PatformFactory.GetPlatforms().ToList();

            foreach (var platform in platforms)
            {
                Console.WriteLine($"{platform.Id}: {platform.Vendor} {platform.Name}, {platform.Profile}");
                foreach (var device in platform.Devices)
                {
                    Console.WriteLine($"\t{device.Id}: {device.Name} - {device.Type}");
                }
            }

            var targetDevice = platforms[0].Devices.First();
            var ctx = new Context(targetDevice);

            targetDevice.Dispose();
            ctx.Dispose();

            Console.ReadLine();
        }
    }
}
