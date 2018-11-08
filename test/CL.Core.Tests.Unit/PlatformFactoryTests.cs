using CL.Core.API;
using CL.Core.Fakes;
using System;
using System.Linq;
using Xunit;

namespace CL.Core.Tests.Unit
{
    public class PlatformFactoryTests
    {

        [Fact]
        public void CtorShouldThrowExceptionIfApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PlatformFactory(null));
        }

        [Fact]
        public void GetPlatformsShouldThrowExceptionIfGetPlatformsReturnsErrorCode()
        {
            var api = new FakeOpenClApi();
            api.FakePlatformApi.clGetPlatformIDsResult = OpenClErrorCode.InvalidPlatform;
            var factory = new PlatformFactory(api);

            Assert.Throws<ClCoreException>(() => factory.GetPlatforms().ToArray());
        }

        [Fact]
        public void GetPlatformsShouldReturnEveryPlatform()
        {
            var api = new FakeOpenClApi();
            var factory = new PlatformFactory(api);
            api.FakePlatformApi.clGetPlatformIDsNumPlatforms = 2;

            var platforms = factory.GetPlatforms().ToArray();

            Assert.Equal(2, platforms.Length);
            Assert.Equal(new IntPtr(1), platforms[0].Id);
            Assert.Equal(new IntPtr(2), platforms[1].Id);
        }

    }
}
