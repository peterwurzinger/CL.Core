using CL.Core.API;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CL.Core.Model
{
    public class Kernel : IHasId, IDisposable
    {
        public IntPtr Id { get; }
        public Program Program { get; }
        public string Name { get; }
        public uint NumberOfArguments { get; }

        private readonly IOpenClApi _api;

        private bool _disposed;

        internal Kernel(IOpenClApi api, Program program, string name)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            Program = program ?? throw new ArgumentNullException(nameof(program));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            Name = name;

            var id = _api.KernelApi.clCreateKernel(program.Id, name, out var error);
            error.ThrowOnError();

            Id = id;

            var infoHelper = new InfoHelper<KernelInfoParameter>(this, _api.KernelApi.clGetKernelInfo);
            NumberOfArguments = infoHelper.GetValue<uint>(KernelInfoParameter.NumberOfArguments);
        }

        public void SetArgument<TArg>(int argIndex, TArg value)
            where TArg : unmanaged
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            ValidateArgIndex(argIndex);
            var handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            var error = _api.KernelApi.clSetKernelArg(Id, (uint)argIndex, (ulong)Marshal.SizeOf<TArg>(), handle.AddrOfPinnedObject());
            handle.Free();
            error.ThrowOnError();
        }

        public void SetMemoryArgument(int argIndex, MemoryObject memoryObject)
        {
            SetArgument(argIndex, memoryObject.Id);
        }

        private void ValidateArgIndex(int argIndex)
        {
            if (argIndex < 0 && argIndex >= NumberOfArguments)
                throw new ArgumentOutOfRangeException(nameof(argIndex), argIndex, $"The max index for {nameof(argIndex)} is {NumberOfArguments - 1}.");
        }

        public Event Execute(CommandQueue commandQueue, params GlobalWorkParameters[] workSize)
        {
            var globalWorkSize = workSize.Select(w => w.GlobalWorkSize).ToArray();
            var globalWorkOffset = workSize.Select(w => w.GlobalWorkOffset).ToArray();

            return Execute(commandQueue, (uint)workSize.Length, globalWorkSize, globalWorkOffset);
        }

        public Event Execute(CommandQueue commandQueue, params GlobalLocalWorkParameters[] globalLocalWorkSize)
        {
            var globalWorkSize = globalLocalWorkSize.Select(w => w.GlobalWorkSize).ToArray();
            var globalWorkOffset = globalLocalWorkSize.Select(w => w.GlobalWorkOffset).ToArray();
            var localWorkSize = globalLocalWorkSize.Select(w => w.LocalWorkSize).ToArray();

            return Execute(commandQueue, (uint)globalLocalWorkSize.Length, globalWorkSize, globalWorkOffset, localWorkSize);
        }

        private unsafe Event Execute(CommandQueue commandQueue, uint workDimensions, uint[] globalWorkSize, uint[] globalWorkOffset, uint[] localWorkSize = null)
        {
            if (workDimensions == 0)
                throw new ArgumentOutOfRangeException(nameof(workDimensions), workDimensions, "Work dimensions must be greater than 0.");

            if (workDimensions > commandQueue.Device.MaxWorkItemDimensions)
                throw new ArgumentOutOfRangeException(nameof(workDimensions), workDimensions, $"Exceeding devices maximum work item dimensions of {commandQueue.Device.MaxWorkItemDimensions}");

            var globalWorkSizeArray = globalWorkSize.ToArray();

            var dimensionsWorkSize = globalWorkSizeArray.Zip(globalWorkOffset.ToArray(), (size, offset) => size + offset).ToArray();
            var maxAddress = Math.Pow(2, commandQueue.Device.AddressBits) - 1;

            var dimensionWorkSizeExceptions = new List<Exception>();
            for (var i = 0; i < dimensionsWorkSize.Length; i++)
            {
                if (dimensionsWorkSize[i] > maxAddress)
                    dimensionWorkSizeExceptions.Add(new ArgumentException($"Work size + offset of dimension {i} must not exceed devices address width of {maxAddress}"));
            }
            if (dimensionWorkSizeExceptions.Any())
                throw new AggregateException(dimensionWorkSizeExceptions);

            var handles = new List<MemoryHandle>();

            var globalWorkSizeHandle = globalWorkSize.AsMemory().Pin();
            handles.Add(globalWorkSizeHandle);
            var globalWorkSizePtr = new UIntPtr(globalWorkSizeHandle.Pointer);

            var globalWorkOffsetHandle = globalWorkOffset.AsMemory().Pin();
            handles.Add(globalWorkOffsetHandle);
            var globalWorkOffsetPtr = new UIntPtr(globalWorkOffsetHandle.Pointer);

            var localWorkSizePtr = UIntPtr.Zero;
            if (localWorkSize != null)
            {
                var localWorkSizeArray = localWorkSize.ToArray();
                var workItemsPerWorkGroup = localWorkSizeArray.Aggregate(1L, (workSize, dimWorkSize) => workSize * dimWorkSize);
                if (workItemsPerWorkGroup > commandQueue.Device.MaxWorkGroupSize)
                    throw new ArgumentException($"Total number of work-items in a work-group must not exceed devices maximum work-group size of {commandQueue.Device.MaxWorkGroupSize}.");

                var workItemSizesExceptions = new List<Exception>();
                for (var i = 0; i < dimensionsWorkSize.Length; i++)
                {
                    if (localWorkSizeArray[i] > commandQueue.Device.MaxWorkItemSizes[i])
                        workItemSizesExceptions.Add(new ArgumentException($"Number of work-items in a work-group in dimension {i} must not exceed devices maximum of {commandQueue.Device.MaxWorkItemSizes[i]}"));
                }
                if (workItemSizesExceptions.Any())
                    throw new AggregateException(workItemSizesExceptions);

                var localWorkSizeHandle = localWorkSize.AsMemory().Pin();
                handles.Add(localWorkSizeHandle);
                localWorkSizePtr = new UIntPtr(localWorkSizeHandle.Pointer);
            }

            var error = _api.KernelApi.clEnqueueNDRangeKernel(commandQueue.Id, Id, workDimensions, globalWorkOffsetPtr,
                globalWorkSizePtr, localWorkSizePtr, 0, IntPtr.Zero, out var evt);
            handles.ForEach(m => m.Dispose());
            error.ThrowOnError();

            return new Event(_api, evt);
        }

        private void ReleaseUnmanagedResources()
        {
            if (Id != IntPtr.Zero)
                _api?.KernelApi.clReleaseKernel(Id);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            ReleaseUnmanagedResources();
            if (disposing)
            {
                //TODO: Managed resources to dispose?
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Kernel()
        {
            Dispose(false);
        }
    }
}
