using CL.Core.API;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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

            throw new NotImplementedException();
        }

        //TODO: Re-Introduce BufferBase <- (Buffer, SubBuffer) to implement Read/Write-operations there

        public ReadOnlySpan<T> Read(CommandQueue commandQueue)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (commandQueue == null)
                throw new ArgumentNullException(nameof(commandQueue));

            var memory = new byte[Size];
            var hdl = GCHandle.Alloc(memory, GCHandleType.Pinned);
            var error = Api.BufferApi.clEnqueueReadBuffer(commandQueue.Id, Id, true, 0, (uint)Size, hdl.AddrOfPinnedObject(), 0,
                null, out _);

            hdl.Free();
            error.ThrowOnError();

            return MemoryMarshal.Cast<byte, T>(memory);
        }

        public unsafe void Write(CommandQueue commandQueue, ReadOnlyMemory<T> data)

        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (commandQueue == null)
                throw new ArgumentNullException(nameof(commandQueue));

            var handle = data.Pin();
            var error = Api.BufferApi.clEnqueueWriteBuffer(commandQueue.Id, Id, true, 0, (uint)Size, new IntPtr(handle.Pointer), 0, null, out _);
            handle.Dispose();
            error.ThrowOnError();
        }

        public Task<ReadOnlyMemory<T>> ReadAsync(CommandQueue commandQueue)
        {
            throw new NotImplementedException();
            //clWaitForEvent returned by clEnqueueRead
        }

        public Task WriteAsync(CommandQueue commandQueue, ReadOnlyMemory<T> data)
        {
            throw new NotImplementedException();
            //clWaitForEvent returned by clEnqueueWrite
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
