namespace CL.Core.API
{
    public enum KernelInfoParameter
    {
        FunctionName = 0b1_0001_1001_0000,
        NumberOfArguments = 0b1_0001_1001_0001,
        ReferenceCount = 0b1_0001_1001_0010,
        Context = 0b1_0001_1001_0011,
        Program = 0b1_0001_1001_0100
    }
}