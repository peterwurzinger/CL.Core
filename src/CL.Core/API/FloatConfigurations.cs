using System;

namespace CL.Core.API
{
    [Flags]
    public enum FloatConfigurations
    {
        Denorm = 0b_0000_0001,
        SupportsInfinityAndNaN = 0b_0000_0010,
        RoundToNearest = 0b_0000_0100,
        RoundToZero = 0b_0000_1000,
        RoundToInfinity = 0b_0001_0000,
        FusedMultiplyAdd = 0b_0010_0000,
        SoftwareFloat = 0b_0100_0000
    }
}