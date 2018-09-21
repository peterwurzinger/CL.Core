using System;

namespace CL.Core.API
{
    [Flags]
#pragma warning disable CA1714 // Flags enums should have plural names
#pragma warning disable CA1028 // Enum Storage should be Int32
    public enum DeviceType : uint
#pragma warning restore CA1028 // Enum Storage should be Int32
#pragma warning restore CA1714 // Flags enums should have plural names
    {
        Default = 0b1,
        CPU = 0b10,
        GPU = 0b100,
        Accelerator = 0b1000,
        Custom = 0b1_0000,
        All = 0b1111_1111_1111_1111_1111_1111_1111_1111
    }
}