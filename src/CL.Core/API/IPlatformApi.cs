using System;

namespace CL.Core.API
{
    public interface IPlatformApi
    {
        OpenClErrorCode clGetPlatformIDs(uint numEntries, IntPtr[] platforms, out uint numPlatforms);
        OpenClErrorCode clGetPlatformInfo(IntPtr platform, PlatformInfoParameter parameters, uint pValueSize, IntPtr parameterValue, out uint parameterValueSizeReturned);
    }
}
