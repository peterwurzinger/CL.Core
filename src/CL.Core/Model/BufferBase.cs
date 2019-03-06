using CL.Core.API;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CL.Core.Model
{
    public abstract class BufferBase<T> : MemoryObject
        where T : unmanaged
    {
        private bool _disposed;
        public ulong Length { get; }

        internal BufferBase(IOpenClApi api, Context context, IntPtr id, MemoryHandle? hostMemory = null)
            : base(api, context, id, hostMemory)
        {
            Length = Size / (uint)Marshal.SizeOf<T>();
        }

        internal BufferBase(IOpenClApi api, Context context, IntPtr id)
            : this(api, context, id, null)
        {
        }

        public IReadOnlyCollection<T> Read(CommandQueue commandQueue)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (commandQueue == null)
                throw new ArgumentNullException(nameof(commandQueue));

            var memory = new T[Length];

            var hdl = GCHandle.Alloc(memory, GCHandleType.Pinned);
            try
            {
                var error = Api.BufferApi.clEnqueueReadBuffer(commandQueue.Id, Id, true, 0, (uint)Size,
                    hdl.AddrOfPinnedObject(), 0,
                    null, out _);

                error.ThrowOnError();

                return memory;
            }
            finally
            {
                hdl.Free();
            }
        }

        public unsafe void Write(CommandQueue commandQueue, ReadOnlySpan<T> data)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (commandQueue == null)
                throw new ArgumentNullException(nameof(commandQueue));

            fixed (void* ptr = data)
            {
                var error = Api.BufferApi.clEnqueueWriteBuffer(commandQueue.Id, Id, true, 0, (uint)Size,
                    (IntPtr)ptr, 0, null, out _);
                error.ThrowOnError();
            }
        }

        public async Task<IReadOnlyCollection<T>> ReadAsync(CommandQueue commandQueue)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (commandQueue == null)
                throw new ArgumentNullException(nameof(commandQueue));

            var memory = new T[Length];
            var hdl = GCHandle.Alloc(memory, GCHandleType.Pinned);
            var error = Api.BufferApi.clEnqueueReadBuffer(commandQueue.Id, Id, true, 0, (uint)Size, hdl.AddrOfPinnedObject(), 0,
                null, out var evt);

            try
            {
                error.ThrowOnError();

                var eventObject = new Event(Api, evt);
                await eventObject.WaitCompleteAsync().ConfigureAwait(false);

                return memory;
            }
            finally
            {
                hdl.Free();
            }
        }

        public async Task WriteAsync(CommandQueue commandQueue, ReadOnlyMemory<T> data)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (commandQueue == null)
                throw new ArgumentNullException(nameof(commandQueue));

            var handle = data.Pin();

            OpenClErrorCode error;
            IntPtr evt;

            unsafe
            {
                error = Api.BufferApi.clEnqueueWriteBuffer(commandQueue.Id, Id, false, 0, (uint)Size,
                    new IntPtr(handle.Pointer), 0, null, out evt);
            }

            try
            {
                error.ThrowOnError();

                var eventObj = new Event(Api, evt);
                await eventObj.WaitCompleteAsync().ConfigureAwait(false);
            }
            finally
            {
                handle.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
                _disposed = true;

            base.Dispose(disposing);
        }

    }
}