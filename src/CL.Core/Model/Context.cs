using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Core.Model
{
    public class Context : IHasId, IDisposable
    {
        public IntPtr Id { get; }

        public IReadOnlyCollection<Device> Devices { get; }

        //TODO: Context-Properties

        protected readonly IContextInterop ContextInterop;
        private bool _disposed;

        public Context(IContextInterop contextInterop, IReadOnlyCollection<Device> devices)
        {
            ContextInterop = contextInterop ?? throw new ArgumentNullException(nameof(contextInterop));
            if (devices == null) throw new ArgumentNullException(nameof(devices));
            if (devices.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(devices));

            if (devices.GroupBy(d => d.Platform.Id).Count() > 1)
                throw new ClCoreException("Contexts containing devices from multiple platforms are not supported.");

            Devices = devices;

            //TODO: Elaborate on this
            // ReSharper disable once VirtualMemberCallInConstructor
            Id = CreateUnmanagedContext(contextInterop, devices);
        }

        protected internal virtual IntPtr CreateUnmanagedContext(IContextInterop contextInterop, IReadOnlyCollection<Device> devices)
        {
            var id = contextInterop.clCreateContext(IntPtr.Zero, (uint)devices.Count, devices.Select(device => device.Id).ToArray(), IntPtr.Zero, IntPtr.Zero, out var error);
            error.ThrowOnError();
            return id;
        }

        private void ReleaseUnmanagedResources()
        {
            ContextInterop.clReleaseContext(Id).ThrowOnError();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }
            ReleaseUnmanagedResources();
            // TODO: set large fields to null - if any

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Context()
        {
            Dispose(false);
        }
    }
}
