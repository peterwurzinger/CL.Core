using System;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core.MemoryObjectConfiguration
{
    public abstract class BufferMemoryBehaviorConfiguration<T> : BufferConfigurationStep<T>
        where T : unmanaged
    {
        protected MemoryFlags Flags { get; set; }

        internal BufferMemoryBehaviorConfiguration(IOpenClApi api, Context context, Action<Buffer<T>> memoryObjectCreatedCallback, MemoryFlags flags)
            : base(api, context, memoryObjectCreatedCallback)
        {
            Flags = flags;
        }

        public Buffer<T> AsReadOnly()
        {
            Flags |= MemoryFlags.ReadOnly;
            return BuildInternal();
        }

        public Buffer<T> AsWriteOnly()
        {
            Flags |= MemoryFlags.WriteOnly;
            return BuildInternal();
        }

        public Buffer<T> AsReadWrite()
        {
            Flags |= MemoryFlags.ReadWrite;
            return BuildInternal();
        }

        private Buffer<T> BuildInternal()
        {
            var buffer = Build();
            MemoryObjectCreatedCallback(buffer);
            return buffer;
        }

        protected abstract Buffer<T> Build();


    }
}