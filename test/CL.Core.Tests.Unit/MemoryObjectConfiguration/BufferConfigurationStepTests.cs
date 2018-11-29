using System;
using System.Linq;
using CL.Core.Fakes;
using CL.Core.Model;
using Xunit;

namespace CL.Core.Tests.Unit.MemoryObjectConfiguration
{
    public class BufferConfigurationStepTests
    {
        private readonly Context _context;
        private readonly FakeOpenClApi _api;

        public BufferConfigurationStepTests()
        {
            _api = new FakeOpenClApi();
            var platformFactory = new PlatformFactory(_api);

            _api.FakePlatformApi.clGetPlatformIDsNumPlatforms = 1;
            _api.FakeDeviceApi.clGetDeviceIDsNumDevices = 1;
            var platform = platformFactory.GetPlatforms().First();
            _context = platform.CreateContext(platform.Devices);
        }

        [Fact]
        public void CtorShouldThrowExceptionIfApiNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FakeBufferConfigurationStep<byte>(null, _context, null));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfContextNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FakeBufferConfigurationStep<byte>(_api, null, null));
        }

        [Fact]
        public void CtorShouldSetProperties()
        {
            void Callback(Buffer<byte> b)
            {
            }

            var configStep = new FakeBufferConfigurationStep<byte>(_api, _context, Callback);

            Assert.Equal(_api, configStep.GetApi);
            Assert.Equal(_context, configStep.GetContext);
            Assert.Equal(Callback, configStep.GetBufferCreatedCallback);
        }
    }
}
