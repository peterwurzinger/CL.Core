﻿using CL.Core.API;
using CL.Core.Model;
using System;
using System.Runtime.InteropServices;

namespace CL.Core
{
    public class BufferMemoryUsageConfiguration<T> : BufferMemoryBehaviorConfiguration<T>
        where T : unmanaged
    {
        private readonly Memory<T> _hostMemory;

        public BufferMemoryUsageConfiguration(IOpenClApi api, Context context, Action<Buffer<T>> bufferCreatedCallback, MemoryFlags flags, Memory<T> hostMemory)
            : base(api, context, bufferCreatedCallback, flags)
        {
            _hostMemory = hostMemory;
        }

        protected override unsafe Buffer<T> Build()
        {
            var typeSize = Marshal.SizeOf<T>();

            var handle = _hostMemory.Pin();
            var id = Api.BufferApi.clCreateBuffer(Context.Id, Flags, (uint)typeSize * (uint)_hostMemory.Length, new IntPtr(handle.Pointer), out var error);
            if (error != OpenClErrorCode.Success)
            {
                handle.Dispose();
                error.ThrowOnError();
            }
            return new Buffer<T>(Api, Context, id, handle);
        }
    }
}