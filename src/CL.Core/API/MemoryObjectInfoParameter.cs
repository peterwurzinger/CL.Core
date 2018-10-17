namespace CL.Core.API
{
    public enum MemoryObjectInfoParameter
    {
        Type = 0b1_0001_0000_0000,
        Flags = 0b1_0001_0000_0001,
        Size = 0b1_0001_0000_0010,
        HostPointer = 0b1_0001_0000_0011,
        MapCount = 0b1_0001_0000_0100,
        ReferenceCount = 0b1_0001_0000_0101,
        Context = 0b1_0001_0000_0111,

        //Offset = 0b1_0001_0000_1000,
        //AssociatedMemoryObject = 0b1_0001_0000_1001,
        //TODO: Offset and AssociatedMemoryObject  supported in OpenCL 1.1? Reference-Card says so, API-Doc (https://www.khronos.org/registry/OpenCL/sdk/1.1/docs/man/xhtml/enums.html) does not
    }
}