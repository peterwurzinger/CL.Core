using CL.Core.API;
using System;
using System.Linq;
using System.Text;

namespace CL.Core.Model
{
    public class Device : IHasId, IDisposable, IEquatable<Device>
    {
        private readonly IDeviceInfoInterop _deviceInfoInterop;

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
        public bool Available => BitConverter.ToBoolean(InfoHelper.GetInfo(_deviceInfoInterop.clGetDeviceInfo, Id, DeviceInfoParameter.DeviceAvailable), 0);

        /// <summary>
        /// Maximum number of work-items that can be specified in each dimension of the work-group to clEnqueueNDRangeKernel. Returns n size_t entries, where n is the value returned by the query for CL_DEVICE_MAX_WORK_ITEM_DIMENSIONS.The minimum value is (1, 1, 1).
        /// </summary>
        public uint[] MaxWorkItemSizes { get; }

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
        public uint GlobalMemorySize { get; }

        /// <summary>
        /// Maximum configured clock frequency of the device in MHz.
        /// </summary>
        public uint MaxClockFrequency { get; }

        /// <summary>
        /// The default compute device address space size specified as an unsigned integer value in bits. Currently supported values are 32 or 64 bits.
        /// </summary>
        public uint AddressBits { get; }

        #endregion

        internal Device(Platform platform, IntPtr deviceId, IDeviceInfoInterop deviceInfoInterop)
        {
            Platform = platform ?? throw new ArgumentNullException(nameof(platform));
            _deviceInfoInterop = deviceInfoInterop ?? throw new ArgumentNullException(nameof(deviceInfoInterop));

            Id = deviceId;
            Name = Encoding.Default.GetString(InfoHelper.GetInfo(_deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.Name));

            Type = (DeviceType)BitConverter.ToUInt32(InfoHelper.GetInfo(deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.Type), 0);
            VendorId = BitConverter.ToUInt32(InfoHelper.GetInfo(deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.VendorId), 0);
            MaxComputeUnits = BitConverter.ToUInt32(InfoHelper.GetInfo(deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.MaxComputeUnits), 0);
            MaxWorkItemDimensions = BitConverter.ToUInt32(InfoHelper.GetInfo(deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.MaxWorkItemDimensions), 0);
            MaxWorkGroupSize = BitConverter.ToUInt32(InfoHelper.GetInfo(deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.MaxWorkGroupSize), 0);

            //TODO: Get bit width of size_t - could vary between uchar and int64 ... -.- Correlation to AddressBits - property?
            var workItemSizeBytes = InfoHelper.GetInfo(deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.MaxWorkItemSizes);
            MaxWorkItemSizes = new uint[MaxWorkItemDimensions];

            for (var i = 0; i < MaxWorkItemDimensions; i++)
                MaxWorkItemSizes[i] = BitConverter.ToUInt32(workItemSizeBytes.Skip(i * 8).Take(8).ToArray(), 0);

            MaxClockFrequency = BitConverter.ToUInt32(InfoHelper.GetInfo(deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.MaxClockFrequency), 0);
            GlobalMemorySize = BitConverter.ToUInt32(InfoHelper.GetInfo(deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.GlobalMemSize), 0);
            MaxConstantArgs = BitConverter.ToUInt32(InfoHelper.GetInfo(deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.MaxConstantArgs), 0);
            AddressBits = BitConverter.ToUInt32(InfoHelper.GetInfo(deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.AddressBits), 0);
            Vendor = Encoding.Default.GetString(InfoHelper.GetInfo(_deviceInfoInterop.clGetDeviceInfo, deviceId, DeviceInfoParameter.Vendor));
        }

        private void ReleaseUnmanagedResources()
        {
            if (_deviceInfoInterop == null || Id == IntPtr.Zero)
                return;

            _deviceInfoInterop.clReleaseDevice(Id).ThrowOnError();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Device()
        {
            ReleaseUnmanagedResources();
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
