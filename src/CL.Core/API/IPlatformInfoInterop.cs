﻿using System;

namespace CL.Core.API
{
    public interface IPlatformInfoInterop
    {
        OpenClErrorCode clGetPlatformIDs(uint numEntries, IntPtr[] platforms, out uint numPlatforms);
        OpenClErrorCode clGetPlatformInfo(IntPtr platform, PlatformInfoParameter parameters, uint pValueSize, byte[] parameterValue, out uint parameterValueSizeReturned);
    }
}
