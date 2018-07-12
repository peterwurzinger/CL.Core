using System;
using System.Collections.Generic;
using System.Linq;
using CL.Core.Native;

namespace CL.Core.Model
{
    public class Context : IDisposable
    {
        public long Id { get; }

        public IReadOnlyCollection<Device> Devices { get; }

        //TODO: Notification Function pointer: Beware of pinning! https://stackoverflow.com/questions/11400476/pin-a-function-pointer
        //TODO: Context-Properties
        //TODO: User-Data
        public Context(IReadOnlyCollection<Device> devices)
        {
            if (devices == null)
                throw new ArgumentNullException(nameof(devices));
            if (devices.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(devices));

            Devices = devices;
            var id = ContextCalls.CreateContext(IntPtr.Zero, (uint)devices.Count,
                devices.Select(device => new IntPtr(device.Id)).ToArray(), IntPtr.Zero, IntPtr.Zero, out var error);

            //TODO: Check error
            Id = id.ToInt64();
        }

        public Context(Device device)
            : this(new [] { device})
        {

        }

        private void ReleaseUnmanagedResources()
        {
            //TODO: Release notify function-pointer?
            ContextCalls.ReleaseContext(new IntPtr(Id));
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
