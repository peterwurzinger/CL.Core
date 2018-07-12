using System;
using System.Runtime.InteropServices;

namespace CL.Core.Native
{
    public static class DeviceCalls
    {
        [DllImport(Constants.DLL, EntryPoint = "clGetDeviceIDs")]
        public static extern int clGetDeviceIDs(IntPtr platformId, DeviceType type, uint numEntries, [MarshalAs(UnmanagedType.LPArray)] IntPtr[] devices, [MarshalAs(UnmanagedType.U4)] out uint numDevices);

        [DllImport(Constants.DLL, EntryPoint = "clGetDeviceInfo")]
        public static extern int clGetDeviceInfo(IntPtr device, DeviceInfoParameter parameter, UIntPtr pValueSize, byte[] paramValue, out UIntPtr paramValueSizeRet);

        [DllImport(Constants.DLL, EntryPoint = "clReleaseDevice")]
        public static extern int clReleaseDevice(IntPtr device);
    }
}
