using System;

namespace CL.Core.API
{
    public interface IEventApi
    {
        IntPtr clCreateUserEvent(IntPtr context, out OpenClErrorCode errorCodeRet);
        OpenClErrorCode clSetUserEventStatus(IntPtr evt, int executionStatus);
        OpenClErrorCode clWaitForEvents(uint numEvents, IntPtr[] eventList);
        OpenClErrorCode clSetEventCallback(IntPtr evt, EventCommandExecutionStatus commandExecCallbackType, IntPtr pfnEventNotify, IntPtr userData);
        OpenClErrorCode clRetainEvent(IntPtr evt);
        OpenClErrorCode clReleaseEvent(IntPtr evt);
        OpenClErrorCode clGetEventInfo(IntPtr evt, EventInfoParameter paramName, uint paramValueSize, IntPtr paramValue, out uint paramValueSizeRet);
    }
}