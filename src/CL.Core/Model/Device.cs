using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace CL.Core.Model
{
    public class Device : IHasId, IDisposable, IEquatable<Device>
    {
        private readonly IDeviceApi _deviceApi;
        private readonly InfoHelper<DeviceInfoParameter> _deviceInfoHelper;
        private bool _disposed;

        #region Properties

        /// <summary>
        /// ?
        /// </summary>
        public IntPtr Id { get; }

        public Platform Platform { get; }

        /// <summary>
        /// Device name string.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The OpenCL device type. Currently supported values are one of or a combination of: <see cref="DeviceType.CPU"/>, <see cref="DeviceType.GPU"/>, <see cref="DeviceType.Accelerator"/>, or <see cref="DeviceType.Default"/>.
        /// </summary>
        public DeviceType Type { get; }

        /// <summary>
        /// Vendor name string.
        /// </summary>
        public string Vendor { get; }

        /// <summary>
        /// A unique device vendor identifier. An example of a unique device identifier could be the PCIe ID.
        /// </summary>
        public uint VendorId { get; }

        /// <summary>
        /// Is true if the device is available and false if the device is not available.
        /// </summary>
        public bool Available => _deviceInfoHelper.GetValue<bool>(DeviceInfoParameter.DeviceAvailable);

        /// <summary>
        /// Maximum number of work-items that can be specified in each dimension of the work-group to clEnqueueNDRangeKernel. Returns n size_t entries, where n is the value returned by the query for CL_DEVICE_MAX_WORK_ITEM_DIMENSIONS.The minimum value is (1, 1, 1).
        /// </summary>
        public IReadOnlyList<ulong> MaxWorkItemSizes { get; }

        /// <summary>
        /// Maximum number of work-items in a work-group executing a kernel using the data parallel execution model. The minimum value is 1.
        /// </summary>
        public uint MaxWorkGroupSize { get; }

        /// <summary>
        /// Maximum dimensions that specify the global and local work-item IDs used by the data parallel execution model. The minimum value is 3.
        /// </summary>
        public uint MaxWorkItemDimensions { get; }

        /// <summary>
        /// The number of parallel compute cores on the OpenCL device. The minimum value is 1.
        /// </summary>
        public uint MaxComputeUnits { get; }

        /// <summary>
        /// Max number of arguments declared with the __constant qualifier in a kernel. The minimum value is 8.
        /// </summary>
        public uint MaxConstantArgs { get; }

        /// <summary>
        /// Size of global device memory in bytes.
        /// </summary>
        public ulong GlobalMemorySize { get; }

        /// <summary>
        /// Maximum configured clock frequency of the device in MHz.
        /// </summary>
        public uint MaxClockFrequency { get; }

        /// <summary>
        /// The default compute device address space size specified as an unsigned integer value in bits. Currently supported values are 32 or 64 bits.
        /// </summary>
        public uint AddressBits { get; }

        public uint PreferredVectorWidthChar { get; }
        public uint PreferredVectorWidthShort { get; }
        public uint PreferredVectorWidthInt { get; }
        public uint PreferredVectorWidthLong { get; }
        public uint PreferredVectorWidthFloat { get; }
        public uint PreferredVectorWidthDouble { get; }
        public uint NativeVectorWidthChar { get; }
        public uint NativeVectorWidthShort { get; }
        public uint NativeVectorWidthInt { get; }
        public uint NativeVectorWidthLong { get; }
        public uint NativeVectorWidthFloat { get; }
        public uint NativeVectorWidthDouble { get; }
        public uint NativeVectorWidthHalf { get; }

        public uint MaxReadImageArgs { get; }
        public uint MaxWriteImageArgs { get; }
        public ulong MaxMemAllocSize { get; }

        public ulong Image2DMaxWidth { get; }
        public ulong Image2DMaxHeight { get; }

        public ulong Image3DMaxWidth { get; }
        public ulong Image3DMaxHeight { get; }
        public ulong Image3DMaxDepth { get; }

        public bool ImageSupport { get; }
        public ulong MaxParameterSize { get; }
        public uint MaxSamplers { get; }
        public uint MemBaseAddressAlign { get; }
        public uint MinDataTypeAlignSize { get; }
        public FloatConfigurations SinglePrecisionFloatConfigurations { get; }
        public CacheType GlobalMemCacheType { get; }
        public uint GlobalMemCacheLineSize { get; }
        public ulong GlobalMemCacheSize { get; }
        public ulong MaxConstantBufferSize { get; }
        public LocalMemoryType LocalMemoryType { get; }
        public ulong LocalMemorySize { get; }
        public bool ErrorCorrectionSupport { get; }
        public ulong ProfilingTimerResolution { get; }
        // ReSharper disable once IdentifierTypo
        public bool EndianLittle { get; }
        public bool CompilerAvailable => _deviceInfoHelper.GetValue<bool>(DeviceInfoParameter.CompilerAvailable);
        public ExecutionCapabilities ExecutionCapabilities { get; }
        public CommandQueueProperties QueueProperties { get; }
        public string DriverVersion { get; }
        public FloatConfigurations? DoublePrecisionFloatConfigurations { get; }
        public FloatConfigurations? HalfPrecisionFloatConfigurations { get; }
        public uint PreferredVectorWidthHalf { get; }
        public bool HostUnifiedMemory { get; }
        public string OpenClCVersion { get; }

        #endregion

        internal Device(Platform platform, IntPtr deviceId, IDeviceApi deviceApi)
        {
            Platform = platform ?? throw new ArgumentNullException(nameof(platform));
            _deviceApi = deviceApi ?? throw new ArgumentNullException(nameof(deviceApi));

            Id = deviceId;
            _deviceInfoHelper = new InfoHelper<DeviceInfoParameter>(this, _deviceApi.clGetDeviceInfo);

            var encoding = Encoding.Default;
            Name = _deviceInfoHelper.GetStringValue(DeviceInfoParameter.Name, encoding);
            Type = _deviceInfoHelper.GetValue<DeviceType>(DeviceInfoParameter.Type);
            VendorId = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.VendorId);
            MaxComputeUnits = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.MaxComputeUnits);
            MaxWorkItemDimensions = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.MaxWorkItemDimensions);
            MaxWorkGroupSize = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.MaxWorkGroupSize);

            MaxWorkItemSizes = _deviceInfoHelper.GetValues<ulong>(DeviceInfoParameter.MaxWorkItemSizes).ToArray();

            //TODO: Get bit width of size_t from AddressBits - property
            //From enqueueNDRangeKernel: The sizeof(size_t) for a device can be determined using CL_DEVICE_ADDRESS_BITS

            MaxClockFrequency = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.MaxClockFrequency);
            GlobalMemorySize = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.GlobalMemSize);
            MaxConstantArgs = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.MaxConstantArgs);
            AddressBits = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.AddressBits);
            Vendor = _deviceInfoHelper.GetStringValue(DeviceInfoParameter.Vendor, encoding);

            PreferredVectorWidthChar = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.PreferredVectorWidthChar);
            PreferredVectorWidthDouble = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.PreferredVectorWidthDouble);
            PreferredVectorWidthFloat = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.PreferredVectorWidthFloat);
            PreferredVectorWidthHalf = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.PreferredVectorWidthHalf);
            PreferredVectorWidthInt = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.PreferredVectorWidthInt);
            PreferredVectorWidthLong = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.PreferredVectorWidthLong);
            PreferredVectorWidthShort = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.PreferredVectorWidthShort);

            NativeVectorWidthChar = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.NativeVectorWidthChar);
            NativeVectorWidthDouble = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.NativeVectorWidthDouble);
            NativeVectorWidthFloat = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.NativeVectorWidthFloat);
            NativeVectorWidthHalf = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.NativeVectorWidthHalf);
            NativeVectorWidthInt = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.NativeVectorWidthInt);
            NativeVectorWidthLong = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.NativeVectorWidthLong);
            NativeVectorWidthShort = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.NativeVectorWidthShort);

            Image2DMaxHeight = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.Image2DMaxHeight);
            Image2DMaxWidth = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.Image2DMaxWidth);
            Image3DMaxHeight = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.Image3DMaxHeight);
            Image3DMaxWidth = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.Image3DMaxWidth);
            Image3DMaxDepth = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.Image3DMaxDepth);

            MaxReadImageArgs = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.MaxReadImageArgs);
            MaxWriteImageArgs = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.MaxWriteImageArgs);

            ImageSupport = _deviceInfoHelper.GetValue<bool>(DeviceInfoParameter.ImageSupport);
            MaxSamplers = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.MaxSamplers);

            MaxParameterSize = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.MaxParameterSize);
            MemBaseAddressAlign = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.MemBaseAddrAlign);
            MinDataTypeAlignSize = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.MinDataTypeAlignSize);
            MaxMemAllocSize = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.MaxMemAllocSize);

            GlobalMemCacheType = _deviceInfoHelper.GetValue<CacheType>(DeviceInfoParameter.GlobalMemCacheType);
            GlobalMemCacheSize = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.GlobalMemCacheSize);
            GlobalMemCacheLineSize = _deviceInfoHelper.GetValue<uint>(DeviceInfoParameter.GlobalMemCachelineSize);
            MaxConstantBufferSize = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.MaxConstantBufferSize);
            LocalMemorySize = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.LocalMemSize);
            LocalMemoryType = _deviceInfoHelper.GetValue<LocalMemoryType>(DeviceInfoParameter.LocalMemType);

            ErrorCorrectionSupport = _deviceInfoHelper.GetValue<bool>(DeviceInfoParameter.ErrorCorrectionSupport);
            ProfilingTimerResolution = _deviceInfoHelper.GetValue<ulong>(DeviceInfoParameter.ProfilingTimerResolution);
            EndianLittle = _deviceInfoHelper.GetValue<bool>(DeviceInfoParameter.EndianLittle);
            DriverVersion = _deviceInfoHelper.GetStringValue(DeviceInfoParameter.DriverVersion, encoding);
            OpenClCVersion = _deviceInfoHelper.GetStringValue(DeviceInfoParameter.OpenClCVersion, encoding);
            HostUnifiedMemory = _deviceInfoHelper.GetValue<bool>(DeviceInfoParameter.HostUnifiedMemory);
            ExecutionCapabilities = _deviceInfoHelper.GetValue<ExecutionCapabilities>(DeviceInfoParameter.ExecutionCapabilities);
            QueueProperties = _deviceInfoHelper.GetValue<CommandQueueProperties>(DeviceInfoParameter.QueueProperties);

            SinglePrecisionFloatConfigurations = _deviceInfoHelper.GetValue<FloatConfigurations>(DeviceInfoParameter.SingleFpConfig);

            //Is double precision supported
            if (NativeVectorWidthDouble != 0)
                DoublePrecisionFloatConfigurations = _deviceInfoHelper.GetValue<FloatConfigurations>(DeviceInfoParameter.DoubleFpConfig);

            //Is half precision supported
            if (NativeVectorWidthHalf != 0)
                HalfPrecisionFloatConfigurations = _deviceInfoHelper.GetValue<FloatConfigurations>(DeviceInfoParameter.HalfFpConfig);

        }

        private void ReleaseUnmanagedResources()
        {
            if (_deviceApi == null || Id == IntPtr.Zero)
                return;

            _deviceApi.clReleaseDevice(Id).ThrowOnError();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Device()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            ReleaseUnmanagedResources();

            _disposed = true;
        }

        public bool Equals(Device other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Device)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
