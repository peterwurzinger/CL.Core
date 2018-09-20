using CL.Core.API;
using System;

namespace CL.Core.Fakes
{
    public class FakeDeviceApi : IDeviceApi
    {

        public FakeDeviceApi()
        {
            //Return at least 4 bytes to make result interpretable as numeric value
            clGetDeviceInfoParamValueSizeRet = 4;
        }

        public OpenClErrorCode? clGetDeviceIDsResult { get; set; }
        public uint clGetDeviceIDsNumDevices { get; set; }
        public OpenClErrorCode clGetDeviceIDs(IntPtr platformId, DeviceType type, uint numEntries, IntPtr[] devices, out uint numDevices)
        {
            numDevices = clGetDeviceIDsNumDevices;
            return clGetDeviceIDsResult ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clGetDeviceInfoResult { get; set; }
        public uint clGetDeviceInfoParamValueSizeRet { get; set; }
        public OpenClErrorCode clGetDeviceInfo(IntPtr device, DeviceInfoParameter parameter, uint pValueSize, IntPtr paramValue,
            out uint paramValueSizeRet)
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
