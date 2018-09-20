using CL.Core.API;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        public IDictionary<Device, BuildInfo> Builds { get; }

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

            Builds = new ConcurrentDictionary<Device, BuildInfo>();
        }

        //TODO: Build overloads for every device associated with program

        public void Build(IReadOnlyCollection<Device> devices)
        {
            //TODO: Is there a need to implement Build(devices) synchronously?
            BuildAsync(devices).Wait();
        }

        //TODO: Compilation options & optimizations
        public Task BuildAsync(IReadOnlyCollection<Device> devices)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (devices == null)
                throw new ArgumentNullException(nameof(devices));

            var build = new AsyncBuild(_api.ProgramApi, this, devices);

            return build.BuildTask.ContinueWith(t =>
            {
                build.Dispose();
                UpdateBuildInfos(devices);
            });
        }

        private void UpdateBuildInfos(IEnumerable<Device> devices)
        {
            //TODO: lock? Concurrent builds would lead to race conditions

            Builds.Clear();

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


                Builds[device] = new BuildInfo(status, log, options, new Memory<byte>(availableBinaries[device.Id]));
            }

            if (buildErrors.Any())
                throw new AggregateException("Error while building program.", buildErrors);
        }

        private unsafe Dictionary<IntPtr, byte[]> GetBinaries()
        {
            var sortedDevices = _programInfoHelper.GetValues<IntPtr>(ProgramInfoParameter.Devices);
            var binarySizes = _programInfoHelper.GetValues<long>(ProgramInfoParameter.BinarySizes).ToArray();

            var memoryPointers = new IntPtr[binarySizes.Length];
            for (var i = 0; i < binarySizes.Length; i++)
                memoryPointers[i] = Marshal.AllocHGlobal((int)binarySizes[i]);

            fixed (void* handle = memoryPointers)
            {
                var error = _api.ProgramApi.clGetProgramInfo(Id, ProgramInfoParameter.Binaries, (uint)(sizeof(IntPtr) * binarySizes.Length), new IntPtr(handle), out _);
                error.ThrowOnError();
            }

            var binaries = new Dictionary<IntPtr, byte[]>();
            for (var i = 0; i < memoryPointers.Length; i++)
            {
                binaries[sortedDevices[i]] = new ReadOnlySpan<byte>(memoryPointers[i].ToPointer(), (int)binarySizes[i]).ToArray();
                Marshal.FreeHGlobal(memoryPointers[i]);
            }

            return binaries;
        }

        private void ReleaseUnmanagedResources()
        {
            if (Id != IntPtr.Zero)
                _api?.ProgramApi?.clReleaseProgram(Id);
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                //TODO: Release kernels
                Builds.Clear();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            _disposed = true;
        }

        ~Program()
        {
            Dispose(false);
        }
    }
}
