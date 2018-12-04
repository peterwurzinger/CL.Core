using System;
using System.Runtime.InteropServices;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core.MemoryObjectConfiguration
{
    public class SubBufferStubConfiguration<T> : ConfigurationStep<T, SubBuffer<T>>
        where T : unmanaged
    {
        private readonly Buffer<T> _parent;

        internal SubBufferStubConfiguration(IOpenClApi api, Context context, Action<SubBuffer<T>> memoryObjectCreatedCallback, Buffer<T> parent)
            : base(api, context, memoryObjectCreatedCallback)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public SubBufferMemoryBehaviorConfiguration<T> WithSize(int size)
        {
            return WithSize(0, size);
        }

        public SubBufferMemoryBehaviorConfiguration<T> WithSize(int offset, int size)
        {
            var elementSize = Marshal.SizeOf<T>();
            var region = new BufferRegion((uint)elementSize * (uint)offset, (uint)elementSize * (uint)size);
            return new SubBufferMemoryBehaviorConfiguration<T>(Api, Context, MemoryObjectCreatedCallback, _parent, region);
        }
    }
}
