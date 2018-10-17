using CL.Core.API;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace CL.Core.Model
{
    public class Buffer<T> : MemoryObject
        where T : unmanaged
    {
        private bool _disposed;
        private readonly IList<SubBuffer<T>> _attachedSubBuffers;

        internal Buffer(IOpenClApi api, Context context, IntPtr id, MemoryHandle? hostMemory = null)
            : base(api, context, id, hostMemory)
        {
            _attachedSubBuffers = new List<SubBuffer<T>>();
        }

        internal Buffer(IOpenClApi api, Context context, IntPtr id)
        : this(api, context, id, null)
        {
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
                }
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
