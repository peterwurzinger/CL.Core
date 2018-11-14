using CL.Core.API;
using System;
using System.Collections.Generic;
using CL.Core.Fakes.OpenCL;

namespace CL.Core.Fakes
{
    public class FakeCommandQueueApi : ICommandQueueApi
    {
        public IDictionary<IntPtr, FakeCommandQueue> FakeCommandQueues { get; }

        public FakeCommandQueueApi()
        {
            FakeCommandQueues = new Dictionary<IntPtr, FakeCommandQueue>();
        }

        public IntPtr? clCreateCommandQueueResult { get; set; }
        public OpenClErrorCode? clCreateCommandQueueErrorCode { get; set; }
        public IntPtr clCreateCommandQueue(IntPtr context, IntPtr device, CommandQueueProperties properties,
            out OpenClErrorCode errorCode)
        {
            errorCode = clCreateCommandQueueErrorCode ?? OpenClErrorCode.Success;
            var id = clCreateCommandQueueResult ?? new IntPtr(1);

            if (errorCode == OpenClErrorCode.Success)
                FakeCommandQueues[id] = new FakeCommandQueue(context, device, properties);

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
        public OpenClErrorCode clGetCommandQueueInfo(IntPtr commandQueue, CommandQueueInfoParameter paramName, uint paramValueSize,
            IntPtr paramValue, out uint paramValueSizeReturn)
        {
            var errorCode = clGetCommandQueueInfoResult ?? OpenClErrorCode.Success;
            return FakeCommandQueues[commandQueue].GetInfo(paramName, paramValueSize, paramValue,
                out paramValueSizeReturn, errorCode);
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
