﻿namespace CL.Core.API
{
    public interface IOpenClApi
    {
        IPlatformApi PlatformApi { get; }
        IDeviceApi DeviceApi { get; }
        ICommandQueueApi CommandQueueApi { get; }
        IContextApi ContextApi { get; }
        IBufferApi BufferApi { get; }
    }
}
