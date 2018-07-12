using System;
using System.Runtime.InteropServices;

namespace CL.Core.Native
{
    public static class ContextCalls
    {
        [DllImport(Constants.DLL, EntryPoint = "clCreateContext")]
        public static extern IntPtr CreateContext(IntPtr properties, uint numDevices, IntPtr[] deviceIds,
            IntPtr pfnNotify, IntPtr userData, out int errorCode);


        [DllImport(Constants.DLL, EntryPoint = "clReleaseContext")]
        public static extern int ReleaseContext(IntPtr contextId);
    }
}
