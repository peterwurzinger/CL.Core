using CL.Core.API;
using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class KernelTests : UnitTestBase
    {
        private readonly Kernel _target;

        public KernelTests()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.FakeDeviceApi);
            var context = new Context(FakeOpenClApi, new[] { device });
            var program = new Program(FakeOpenClApi, context, new[] { "TestSource" });

            _target = program.CreateKernel("TestKernel");
        }

        [Fact]
        public void CtorShouldThrowExceptionIfApiNull()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.FakeDeviceApi);
            var program = new Program(FakeOpenClApi, new Context(FakeOpenClApi, new[] { device }), new[] { "TestSource" });

            Assert.Throws<ArgumentNullException>(() => new Kernel(null, program, "TestKernel"));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfProgramNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Kernel(FakeOpenClApi, null, "TestKernel"));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfNameNullOrEmpty()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.FakeDeviceApi);
            var program = new Program(FakeOpenClApi, new Context(FakeOpenClApi, new[] { device }), new[] { "TestSource" });

            Assert.Throws<ArgumentException>(() => new Kernel(FakeOpenClApi, program, null));
            Assert.Throws<ArgumentException>(() => new Kernel(FakeOpenClApi, program, ""));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfCreateKernelReturnsError()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.FakeDeviceApi);
            var program = new Program(FakeOpenClApi, new Context(FakeOpenClApi, new[] { device }), new[] { "TestSource" });
            FakeOpenClApi.FakeKernelApi.clCreateKernelErrorCode = OpenClErrorCode.InvalidKernelName;

            Assert.Throws<ClCoreException>(() => new Kernel(FakeOpenClApi, program, "TestKernel"));
        }

        [Fact]
        public void CtorShouldSetProperties()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.FakeDeviceApi);
            var program = new Program(FakeOpenClApi, new Context(FakeOpenClApi, new[] { device }), new[] { "TestSource" });
            var id = new IntPtr(5);
            FakeOpenClApi.FakeKernelApi.clCreateKernelReturn = id;
            const string name = "TestKernel";

            var kernel = new Kernel(FakeOpenClApi, program, name);

            Assert.Equal(id, kernel.Id);
            Assert.Equal(program, kernel.Program);
            Assert.Equal(name, kernel.Name);
        }

        [Fact]
        public void SetArgumentShouldThrowExceptionIfDisposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _target.SetArgument(0, 15));
        }

        [Fact]
        public void SetArgumentShouldThrowExceptionIfArgIndexInvalid()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _target.SetArgument((int)_target.NumberOfArguments, 15));
            Assert.Throws<ArgumentOutOfRangeException>(() => _target.SetArgument(-1, 15));
        }

        [Fact]
        public void SetArgumentShouldThrowExceptionIfSetKernelArgReturnsError()
        {
            FakeOpenClApi.FakeKernelApi.clSetKernelArgReturn = OpenClErrorCode.InvalidArgumentValue;
            Assert.Throws<ClCoreException>(() => _target.SetArgument(0, 15));
        }

        [Fact]
        public void SetArgumentShouldSetArgumentForKernelArg()
        {
            _target.SetArgument(0, 15);
        }

        //TODO: To be continued
    }
}
