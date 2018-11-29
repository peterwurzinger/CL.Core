using System;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core.MemoryObjectConfiguration
{
    public abstract class ConfigurationStep<T, TMemoryObject>
        where T : unmanaged
        where TMemoryObject : BufferBase<T>
    {
        protected IOpenClApi Api { get; }
        protected Context Context { get; }
        protected Action<TMemoryObject> MemoryObjectCreatedCallback { get; }

        protected internal ConfigurationStep(IOpenClApi api, Context context, Action<TMemoryObject> memoryObjectCreatedCallback)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            MemoryObjectCreatedCallback = memoryObjectCreatedCallback;
        }
    }
}