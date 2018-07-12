using System;

namespace CL.Core.Native
{
    [Flags]
    public enum DeviceType : uint
    {
        Default = 0b1,
        CPU = 0b10,
        GPU = 0b100,
        Accelerator = 0b1000,
        Custom = 0b1_0000,
        All = 0b1111_1111_1111_1111_1111_1111_1111_1111
    }
}