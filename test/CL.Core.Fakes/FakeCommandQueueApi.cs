using CL.Core.API;
using System;
using System.Collections.Generic;

namespace CL.Core.Fakes
{
    public class FakeCommandQueueApi : ICommandQueueApi
    {
        public IDictionary<IntPtr, FakeOpenClCommandQueue> FakeCommandQueues { get; }

        public FakeCommandQueueApi()
        {
            FakeCommandQueues = new Dictionary<IntPtr, FakeOpenClCommandQueue>();
            //Return at least 4 bytes to make result interpretable as numeric value
            clGetCommandQueueInfoParamValueSizeReturn = 4;
        }

        public IntPtr? clCreateCommandQueueResult { get; set; }
        public OpenClErrorCode? clCreateCommandQueueErrorCode { get; set; }
        public IntPtr clCreateCommandQueue(IntPtr context, IntPtr device, CommandQueueProperties properties,
            out OpenClErrorCode errorCode)
        {
            errorCode = clCreateCommandQueueErrorCode ?? OpenClErrorCode.Success;
            var id = clCreateCommandQueueResult ?? new IntPtr(1);

            if (errorCode == OpenClErrorCode.Success)
                FakeCommandQueues[id] = new FakeOpenClCommandQueue(context, device, properties);

            return id;
        }

        public OpenClErrorCode? clRetainCommandQueueResult { get; set; }
        public OpenClErrorCode clRetainCommandQueue(IntPtr commandQueue)
        {
            var errorCode = clRetainCommandQueueResult ?? OpenClErrorCode.Success;

            if (errorCode == OpenClErrorCode.Success)
                FakeCommandQueues[commandQueue].Retained = true;

            return errorCode;
        }

        public OpenClErrorCode? clReleaseCommandQueueResult { get; set; }
        public OpenClErrorCode clReleaseCommandQueue(IntPtr commandQueue)
        {
            var errorCode = clReleaseCommandQueueResult ?? OpenClErrorCode.Success;

            if (errorCode == OpenClErrorCode.Success)
                FakeCommandQueues[commandQueue].Released = true;

            return errorCode;
        }

        public OpenClErrorCode? clGetCommandQueueInfoResult { get; set; }
        public uint clGetCommandQueueInfoParamValueSizeReturn { get; set; }
        public OpenClErrorCode clGetCommandQueueInfo(IntPtr commandQueue, CommandQueueInfoParameter paramName, uint paramValueSize,
            byte[] paramValue, out uint paramValueSizeReturn)
        {
            paramValueSizeReturn = clGetCommandQueueInfoParamValueSizeReturn;
            return clGetCommandQueueInfoResult ?? 0;
        }

        public OpenClErrorCode? clFlushResult { get; set; }
        public OpenClErrorCode clFlush(IntPtr id)
        {
            var errorCode = clFlushResult ?? OpenClErrorCode.Success;

            if (errorCode == OpenClErrorCode.Success)
                FakeCommandQueues[id].Flushed = true;

            return errorCode;
        }

        public OpenClErrorCode? clFinishResult { get; set; }
        public OpenClErrorCode clFinish(IntPtr commandQueue)
        {
            var errorCode = clFinishResult ?? OpenClErrorCode.Success;

            if (errorCode == OpenClErrorCode.Success)
                FakeCommandQueues[commandQueue].Finished = true;

            return errorCode;
        }
    }
}
