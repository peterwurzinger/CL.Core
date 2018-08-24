using System;

namespace CL.Core.API
{
    public interface ICommandQueueApi
    {
        IntPtr clCreateCommandQueue(IntPtr context, IntPtr device, CommandQueueProperties properties, out OpenClErrorCode error);

        OpenClErrorCode clRetainCommandQueue(IntPtr commandQueue);

        OpenClErrorCode clReleaseCommandQueue(IntPtr commandQueue);

        OpenClErrorCode clGetCommandQueueInfo(IntPtr commandQueue, CommandQueueInfoParameter paramName,
            uint paramValueSize, byte[] paramValue, out uint paramValueSizeReturn);
    }
}
