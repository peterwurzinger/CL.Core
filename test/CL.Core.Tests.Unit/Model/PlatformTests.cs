using CL.Core.Fakes;
using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class PlatformTests
    {
        private readonly FakePlatformInfoInterop _fakePlatformInfoInterop;
        private readonly FakeDeviceInfoInterop _fakeDeviceInfoInterop;

        public PlatformTests()
        {
            _fakePlatformInfoInterop = new FakePlatformInfoInterop();
            _fakeDeviceInfoInterop = new FakeDeviceInfoInterop();
        }

        [Fact]
        public void CtorShouldThrowExceptionIfPlatformInfoServiceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Platform(IntPtr.Zero, null, _fakeDeviceInfoInterop));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfDeviceInfoServiceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Platform(IntPtr.Zero, _fakePlatformInfoInterop, null));
        }

        //TODO: To be continued

    }
}
