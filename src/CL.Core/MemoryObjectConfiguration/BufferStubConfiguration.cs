using System;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core.MemoryObjectConfiguration
{
    public class BufferStubConfiguration<T> : BufferConfigurationStep<T>
        where T : unmanaged
    {
        internal BufferStubConfiguration(IOpenClApi api, Context context, Action<Buffer<T>> memoryObjectCreatedCallback)
            : base(api, context, memoryObjectCreatedCallback)
        {
        }

        public BufferMemoryBehaviorConfiguration<T> ByAllocation(ulong numElements)
        {
            return new BufferMemoryAllocationConfiguration<T>(Api, Context, MemoryObjectCreatedCallback, MemoryFlags.AllocHostPointer, numElements);
        }

        public BufferMemoryBehaviorConfiguration<T> ByCopy(Memory<T> data)
        {
            return new BufferMemoryUsageConfiguration<T>(Api, Context, MemoryObjectCreatedCallback, MemoryFlags.CopyHostPointer, data);
        }

        public BufferMemoryBehaviorConfiguration<T> ByAllocationAndCopy(Memory<T> data)
        {
            return new BufferMemoryUsageConfiguration<T>(Api, Context, MemoryObjectCreatedCallback, MemoryFlags.CopyHostPointer | MemoryFlags.AllocHostPointer, data);
        }

        public BufferMemoryBehaviorConfiguration<T> ByHostMemory(Memory<T> data)
        {
            return new BufferMemoryUsageConfiguration<T>(Api, Context, MemoryObjectCreatedCallback, MemoryFlags.UseHostPointer, data);
        }
    }
}
