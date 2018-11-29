using CL.Core.Model;
using CL.Core.Tests.Unit.Model;
using System;
using CL.Core.API;
using CL.Core.MemoryObjectConfiguration;
using Xunit;

namespace CL.Core.Tests.Unit
{
    public class BufferConfigurationTests : UnitTestBase
    {
        private readonly BufferStubConfiguration<byte> _stubConfiguration;

        public BufferConfigurationTests()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var devices = new[] { new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi) };
            var context = new Context(FakeOpenClApi, devices);

            _stubConfiguration = context.CreateBuffer<byte>();
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
            var buffer = _stubConfiguration.ByAllocation(1).AsReadOnly();

            Assert.True(buffer.Flags.HasFlag(MemoryFlags.ReadOnly));
        }

        [Fact]
        public void AsWriteOnlyShouldSetCorrectFlags()
        {
            var buffer = _stubConfiguration.ByAllocation(1).AsWriteOnly();

            Assert.True(buffer.Flags.HasFlag(MemoryFlags.WriteOnly));
        }

        [Fact]
        public void AsReadWriteShouldSetCorrectFlags()
        {
            var buffer = _stubConfiguration.ByAllocation(1).AsReadWrite();

            Assert.True(buffer.Flags.HasFlag(MemoryFlags.ReadWrite));
        }

        [Fact]
        public void ByAllocationShouldThrowExceptionIfCreateBufferReturnsError()
        {
            FakeOpenClApi.FakeBufferApi.clCreateBufferErrorCode = OpenClErrorCode.InvalidBufferSize;
            Assert.Throws<ClCoreException>(() => _stubConfiguration.ByAllocation(1).AsReadWrite());
        }

        [Fact]
        public void ByHostMemoryShouldThrowExceptionIfCreateBufferReturnsError()
        {
            FakeOpenClApi.FakeBufferApi.clCreateBufferErrorCode = OpenClErrorCode.InvalidBufferSize;
            Assert.Throws<ClCoreException>(() => _stubConfiguration.ByHostMemory(new byte[0]).AsReadWrite());
        }
    }
}
