using System;
using System.Runtime.InteropServices;

namespace CL.Core.API
{
    public interface IDeviceApi
    {
        OpenClErrorCode clGetDeviceIDs(IntPtr platformId, DeviceType type, uint numEntries, [MarshalAs(UnmanagedType.LPArray)] IntPtr[] devices, [MarshalAs(UnmanagedType.U4)] out uint numDevices);

        OpenClErrorCode clGetDeviceInfo(IntPtr device, DeviceInfoParameter parameter, uint pValueSize, byte[] paramValue, out uint paramValueSizeRet);

        OpenClErrorCode clReleaseDevice(IntPtr device);
    }
}
