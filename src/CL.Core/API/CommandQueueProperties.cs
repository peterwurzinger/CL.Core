﻿using System;

namespace CL.Core.API
{
    [Flags]
    public enum CommandQueueProperties
    {
        ProfilingEnable = 0b0001,
        OutOfOrderExecModeEnable = 0b0010
    }
}