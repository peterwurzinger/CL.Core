using System;

namespace CL.Core.API
{
    public interface IProgramApi
    {
        IntPtr clCreateProgramWithSource(IntPtr context, uint count, string[] strings, uint[] lengths, out OpenClErrorCode errorCodeRet);
        //TODO: Binary?

        OpenClErrorCode clBuildProgram(IntPtr program, uint numDevices, IntPtr[] devices, string options,
            IntPtr pfnNotify, IntPtr userData);

        OpenClErrorCode clGetProgramBuildInfo(IntPtr program, IntPtr device, ProgramBuildInfo paramName,
            uint paramValueSize, byte[] paramValue, out uint paramValueSizeReturned);

        OpenClErrorCode clRetainProgram(IntPtr program);

        OpenClErrorCode clReleaseProgram(IntPtr program);
    }
}