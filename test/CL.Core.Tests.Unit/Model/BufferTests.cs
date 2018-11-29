using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class BufferTests : UnitTestBase
    {
        private readonly Buffer<byte> _target;

        public BufferTests()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(2), FakeOpenClApi.FakeDeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });

            var hostMemory = new byte[10];
            _target = context.CreateBuffer<byte>().ByHostMemory(hostMemory).AsReadWrite();
        }

        [Fact]
        public void CtorShouldInitializeSubBuffers()
        {
            Assert.NotNull(_target.SubBuffers);
        }


    }
}
