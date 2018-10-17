﻿using CL.Core.API;
using System;
using System.Runtime.InteropServices;

namespace CL.Core.Model
{
    public class Kernel : IHasId, IDisposable
    {
        public IntPtr Id { get; }
        public Program Program { get; }
        public string Name { get; }
        public uint NumberOfArguments { get; }

        private readonly IOpenClApi _api;

        private bool _disposed;

        internal Kernel(IOpenClApi api, Program program, string name)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            Program = program ?? throw new ArgumentNullException(nameof(program));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            Name = name;

            var id = _api.KernelApi.clCreateKernel(program.Id, name, out var error);
            error.ThrowOnError();

            Id = id;

            var infoHelper = new InfoHelper<KernelInfoParameter>(this, _api.KernelApi.clGetKernelInfo);
            NumberOfArguments = infoHelper.GetValue<uint>(KernelInfoParameter.NumberOfArguments);
        }

        public void SetArgument<TArg>(int argIndex, TArg value)
            where TArg : unmanaged
        {
            ValidateArgIndex(argIndex);
            var handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            var error = _api.KernelApi.clSetKernelArg(Id, (uint)argIndex, (ulong)Marshal.SizeOf<TArg>(),
                handle.AddrOfPinnedObject());
            handle.Free();
            error.ThrowOnError();
        }

        public void SetMemoryArgument<TMem>(int argIndex, TMem memoryObject)
            where TMem : MemoryObject
        {
            ValidateArgIndex(argIndex);

            //TODO: Size of memory object, well...¯\_(ツ)_/¯
            //clSetKernelArg(Id, index, memoryObject.Size(?), memoryObject.Id);
        }

        private void ValidateArgIndex(int argIndex)
        {
            if (argIndex < 0 && argIndex >= NumberOfArguments)
                throw new ArgumentOutOfRangeException(nameof(argIndex), argIndex, $"The max index for {nameof(argIndex)} is {NumberOfArguments - 1}.");
        }

        private void ReleaseUnmanagedResources()
        {
            if (Id != IntPtr.Zero)
                _api?.KernelApi.clReleaseKernel(Id);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            ReleaseUnmanagedResources();
            if (disposing)
            {
                //TODO: Managed resources to dispose?
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Kernel()
        {
            Dispose(false);
        }
    }
}
