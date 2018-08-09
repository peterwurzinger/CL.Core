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
        private readonly IList<CommandQueue> _attachtedCommandQueues;
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
            _attachtedCommandQueues = new List<CommandQueue>();
        }

        protected internal virtual IntPtr CreateUnmanagedContext(IContextInterop contextInterop, IReadOnlyCollection<Device> devices)
        {
            var id = contextInterop.clCreateContext(IntPtr.Zero, (uint)devices.Count, devices.Select(device => device.Id).ToArray(), IntPtr.Zero, IntPtr.Zero, out var error);
            error.ThrowOnError();
            return id;
        }

        public CommandQueue CreateCommandQueue(Device device, bool enableProfiling, bool enableOutOfOrderExecutionMode, ICommandQueueInterop commandQueueInterop)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (device == null)
                throw new ArgumentNullException(nameof(device));

            if (!Devices.Contains(device))
                throw new ArgumentException("Device is not attachted to calling calling context.", nameof(device));

            var commandQueue = new CommandQueue(this, device, enableProfiling, enableOutOfOrderExecutionMode, commandQueueInterop);
            _attachtedCommandQueues.Add(commandQueue);

            return commandQueue;
        }

        private void ReleaseUnmanagedResources()
        {
            if (ContextInterop == null || Id == IntPtr.Zero)
                return;

            ContextInterop.clReleaseContext(Id).ThrowOnError();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                foreach (var commandQueue in _attachtedCommandQueues)
                    commandQueue.Dispose();
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
