using CL.Core.API;
using CL.Core.Fakes;
using CL.Core.Model;
using CL.Core.Tests.Unit.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit
{
    public class InfoHelperTests : UnitTestBase
    {
        private readonly FakeEntityApi _fakeEntityApi;
        private readonly InfoHelper<FakeEntityInformationParameter> _target;

        public InfoHelperTests()
        {
            _fakeEntityApi = new FakeEntityApi();

            _target = new InfoHelper<FakeEntityInformationParameter>(_fakeEntityApi.Entity, _fakeEntityApi.GetFakeEntityInfo);
        }

        [Fact]
        public void CtorShouldThrowExceptionIfEntityNull()
        {
            Assert.Throws<ArgumentNullException>(() => new InfoHelper<FakeEntityInformationParameter>(null, _fakeEntityApi.GetFakeEntityInfo));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfInfoFuncNull()
        {
            Assert.Throws<ArgumentNullException>(() => new InfoHelper<FakeEntityInformationParameter>(_fakeEntityApi.Entity, null));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfEncodingNull()
        {
            Assert.Throws<ArgumentNullException>(() => new InfoHelper<FakeEntityInformationParameter>(_fakeEntityApi.Entity, _fakeEntityApi.GetFakeEntityInfo, null));
        }

        [Fact]
        public void GetValueShouldThrowExceptionIfInfoFuncReturnsErrorOnFirstCall()
        {
            _fakeEntityApi.FirstCallResult = OpenClErrorCode.InvalidArgumentIndex;

            Assert.Throws<ClCoreException>(() => _target.GetValue<uint>(FakeEntityInformationParameter.FancyNumber));
        }

        [Fact]
        public void GetValueShouldThrowExceptionIfInfoFuncReturnsErrorOnSecondCall()
        {
            _fakeEntityApi.SecondCallResult = OpenClErrorCode.InvalidArgumentSize;

            Assert.Throws<ClCoreException>(() => _target.GetValue<uint>(FakeEntityInformationParameter.FancyNumber));
        }

        [Fact]
        public void GetValueShouldReturnValueIdentifiedByParameter()
        {
            var result = _target.GetValue<uint>(FakeEntityInformationParameter.FancyNumber);

            Assert.Equal(_fakeEntityApi.Entity.FancyNumber, result);
        }

        [Fact]
        public void GetValuesShouldThrowExceptionIfInfoFuncReturnsErrorOnFirstCall()
        {
            _fakeEntityApi.FirstCallResult = OpenClErrorCode.InvalidArgumentIndex;

            Assert.Throws<ClCoreException>(() => _target.GetValues<int>(FakeEntityInformationParameter.FancyNumbers));
        }

        [Fact]
        public void GetValuesShouldThrowExceptionIfInfoFuncReturnsErrorOnSecondCall()
        {
            _fakeEntityApi.SecondCallResult = OpenClErrorCode.InvalidArgumentSize;

            Assert.Throws<ClCoreException>(() => _target.GetValues<int>(FakeEntityInformationParameter.FancyNumbers));
        }

        [Fact]
        public void GetValuesShouldReturnValuesIdentifiedByParameter()
        {
            var result = _target.GetValues<int>(FakeEntityInformationParameter.FancyNumbers);

            Assert.Equal(_fakeEntityApi.Entity.FancyNumbers, result);
        }

        [Fact]
        public void GetStringValueShouldThrowExceptionIfInfoFuncReturnsErrorOnFirstCall()
        {
            _fakeEntityApi.FirstCallResult = OpenClErrorCode.InvalidArgumentIndex;

            Assert.Throws<ClCoreException>(() => _target.GetStringValue(FakeEntityInformationParameter.FancyName));
        }

        [Fact]
        public void GetStringValueShouldThrowExceptionIfInfoFuncReturnsErrorOnSecondCall()
        {
            _fakeEntityApi.SecondCallResult = OpenClErrorCode.InvalidArgumentSize;

            Assert.Throws<ClCoreException>(() => _target.GetStringValue(FakeEntityInformationParameter.FancyName));
        }

        [Fact]
        public void GetStringValueShouldReturnStringIdentifiedByParameter()
        {
            var result = _target.GetStringValue(FakeEntityInformationParameter.FancyName);

            Assert.Equal(_fakeEntityApi.Entity.FancyName, result);
        }

        private class FakeEntity : IHasId, IInfoProvider<FakeEntityInformationParameter>
        {
            public IntPtr Id => new IntPtr(1);
            public uint FancyNumber { get; }
            public int[] FancyNumbers { get; }
            public string FancyName { get; }
            public InfoLookup<FakeEntityInformationParameter> Infos { get; }

            public FakeEntity()
            {
                FancyNumber = 10;
                FancyNumbers = new[] { 1, 2, 3 };
                FancyName = "Leopold";

                Infos = new InfoLookup<FakeEntityInformationParameter>
                {
                    {FakeEntityInformationParameter.FancyNumber, FancyNumber },
                    {FakeEntityInformationParameter.FancyNumbers, FancyNumbers},
                    {FakeEntityInformationParameter.FancyName, FancyName}
                };
            }
        }

        private enum FakeEntityInformationParameter
        {
            FancyNumber,
            FancyNumbers,
            FancyName
        }

        private class FakeEntityApi
        {
            public FakeEntity Entity { get; }
            public OpenClErrorCode? FirstCallResult { private get; set; }
            public OpenClErrorCode? SecondCallResult { private get; set; }

            public FakeEntityApi()
            {
                Entity = new FakeEntity();
            }

            public OpenClErrorCode GetFakeEntityInfo(IntPtr handle, FakeEntityInformationParameter parameterName, uint valueSize, IntPtr paramValue, out uint parameterValueSizeReturn)
            {
                return valueSize == 0
                            ? Entity.GetInfo(parameterName, valueSize, paramValue, out parameterValueSizeReturn, FirstCallResult ?? OpenClErrorCode.Success)
                            : Entity.GetInfo(parameterName, valueSize, paramValue, out parameterValueSizeReturn, SecondCallResult ?? OpenClErrorCode.Success);
            }
        }
    }
}
