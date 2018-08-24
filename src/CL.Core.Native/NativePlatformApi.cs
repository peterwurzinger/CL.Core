using System;
using System.Runtime.InteropServices;
using CL.Core.API;

namespace CL.Core.Native
{
    internal class NativePlatformApi : IPlatformApi
    {
        [DllImport(Constants.DLL, EntryPoint = "clGetPlatformIDs")]
        public static extern OpenClErrorCode clGetPlatformIDs([MarshalAs(UnmanagedType.U4)] uint numEntries, [MarshalAs(UnmanagedType.LPArray)] IntPtr[] platforms, [MarshalAs(UnmanagedType.U4)] out uint numPlatforms);

        [DllImport(Constants.DLL, EntryPoint = "clGetPlatformInfo")]
        public static extern OpenClErrorCode clGetPlatformInfo(IntPtr platform, PlatformInfoParameter parameter, uint pValueSize, byte[] parameterValue, out uint parameterValueSizeReturned);

        OpenClErrorCode IPlatformApi.clGetPlatformInfo(IntPtr platform, PlatformInfoParameter parameter, uint pValueSize, byte[] parameterValue,
            out uint parameterValueSizeReturned)
        {
            return clGetPlatformInfo(platform, parameter, pValueSize, parameterValue, out parameterValueSizeReturned);
        }

        OpenClErrorCode IPlatformApi.clGetPlatformIDs(uint numEntries, IntPtr[] platforms, out uint numPlatforms)
        {
            return clGetPlatformIDs(numEntries, platforms, out numPlatforms);
        }
    }
}