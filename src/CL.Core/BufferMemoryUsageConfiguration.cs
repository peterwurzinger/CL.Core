using CL.Core.API;
using CL.Core.Model;
using System;

namespace CL.Core
{
    public class BufferMemoryUsageConfiguration<T> : BufferMemoryBehaviorConfiguration<T>
        where T : unmanaged
    {
        private readonly Memory<T> _hostMemory;

        public BufferMemoryUsageConfiguration(IOpenClApi api, Context context, Action<Buffer<T>> bufferCreatedCallback, MemoryFlags flags, Memory<T> hostMemory)
            : base(api, context, bufferCreatedCallback, flags)
        {
            _hostMemory = hostMemory;
        }

        protected override Buffer<T> Build()
        {
            return new Buffer<T>(Api, Context, Flags, _hostMemory);
        }
    }
}