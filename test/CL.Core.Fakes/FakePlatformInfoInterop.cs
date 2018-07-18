using CL.Core.API;
using System;

namespace CL.Core.Fakes
{
    public class FakePlatformInfoInterop : IPlatformInfoInterop
    {
        public OpenClErrorCode? clGetPlatformIDsResult { get; set; }
        public uint clGetPlatformIDsNumPlatforms { get; set; }
        public OpenClErrorCode clGetPlatformIDs(uint numEntries, IntPtr[] platforms, out uint numPlatforms)
        {
            numPlatforms = clGetPlatformIDsNumPlatforms;
            return clGetPlatformIDsResult ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clGetPlatformInfoResult { get; set; }
        public UIntPtr clGetPlatformInfoParameterValueSizeReturned { get; set; }
        public OpenClErrorCode clGetPlatformInfo(IntPtr platform, uint parameters, UIntPtr pValueSize, byte[] parameterValue,
            out UIntPtr parameterValueSizeReturned)
        {
            parameterValueSizeReturned = clGetPlatformInfoParameterValueSizeReturned;
            return clGetPlatformInfoResult ?? OpenClErrorCode.Success;
        }
    }
}
