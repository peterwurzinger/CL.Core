using CL.Core.API;
using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class ProgramTests : UnitTestBase
    {
        private readonly Context _context;
        private readonly string[] _sources;

        public ProgramTests()
        {
            var platform = new Platform(new IntPtr(1), FakeOpenClApi);
            var device = new Device(platform, new IntPtr(1), FakeOpenClApi.DeviceApi);

            _context = new Context(FakeOpenClApi, new[] { device });
            _sources = new[] { "__kernel void Test() {}" };
        }

        [Fact]
        public void CtorShouldThrowExceptionIfApiIsNull()
        {
            Assert.Throws<ArgumentNullException>("api", () => new Program(null, _context, _sources));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfContextIsNull()
        {
            Assert.Throws<ArgumentNullException>("context", () => new Program(FakeOpenClApi, null, _sources));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfSourcesNull()
        {
            Assert.Throws<ArgumentNullException>("sources", () => new Program(FakeOpenClApi, _context, null as string[]));
        }

        [Fact]
        public void CtorShouldCreateProgram()
        {
            var program = new Program(FakeOpenClApi, _context, _sources);

            Assert.NotNull(FakeOpenClApi.FakeProgramApi.FakePrograms[program.Id]);
        }

        [Fact]
        public void CtorShouldThrowOnError()
        {
            FakeOpenClApi.FakeProgramApi.clCreateProgramWithSourceErrorCode = OpenClErrorCode.InvalidValue;

            Assert.Throws<ClCoreException>(() => new Program(FakeOpenClApi, _context, _sources));
        }

        [Fact]
        public void CtorShouldSetId()
        {
            FakeOpenClApi.FakeProgramApi.clCreateProgramWithSourceReturn = new IntPtr(2);

            var program = new Program(FakeOpenClApi, _context, _sources);

            Assert.Equal(new IntPtr(2), program.Id);
        }

        [Fact]
        public void CtorShouldInitBuildsDirectory()
        {
            var program = new Program(FakeOpenClApi, _context, _sources);

            Assert.NotNull(program.Builds);
        }

        [Fact]
        public void CtorShouldInitAttachedKernelList()
        {
            var program = new Program(FakeOpenClApi, _context, _sources);

            Assert.NotNull(program.Kernels);
            Assert.Empty(program.Kernels);
        }

        [Fact]
        public void GetContextShouldReturnContextProvidedInConstructor()
        {
            var program = new Program(FakeOpenClApi, _context, _sources);

            Assert.Equal(_context, program.Context);
        }

        [Fact]
        public void DisposeShouldNotThrowExceptionIfDisposedMultipleTimes()
        {
            var program = new Program(FakeOpenClApi, _context, _sources);

            program.Dispose();
            program.Dispose();
        }

        [Fact]
        public void DisposeShouldReleaseProgram()
        {
            var program = new Program(FakeOpenClApi, _context, _sources);

            program.Dispose();

            Assert.True(FakeOpenClApi.FakeProgramApi.FakePrograms[program.Id].Released);
        }

        [Fact]
        public void BuildShouldThrowExceptionIfDisposed()
        {
            var program = new Program(FakeOpenClApi, _context, _sources);

            program.Dispose();

            Assert.Throws<ObjectDisposedException>(() => program.Build(_context.Devices));
        }

        [Fact]
        public void BuildShouldThrowExceptionIfDevicesNull()
        {
            var program = new Program(FakeOpenClApi, _context, _sources);

            Assert.Throws<ArgumentNullException>("devices", () => program.Build(null));
        }

        [Fact]
        public void CreateKernelShouldThrowExceptionIfDisposed()
        {
            var program = new Program(FakeOpenClApi, _context, _sources);

            program.Dispose();

            Assert.Throws<ObjectDisposedException>(() => program.CreateKernel("Testkernel"));
        }

        [Fact]
        public void CreateKernelShouldCreateKernel()
        {
            var program = new Program(FakeOpenClApi, _context, _sources);

            var kernel = program.CreateKernel("Testkernel");

            Assert.NotNull(kernel);
        }
    }
}
