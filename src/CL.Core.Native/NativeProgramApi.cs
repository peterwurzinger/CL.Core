using CL.Core.API;
using System;
using System.Runtime.InteropServices;

namespace CL.Core.Native
{
    internal class NativeProgramApi : IProgramApi
    {
        [DllImport(Constants.DLL, EntryPoint = "clCreateProgramWithSource")]
        public static extern IntPtr clCreateProgramWithSource(IntPtr context, uint count, string[] strings, uint[] lengths,
            out OpenClErrorCode errorCodeRet);

        [DllImport(Constants.DLL, EntryPoint = "clGetProgramInfo")]
        public static extern OpenClErrorCode clGetProgramInfo(IntPtr program,
            ProgramInfoParameter paramName, uint paramValueSize,
            IntPtr paramValue, out uint paramValueSizeReturned);

        [DllImport(Constants.DLL, EntryPoint = "clBuildProgram", CharSet = CharSet.Unicode)]
        public static extern OpenClErrorCode clBuildProgram(IntPtr program, uint numDevices, IntPtr[] devices, string options,
            IntPtr pfnNotify, IntPtr userData);


        [DllImport(Constants.DLL, EntryPoint = "clRetainProgram")]
        public static extern OpenClErrorCode clRetainProgram(IntPtr program);

        [DllImport(Constants.DLL, EntryPoint = "clReleaseProgram")]
        public static extern OpenClErrorCode clReleaseProgram(IntPtr program);

        [DllImport(Constants.DLL, EntryPoint = "clGetProgramBuildInfo")]
        public static extern OpenClErrorCode clGetProgramBuildInfo(IntPtr program, IntPtr device,
            ProgramBuildInfoParameter paramName, uint paramValueSize,
            IntPtr paramValue, out uint paramValueSizeReturned);

        IntPtr IProgramApi.clCreateProgramWithSource(IntPtr context, uint count, string[] strings, uint[] lengths,
            out OpenClErrorCode errorCodeRet)
        {
            return clCreateProgramWithSource(context, count, strings, lengths, out errorCodeRet);
        }

        OpenClErrorCode IProgramApi.clGetProgramInfo(IntPtr program, ProgramInfoParameter paramName, uint paramValueSize, IntPtr paramValue,
            out uint paramValueSizeReturned)
        {
            return clGetProgramInfo(program, paramName, paramValueSize, paramValue, out paramValueSizeReturned);
        }

        OpenClErrorCode IProgramApi.clBuildProgram(IntPtr program, uint numDevices, IntPtr[] devices, string options, IntPtr pfnNotify,
            IntPtr userData)
        {
            return clBuildProgram(program, numDevices, devices, options, pfnNotify, userData);
        }

        OpenClErrorCode IProgramApi.clRetainProgram(IntPtr program)
        {
            return clRetainProgram(program);
        }

        OpenClErrorCode IProgramApi.clReleaseProgram(IntPtr program)
        {
            return clReleaseProgram(program);
        }

        OpenClErrorCode IProgramApi.clGetProgramBuildInfo(IntPtr program, IntPtr device, ProgramBuildInfoParameter paramName, uint paramValueSize,
            IntPtr paramValue, out uint paramValueSizeReturned)
        {
            return clGetProgramBuildInfo(program, device, paramName, paramValueSize, paramValue,
                out paramValueSizeReturned);
        }
    }
}