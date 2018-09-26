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
        public void CtorShouldThrowExceptionIfInteropCallUnsuccessful()
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

        [Fact]
        public void CtorShouldSetDevice()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);

            Assert.NotNull(cq.Device);
            Assert.Equal(device, cq.Device);
        }

        [Fact]
        public void CtorShouldSetContext()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);

            Assert.NotNull(cq.Context);
            Assert.Equal(device, cq.Device);
        }

        [Fact]
        public void DisposeShouldNotThrowExceptionIfDisposedMultipleTimes()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });
            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);

            cq.Dispose();
            cq.Dispose();
        }

        [Fact]
        public void GetHashCodeShouldReturnHashCodeOfId()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);

            Assert.Equal(cq.Id.GetHashCode(), cq.GetHashCode());
        }

        [Fact]
        public void EqualsShouldReturnFalseIfOtherIsNull()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);

            Assert.False(cq.Equals(null));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfOtherIsSameReference()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);
            var other = cq;

            Assert.True(cq.Equals(other));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfCommandQueuesHaveSameIds()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            FakeOpenClApi.FakeCommandQueueApi.clCreateCommandQueueResult = new IntPtr(2);
            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);
            var other = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);

            Assert.True(cq.Equals(other));
        }

        [Fact]
        public void EqualsShouldReturnFalseIfObjIsNull()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);

            Assert.False(cq.Equals((object)null));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfObjIsSameReference()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);
            object other = cq;

            Assert.True(cq.Equals(other));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfObjIsOfSameTypeAndHasSameId()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            FakeOpenClApi.FakeCommandQueueApi.clCreateCommandQueueResult = new IntPtr(1);
            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);
            object other = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);

            Assert.True(cq.Equals(other));
        }

        [Fact]
        public void EqualsShouldReturnFalseIfObjIsOfDifferentType()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            var cq = new CommandQueue(context, device, false, false, FakeOpenClApi.CommandQueueApi);
            object other = new SuspiciousCommandQueue(context, device, false, false, FakeOpenClApi.FakeCommandQueueApi);

            Assert.False(cq.Equals(other));
        }

        //TODO: Tests for Command Queue properties
        private class SuspiciousCommandQueue : CommandQueue
        {
            internal SuspiciousCommandQueue(Context context, Device device, bool enableProfiling, bool enableOutOfOrderExecutionMode, ICommandQueueApi commandQueueApi) : base(context, device, enableProfiling, enableOutOfOrderExecutionMode, commandQueueApi)
            {
            }
        }
    }
}
