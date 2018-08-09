using CL.Core.API;
using System;

namespace CL.Core.Model
{
    public class CommandQueue : IHasId, IDisposable, IEquatable<CommandQueue>
    {
        private readonly ICommandQueueInterop _commandQueueInterop;
        private bool _disposed;
        public IntPtr Id { get; }
        public Context Context { get; }
        public Device Device { get; }

        internal CommandQueue(Context context, Device device, bool enableProfiling, bool enableOutOfOrderExcecutionMode, ICommandQueueInterop commandQueueInterop)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Device = device ?? throw new ArgumentNullException(nameof(device));
            _commandQueueInterop = commandQueueInterop ?? throw new ArgumentNullException(nameof(commandQueueInterop));

            CommandQueueProperties props = 0b0;
            if (enableProfiling)
                props |= CommandQueueProperties.ProfilingEnable;
            if (enableOutOfOrderExcecutionMode)
                props |= CommandQueueProperties.OutOfOrderExecModeEnable;

            var id = _commandQueueInterop.clCreateCommandQueue(context.Id, device.Id, props, out var error);
            error.ThrowOnError();

            Id = id;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);

            _disposed = true;
        }

        ~CommandQueue()
        {
            if (_commandQueueInterop == null || Id == IntPtr.Zero)
                return;

            ReleaseUnmanagedResources();
        }

        private void ReleaseUnmanagedResources()
        {
            var error = _commandQueueInterop.clReleaseCommandQueue(Id);
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
