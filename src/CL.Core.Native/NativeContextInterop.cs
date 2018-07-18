using System;
using System.Runtime.InteropServices;
using CL.Core.API;

namespace CL.Core.Native
{
    public class NativeContextInterop : IContextInterop
    {
        [DllImport(Constants.DLL, EntryPoint = "clCreateContext")]
        public static extern IntPtr clCreateContext(IntPtr properties, uint numDevices, IntPtr[] deviceIds,
            IntPtr pfnNotify, IntPtr userData, out OpenClErrorCode errorCode);

        [DllImport(Constants.DLL, EntryPoint = "clReleaseContext")]
        public static extern OpenClErrorCode clReleaseContext(IntPtr contextId);

        OpenClErrorCode IContextInterop.clReleaseContext(IntPtr contextId)
        {
            return clReleaseContext(contextId);
        }

        IntPtr IContextInterop.clCreateContext(IntPtr properties, uint numDevices, IntPtr[] deviceIds, IntPtr pfnNotify, IntPtr userData,
            out OpenClErrorCode errorCode)
        {
            return clCreateContext(properties, numDevices, deviceIds, pfnNotify, userData, out errorCode);
        }
    }
}
