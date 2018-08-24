using CL.Core.API;
using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class CommandQueueTests : UnitTestBase
    {

        [Fact]
        public void CtorShouldThrowExceptionIfContextNull()
        {
            var platform = new Platform(IntPtr.Zero, FakeOpenClApi);
            var device = new Device(platform, IntPtr.Zero, FakeOpenClApi.DeviceApi);

            Assert.Throws<ArgumentNullException>(() => new CommandQueue(null, device, false, false, FakeOpenClApi.CommandQueueApi));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfDeviceNull()
        {
            var platform = new Platform(IntPtr.Zero, FakeOpenClApi);
            var device = new Device(platform, IntPtr.Zero, FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            Assert.Throws<ArgumentNullException>(() => new CommandQueue(context, null, false, false, FakeOpenClApi.CommandQueueApi));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfCommandQueueApiNull()
        {
            var platform = new Platform(IntPtr.Zero, FakeOpenClApi);
            var device = new Device(platform, IntPtr.Zero, FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            Assert.Throws<ArgumentNullException>(() => new CommandQueue(context, device, false, false, null));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfInteropCallUnsuccessfull()
        {
            var platform = new Platform(IntPtr.Zero, FakeOpenClApi);
            var device = new Device(platform, IntPtr.Zero, FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });
            FakeOpenClApi.FakeCommandQueueApi.clCreateCommandQueueErrorCode = OpenClErrorCode.InvalidContext;

            Assert.Throws<ClCoreException>(() => new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi));
        }

        [Fact]
        public void CtorShouldSetId()
        {
            var platform = new Platform(IntPtr.Zero, FakeOpenClApi);
            var device = new Device(platform, IntPtr.Zero, FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });
            FakeOpenClApi.FakeCommandQueueApi.clCreateCommandQueueResult = new IntPtr(2);

            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);
            Assert.Equal(new IntPtr(2), cq.Id);
        }

        //TODO: Tests for Command Queue properties

    }
}
