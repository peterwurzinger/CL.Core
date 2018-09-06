using System;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core
{
    public class BufferMemoryAllocationConfiguration<T> : BufferMemoryBehaviorConfiguration<T>
        where T : unmanaged
    {
        private readonly uint _numElements;

        public BufferMemoryAllocationConfiguration(IOpenClApi api, Context context, Action<Buffer<T>> bufferCreatedCallback, MemoryFlags flags, uint numElements)
            : base(api, context, bufferCreatedCallback, flags)
        {
            _numElements = numElements;
        }

        protected override Buffer<T> Build()
        {
            return new Buffer<T>(Api, Context, Flags, _numElements);
        }
    }
}