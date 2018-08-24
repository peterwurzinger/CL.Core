using CL.Core.API;
using System;
using System.Runtime.InteropServices;

namespace CL.Core.Native
{
    internal class NativeCommandQueueApi : ICommandQueueApi
    {

        [DllImport(Constants.DLL, EntryPoint = "clRetainCommandQueue")]
        public static extern OpenClErrorCode clRetainCommandQueue(IntPtr commandQueue);


        [DllImport(Constants.DLL, EntryPoint = "clCreateCommandQueue")]
        public static extern IntPtr clCreateCommandQueue(IntPtr context, IntPtr device, CommandQueueProperties properties,
            out OpenClErrorCode error);


        [DllImport(Constants.DLL, EntryPoint = "clReleaseCommandQueue")]
        public static extern OpenClErrorCode clReleaseCommandQueue(IntPtr commandQueue);

        [DllImport(Constants.DLL, EntryPoint = "clGetCommandQueueInfo")]
        public static extern OpenClErrorCode clGetCommandQueueInfo(IntPtr commandQueue,
            CommandQueueInfoParameter paramName, uint paramValueSize,
            byte[] paramValue, out uint paramValueSizeReturn);

        IntPtr ICommandQueueApi.clCreateCommandQueue(IntPtr context, IntPtr device, CommandQueueProperties properties, out OpenClErrorCode error)
        {
            return clCreateCommandQueue(context, device, properties, out error);
        }

        OpenClErrorCode ICommandQueueApi.clRetainCommandQueue(IntPtr commandQueue)
        {
            return clRetainCommandQueue(commandQueue);
        }

        OpenClErrorCode ICommandQueueApi.clReleaseCommandQueue(IntPtr commandQueue)
        {
            return clReleaseCommandQueue(commandQueue);
        }

        OpenClErrorCode ICommandQueueApi.clGetCommandQueueInfo(IntPtr commandQueue, CommandQueueInfoParameter paramName, uint paramValueSize,
            byte[] paramValue, out uint paramValueSizeReturn)
        {
            return clGetCommandQueueInfo(commandQueue, paramName, paramValueSize, paramValue,
                out paramValueSizeReturn);
        }
    }
}
