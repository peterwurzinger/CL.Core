using System;
using CL.Core.API;

namespace CL.Core.Model
{
    public abstract class MemoryObject : IHasId, IDisposable
    {
        public IntPtr Id { get; protected set; }
        public Context Context { get; }

        protected IOpenClApi Api { get; }
        private bool _disposed;

        protected MemoryObject(IOpenClApi api, Context context)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private void ReleaseUnmanagedResources()
        {
            if (Api?.BufferApi == null || Id == IntPtr.Zero)
                return;

            Api.BufferApi.clReleaseMemObject(Id).ThrowOnError();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            ReleaseUnmanagedResources();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MemoryObject()
        {
            Dispose(false);
        }
    }
}