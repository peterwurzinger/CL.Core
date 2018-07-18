using System;
using System.Collections.Generic;
using System.Linq;
using CL.Core.API;

namespace CL.Core.Model
{
    public class Context : IDisposable
    {
        private readonly IContextInterop _contextInterop;
        public long Id { get; }
        public IReadOnlyCollection<Device> Devices { get; }

        
        //TODO: Notification Function pointer: Beware of pinning! https://stackoverflow.com/questions/11400476/pin-a-function-pointer
        //TODO: Context-Properties
        //TODO: User-Data
        public Context(IReadOnlyCollection<Device> devices, IContextInterop contextInterop)
        {
            if (devices == null)
                throw new ArgumentNullException(nameof(devices));
            if (devices.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(devices));

            _contextInterop = contextInterop ?? throw new ArgumentNullException(nameof(contextInterop));

            Devices = devices;
            var id = contextInterop.clCreateContext(IntPtr.Zero, (uint)devices.Count,
                devices.Select(device => new IntPtr(device.Id)).ToArray(), IntPtr.Zero, IntPtr.Zero, out var error);
            error.ThrowOnError();

            Id = id.ToInt64();
        }

        public Context(Device device, IContextInterop contextInterop)
            : this(new [] { device}, contextInterop)
        {

        }

        private void ReleaseUnmanagedResources()
        {
            //TODO: Release notify function-pointer?
            _contextInterop.clReleaseContext(new IntPtr(Id)).ThrowOnError();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Context()
        {
            ReleaseUnmanagedResources();
        }
    }
}
