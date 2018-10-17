using CL.Core.API;
using System;

namespace CL.Core.Fakes
{
    public class FakeBufferApi : IBufferApi
    {
        public IntPtr? clCreateBufferResult { get; set; }
        public OpenClErrorCode? clCreateBufferErrorCode { get; set; }
        public IntPtr clCreateBuffer(IntPtr context, MemoryFlags flags, uint size, IntPtr hostPtr, out OpenClErrorCode errorCode)
        {
            errorCode = clCreateBufferErrorCode ?? OpenClErrorCode.Success;
            return clCreateBufferResult ?? IntPtr.Zero;
        }

        public IntPtr? clCreateSubBufferResult { get; set; }
        public OpenClErrorCode? clCreateSubBufferErrorCode { get; set; }
        public IntPtr clCreateSubBuffer(IntPtr buffer, MemoryFlags flags, BufferCreateType bufferCreateType, IntPtr bufferCreateInfo,
            out OpenClErrorCode errorCode)
        {
            errorCode = clCreateSubBufferErrorCode ?? OpenClErrorCode.Success;
            return clCreateSubBufferResult ?? IntPtr.Zero;
        }

        public OpenClErrorCode? clEnqueueReadBufferResult { get; set; }
        public IntPtr? clEnqueueReadBufferEvent { get; set; }
        public OpenClErrorCode clEnqueueReadBuffer(IntPtr commandQueue, IntPtr buffer, bool blockingRead, uint offset, uint cb,
            IntPtr mem, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr @event)
        {
            @event = clEnqueueReadBufferEvent ?? IntPtr.Zero;
            return clEnqueueReadBufferResult ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clEnqueueWriteBufferResult { get; set; }
        public IntPtr? clEnqueueWriteBufferEvent { get; set; }
        public OpenClErrorCode clEnqueueWriteBuffer(IntPtr commandQueue, IntPtr buffer, bool blockingWrite, uint offset, uint cb,
            IntPtr mem, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr evt)
        {
            evt = clEnqueueWriteBufferEvent ?? IntPtr.Zero;
            return clEnqueueWriteBufferResult ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clReleaseMemObjectResult { get; set; }
        public OpenClErrorCode clReleaseMemObject(IntPtr memObj)
        {
            return clReleaseMemObjectResult ?? OpenClErrorCode.Success;
        }

        public uint? clGetMemObjectInfoParamValueSizeRet { get; set; }
        public OpenClErrorCode? clGetMemObjectInfoErrorCode { get; set; }
        public OpenClErrorCode clGetMemObjectInfo(IntPtr memObj, MemoryObjectInfoParameter paramName, uint paramValueSize,
            IntPtr paramValue, out uint paramValueSizeRet)
        {
            paramValueSizeRet = clGetMemObjectInfoParamValueSizeRet ?? 4;
            return clGetMemObjectInfoErrorCode ?? OpenClErrorCode.Success;
        }
    }
}