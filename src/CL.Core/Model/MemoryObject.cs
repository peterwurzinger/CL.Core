using CL.Core.API;
using System;
using System.Buffers;

namespace CL.Core.Model
{
    public abstract class MemoryObject : IHasId, IDisposable
    {
        public IntPtr Id { get; }
        public Context Context { get; }
        public ulong Size { get; }
        public MemoryFlags Flags { get; }

        protected IOpenClApi Api { get; }

        private MemoryHandle? _hostMemory;
        private bool _disposed;

        protected MemoryObject(IOpenClApi api, Context context, IntPtr id, MemoryHandle? hostMemory = null)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Id = id;
            _hostMemory = hostMemory;

            var infoHelper = new InfoHelper<MemoryObjectInfoParameter>(this, Api.BufferApi.clGetMemObjectInfo);
            Flags = infoHelper.GetValue<MemoryFlags>(MemoryObjectInfoParameter.Flags);
            Size = infoHelper.GetValue<ulong>(MemoryObjectInfoParameter.Size);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _hostMemory?.Dispose();
            }
            ReleaseUnmanagedResources();
            _disposed = true;
        }

        private void ReleaseUnmanagedResources()
        {
            //Push to MemoryObject?
            if (Api != null && Id != IntPtr.Zero)
                Api.BufferApi.clReleaseMemObject(Id);
        }

        ~MemoryObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}