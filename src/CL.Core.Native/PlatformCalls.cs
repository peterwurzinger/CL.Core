using System;
using System.Runtime.InteropServices;

namespace CL.Core.Native
{
    public static class PlatformCalls
    {
        [DllImport(Constants.DLL, EntryPoint = "clGetPlatformIDs")]
        public static extern int clGetPlatformIDs([MarshalAs(UnmanagedType.U4)] uint numEntries, [MarshalAs(UnmanagedType.LPArray)] IntPtr[] platforms, [MarshalAs(UnmanagedType.U4)] out uint numPlatforms);

        [DllImport(Constants.DLL, EntryPoint = "clGetPlatformInfo")]
        public static extern int clGetPlatformInfo(IntPtr platform, uint parameters, UIntPtr pValueSize, byte[] parameterValue, out UIntPtr parameterValueSizeReturned);
    }
}