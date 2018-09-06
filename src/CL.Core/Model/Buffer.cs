using CL.Core.API;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CL.Core.Model
{
    public class Buffer<T> : BufferBase
        where T : unmanaged
    {
        private bool _disposed;
        private readonly IList<SubBuffer<T>> _attachedSubBuffers;
        private readonly MemoryHandle? _handle;

        internal unsafe Buffer(IOpenClApi api, Context context, MemoryFlags flags, Memory<T> hostMemory)
           : base(api, context)
        {
            var typeSize = Marshal.SizeOf<T>();

            _handle = hostMemory.Pin();
            var id = Api.BufferApi.clCreateBuffer(context.Id, flags, (uint)typeSize * (uint)hostMemory.Length, new IntPtr(_handle.Value.Pointer), out var error);
            error.ThrowOnError();

            Id = id;

            _attachedSubBuffers = new List<SubBuffer<T>>();
        }

        internal Buffer(IOpenClApi api, Context context, MemoryFlags flags, uint numElements)
            : base(api, context)
        {
            var typeSize = Marshal.SizeOf<T>();

            var id = Api.BufferApi.clCreateBuffer(context.Id, flags, (uint)typeSize * numElements, IntPtr.Zero, out var error);
            error.ThrowOnError();

            //TODO: Query for host-pointer and store in memory-handle?

            Id = id;
            _attachedSubBuffers = new List<SubBuffer<T>>();
        }

        public SubBuffer<T> CreateSubBuffer()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            var subBuffer = new SubBuffer<T>(Api, this);
            _attachedSubBuffers.Add(subBuffer);

            return subBuffer;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var subBuffer in _attachedSubBuffers)
                        subBuffer.Dispose();

                    _handle?.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
