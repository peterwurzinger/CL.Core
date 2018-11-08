using CL.Core.API;
using System;

namespace CL.Core.Fakes
{
    public class FakePlatformApi : IPlatformApi
    {
        public OpenClErrorCode? clGetPlatformIDsResult { get; set; }
        public uint clGetPlatformIDsNumPlatforms { get; set; }
        public OpenClErrorCode clGetPlatformIDs(uint numEntries, IntPtr[] platforms, out uint numPlatforms)
        {
            numPlatforms = clGetPlatformIDsNumPlatforms;

            var errorCode = clGetPlatformIDsResult ?? OpenClErrorCode.Success;
            if (errorCode == OpenClErrorCode.Success && platforms != null)
            {
                for (var i = 0; i < platforms.Length; i++)
                    platforms[i] = new IntPtr(i + 1);
            }

            return errorCode;
        }

        public OpenClErrorCode? clGetPlatformInfoResult { get; set; }
        public uint? clGetPlatformInfoParameterValueSizeReturned { get; set; }
        public OpenClErrorCode clGetPlatformInfo(IntPtr platform, PlatformInfoParameter parameters, uint pValueSize, IntPtr parameterValue,
            out uint parameterValueSizeReturned)
        {
            parameterValueSizeReturned = clGetPlatformInfoParameterValueSizeReturned ?? 8;
            return clGetPlatformInfoResult ?? OpenClErrorCode.Success;
        }
    }
}
