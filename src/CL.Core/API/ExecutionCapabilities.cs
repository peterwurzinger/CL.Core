using System;

namespace CL.Core.API
{
    [Flags]
    public enum ExecutionCapabilities
    {
        Kernel = 0b_0001,
        NativeKernel = 0b_0010
    }
}