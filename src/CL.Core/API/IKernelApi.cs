using System;

namespace CL.Core.API
{
    public interface IKernelApi
    {
        IntPtr clCreateKernel(IntPtr program, string kernelName, out OpenClErrorCode errorCodeRet);
        OpenClErrorCode clReleaseKernel(IntPtr kernel);
        OpenClErrorCode clSetKernelArg(IntPtr kernel, uint argIndex, ulong argSize, IntPtr argValue);
        OpenClErrorCode clGetKernelInfo(IntPtr kernel, KernelInfoParameter paramName, uint paramValueSize, IntPtr paramValue, out uint paramValueSizeReturned);

        OpenClErrorCode clEnqueueNDRangeKernel(IntPtr commandQueue, IntPtr kernel, uint workDim, UIntPtr globalWorkOffset, UIntPtr globalWorkSize, UIntPtr localWorkSize, uint numEventsInWaitList, IntPtr eventWaitList, out IntPtr evt);
    }
}