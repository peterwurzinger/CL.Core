using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Core.API;

namespace CL.Core.Model
{
    public class Platform
    {
        private readonly IPlatformInfoInterop _platformInterop;

        public long Id { get; }
        public string Name { get; }
        public string Vendor { get; }
        public string Profile { get; }
        public IReadOnlyCollection<string> Extensions { get; }
        public IReadOnlyCollection<Device> Devices { get; }

        internal Platform(IntPtr platformId, IPlatformInfoInterop platformInterop, IDeviceInfoInterop deviceInfoInterop)
        {
            _platformInterop = platformInterop ?? throw new ArgumentNullException(nameof(platformInterop));
            if (deviceInfoInterop == null) throw new ArgumentNullException(nameof(deviceInfoInterop));

            //TODO: Check if Id exists
            Id = platformId.ToInt64();
            Name = GetPlatformInfo(PlatformInfoParameter.Name);
            Vendor = GetPlatformInfo(PlatformInfoParameter.Vendor);
            Profile = GetPlatformInfo(PlatformInfoParameter.Profile);
            Extensions = GetPlatformInfo(PlatformInfoParameter.Extensions).Split(' ');


            var errorCode = deviceInfoInterop.clGetDeviceIDs(platformId, DeviceType.All, 0, null, out var numDevices);
            errorCode.ThrowOnError();

            var deviceIds = new IntPtr[numDevices];
            errorCode = deviceInfoInterop.clGetDeviceIDs(platformId, DeviceType.All, numDevices, deviceIds, out _);
            errorCode.ThrowOnError();

            Devices = deviceIds.Select(deviceId => new Device(deviceId, deviceInfoInterop)).ToList().AsReadOnly();
        }


        private string GetPlatformInfo(PlatformInfoParameter parameter)
        {
            var error = _platformInterop.clGetPlatformInfo(new IntPtr(Id), (uint)parameter, UIntPtr.Zero, null, out var infoSize);
            error.ThrowOnError();

            var info = new byte[infoSize.ToUInt32()];
            error = _platformInterop.clGetPlatformInfo(new IntPtr(Id), (uint)parameter, infoSize, info, out _);
            error.ThrowOnError();

            //TODO: Encoding configurable?
            return Encoding.Default.GetString(info);
        }
    }
}
