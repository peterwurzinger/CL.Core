using CL.Core.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CL.Core.Model
{
    public class Context : IHasId, IDisposable
    {
        public event EventHandler<ContextNotificationEventArgs> Notification;

        public IntPtr Id { get; }
        public IReadOnlyCollection<Device> Devices { get; }

        //TODO: Context-Properties

        protected IOpenClApi OpenClApi { get; }

        private readonly IList<CommandQueue> _attachedCommandQueues;
        private readonly IList<MemoryObject> _attachedMemoryObjects;
        private readonly IList<Program> _attachedProgramObjects;

        private GCHandle _delegateHandle;
        private bool _disposed;

        //TODO: Extract as factory-method in platform-class, since contexts spanning multiple platforms is not supported anyway 
        public Context(IOpenClApi openClApi, IReadOnlyCollection<Device> devices)
        {
            OpenClApi = openClApi ?? throw new ArgumentNullException(nameof(openClApi));
            if (devices == null) throw new ArgumentNullException(nameof(devices));
            if (devices.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(devices));

            if (devices.GroupBy(d => d.Platform.Id).Count() > 1)
                throw new ClCoreException("Contexts containing devices from multiple platforms are not supported.");

            Devices = devices;

            _delegateHandle = GCHandle.Alloc((ContextErrorDelegate)NotificationCallbackProxy);
            var fp = Marshal.GetFunctionPointerForDelegate((ContextErrorDelegate)NotificationCallbackProxy);

            var id = OpenClApi.ContextApi.clCreateContext(IntPtr.Zero, (uint)devices.Count, devices.Select(device => device.Id).ToArray(), fp, IntPtr.Zero, out var error);
            error.ThrowOnError();
            Id = id;

            _attachedCommandQueues = new List<CommandQueue>();
            _attachedMemoryObjects = new List<MemoryObject>();
            _attachedProgramObjects = new List<Program>();
        }

        private void NotificationCallbackProxy(string error, IntPtr privateInfo, int cb, IntPtr userData)
        {
            unsafe
            {
                var span = new ReadOnlySpan<byte>(privateInfo.ToPointer(), cb);
                var mem = new ReadOnlyMemory<byte>(span.ToArray());

                var handler = Notification;
                if (handler != null)
                {
                    var args = new ContextNotificationEventArgs(error, mem);
                    handler(this, args);
                }
            }
        }

        public CommandQueue CreateCommandQueue(Device device, bool enableProfiling, bool enableOutOfOrderExecutionMode)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (device == null)
                throw new ArgumentNullException(nameof(device));

            if (!Devices.Contains(device))
                throw new ArgumentException("Device is not attached to calling context.", nameof(device));

            var commandQueue = new CommandQueue(this, device, enableProfiling, enableOutOfOrderExecutionMode, OpenClApi.CommandQueueApi);
            _attachedCommandQueues.Add(commandQueue);

            return commandQueue;
        }

        public BufferStubConfiguration<T> CreateBuffer<T>()
        where T : unmanaged
        {
            return new BufferStubConfiguration<T>(OpenClApi, this, b => _attachedMemoryObjects.Add(b));
        }

        public Program CreateProgram(params FileInfo[] sourceFiles)
        {
            //TODO: Provide extension methods for string-sources
            var sources = new string[sourceFiles.Length];
            for (var i = 0; i < sourceFiles.Length; i++)
                sources[i] = File.ReadAllText(sourceFiles[i].FullName);

            var program = new Program(OpenClApi, this, sources);
            _attachedProgramObjects.Add(program);
            return program;
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
                _delegateHandle.Free();

                foreach (var commandQueue in _attachedCommandQueues)
                    commandQueue.Dispose();

                foreach (var memoryObject in _attachedMemoryObjects)
                    memoryObject.Dispose();

                foreach (var programObject in _attachedProgramObjects)
                    programObject.Dispose();
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
