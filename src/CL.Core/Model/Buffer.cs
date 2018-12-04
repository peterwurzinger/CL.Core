using CL.Core.API;
using System;
using System.Buffers;
using System.Collections.Generic;
using CL.Core.MemoryObjectConfiguration;

namespace CL.Core.Model
{
    public class Buffer<T> : BufferBase<T>
        where T : unmanaged
    {
        private readonly List<SubBuffer<T>> _attachedSubBuffers;
        private bool _disposed;

        public IReadOnlyCollection<SubBuffer<T>> SubBuffers => _attachedSubBuffers;

        internal Buffer(IOpenClApi api, Context context, IntPtr id, MemoryHandle? hostMemory = null)
            : base(api, context, id, hostMemory)
        {
            _attachedSubBuffers = new List<SubBuffer<T>>();
        }

        internal Buffer(IOpenClApi api, Context context, IntPtr id)
        : this(api, context, id, null)
        {
        }

        public SubBufferStubConfiguration<T> CreateSubBuffer()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            return new SubBufferStubConfiguration<T>(Api, Context, b => _attachedSubBuffers.Add(b), this);
        }


        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var subBuffer in _attachedSubBuffers)
                        subBuffer.Dispose();
                }
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
