using CL.Core.API;
using CL.Core.Model;
using System;

namespace CL.Core
{
    public class BufferStubConfiguration<T> : BufferConfigurationStep<T>
        where T : unmanaged
    {
        public BufferStubConfiguration(IOpenClApi api, Context context, Action<Buffer<T>> bufferCreatedCallback)
            : base(api, context, bufferCreatedCallback)
        {
        }

        public BufferMemoryBehaviorConfiguration<T> ByAllocation(uint numElements)
        {
            return new BufferMemoryAllocationConfiguration<T>(Api, Context, BufferCreatedCallback, MemoryFlags.AllocHostPointer, numElements);
        }

        public BufferMemoryBehaviorConfiguration<T> ByCopy(Memory<T> data)
        {
            return new BufferMemoryUsageConfiguration<T>(Api, Context, BufferCreatedCallback, MemoryFlags.CopyHostPointer, data);
        }

        public BufferMemoryBehaviorConfiguration<T> ByAllocationAndCopy(Memory<T> data)
        {
            return new BufferMemoryUsageConfiguration<T>(Api, Context, BufferCreatedCallback, MemoryFlags.CopyHostPointer | MemoryFlags.AllocHostPointer, data);
        }

        public BufferMemoryBehaviorConfiguration<T> ByHostMemory(Memory<T> data)
        {
            return new BufferMemoryUsageConfiguration<T>(Api, Context, BufferCreatedCallback, MemoryFlags.UseHostPointer, data);
        }
    }
}
