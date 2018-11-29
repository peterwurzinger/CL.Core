using CL.Core.API;
using System;
using System.Buffers;
using System.Runtime.InteropServices;

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

            var memory = new T[Size];
            var hdl = GCHandle.Alloc(memory, GCHandleType.Pinned);
            var error = Api.BufferApi.clEnqueueReadBuffer(commandQueue.Id, Id, true, 0, (uint)Size, hdl.AddrOfPinnedObject(), 0,
                null, out _);

            hdl.Free();
            error.ThrowOnError();

            return memory;
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

        public Event<ReadOnlyMemory<T>> ReadAsync(CommandQueue commandQueue)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (commandQueue == null)
                throw new ArgumentNullException(nameof(commandQueue));

            var memory = new T[Size];
            var hdl = GCHandle.Alloc(memory, GCHandleType.Pinned);
            var error = Api.BufferApi.clEnqueueReadBuffer(commandQueue.Id, Id, true, 0, (uint)Size, hdl.AddrOfPinnedObject(), 0,
                null, out var evt);

            hdl.Free();
            error.ThrowOnError();

            return new Event<ReadOnlyMemory<T>>(Api, evt, memory);
        }

        public unsafe Event WriteAsync(CommandQueue commandQueue, ReadOnlyMemory<T> data)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (commandQueue == null)
                throw new ArgumentNullException(nameof(commandQueue));

            var handle = data.Pin();
            var error = Api.BufferApi.clEnqueueWriteBuffer(commandQueue.Id, Id, false, 0, (uint)Size, new IntPtr(handle.Pointer), 0, null, out var evt);
            handle.Dispose();
            error.ThrowOnError();

            return new Event(Api, evt);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
                _disposed = true;

            base.Dispose(disposing);
        }

    }
}