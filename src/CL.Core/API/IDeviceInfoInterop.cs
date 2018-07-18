﻿using System;
using System.Runtime.InteropServices;

namespace CL.Core.API
{
    public interface IDeviceInfoInterop
    {
        OpenClErrorCode clGetDeviceIDs(IntPtr platformId, DeviceType type, uint numEntries, [MarshalAs(UnmanagedType.LPArray)] IntPtr[] devices, [MarshalAs(UnmanagedType.U4)] out uint numDevices);

        OpenClErrorCode clGetDeviceInfo(IntPtr device, DeviceInfoParameter parameter, UIntPtr pValueSize, byte[] paramValue, out UIntPtr paramValueSizeRet);

        OpenClErrorCode clReleaseDevice(IntPtr device);
    }
}
