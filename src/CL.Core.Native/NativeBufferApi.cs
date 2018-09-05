using CL.Core.API;
using System;
using System.Runtime.InteropServices;

namespace CL.Core.Native
{
    public class NativeBufferApi : IBufferApi
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
            IntPtr ptr, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr @event);

        [DllImport(Constants.DLL, EntryPoint = "clEnqueueWriteBuffer")]
        public static extern OpenClErrorCode clEnqueueWriteBuffer(IntPtr commandQueue, IntPtr buffer,
            bool blockingWrite, uint offset, uint cb,
            IntPtr ptr, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr @event);


        IntPtr IBufferApi.clCreateSubBuffer(IntPtr buffer, MemoryFlags flags, BufferCreateType bufferCreateType, IntPtr bufferCreateInfo,
            out OpenClErrorCode errorCode)
        {
            return clCreateSubBuffer(buffer, flags, bufferCreateType, bufferCreateInfo, out errorCode);
        }

        OpenClErrorCode IBufferApi.clEnqueueReadBuffer(IntPtr commandQueue, IntPtr buffer, bool blockingRead, uint offset, uint cb,
            IntPtr ptr, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr @event)
        {
            return clEnqueueReadBuffer(commandQueue, buffer, blockingRead, offset, cb, ptr, numEventsInWaitList, eventWaitList, out @event);
        }

        OpenClErrorCode IBufferApi.clEnqueueWriteBuffer(IntPtr commandQueue, IntPtr buffer, bool blockingWrite, uint offset, uint cb,
            IntPtr ptr, uint numEventsInWaitList, IntPtr[] eventWaitList, out IntPtr @event)
        {
            return clEnqueueWriteBuffer(commandQueue, buffer, blockingWrite, offset, cb, ptr, numEventsInWaitList, eventWaitList, out @event);
        }

        IntPtr IBufferApi.clCreateBuffer(IntPtr context, MemoryFlags flags, uint size, IntPtr hostPtr, out OpenClErrorCode errorCode)
        {
            return clCreateBuffer(context, flags, size, hostPtr, out errorCode);
        }
    }
}
