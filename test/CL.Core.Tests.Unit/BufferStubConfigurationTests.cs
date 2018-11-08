using CL.Core.Model;
using CL.Core.Tests.Unit.Model;
using System;
using CL.Core.API;
using Xunit;

namespace CL.Core.Tests.Unit
{
    public class BufferStubConfigurationTests : UnitTestBase
    {
        private readonly Context _context;
        private readonly BufferStubConfiguration<byte> _stubConfiguration;

        public BufferStubConfigurationTests()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var devices = new[] { new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi) };
            _context = new Context(FakeOpenClApi, devices);

            _stubConfiguration = _context.CreateBuffer<byte>();
        }

        [Fact]
        public void ByAllocationShouldSetCorrectFlagsAndSize()
        {
            var buffer = _stubConfiguration.ByAllocation(1).AsReadOnly();

            Assert.True(buffer.Flags.HasFlag(MemoryFlags.AllocHostPointer));
            Assert.Equal((ulong)1, buffer.Size);
        }

        [Fact]
        public void ByCopyShouldSetCorrectFlagsAndSize()
        {
            var buffer = _stubConfiguration.ByCopy(new byte[1]).AsReadOnly();

            Assert.True(buffer.Flags.HasFlag(MemoryFlags.CopyHostPointer));
            Assert.Equal((ulong)1, buffer.Size);
        }

        [Fact]
        public void ByAllocationAndCopyShouldSetCorrectFlagsAndSize()
        {
            var buffer = _stubConfiguration.ByAllocationAndCopy(new byte[1]).AsReadOnly();

            Assert.True(buffer.Flags.HasFlag(MemoryFlags.CopyHostPointer));
            Assert.True(buffer.Flags.HasFlag(MemoryFlags.AllocHostPointer));
            Assert.Equal((ulong)1, buffer.Size);
        }

        [Fact]
        public void ByHostMemoryShouldSetCorrectFlagsAndSize()
        {
            var buffer = _stubConfiguration.ByHostMemory(new byte[1]).AsReadOnly();
            
            Assert.True(buffer.Flags.HasFlag(MemoryFlags.UseHostPointer));
            Assert.Equal((ulong)1, buffer.Size);
        }

        [Fact]
        public void AsReadOnlyShouldSetCorrectFlags()
        {

        }
    }
}
