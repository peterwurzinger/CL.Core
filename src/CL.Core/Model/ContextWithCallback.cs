using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CL.Core.Model
{
    public class ContextWithCallback<TUserData> : Context
    {
        public delegate void ContextErrorUserCallback(string error, ReadOnlySpan<byte> privateInfo, TUserData userData);

        private readonly ContextErrorUserCallback _userCallback;
        private readonly TUserData _userData;
        private bool _disposed;
        private GCHandle _delegateHandle;

        public ContextWithCallback(IReadOnlyCollection<Device> devices, IOpenClApi openClApi, ContextErrorUserCallback userCallback, TUserData userData)
            : base(openClApi, devices)
        {
            _userCallback = userCallback ?? throw new ArgumentNullException(nameof(userCallback));
            _userData = userData;
        }

        protected internal override IntPtr CreateUnmanagedContext(IContextApi contextApi, IReadOnlyCollection<Device> devices)
        {
            _delegateHandle = GCHandle.Alloc((ContextErrorDelegate)ErrorCallbackProxy);
            var fp = Marshal.GetFunctionPointerForDelegate((ContextErrorDelegate)ErrorCallbackProxy);

            //TODO: Is there a need to pass User-Data to OpenCL? In my understanding it only gets passed back when the callback function is invoked, so why not keep it in managed memory?
            var id = contextApi.clCreateContext(IntPtr.Zero, (uint)devices.Count, devices.Select(device => device.Id).ToArray(), fp, IntPtr.Zero, out var error);
            error.ThrowOnError();
            return id;
        }

        private void ErrorCallbackProxy(string error, IntPtr privateInfo, int cb, IntPtr userData)
        {
            unsafe
            {
                var span = new ReadOnlySpan<byte>(privateInfo.ToPointer(), cb);
                _userCallback(error, span, _userData);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _delegateHandle.Free();
                }
                //TODO: Unmanaged resources to free?
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
