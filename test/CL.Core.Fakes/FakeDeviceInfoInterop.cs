using System;
using CL.Core.API;

namespace CL.Core.Fakes
{
    public class FakeDeviceInfoInterop : IDeviceInfoInterop
    {
        public OpenClErrorCode? clGetDeviceIDsResult { get; set; }
        public uint clGetDeviceIDsNumDevices { get; set; }
        public OpenClErrorCode clGetDeviceIDs(IntPtr platformId, DeviceType type, uint numEntries, IntPtr[] devices, out uint numDevices)
        {
            numDevices = clGetDeviceIDsNumDevices;
            return clGetDeviceIDsResult ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clGetDeviceInfoResult { get; set; }
        public UIntPtr clGetDeviceInfoParamValueSizeRet { get; set; }
        public OpenClErrorCode clGetDeviceInfo(IntPtr device, DeviceInfoParameter parameter, UIntPtr pValueSize, byte[] paramValue,
            out UIntPtr paramValueSizeRet)
        {
            paramValueSizeRet = clGetDeviceInfoParamValueSizeRet;
            return clGetDeviceInfoResult ?? 0;
        }

        public OpenClErrorCode? clReleaseDeviceResult { get; set; }
        public OpenClErrorCode clReleaseDevice(IntPtr device)
        {
            return clReleaseDeviceResult ?? OpenClErrorCode.Success;
        }
    }
}
