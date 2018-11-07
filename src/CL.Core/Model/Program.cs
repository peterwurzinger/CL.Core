using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CL.Core.Model
{
    public class Program : IHasId, IDisposable
    {
        private readonly IOpenClApi _api;
        private bool _disposed;

        public IntPtr Id { get; }
        public Context Context { get; }
        public IReadOnlyCollection<Kernel> Kernels => _attachedKernels;
        public IReadOnlyDictionary<Device, BuildInfo> Builds => _builds;

        private Dictionary<Device, BuildInfo> _builds;
        private readonly List<Kernel> _attachedKernels;

        internal unsafe Program(IOpenClApi api, Context context, IReadOnlyDictionary<Device, ReadOnlyMemory<byte>> deviceBinaries)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            if (deviceBinaries == null) throw new ArgumentNullException(nameof(deviceBinaries));

            var handles = deviceBinaries.Values.Select(f => f.Pin()).ToArray();

            var binaryStatus = new OpenClErrorCode[deviceBinaries.Count];
            var id = api.ProgramApi.clCreateProgramWithBinary(context.Id, (uint)deviceBinaries.Count,
                deviceBinaries.Select(bin => bin.Key.Id).ToArray(),
                deviceBinaries.Select(bin => (uint)bin.Value.Length).ToArray(),
                handles.Select(h => new IntPtr(h.Pointer)).ToArray(), binaryStatus, out var errorCode);

            foreach (var hdl in handles)
                hdl.Dispose();

            errorCode.ThrowOnError();
            Id = id;

            //TODO: Validate binaryStatus?

            _builds = new Dictionary<Device, BuildInfo>(deviceBinaries.Zip(binaryStatus, (binary, status) =>
                                                new KeyValuePair<Device, BuildInfo>(binary.Key, new BuildInfo(status == OpenClErrorCode.Success ? BuildStatus.Success : BuildStatus.Error, binary.Value))
                            ).ToDictionary(k => k.Key, v => v.Value));

            _attachedKernels = new List<Kernel>();
        }

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

            _builds = new Dictionary<Device, BuildInfo>();
            _attachedKernels = new List<Kernel>();
        }

        //TODO: Build overloads for every device associated with program

        public void Build(IReadOnlyCollection<Device> devices)
        {
            //TODO: Is there a need to implement Build(devices) synchronously?
            BuildAsync(devices).GetAwaiter().GetResult();
        }

        public Task BuildAsync(IReadOnlyCollection<Device> devices, params string[] options)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (devices == null)
                throw new ArgumentNullException(nameof(devices));

            _builds = null;
            var build = new AsyncBuild(_api.ProgramApi, this, devices, options ?? Array.Empty<string>());

            return build.WaitAsync().ContinueWith(t => { _builds = t.Result; }, TaskScheduler.Current);
        }

        public Kernel CreateKernel(string name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            var kernel = new Kernel(_api, this, name);
            _attachedKernels.Add(kernel);
            return kernel;
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
