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

        protected readonly IOpenClApi OpenClApi;
        private readonly IList<CommandQueue> _attachedCommandQueues;
        private readonly IList<MemoryObject> _attachedMemoryObjects;
        private bool _disposed;

        //TODO: Make method of Platform, since contexts spanning multiple platforms is not supported anyway 
        public Context(IOpenClApi openClApi, IReadOnlyCollection<Device> devices)
        {
            OpenClApi = openClApi ?? throw new ArgumentNullException(nameof(openClApi));
            if (devices == null) throw new ArgumentNullException(nameof(devices));
            if (devices.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(devices));

            if (devices.GroupBy(d => d.Platform.Id).Count() > 1)
                throw new ClCoreException("Contexts containing devices from multiple platforms are not supported.");

            Devices = devices;

            //TODO: Elaborate on this
            // ReSharper disable once VirtualMemberCallInConstructor
            Id = CreateUnmanagedContext(openClApi.ContextApi, devices);

            _attachedCommandQueues = new List<CommandQueue>();
            _attachedMemoryObjects = new List<MemoryObject>();
        }

        protected internal virtual IntPtr CreateUnmanagedContext(IContextApi contextApi, IReadOnlyCollection<Device> devices)
        {
            var id = contextApi.clCreateContext(IntPtr.Zero, (uint)devices.Count, devices.Select(device => device.Id).ToArray(), IntPtr.Zero, IntPtr.Zero, out var error);
            error.ThrowOnError();
            return id;
        }

        public CommandQueue CreateCommandQueue(Device device, bool enableProfiling, bool enableOutOfOrderExecutionMode)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (device == null)
                throw new ArgumentNullException(nameof(device));

            if (!Devices.Contains(device))
                throw new ArgumentException("Device is not attachted to calling calling context.", nameof(device));

            var commandQueue = new CommandQueue(this, device, enableProfiling, enableOutOfOrderExecutionMode, OpenClApi.CommandQueueApi);
            _attachedCommandQueues.Add(commandQueue);

            return commandQueue;
        }

        public BufferStubConfiguration<T> CreateBuffer<T>()
        where T : unmanaged
        {
            return new BufferStubConfiguration<T>(OpenClApi, this, b => _attachedMemoryObjects.Add(b));
        }

        private void ReleaseUnmanagedResources()
        {
            if (OpenClApi?.ContextApi == null || Id == IntPtr.Zero)
                return;

            OpenClApi.ContextApi.clReleaseContext(Id).ThrowOnError();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                foreach (var commandQueue in _attachedCommandQueues)
                    commandQueue.Dispose();

                foreach (var memoryObject in _attachedMemoryObjects)
                    memoryObject.Dispose();
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
