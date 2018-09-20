using System;
using System.Collections.Generic;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core
{
    public class PlatformFactory
    {
        private readonly IOpenClApi _openClApi;

        public PlatformFactory(IOpenClApi openClApi)
        {
            _openClApi = openClApi ?? throw new ArgumentNullException(nameof(openClApi));
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            var errorCode = _openClApi.PlatformApi.clGetPlatformIDs(0, null, out var platformsCount);
            errorCode.ThrowOnError();

            var platforms = new IntPtr[platformsCount];
            errorCode = _openClApi.PlatformApi.clGetPlatformIDs(platformsCount, platforms, out _);
            errorCode.ThrowOnError();


            foreach (var platformId in platforms)
            {
                yield return new Platform(platformId, _openClApi);
            }
        }
    }
}
