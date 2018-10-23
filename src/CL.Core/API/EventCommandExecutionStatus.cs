namespace CL.Core.API
{
#pragma warning disable CA1717 // Only FlagsAttribute enums should have plural names
    public enum EventCommandExecutionStatus
#pragma warning restore CA1717 // Only FlagsAttribute enums should have plural names
    {
        Complete = 0b_0000,
        Running = 0b_0001,
        Submitted = 0b_0010,
        Queued = 0b_0011
    }
}