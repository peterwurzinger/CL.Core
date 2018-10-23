namespace CL.Core.API
{
    public enum EventInfoParameter
    {
        CommandQueue = 0b_1_0001_1101_0000,
        CommandType = 0b_1_0001_1101_0001,
        Context = 0b_1_0001_1101_0010,
        ReferenceCount = 0b_1_0001_1101_0011,
        CommandExecutionStatus = 0b_1_0001_1101_0100
    }
}