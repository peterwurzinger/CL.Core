using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CL.Core.Model
{
    public class Context : IDisposable, IEquatable<Context>
    {
        private readonly IContextInterop _contextInterop;

        //TODO: Maybe extract delegate-full context to subclass e.g. Context<TUserData>?
        private readonly GCHandle? _delegateHandle;

        public long Id { get; }
        public IReadOnlyCollection<Device> Devices { get; }

        public Context(Device device, IContextInterop contextInterop)
            : this(new[] { device }, contextInterop, null)
        {

        }

        //TODO: Context-Properties
        public Context(IReadOnlyCollection<Device> devices, IContextInterop contextInterop, Action<string, Memory<byte>, object> errorCallback)
        {
            if (devices == null)
                throw new ArgumentNullException(nameof(devices));
            if (devices.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(devices));

            _contextInterop = contextInterop ?? throw new ArgumentNullException(nameof(contextInterop));
            Devices = devices;

            _delegateHandle = GCHandle.Alloc((ContextErrorDelegate)ErrorCallback);
            var fp = Marshal.GetFunctionPointerForDelegate((ContextErrorDelegate)ErrorCallback);

            //TODO: Is there a further need to pass User-Data to OpenCL? In my understanding it only gets passed back when the callback function is invoked, so why not keep it in managed memory?
            var id = contextInterop.clCreateContext(IntPtr.Zero, (uint)devices.Count,
                devices.Select(device => new IntPtr(device.Id)).ToArray(), fp, IntPtr.Zero, out var error);
            error.ThrowOnError();

            Id = id.ToInt64();
        }

        private void ErrorCallback(string error, IntPtr privateInfo, int cb, IntPtr userData)
        {
            //TODO: Do proxy-stuff and call user-callback
        }

        private void ReleaseUnmanagedResources()
        {
            _contextInterop.clReleaseContext(new IntPtr(Id)).ThrowOnError();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();

            //Ignore this for now. TODO
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            _delegateHandle?.Free();

            GC.SuppressFinalize(this);
        }

        ~Context()
        {
            ReleaseUnmanagedResources();
        }

        public bool Equals(Context other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Context)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}