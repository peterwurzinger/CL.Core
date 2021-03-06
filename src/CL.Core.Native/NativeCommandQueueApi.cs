﻿using CL.Core.API;
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
            out OpenClErrorCode errorCode);


        [DllImport(Constants.DLL, EntryPoint = "clReleaseCommandQueue")]
        public static extern OpenClErrorCode clReleaseCommandQueue(IntPtr commandQueue);

        [DllImport(Constants.DLL, EntryPoint = "clGetCommandQueueInfo")]
        public static extern OpenClErrorCode clGetCommandQueueInfo(IntPtr commandQueue,
            CommandQueueInfoParameter paramName, uint paramValueSize,
            IntPtr paramValue, out uint paramValueSizeReturn);

        [DllImport(Constants.DLL, EntryPoint = "clFlush")]
        public static extern OpenClErrorCode clFlush(IntPtr commandQueue);


        [DllImport(Constants.DLL, EntryPoint = "clFinish")]
        public static extern OpenClErrorCode clFinish(IntPtr commandQueue);

        IntPtr ICommandQueueApi.clCreateCommandQueue(IntPtr context, IntPtr device, CommandQueueProperties properties, out OpenClErrorCode errorCode)
        {
            return clCreateCommandQueue(context, device, properties, out errorCode);
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
            IntPtr paramValue, out uint paramValueSizeReturn)
        {
            return clGetCommandQueueInfo(commandQueue, paramName, paramValueSize, paramValue,
                out paramValueSizeReturn);
        }

        OpenClErrorCode ICommandQueueApi.clFlush(IntPtr commandQueue)
        {
            return clFlush(commandQueue);
        }

        OpenClErrorCode ICommandQueueApi.clFinish(IntPtr commandQueue)
        {
            return clFinish(commandQueue);
        }
    }
}
