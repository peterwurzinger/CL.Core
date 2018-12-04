using System;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core.MemoryObjectConfiguration
{
    public abstract class BufferConfigurationStep<T> : ConfigurationStep<T, Buffer<T>>
        where T : unmanaged
    {
        protected internal BufferConfigurationStep(IOpenClApi api, Context context, Action<Buffer<T>> memoryObjectCreatedCallback)
            : base (api, context, memoryObjectCreatedCallback)
        {
        }
    }
}