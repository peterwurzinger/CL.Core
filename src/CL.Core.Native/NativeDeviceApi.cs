using System;
using System.Runtime.InteropServices;
using CL.Core.API;

namespace CL.Core.Native
{
    internal class NativeDeviceApi : IDeviceApi
    {
        [DllImport(Constants.DLL, EntryPoint = "clGetDeviceIDs")]
        public static extern OpenClErrorCode clGetDeviceIDs(IntPtr platformId, DeviceType type, uint numEntries, [MarshalAs(UnmanagedType.LPArray)] IntPtr[] devices, [MarshalAs(UnmanagedType.U4)] out uint numDevices);

        [DllImport(Constants.DLL, EntryPoint = "clGetDeviceInfo")]
        public static extern OpenClErrorCode clGetDeviceInfo(IntPtr device, DeviceInfoParameter parameter, uint pValueSize, byte[] paramValue, out uint paramValueSizeRet);

        [DllImport(Constants.DLL, EntryPoint = "clReleaseDevice")]
        public static extern OpenClErrorCode clReleaseDevice(IntPtr device);

        OpenClErrorCode IDeviceApi.clGetDeviceInfo(IntPtr device, DeviceInfoParameter parameter, uint pValueSize, byte[] paramValue,
            out uint paramValueSizeRet)
        {
            return clGetDeviceInfo(device, parameter, pValueSize, paramValue, out paramValueSizeRet);
        }

        OpenClErrorCode IDeviceApi.clReleaseDevice(IntPtr device)
        {
            return clReleaseDevice(device);
        }

        OpenClErrorCode IDeviceApi.clGetDeviceIDs(IntPtr platformId, DeviceType type, uint numEntries, IntPtr[] devices, out uint numDevices)
        {
            return clGetDeviceIDs(platformId, type, numEntries, devices, out numDevices);
        }
    }
}
