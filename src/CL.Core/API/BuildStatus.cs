namespace CL.Core.API
{
#pragma warning disable CA1717 // Only FlagsAttribute enums should have plural names
    public enum BuildStatus
#pragma warning restore CA1717 // Only FlagsAttribute enums should have plural names
    {
        Success = 0,
        None = -1,
        Error = -2,
        InProgress = -3
    }
}