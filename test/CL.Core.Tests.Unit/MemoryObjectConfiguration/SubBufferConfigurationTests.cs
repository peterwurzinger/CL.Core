using CL.Core.API;
using CL.Core.MemoryObjectConfiguration;
using CL.Core.Model;
using CL.Core.Tests.Unit.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.MemoryObjectConfiguration
{
    public class SubBufferConfigurationTests : UnitTestBase
    {
        private readonly SubBufferStubConfiguration<byte> _configurationStub;
        private readonly Context _context;


        public SubBufferConfigurationTests()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var devices = new[] { new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi) };
            _context = new Context(FakeOpenClApi, devices);

            var buffer = _context.CreateBuffer<byte>().ByAllocation(10).AsReadOnly();
            _configurationStub = buffer.CreateSubBuffer();
        }

        [Fact]
        public void WithSizeShouldSetCorrectSize()
        {
            var subBuffer = _configurationStub.WithSize(1).AsReadOnly();

            Assert.Equal((ulong)1, subBuffer.Size);
        }

        [Fact]
        public void WithSizeShouldSetCorrectOffsetAndSize()
        {
            var subBuffer = _configurationStub.WithSize(1, 1).AsReadOnly();

            Assert.Equal((ulong)1, subBuffer.Size);
            //TODO: Can't read offset :-(
        }

        [Fact]
        public void AsReadOnlyShouldPassCorrectFlag()
        {
            var subBuffer = _configurationStub.WithSize(1).AsReadOnly();
            Assert.True(FakeOpenClApi.FakeBufferApi.FakeMemoryObjects[subBuffer.Id].Flags.HasFlag(MemoryFlags.ReadOnly));
        }

        [Fact]
        public void AsWriteOnlyShouldPassCorrectFlag()
        {
            var subBuffer = _configurationStub.WithSize(1).AsWriteOnly();
            Assert.True(FakeOpenClApi.FakeBufferApi.FakeMemoryObjects[subBuffer.Id].Flags.HasFlag(MemoryFlags.WriteOnly));
        }

        [Fact]
        public void AsReadWriteShouldPassCorrectFlag()
        {
            var subBuffer = _configurationStub.WithSize(1).AsReadWrite();
            Assert.True(FakeOpenClApi.FakeBufferApi.FakeMemoryObjects[subBuffer.Id].Flags.HasFlag(MemoryFlags.ReadWrite));
        }

        [Fact]
        public void ConfigurationStubShouldThrowExceptionIfParentNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SubBufferStubConfiguration<byte>(FakeOpenClApi, _context, b => { }, null));
        }
    }
}
