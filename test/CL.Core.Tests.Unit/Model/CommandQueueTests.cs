using CL.Core.API;
using CL.Core.Fakes;
using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class CommandQueueTests
    {

        private readonly FakeContextInterop _fakeContextInterop;
        private readonly FakeDeviceInfoInterop _fakeDeviceInfoInterop;
        private readonly FakePlatformInfoInterop _fakePlatformInfoInterop;
        private readonly FakeCommandQueueInterop _fakeCommandQueueInterop;
        public CommandQueueTests()
        {
            _fakePlatformInfoInterop = new FakePlatformInfoInterop();
            _fakeDeviceInfoInterop = new FakeDeviceInfoInterop();
            _fakeContextInterop = new FakeContextInterop();
            _fakeCommandQueueInterop = new FakeCommandQueueInterop();
        }

        [Fact]
        public void CtorShouldThrowExceptionIfContextNull()
        {
            var platform = new Platform(IntPtr.Zero, _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device = new Device(platform, IntPtr.Zero, _fakeDeviceInfoInterop);

            Assert.Throws<ArgumentNullException>(() => new CommandQueue(null, device, false, false, _fakeCommandQueueInterop));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfDeviceNull()
        {
            var platform = new Platform(IntPtr.Zero, _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device = new Device(platform, IntPtr.Zero, _fakeDeviceInfoInterop);
            var context = new Context(_fakeContextInterop, new[] { device });

            Assert.Throws<ArgumentNullException>(() => new CommandQueue(context, null, false, false, _fakeCommandQueueInterop));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfCommandQueueInteropNull()
        {
            var platform = new Platform(IntPtr.Zero, _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device = new Device(platform, IntPtr.Zero, _fakeDeviceInfoInterop);
            var context = new Context(_fakeContextInterop, new[] { device });

            Assert.Throws<ArgumentNullException>(() => new CommandQueue(context, device, false, false, null));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfInteropCallUnsuccessfull()
        {
            var platform = new Platform(IntPtr.Zero, _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device = new Device(platform, IntPtr.Zero, _fakeDeviceInfoInterop);
            var context = new Context(_fakeContextInterop, new[] { device });
            _fakeCommandQueueInterop.clCreateCommandQueueErrorCode = OpenClErrorCode.InvalidContext;

            Assert.Throws<ClCoreException>(() => new CommandQueue(context, device, false, false, _fakeCommandQueueInterop));
        }

        [Fact]
        public void CtorShouldSetId()
        {
            var platform = new Platform(IntPtr.Zero, _fakePlatformInfoInterop, _fakeDeviceInfoInterop);
            var device = new Device(platform, IntPtr.Zero, _fakeDeviceInfoInterop);
            var context = new Context(_fakeContextInterop, new[] { device });
            _fakeCommandQueueInterop.clCreateCommandQueueResult = new IntPtr(2);

            var cq = new CommandQueue(context, device, false, false, _fakeCommandQueueInterop);
            Assert.Equal(new IntPtr(2), cq.Id);
        }

        //TODO: Tests for Command Queue properties

    }
}
