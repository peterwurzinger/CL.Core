using CL.Core.API;
using CL.Core.Model;
using System;

namespace CL.Core
{
    public abstract class BufferConfigurationStep<T>
        where T : unmanaged
    {
        protected readonly IOpenClApi Api;
        protected readonly Context Context;
        protected readonly Action<Buffer<T>> BufferCreatedCallback;

        protected internal BufferConfigurationStep(IOpenClApi api, Context context, Action<Buffer<T>> bufferCreatedCallback)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            BufferCreatedCallback = bufferCreatedCallback;
        }
    }
}