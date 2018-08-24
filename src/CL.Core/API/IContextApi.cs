using System;

namespace CL.Core.API
{
    public interface IContextApi
    {
        IntPtr clCreateContext(IntPtr properties, uint numDevices, IntPtr[] deviceIds,
            IntPtr pfnNotify, IntPtr userData, out OpenClErrorCode errorCode);

        OpenClErrorCode clReleaseContext(IntPtr contextId);

    }
}
