namespace CL.Core.API
{
    public enum ProgramInfoParameter
    {
        ReferenceCount = 0b1_0001_0110_0000,
        Context = 0b1_0001_0110_0001,
        NumDevices = 0b1_0001_0110_0010,
        Devices = 0b1_0001_0110_0011,
        Source = 0b1_0001_0110_0100,
        BinarySizes = 0b1_0001_0110_0101,
        Binaries = 0b1_0001_0110_0110
    }
}