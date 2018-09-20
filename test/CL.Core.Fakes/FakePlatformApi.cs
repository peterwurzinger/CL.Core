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
            return clGetPlatformIDsResult ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clGetPlatformInfoResult { get; set; }
        public uint clGetPlatformInfoParameterValueSizeReturned { get; set; }
        public OpenClErrorCode clGetPlatformInfo(IntPtr platform, PlatformInfoParameter parameters, uint pValueSize, IntPtr parameterValue,
            out uint parameterValueSizeReturned)
        {
            parameterValueSizeReturned = clGetPlatformInfoParameterValueSizeReturned;
            return clGetPlatformInfoResult ?? OpenClErrorCode.Success;
        }
    }
}
