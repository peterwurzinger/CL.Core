using CL.Core.API;
using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class DeviceTests : UnitTestBase
    {

        [Fact]
        public void CtorShouldThrowExceptionIfDeviceServiceIsNull()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            Assert.Throws<ArgumentNullException>("deviceApi", () => new Device(platform, new IntPtr(1), null));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfPlatformIsNull()
        {
            Assert.Throws<ArgumentNullException>("platform", () => new Device(null, new IntPtr(1), FakeOpenClApi.DeviceApi));
        }

        [Fact]
        public void CtorShouldSetDeviceId()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            Assert.Equal(new IntPtr(1), device.Id);
        }

        [Fact]
        public void CtorShouldNotThrowExceptionIfDisposedMultipleTimes()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            device.Dispose();
            device.Dispose();
        }

        [Fact]
        public void GetHashCodeShouldReturnHashCodeOfId()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            Assert.Equal(device.Id.GetHashCode(), device.GetHashCode());
        }

        [Fact]
        public void EqualsShouldReturnFalseIfOtherIsNull()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device1 = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            Assert.False(device1.Equals(null));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfOtherIsSameReference()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            Assert.True(device.Equals(device));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfDevicesHaveSameIds()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device1 = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var device2 = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            Assert.True(device1.Equals(device2));
        }

        [Fact]
        public void EqualsShouldReturnFalseIfObjIsNull()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            Assert.False(device.Equals((object)null));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfObjIsSameReference()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var obj = (object)device;

            Assert.True(device.Equals(obj));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfObjIsOfSameTypeAndHasSameId()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device1 = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            object device2 = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            Assert.True(device1.Equals(device2));
        }

        [Fact]
        public void EqualsShouldReturnFalseIfObjIsOfDifferentType()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device1 = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);
            var device2 = (object)new SuspiciousDevice(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            Assert.False(device1.Equals(device2));
        }

        private class SuspiciousDevice : Device
        {
            internal SuspiciousDevice(Platform platform, IntPtr deviceId, IDeviceApi deviceApi) : base(platform, deviceId, deviceApi)
            {
            }
        }
    }
}
