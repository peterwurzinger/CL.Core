using CL.Core.API;
using System;
using System.Runtime.InteropServices;

namespace CL.Core.Native
{
    internal class NativeKernelApi : IKernelApi
    {
        [DllImport(Constants.DLL, EntryPoint = "clCreateKernel")]
        public static extern IntPtr clCreateKernel(IntPtr program, [MarshalAs(UnmanagedType.LPStr)]string kernelName, out OpenClErrorCode errorCodeRet);

        [DllImport(Constants.DLL, EntryPoint = "clReleaseKernel")]
        public static extern OpenClErrorCode clReleaseKernel(IntPtr kernel);

        [DllImport(Constants.DLL, EntryPoint = "clSetKernelArg")]
        public static extern OpenClErrorCode clSetKernelArg(IntPtr kernel, uint argIndex, ulong argSize,
            IntPtr argValue);

        [DllImport(Constants.DLL, EntryPoint = "clGetKernelInfo")]
        public static extern OpenClErrorCode clGetKernelInfo(IntPtr kernel, KernelInfoParameter paramName,
            uint paramValueSize, IntPtr paramValue,
            out uint paramValueSizeReturned);

        IntPtr IKernelApi.clCreateKernel(IntPtr program, string kernelName, out OpenClErrorCode errorCodeRet)
        {
            return clCreateKernel(program, kernelName, out errorCodeRet);
        }

        OpenClErrorCode IKernelApi.clReleaseKernel(IntPtr kernel)
        {
            return clReleaseKernel(kernel);
        }

        OpenClErrorCode IKernelApi.clSetKernelArg(IntPtr kernel, uint argIndex, ulong argSize, IntPtr argValue)
        {
            return clSetKernelArg(kernel, argIndex, argSize, argValue);
        }

        OpenClErrorCode IKernelApi.clGetKernelInfo(IntPtr kernel, KernelInfoParameter paramName, uint paramValueSize, IntPtr paramValue,
            out uint paramValueSizeReturned)
        {
            return clGetKernelInfo(kernel, paramName, paramValueSize, paramValue, out paramValueSizeReturned);
        }
    }
}