using System;
using System.Linq;
using System.Text;
using CL.Core.API;

namespace CL.Core.Model
{
    public class Device : IDisposable
    {
        private readonly IDeviceInfoInterop _deviceInfoInterop;

        #region Properties

        /// <summary>
        /// ?
        /// </summary>
        public long Id { get; }

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
        public bool Available { get; }

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

        internal Device(IntPtr deviceId, IDeviceInfoInterop deviceInfoInterop)
        {
            _deviceInfoInterop = deviceInfoInterop ?? throw new ArgumentNullException(nameof(deviceInfoInterop));
            Id = deviceId.ToInt64();
            Name = GetDeviceInfo(DeviceInfoParameter.Name);
            Type = (DeviceType)BitConverter.ToUInt32(GetDeviceInfoNoString(DeviceInfoParameter.Type), 0);
            VendorId = BitConverter.ToUInt32(GetDeviceInfoNoString(DeviceInfoParameter.VendorId), 0);
            MaxComputeUnits = BitConverter.ToUInt32(GetDeviceInfoNoString(DeviceInfoParameter.MaxComputeUnits), 0);
            MaxWorkItemDimensions = BitConverter.ToUInt32(GetDeviceInfoNoString(DeviceInfoParameter.MaxWorkItemDimensions), 0);
            MaxWorkGroupSize = BitConverter.ToUInt32(GetDeviceInfoNoString(DeviceInfoParameter.MaxWorkGroupSize), 0);

            //TODO: Get bit width of size_t - could vary between uchar and int64 ... -.- Correlation to AddressBits - property?
            var workItemSizeBytes = GetDeviceInfoNoString(DeviceInfoParameter.MaxWorkItemSizes);
            MaxWorkItemSizes = new uint[MaxWorkItemDimensions];

            for (var i = 0; i < MaxWorkItemDimensions; i++)
                MaxWorkItemSizes[i] = BitConverter.ToUInt32(workItemSizeBytes.Skip(i * 8).Take(8).ToArray(), 0);

            Available = BitConverter.ToBoolean(GetDeviceInfoNoString(DeviceInfoParameter.DeviceAvailable), 0);
            MaxClockFrequency = BitConverter.ToUInt32(GetDeviceInfoNoString(DeviceInfoParameter.MaxClockFrequency), 0);
            GlobalMemorySize = BitConverter.ToUInt32(GetDeviceInfoNoString(DeviceInfoParameter.GlobalMemSize), 0);
            MaxConstantArgs = BitConverter.ToUInt32(GetDeviceInfoNoString(DeviceInfoParameter.MaxConstantArgs), 0);
            AddressBits = BitConverter.ToUInt32(GetDeviceInfoNoString(DeviceInfoParameter.AddressBits), 0);
            Vendor = GetDeviceInfo(DeviceInfoParameter.Vendor);
        }

        private string GetDeviceInfo(DeviceInfoParameter parameter)
        {
            return Encoding.Default.GetString(GetDeviceInfoNoString(parameter));
        }

        private byte[] GetDeviceInfoNoString(DeviceInfoParameter parameter)
        {
            var error = _deviceInfoInterop.clGetDeviceInfo(new IntPtr(Id), parameter, UIntPtr.Zero, null, out var infoSize);
            error.ThrowOnError();

            var info = new byte[infoSize.ToUInt32()];
            error = _deviceInfoInterop.clGetDeviceInfo(new IntPtr(Id), parameter, infoSize, info, out _);
            error.ThrowOnError();

            return info;
        }

        private void ReleaseUnmanagedResources()
        {
            _deviceInfoInterop.clReleaseDevice(new IntPtr(Id)).ThrowOnError();
            // TODO release unmanaged resources here
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
    }
}
