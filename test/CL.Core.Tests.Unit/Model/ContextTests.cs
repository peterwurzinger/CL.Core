using CL.Core.API;
using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class ContextTests : UnitTestBase
    {

        [Fact]
        public void CtorShouldThrowExceptionIfDevicesFromMultiplePlatformsArePassed()
        {
            var platform1 = new Platform(new IntPtr(1), FakeOpenClApi);
            var device1 = new Device(platform1, new IntPtr(1), FakeOpenClApi.DeviceApi);

            var platform2 = new Platform(new IntPtr(2), FakeOpenClApi);
            var device2 = new Device(platform2, new IntPtr(2), FakeOpenClApi.DeviceApi);

            Assert.Throws<ClCoreException>(() => new Context(FakeOpenClApi, new[] { device1, device2 }));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfDevicesNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Context(FakeOpenClApi, null));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfContextServiceNull()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            Assert.Throws<ArgumentNullException>(() => new Context(null, new[] { device }));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfDevicesEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Context(FakeOpenClApi, new Device[] { }));
        }

        [Fact]
        public void CreateCommandQueueShouldThrowExceptionIfAlreadyDisposed()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });
            ctx.Dispose();

            Assert.Throws<ObjectDisposedException>(() => ctx.CreateCommandQueue(device, false, false));
        }

        [Fact]
        public void CreateCommandQueueShouldThrowExceptionIfDeviceNull()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            Assert.Throws<ArgumentNullException>(() => ctx.CreateCommandQueue(null, false, false));
        }

        [Fact]
        public void CreateCommandQueueShouldThrowExceptionIfDeviceNotAttachedToContext()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device1 = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var device2 = new Device(platform, new IntPtr(2), FakeOpenClApi.DeviceApi);
            var ctx1 = new Context(FakeOpenClApi, new[] { device1 });

            Assert.Throws<ArgumentException>(() => ctx1.CreateCommandQueue(device2, false, false));
        }

        [Fact]
        public void CreateCommandQueueShouldReturnCommandQueue()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            var commandQueue = ctx.CreateCommandQueue(device, false, false);
            Assert.NotNull(commandQueue);
        }

    }
}
