using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CL.Core.Model
{
    internal class AsyncBuild
    {
        private delegate void AsyncBuildCallbackDelegate(IntPtr program, IntPtr userData);
        private OpenClErrorCode BuildInfoFuncCurried(IntPtr deviceHandle, ProgramBuildInfoParameter name, uint size, IntPtr paramValue, out uint paramValueSizeReturned)
            => _programApi.clGetProgramBuildInfo(_program.Id, deviceHandle, name, size, paramValue, out paramValueSizeReturned);

        private readonly Program _program;
        private readonly TaskCompletionSource<Dictionary<Device, BuildInfo>> _taskCompletionSource;
        private readonly IProgramApi _programApi;
        private readonly IReadOnlyCollection<Device> _devices;
        private readonly InfoHelper<ProgramInfoParameter> _programInfoHelper;

        private GCHandle _delegateHandle;

        internal AsyncBuild(IProgramApi programApi, Program program, IReadOnlyCollection<Device> devices, string[] options)
        {
            _programApi = programApi ?? throw new ArgumentNullException(nameof(programApi));
            _program = program ?? throw new ArgumentNullException(nameof(program));
            _devices = devices ?? throw new ArgumentNullException(nameof(devices));
            if (options == null) throw new ArgumentNullException(nameof(options));

            _taskCompletionSource = new TaskCompletionSource<Dictionary<Device, BuildInfo>>();
            _programInfoHelper = new InfoHelper<ProgramInfoParameter>(_program, _programApi.clGetProgramInfo);

            var callbackDelegate = (AsyncBuildCallbackDelegate)AsyncBuildCallback;
            _delegateHandle = GCHandle.Alloc(callbackDelegate);
            var fp = Marshal.GetFunctionPointerForDelegate(callbackDelegate);

            var optionsString = string.Join(" ", options);

            var error = programApi.clBuildProgram(program.Id, (uint)devices.Count, devices.Select(d => d.Id).ToArray(), optionsString, fp, IntPtr.Zero);
            if (error != OpenClErrorCode.Success)
                _taskCompletionSource.SetException(new ClCoreException(error));
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
                var status = buildInfoHelper.GetValue<BuildStatus>(ProgramBuildInfoParameter.Status);
                var log = buildInfoHelper.GetStringValue(ProgramBuildInfoParameter.Log);
                var options = buildInfoHelper.GetStringValue(ProgramBuildInfoParameter.Options);

                if (status == BuildStatus.Error)
                    buildErrors.Add(new ProgramBuildException(_program, device, log));

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
                error = _programApi.clGetProgramInfo(_program.Id, ProgramInfoParameter.Binaries, (uint)(sizeof(IntPtr) * binarySizes.Length), new IntPtr(ptr), out _);
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
