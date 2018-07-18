using System;
using System.Collections.Generic;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core
{
    public class PatformFactory
    {
        private readonly IPlatformInfoInterop _platformInfoInterop;
        private readonly IDeviceInfoInterop _deviceInfoInterop;

        public PatformFactory(IPlatformInfoInterop platformInfoInterop, IDeviceInfoInterop deviceInfoInterop)
        {
            _platformInfoInterop = platformInfoInterop ?? throw new ArgumentNullException(nameof(platformInfoInterop));
            _deviceInfoInterop = deviceInfoInterop ?? throw new ArgumentNullException(nameof(deviceInfoInterop));
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            var errorCode = _platformInfoInterop.clGetPlatformIDs(0, null, out var platformsCount);
            errorCode.ThrowOnError();

            var platforms = new IntPtr[platformsCount];
            errorCode = _platformInfoInterop.clGetPlatformIDs(platformsCount, platforms, out _);
            errorCode.ThrowOnError();


            foreach (var platformId in platforms)
            {
                yield return new Platform(platformId, _platformInfoInterop, _deviceInfoInterop);
            }
        }
    }
}
