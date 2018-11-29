using CL.Core.API;
using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class BufferBaseTests : UnitTestBase
    {
        private readonly CommandQueue _commandQueue;
        private readonly Buffer<byte> _target;

        public BufferBaseTests()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(2), FakeOpenClApi.FakeDeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });
            _commandQueue = new CommandQueue(context, device, false, false, FakeOpenClApi.FakeCommandQueueApi);

            var hostMemory = new byte[10];
            _target = context.CreateBuffer<byte>().ByHostMemory(hostMemory).AsReadWrite();
        }

        [Fact]
        public void ReadShouldThrowExceptionIfDisposed()
        {
            _target.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _target.Read(_commandQueue));
        }

        [Fact]
        public void ReadShouldThrowExceptionIfCommandQueueNull()
        {
            Assert.Throws<ArgumentNullException>(() => _target.Read(null));
        }

        [Fact]
        public void ReadShouldThrowExceptionIfEnqueueReadBufferReturnsError()
        {
            FakeOpenClApi.FakeBufferApi.clEnqueueReadBufferResult = OpenClErrorCode.InvalidBufferSize;

            Assert.Throws<ClCoreException>(() => _target.Read(_commandQueue));
        }

        [Fact]
        public void ReadShouldReadFromOpenClBuffer()
        {
            var result = _target.Read(_commandQueue);

            Assert.Equal(_target.Size, (ulong)result.Length);
        }

    }
}
