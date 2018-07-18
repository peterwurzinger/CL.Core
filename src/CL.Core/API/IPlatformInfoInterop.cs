using System;

namespace CL.Core.API
{
    public interface IPlatformInfoInterop
    {
        OpenClErrorCode clGetPlatformIDs(uint numEntries, IntPtr[] platforms, out uint numPlatforms);
        OpenClErrorCode clGetPlatformInfo(IntPtr platform, uint parameters, UIntPtr pValueSize, byte[] parameterValue, out UIntPtr parameterValueSizeReturned);
    }
}
