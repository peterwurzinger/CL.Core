using System;
using System.Runtime.InteropServices;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core.MemoryObjectConfiguration
{
    public class SubBufferMemoryBehaviorConfiguration<T> : ConfigurationStep<T, SubBuffer<T>>
        where T : unmanaged
    {
        private readonly Buffer<T> _parent;
        private readonly BufferRegion _region;

        internal SubBufferMemoryBehaviorConfiguration(IOpenClApi api, Context context, Action<SubBuffer<T>> memoryObjectCreatedCallback, Buffer<T> parent, BufferRegion region)
            : base(api, context, memoryObjectCreatedCallback)
        {
            _parent = parent;
            _region = region;
        }
        
        public SubBuffer<T> AsReadOnly()
        {
            return BuildInternal(MemoryFlags.ReadOnly);
        }

        public SubBuffer<T> AsWriteOnly()
        {
            return BuildInternal(MemoryFlags.WriteOnly);
        }

        public SubBuffer<T> AsReadWrite()
        {
            return BuildInternal(MemoryFlags.ReadWrite);
        }

        private SubBuffer<T> BuildInternal(MemoryFlags flags)
        {
            var handle = GCHandle.Alloc(_region, GCHandleType.Pinned);

            var id = Api.BufferApi.clCreateSubBuffer(_parent.Id, flags, BufferCreateType.Region, handle.AddrOfPinnedObject(), out var error);
            handle.Free();
            error.ThrowOnError();

            var subBuffer = new SubBuffer<T>(Api, _parent, id);
            MemoryObjectCreatedCallback(subBuffer);
            return subBuffer;
        }
    }
}