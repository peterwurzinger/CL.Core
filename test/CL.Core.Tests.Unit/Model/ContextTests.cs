using CL.Core.API;
using CL.Core.Fakes;
using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class ContextTests
    {
        private readonly FakePlatformInfoInterop _fakePlatformInfoInterop;
        private readonly FakeDeviceInfoInterop _fakeDeviceInfoInterop;
        private readonly FakeContextInterop _fakeContextInterop;

        public ContextTests()
        {
            _fakePlatformInfoInterop = new FakePlatformInfoInterop();
            _fakeDeviceInfoInterop = new FakeDeviceInfoInterop();
            _fakeContextInterop = new FakeContextInterop();
        }

        [Fact]
        public void CtorShouldThrowExceptionIfDevicesFromMultiplePlatformsArePassed()
        {
            var platform1 = new Platform(new IntPtr(1), _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device1 = new Device(platform1, new IntPtr(1), _fakeDeviceInfoInterop);

            var platform2 = new Platform(new IntPtr(2), _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device2 = new Device(platform2, new IntPtr(2), _fakeDeviceInfoInterop);

            Assert.Throws<ClCoreException>(() => new Context(_fakeContextInterop, new[] { device1, device2 }));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfDevicesNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Context(_fakeContextInterop, null));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfContextServiceNull()
        {
            var platform = new Platform(new IntPtr(1), _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device = new Device(platform, new IntPtr(1), _fakeDeviceInfoInterop);

            Assert.Throws<ArgumentNullException>(() => new Context(null, new[] { device }));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfDevicesEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Context(_fakeContextInterop, new Device[] { }));
        }

        [Fact]
        public void CreateCommandQueueShouldThrowExceptionIfAlreadyDisposed()
        {
            var platform = new Platform(new IntPtr(1), _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device = new Device(platform, new IntPtr(1), _fakeDeviceInfoInterop);
            var ctx = new Context(_fakeContextInterop, new []{device});
            ctx.Dispose();

            Assert.Throws<ObjectDisposedException>(() => ctx.CreateCommandQueue(device, false, false, new FakeCommandQueueInterop()));
        }

        [Fact]
        public void CreateCommandQueueShouldThrowExceptionIfDeviceNull()
        {
            var platform = new Platform(new IntPtr(1), _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device = new Device(platform, new IntPtr(1), _fakeDeviceInfoInterop);
            var ctx = new Context(_fakeContextInterop, new[] { device });

            Assert.Throws<ArgumentNullException>(() => ctx.CreateCommandQueue(null, false, false, new FakeCommandQueueInterop()));
        }

        [Fact]
        public void CreateCommandQueueShouldThrowExceptionIfDeviceNotAttachedToContext()
        {
            var platform = new Platform(new IntPtr(1), _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device1 = new Device(platform, new IntPtr(1), _fakeDeviceInfoInterop);
            var device2 = new Device(platform, new IntPtr(2), _fakeDeviceInfoInterop);
            var ctx1 = new Context(_fakeContextInterop, new[] { device1 });

            Assert.Throws<ArgumentException>(() => ctx1.CreateCommandQueue(device2, false, false, new FakeCommandQueueInterop()));
        }

        [Fact]
        public void CreateCommandQueueShouldReturnCommandQueue()
        {
            var platform = new Platform(new IntPtr(1), _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device = new Device(platform, new IntPtr(1), _fakeDeviceInfoInterop);
            var ctx = new Context(_fakeContextInterop, new[] { device });

            var commandQueue = ctx.CreateCommandQueue(device, false, false, new FakeCommandQueueInterop());
            Assert.NotNull(commandQueue);
        }

    }
}
