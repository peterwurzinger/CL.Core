using CL.Core.API;
using CL.Core.Model;
using System;

namespace CL.Core
{
    public abstract class BufferMemoryBehaviorConfiguration<T> : BufferConfigurationStep<T>
        where T : unmanaged
    {
        protected MemoryFlags Flags { get; set; }

        internal BufferMemoryBehaviorConfiguration(IOpenClApi api, Context context, Action<Buffer<T>> bufferCreatedCallback, MemoryFlags flags)
            : base(api, context, bufferCreatedCallback)
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
            BufferCreatedCallback(buffer);
            return buffer;
        }

        protected abstract Buffer<T> Build();


    }
}