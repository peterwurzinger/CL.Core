using CL.Core.API;
using CL.Core.Model;
using System;
using CL.Core.MemoryObjectConfiguration;

namespace CL.Core.Fakes
{
    public class FakeBufferConfigurationStep<T> : BufferConfigurationStep<T>
        where T : unmanaged
    {
        public FakeBufferConfigurationStep(IOpenClApi api, Context context, Action<Buffer<T>> bufferCreatedCallback)
            : base(api, context, bufferCreatedCallback)
        {
        }

        public IOpenClApi GetApi => Api;
        public Context GetContext => Context;
        public Action<Buffer<T>> GetBufferCreatedCallback => MemoryObjectCreatedCallback;
    }
}
