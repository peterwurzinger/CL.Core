using System;
using CL.Core.API;

namespace CL.Core.Fakes
{
    public class FakeEventApi : IEventApi
    {
        public IntPtr? clCreateUserEventReturn { get; set; }
        public OpenClErrorCode? clCreateUserEventErrorCode { get; set; }
        public IntPtr clCreateUserEvent(IntPtr context, out OpenClErrorCode errorCodeRet)
        {
            errorCodeRet = clCreateUserEventErrorCode ?? OpenClErrorCode.Success;
            return clCreateUserEventReturn ?? new IntPtr(1);
        }

        public OpenClErrorCode? clSetUserEventStatusReturn { get; set; }
        public OpenClErrorCode clSetUserEventStatus(IntPtr evt, int executionStatus)
        {
            return clSetUserEventStatusReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clWaitForEventsReturn { get; set; }
        public OpenClErrorCode clWaitForEvents(uint numEvents, IntPtr[] eventList)
        {
            return clWaitForEventsReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clSetEventCallbackReturn { get; set; }
        public OpenClErrorCode clSetEventCallback(IntPtr evt, EventCommandExecutionStatus commandExecCallbackType,
            IntPtr pfnEventNotify, IntPtr userData)
        {
            return clSetEventCallbackReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clRetainEventReturn { get; set; }
        public OpenClErrorCode clRetainEvent(IntPtr evt)
        {
            return clRetainEventReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clReleaseEventReturn { get; set; }
        public OpenClErrorCode clReleaseEvent(IntPtr evt)
        {
            return clReleaseEventReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clGetEventInfoReturn { get; set; }
        public uint? clGetEventInfoParamValueSizeRet { get; set; }
        public OpenClErrorCode clGetEventInfo(IntPtr evt, EventInfoParameter paramName, uint paramValueSize, IntPtr paramValue,
            out uint paramValueSizeRet)
        {
            paramValueSizeRet = clGetEventInfoParamValueSizeRet ?? 4;
            return clGetEventInfoReturn ?? OpenClErrorCode.Success;
        }
    }
}