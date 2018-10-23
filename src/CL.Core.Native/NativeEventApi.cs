using CL.Core.API;
using System;
using System.Runtime.InteropServices;

namespace CL.Core.Native
{
    internal class NativeEventApi : IEventApi
    {
        [DllImport(Constants.DLL, EntryPoint = "clCreateUserEvent")]
        public static extern IntPtr clCreateUserEvent(IntPtr context, out OpenClErrorCode errorCodeRet);

        [DllImport(Constants.DLL, EntryPoint = "clSetUserEventStatus")]
        public static extern OpenClErrorCode clSetUserEventStatus(IntPtr evt, int executionStatus);

        [DllImport(Constants.DLL, EntryPoint = "clWaitForEvents")]
        public static extern OpenClErrorCode clWaitForEvents(uint numEvents, IntPtr[] eventList);

        [DllImport(Constants.DLL, EntryPoint = "clSetEventCallback")]
        public static extern OpenClErrorCode clSetEventCallback(IntPtr evt, EventCommandExecutionStatus commandExecCallbackType,
            IntPtr pfnEventNotify, IntPtr userData);

        [DllImport(Constants.DLL, EntryPoint = "clRetainEvent")]
        public static extern OpenClErrorCode clRetainEvent(IntPtr evt);

        [DllImport(Constants.DLL, EntryPoint = "clReleaseEvent")]
        public static extern OpenClErrorCode clReleaseEvent(IntPtr evt);

        [DllImport(Constants.DLL, EntryPoint = "clGetEventInfo")]
        public static extern OpenClErrorCode clGetEventInfo(IntPtr evt, EventInfoParameter paramName,
            uint paramValueSize, IntPtr paramValue, out uint paramValueSizeRet);

        OpenClErrorCode IEventApi.clSetUserEventStatus(IntPtr evt, int executionStatus)
        {
            return clSetUserEventStatus(evt, executionStatus);
        }

        OpenClErrorCode IEventApi.clWaitForEvents(uint numEvents, IntPtr[] eventList)
        {
            return clWaitForEvents(numEvents, eventList);
        }

        OpenClErrorCode IEventApi.clSetEventCallback(IntPtr evt, EventCommandExecutionStatus commandExecCallbackType,
            IntPtr pfnEventNotify, IntPtr userData)
        {
            return clSetEventCallback(evt, commandExecCallbackType, pfnEventNotify, userData);
        }

        OpenClErrorCode IEventApi.clRetainEvent(IntPtr evt)
        {
            return clRetainEvent(evt);
        }

        OpenClErrorCode IEventApi.clReleaseEvent(IntPtr evt)
        {
            return clReleaseEvent(evt);
        }

        OpenClErrorCode IEventApi.clGetEventInfo(IntPtr evt, EventInfoParameter paramName, uint paramValueSize, IntPtr paramValue,
            out uint paramValueSizeRet)
        {
            return clGetEventInfo(evt, paramName, paramValueSize, paramValue, out paramValueSizeRet);
        }

        IntPtr IEventApi.clCreateUserEvent(IntPtr context, out OpenClErrorCode errorCodeRet)
        {
            return clCreateUserEvent(context, out errorCodeRet);
        }
    }
}