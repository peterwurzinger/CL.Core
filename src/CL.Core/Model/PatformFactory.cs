using System;
using System.Collections.Generic;
using CL.Core.Native;

namespace CL.Core.Model
{
    public class PatformFactory
    {
        public static IEnumerable<Platform> GetPlatforms()
        {
            var error = PlatformCalls.clGetPlatformIDs(10, null, out var platformsCount);
            var platforms = new IntPtr[platformsCount];
            error = PlatformCalls.clGetPlatformIDs(platformsCount, platforms, out _);


            foreach (var platformId in platforms)
            {
                yield return new Platform(platformId);
            }
        }
    }
}
