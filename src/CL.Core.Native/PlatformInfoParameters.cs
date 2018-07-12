namespace CL.Core.Native
{
    public enum PlatformInfoParameters : uint
    {
        Profile = 0b1001_0000_0000,
        Version = 0b1001_0000_0001,
        Name = 0b1001_0000_0010,
        Vendor = 0b1001_0000_0011,
        Extensions = 0b1001_0000_0100
    }
}
