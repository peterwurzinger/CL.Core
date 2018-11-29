using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CL.Core.API;

namespace CL.Core.Model
{
    public abstract class BufferBase<T> : MemoryObject
        where T : unmanaged
    {
        private bool _disposed;

        internal BufferBase(IOpenClApi api, Context context, IntPtr id, MemoryHandle? hostMemory = null)
            : base(api, context, id, hostMemory)
        {
        }
        internal BufferBase(IOpenClApi api, Context context, IntPtr id)
            : this(api, context, id, null)
        {
        }

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
                _disposed = true;

            base.Dispose(disposing);
        }

    }
}