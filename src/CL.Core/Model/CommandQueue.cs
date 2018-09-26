using CL.Core.API;
using System;

namespace CL.Core.Model
{
    public class CommandQueue : IHasId, IDisposable, IEquatable<CommandQueue>
    {
        public IntPtr Id { get; }
        public Context Context { get; }
        public Device Device { get; }

        private readonly ICommandQueueApi _commandQueueApi;
        private bool _disposed;

        internal CommandQueue(Context context, Device device, bool enableProfiling, bool enableOutOfOrderExecutionMode, ICommandQueueApi commandQueueApi)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Device = device ?? throw new ArgumentNullException(nameof(device));
            _commandQueueApi = commandQueueApi ?? throw new ArgumentNullException(nameof(commandQueueApi));

            CommandQueueProperties props = 0b0;
            if (enableProfiling)
                props |= CommandQueueProperties.ProfilingEnable;
            if (enableOutOfOrderExecutionMode)
                props |= CommandQueueProperties.OutOfOrderExecModeEnable;

            var id = _commandQueueApi.clCreateCommandQueue(context.Id, device.Id, props, out var error);
            error.ThrowOnError();

            Id = id;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            ReleaseUnmanagedResources();
            _disposed = true;
        }

        ~CommandQueue()
        {
            Dispose(false);
        }

        private void ReleaseUnmanagedResources()
        {
            if (_commandQueueApi == null || Id == IntPtr.Zero)
                return;

            var error = _commandQueueApi.clReleaseCommandQueue(Id);
            error.ThrowOnError();
        }

        public bool Equals(CommandQueue other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CommandQueue)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
