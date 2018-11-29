using System;
using System.Runtime.InteropServices;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core.MemoryObjectConfiguration
{
    public class BufferMemoryAllocationConfiguration<T> : BufferMemoryBehaviorConfiguration<T>
        where T : unmanaged
    {
        private readonly uint _numElements;

        internal BufferMemoryAllocationConfiguration(IOpenClApi api, Context context, Action<Buffer<T>> memoryObjectCreatedCallback, MemoryFlags flags, uint numElements)
            : base(api, context, memoryObjectCreatedCallback, flags)
        {
            _numElements = numElements;
        }

        protected override Buffer<T> Build()
        {
            var typeSize = Marshal.SizeOf<T>();
            var id = Api.BufferApi.clCreateBuffer(Context.Id, Flags, _numElements * (uint)typeSize, IntPtr.Zero, out var error);
            error.ThrowOnError();
            return new Buffer<T>(Api, Context, id);
        }
    }
}