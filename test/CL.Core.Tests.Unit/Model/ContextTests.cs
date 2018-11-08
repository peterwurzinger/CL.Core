using CL.Core.API;
using CL.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void CtorShouldSetId()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            FakeOpenClApi.FakeContextApi.clCreateContextResult = new IntPtr(2);

            var ctx = new Context(FakeOpenClApi, new[] { device });
            Assert.Equal(new IntPtr(2), ctx.Id);
        }

        [Fact]
        public void CtorShouldCreateContext()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            var ctx = new Context(FakeOpenClApi, new[] { device });

            Assert.NotNull(FakeOpenClApi.FakeContextApi.FakeContexts[ctx.Id]);
        }

        [Fact]
        public void CtorShouldSetDevices()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            FakeOpenClApi.FakeContextApi.clCreateContextResult = new IntPtr(2);

            var ctx = new Context(FakeOpenClApi, new[] { device });
            Assert.Single(ctx.Devices);
            Assert.Equal(device, ctx.Devices.Single());
        }

        [Fact]
        public void ContextShouldFireNotificationEvent()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var fired = false;

            var ctx = new Context(FakeOpenClApi, new[] { device });
            ctx.Notification += (sender, args) => { fired = true; };
            FakeOpenClApi.FakeContextApi.FakeContexts[ctx.Id].Notify("Fired", IntPtr.Zero, 0);

            Assert.True(fired);
        }

        [Fact]
        public void ContextShouldNotThrowExceptionIfDisposedMultipleTimes()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            ctx.Dispose();
            ctx.Dispose();
        }

        [Fact]
        public void DisposeShouldReleaseContext()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            ctx.Dispose();

            Assert.True(FakeOpenClApi.FakeContextApi.FakeContexts[ctx.Id].Released);
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
            Assert.Contains(commandQueue, ctx.CommandQueues);
        }

        [Fact]
        public void CreateBufferShouldThrowExceptionIfAlreadyDisposed()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });
            ctx.Dispose();

            Assert.Throws<ObjectDisposedException>(() => ctx.CreateBuffer<byte>());
        }

        [Fact]
        public void CreateBufferShouldInitNewBufferConfigurationChain()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            var bufferConfig = ctx.CreateBuffer<byte>();
            Assert.NotNull(bufferConfig);
            Assert.Empty(ctx.MemoryObjects);
        }

        [Fact]
        public void CreateProgramShouldThrowExceptionIfAlreadyDisposed()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });
            ctx.Dispose();

            Assert.Throws<ObjectDisposedException>(() => ctx.CreateProgram());
        }

        [Fact]
        public void CreateProgramShouldThrowExceptionIfSourcesNull()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            Assert.Throws<ArgumentNullException>("sources", () => ctx.CreateProgram((string[])null));
        }

        [Fact]
        public void CreateProgramShouldThrowExceptionIfSourcesEmpty()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            var sources = new string[] { };
            Assert.Throws<ArgumentException>("sources", () => ctx.CreateProgram(sources));
        }

        [Fact]
        public void CreateProgramShouldReturnProgram()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            var program = ctx.CreateProgram("__kernel TestKernel() {}");

            Assert.NotNull(program);
            Assert.Contains(program, ctx.Programs);
        }

        [Fact]
        public void CreateProgramShouldThrowExceptionIfBinariesNull()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            Assert.Throws<ArgumentNullException>(() =>
                ctx.CreateProgram((IReadOnlyDictionary<Device, ReadOnlyMemory<byte>>)null));
        }

        [Fact]
        public void CreateProgramShouldAttachProgram()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            var program = ctx.CreateProgram(new Dictionary<Device, ReadOnlyMemory<byte>>());

            Assert.Contains(program, ctx.Programs);
        }

        [Fact]
        public void DisposedShouldDisposeAttachedChildren()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            var cq = ctx.CreateCommandQueue(ctx.Devices.First(), false, false);
            //var mem = ctx.CreateBuffer<byte>().ByHostMemory(new byte[1]).AsReadOnly();
            var program = ctx.CreateProgram("__kernel TestKernel() {}");

            ctx.Dispose();

            Assert.True(FakeOpenClApi.FakeCommandQueueApi.FakeCommandQueues[cq.Id].Released);
            Assert.True(FakeOpenClApi.FakeProgramApi.FakePrograms[program.Id].Released);
            //TODO: Check for released buffer object
        }

        [Fact]
        public void CreateBufferShouldTrackCreatedBuffer()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var ctx = new Context(FakeOpenClApi, new[] { device });

            var buffer = ctx.CreateBuffer<byte>().ByAllocation(100).AsReadOnly();

            Assert.Contains(buffer, ctx.MemoryObjects);
        }
    }
}
