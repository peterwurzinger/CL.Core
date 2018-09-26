using CL.Core.API;
using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class PlatformTests : UnitTestBase
    {

        [Fact]
        public void CtorShouldThrowExceptionIfOpenClApiNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Platform(IntPtr.Zero, null));
        }

        [Fact]
        public void CtorShouldSetPlatformId()
        {
            var platform = new Platform(new IntPtr(5), FakeOpenClApi);

            Assert.Equal(new IntPtr(5), platform.Id);
        }

        [Fact]
        public void CtorShouldCreateDevices()
        {
            FakeOpenClApi.FakeDeviceApi.clGetDeviceIDsNumDevices = 1;

            var platform = new Platform(new IntPtr(5), FakeOpenClApi);

            Assert.NotNull(platform.Devices);
            Assert.Single(platform.Devices);
        }

        [Fact]
        public void EqualsShouldReturnFalseIfOtherIsNull()
        {
            var platform = new Platform(new IntPtr(5), FakeOpenClApi);

            Assert.False(platform.Equals(null));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfOtherIsSameReference()
        {
            var platform = new Platform(new IntPtr(5), FakeOpenClApi);
            var other = platform;

            Assert.True(platform.Equals(other));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfOtherHasSameId()
        {
            var platform = new Platform(new IntPtr(5), FakeOpenClApi);
            var other = new Platform(new IntPtr(5), FakeOpenClApi);

            Assert.True(platform.Equals(other));
        }

        [Fact]
        public void EqualsShouldReturnFalseIfObjIsNull()
        {
            var platform = new Platform(new IntPtr(5), FakeOpenClApi);

            Assert.False(platform.Equals((object)null));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfObjIsSameReference()
        {
            var platform = new Platform(new IntPtr(5), FakeOpenClApi);
            object obj = platform;

            Assert.True(platform.Equals(obj));
        }

        [Fact]
        public void EqualsShouldReturnTrueIfObjIsOfSameTypeAndHasSameId()
        {
            var platform = new Platform(new IntPtr(5), FakeOpenClApi);
            object obj = new Platform(new IntPtr(5), FakeOpenClApi);

            Assert.True(platform.Equals(obj));
        }

        [Fact]
        public void EqualsShouldReturnFalseIfObjIsOfDifferentType()
        {
            var platform = new Platform(new IntPtr(5), FakeOpenClApi);
            object obj = new SuspiciousPlatform(new IntPtr(5), FakeOpenClApi);

            Assert.False(platform.Equals(obj));
        }

        [Fact]
        public void GetHashCodeShouldReturnHashCodeOfId()
        {
            var platform = new Platform(new IntPtr(5), FakeOpenClApi);

            Assert.Equal(platform.Id.GetHashCode(), platform.GetHashCode());
        }

        private class SuspiciousPlatform : Platform
        {
            internal SuspiciousPlatform(IntPtr platformId, IOpenClApi openClApi) : base(platformId, openClApi)
            {
            }
        }
    }
}
