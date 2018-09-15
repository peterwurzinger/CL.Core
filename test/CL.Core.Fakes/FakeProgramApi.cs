using System;
using CL.Core.API;

namespace CL.Core.Fakes
{
    public class FakeProgramApi : IProgramApi
    {
        public OpenClErrorCode? clCreateProgramWithSourceErrorCode { get; set; }
        public IntPtr? clCreateProgramWithSourceReturn { get; set; }
        public IntPtr clCreateProgramWithSource(IntPtr context, uint count, string[] strings, uint[] lengths,
            out OpenClErrorCode errorCodeRet)
        {
            errorCodeRet = clCreateProgramWithSourceErrorCode ?? OpenClErrorCode.Success;
            return clCreateProgramWithSourceReturn ?? IntPtr.Zero;
        }

        public OpenClErrorCode? clBuildProgramReturn { get; set; }
        public OpenClErrorCode clBuildProgram(IntPtr program, uint numDevices, IntPtr[] devices, string options, IntPtr pfnNotify,
            IntPtr userData)
        {
            return clBuildProgramReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clGetProgramBuildInfoReturn { get; set; }
        public uint? clGetProgramBuildInfoParamvalueSizeReturned { get; set; }
        public OpenClErrorCode clGetProgramBuildInfo(IntPtr program, IntPtr device, ProgramBuildInfo paramName, uint paramValueSize,
            byte[] paramValue, out uint paramValueSizeReturned)
        {
            paramValueSizeReturned = 4;
            return clGetProgramBuildInfoReturn ?? OpenClErrorCode.Success;
        }


        public OpenClErrorCode? clRetainProgramReturn { get; set; }
        public OpenClErrorCode clRetainProgram(IntPtr program)
        {
            return clRetainProgramReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clReleaseProgramReturn { get; set; }
        public OpenClErrorCode clReleaseProgram(IntPtr program)
        {
            return clReleaseProgramReturn ?? OpenClErrorCode.Success;
        }
    }
}