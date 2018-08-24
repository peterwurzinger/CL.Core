using System;
using CL.Core.API;

namespace CL.Core.Fakes
{
    public class FakeCommandQueueApi : ICommandQueueApi
    {
        public FakeCommandQueueApi()
        {
            //Return at least 4 bytes to make result interpretable as numeric value
            clGetCommandQueueInfoParamValueSizeReturn = 4;
        }

        public IntPtr? clCreateCommandQueueResult { get; set; }
        public OpenClErrorCode? clCreateCommandQueueErrorCode { get; set; }
        public IntPtr clCreateCommandQueue(IntPtr context, IntPtr device, CommandQueueProperties properties,
            out OpenClErrorCode error)
        {
            error = clCreateCommandQueueErrorCode ?? OpenClErrorCode.Success;
            return clCreateCommandQueueResult ?? IntPtr.Zero;
        }

        public OpenClErrorCode? clRetainCommandQueueResult { get; set; }
        public OpenClErrorCode clRetainCommandQueue(IntPtr commandQueue)
        {
            return clRetainCommandQueueResult ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clReleaseCommandQueueResult { get; set; }
        public OpenClErrorCode clReleaseCommandQueue(IntPtr commandQueue)
        {
            return clReleaseCommandQueueResult ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clGetCommandQueueInfoResult { get; set; }
        public uint clGetCommandQueueInfoParamValueSizeReturn { get; set; }
        public OpenClErrorCode clGetCommandQueueInfo(IntPtr commandQueue, CommandQueueInfoParameter paramName, uint paramValueSize,
            byte[] paramValue, out uint paramValueSizeReturn)
        {
            paramValueSizeReturn = clGetCommandQueueInfoParamValueSizeReturn;
            return clGetCommandQueueInfoResult ?? 0;
        }
    }
}
