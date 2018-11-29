using System;

namespace CL.Core.API
{
    public delegate void EventCallback(IntPtr evt, EventCommandExecutionStatus eventCommandExecStatus, IntPtr userData);
}
