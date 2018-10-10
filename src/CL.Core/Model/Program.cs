using CL.Core.API;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Core.Model
{
    public class Program : IHasId, IDisposable
    {
        private readonly IOpenClApi _api;
        private readonly InfoHelper<ProgramInfoParameter> _programInfoHelper;
        private bool _disposed;

        public IntPtr Id { get; }
        public Context Context { get; }

        private readonly ConcurrentDictionary<Device, BuildInfo> _builds;
        private readonly List<Kernel> _attachedKernels;

        public IReadOnlyCollection<Kernel> Kernels => _attachedKernels;
        public IReadOnlyDictionary<Device, BuildInfo> Builds => _builds;

        private OpenClErrorCode BuildInfoFuncCurried(IntPtr deviceHandle, ProgramBuildInfoParameter name, uint size, IntPtr paramValue, out uint paramValueSizeReturned)
            => _api.ProgramApi.clGetProgramBuildInfo(Id, deviceHandle, name, size, paramValue, out paramValueSizeReturned);

        internal Program(IOpenClApi api, Context context, string[] sources)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            Context = context ?? throw new ArgumentNullException(nameof(context));

            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            var id = api.ProgramApi.clCreateProgramWithSource(context.Id, (uint)sources.Length, sources,
                sources.Select(source => (uint)source.Length).ToArray(), out var error);
            error.ThrowOnError();

            Id = id;
            _programInfoHelper = new InfoHelper<ProgramInfoParameter>(this, _api.ProgramApi.clGetProgramInfo);

            _builds = new ConcurrentDictionary<Device, BuildInfo>();
            _attachedKernels = new List<Kernel>();
        }

        //TODO: Build overloads for every device associated with program

        public void Build(IReadOnlyCollection<Device> devices)
        {
            //TODO: Is there a need to implement Build(devices) synchronously?
            BuildAsync(devices).GetAwaiter().GetResult();
        }

        //TODO: Compilation options & optimizations
        public Task BuildAsync(IReadOnlyCollection<Device> devices)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (devices == null)
                throw new ArgumentNullException(nameof(devices));

            var build = new AsyncBuild(_api.ProgramApi, this, devices);

            //TODO: Rethink this. UpdateBuildInfos(...) throwing exception is quite unintuitive

            return build.BuildTask.ContinueWith(t =>
            {
                build.Dispose();
                UpdateBuildInfos(devices);
            }, TaskScheduler.Current);
        }

        public Kernel CreateKernel(string name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            var kernel = new Kernel(_api, this, name);
            _attachedKernels.Add(kernel);
            return kernel;
        }

        private void UpdateBuildInfos(IEnumerable<Device> devices)
        {
            //TODO: lock? Concurrent builds would lead to race conditions

            //TODO: Why clear? Override could also be enough while maintaining builds from before
            _builds.Clear();

            var buildErrors = new List<ProgramBuildException>();

            var availableBinaries = GetBinaries();

            foreach (var device in devices)
            {
                var buildInfoHelper = new InfoHelper<ProgramBuildInfoParameter>(device, BuildInfoFuncCurried);
                var encoding = Encoding.Default;
                var status = buildInfoHelper.GetValue<BuildStatus>(ProgramBuildInfoParameter.Status);
                var log = buildInfoHelper.GetStringValue(ProgramBuildInfoParameter.Log, encoding);
                var options = buildInfoHelper.GetStringValue(ProgramBuildInfoParameter.Options, encoding);

                if (status == BuildStatus.InProgress)
                    throw new InvalidOperationException($"Call to '{nameof(UpdateBuildInfos)}' while build is still in progress");

                if (status == BuildStatus.Error)
                    buildErrors.Add(new ProgramBuildException(this, device, log));


                if (!_disposed)
                    _builds[device] = new BuildInfo(status, log, options, availableBinaries[device.Id]);
            }

            if (buildErrors.Any())
                throw new AggregateException("Error while building program.", buildErrors);
        }

        private unsafe Dictionary<IntPtr, ReadOnlyMemory<byte>> GetBinaries()
        {
            var sortedDevices = _programInfoHelper.GetValues<IntPtr>(ProgramInfoParameter.Devices).ToArray();
            if (!sortedDevices.Any())
                return new Dictionary<IntPtr, ReadOnlyMemory<byte>>();

            var binarySizes = _programInfoHelper.GetValues<ulong>(ProgramInfoParameter.BinarySizes).ToArray();

            var memorySegments = binarySizes.Select(size => new ReadOnlyMemory<byte>(new byte[size])).ToArray();
            var handles = memorySegments.Select(mem => mem.Pin()).ToArray();

            OpenClErrorCode error;
            fixed (void* ptr = handles.Select(handle => new IntPtr(handle.Pointer)).ToArray())
            {
                error = _api.ProgramApi.clGetProgramInfo(Id, ProgramInfoParameter.Binaries, (uint)(sizeof(IntPtr) * binarySizes.Length), new IntPtr(ptr), out _);
            }

            //Unpin the Memory handles
            foreach (var handle in handles)
                handle.Dispose();

            error.ThrowOnError();

            return sortedDevices.Zip(memorySegments,
                                        (device, memory) => new KeyValuePair<IntPtr, ReadOnlyMemory<byte>>(device, memory)
                                    ).ToDictionary(k => k.Key, v => v.Value);
        }

        private void ReleaseUnmanagedResources()
        {
            if (Id != IntPtr.Zero)
                _api?.ProgramApi?.clReleaseProgram(Id);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            ReleaseUnmanagedResources();
            if (disposing)
            {
                _builds.Clear();
                foreach (var kernel in _attachedKernels)
                    kernel.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Program()
        {
            Dispose(false);
        }

    }
}
