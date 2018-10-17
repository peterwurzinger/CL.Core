using CL.Core.API;
using System;
using System.Runtime.InteropServices;

namespace CL.Core.Native
{
    internal class NativeBufferApi : IBufferApi
    {
        [DllImport(Constants.DLL, EntryPoint = "clCreateBuffer")]
        public static extern IntPtr clCreateBuffer(IntPtr context, MemoryFlags flags, uint size, IntPtr hostPtr,
            out OpenClErrorCode errorCode);

        [DllImport(Constants.DLL, EntryPoint = "clCreateSubBuffer")]
        public static extern IntPtr clCreateSubBuffer(IntPtr buffer, MemoryFlags flags,
            BufferCreateType bufferCreateType, IntPtr bufferCreateInfo,
            out OpenClErrorCode errorCode);

        [DllImport(Constants.DLL, EntryPoint = "clEnqueueReadBuffer")]
        public static extern OpenClErrorCode clEnqueueReadBuffer(IntPtr commandQueue, IntPtr buffer, bool blockingRead,
            uint offset, uint cb,
            IntPtr mem, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr @event);

        [DllImport(Constants.DLL, EntryPoint = "clEnqueueWriteBuffer")]
        public static extern OpenClErrorCode clEnqueueWriteBuffer(IntPtr commandQueue, IntPtr buffer,
            bool blockingWrite, uint offset, uint cb,
            IntPtr mem, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr evt);

        [DllImport(Constants.DLL, EntryPoint = "clReleaseMemObject")]
        public static extern OpenClErrorCode clReleaseMemObject(IntPtr memObj);

        [DllImport(Constants.DLL, EntryPoint = "clGetMemObjectInfo")]
        public static extern OpenClErrorCode clGetMemObjectInfo(IntPtr memObj, MemoryObjectInfoParameter paramName,
            uint paramValueSize,
            IntPtr paramValue, out uint paramValueSizeRet);

        IntPtr IBufferApi.clCreateSubBuffer(IntPtr buffer, MemoryFlags flags, BufferCreateType bufferCreateType, IntPtr bufferCreateInfo,
            out OpenClErrorCode errorCode)
        {
            return clCreateSubBuffer(buffer, flags, bufferCreateType, bufferCreateInfo, out errorCode);
        }

        OpenClErrorCode IBufferApi.clEnqueueReadBuffer(IntPtr commandQueue, IntPtr buffer, bool blockingRead, uint offset, uint cb,
            IntPtr mem, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr @event)
        {
            return clEnqueueReadBuffer(commandQueue, buffer, blockingRead, offset, cb, mem, numEventsInWaitList, eventWaitList, out @event);
        }

        OpenClErrorCode IBufferApi.clEnqueueWriteBuffer(IntPtr commandQueue, IntPtr buffer, bool blockingWrite, uint offset, uint cb,
            IntPtr mem, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr evt)
        {
            return clEnqueueWriteBuffer(commandQueue, buffer, blockingWrite, offset, cb, mem, numEventsInWaitList, eventWaitList, out evt);
        }

        IntPtr IBufferApi.clCreateBuffer(IntPtr context, MemoryFlags flags, uint size, IntPtr hostPtr, out OpenClErrorCode errorCode)
        {
            return clCreateBuffer(context, flags, size, hostPtr, out errorCode);
        }

        OpenClErrorCode IBufferApi.clReleaseMemObject(IntPtr memObj)
        {
            return clReleaseMemObject(memObj);
        }

        OpenClErrorCode IBufferApi.clGetMemObjectInfo(IntPtr memObj, MemoryObjectInfoParameter paramName, uint paramValueSize,
            IntPtr paramValue, out uint paramValueSizeRet)
        {
            return clGetMemObjectInfo(memObj, paramName, paramValueSize, paramValue, out paramValueSizeRet);
        }
    }
}
