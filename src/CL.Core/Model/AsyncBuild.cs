using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CL.Core.Model
{
    internal class AsyncBuild
    {
        public Program Program { get; }

        private delegate void AsyncBuildCallbackDelegate(IntPtr program, IntPtr userData);
        private OpenClErrorCode BuildInfoFuncCurried(IntPtr deviceHandle, ProgramBuildInfoParameter name, uint size, IntPtr paramValue, out uint paramValueSizeReturned)
            => _programApi.clGetProgramBuildInfo(Program.Id, deviceHandle, name, size, paramValue, out paramValueSizeReturned);

        private readonly TaskCompletionSource<Dictionary<Device, BuildInfo>> _taskCompletionSource;
        private readonly IProgramApi _programApi;
        private readonly IReadOnlyCollection<Device> _devices;
        private readonly InfoHelper<ProgramInfoParameter> _programInfoHelper;

        private GCHandle _delegateHandle;

        internal AsyncBuild(IProgramApi programApi, Program program, IReadOnlyCollection<Device> devices)
        {
            _programApi = programApi ?? throw new ArgumentNullException(nameof(programApi));
            Program = program ?? throw new ArgumentNullException(nameof(program));
            _devices = devices ?? throw new ArgumentNullException(nameof(devices));

            _taskCompletionSource = new TaskCompletionSource<Dictionary<Device, BuildInfo>>();
            _programInfoHelper = new InfoHelper<ProgramInfoParameter>(Program, _programApi.clGetProgramInfo);

            var callbackDelegate = (AsyncBuildCallbackDelegate)AsyncBuildCallback;
            _delegateHandle = GCHandle.Alloc(callbackDelegate);
            var fp = Marshal.GetFunctionPointerForDelegate(callbackDelegate);

            //Ignoring errors returned by this call intentionally, since they will get handled on callback
            programApi.clBuildProgram(program.Id, (uint)devices.Count, devices.Select(d => d.Id).ToArray(), string.Empty, fp, IntPtr.Zero);

        }

        public Task<Dictionary<Device, BuildInfo>> WaitAsync()
        {
            return _taskCompletionSource.Task;
        }

        private void AsyncBuildCallback(IntPtr program, IntPtr userData)
        {
            _delegateHandle.Free();

            var buildErrors = new List<ProgramBuildException>();
            var builds = new Dictionary<Device, BuildInfo>();

            var availableBinaries = GetBinaries();

            foreach (var device in _devices)
            {
                var buildInfoHelper = new InfoHelper<ProgramBuildInfoParameter>(device, BuildInfoFuncCurried);
                var encoding = Encoding.Default;
                var status = buildInfoHelper.GetValue<BuildStatus>(ProgramBuildInfoParameter.Status);
                var log = buildInfoHelper.GetStringValue(ProgramBuildInfoParameter.Log, encoding);
                var options = buildInfoHelper.GetStringValue(ProgramBuildInfoParameter.Options, encoding);

                if (status == BuildStatus.Error)
                    buildErrors.Add(new ProgramBuildException(Program, device, log));

                builds[device] = new BuildInfo(status, log, options, availableBinaries[device.Id]);
            }

            if (buildErrors.Any())
                _taskCompletionSource.SetException(buildErrors);
            else
                _taskCompletionSource.SetResult(builds);
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
                error = _programApi.clGetProgramInfo(Program.Id, ProgramInfoParameter.Binaries, (uint)(sizeof(IntPtr) * binarySizes.Length), new IntPtr(ptr), out _);
            }

            //Unpin the Memory handles
            foreach (var handle in handles)
                handle.Dispose();

            error.ThrowOnError();

            return sortedDevices.Zip(memorySegments,
                (device, memory) => new KeyValuePair<IntPtr, ReadOnlyMemory<byte>>(device, memory)
            ).ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
