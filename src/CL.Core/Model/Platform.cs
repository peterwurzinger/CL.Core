using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Core.Native;

namespace CL.Core.Model
{
    public class Platform
    {
        public long Id { get; }
        public string Name { get; }
        public string Vendor { get; }
        public string Profile { get; }
        public IReadOnlyCollection<string> Extensions { get; }
        public IReadOnlyCollection<Device> Devices { get; }

        internal Platform(IntPtr platformId)
        {
            //TODO: Check if Id exists
            Id = platformId.ToInt64();
            Name = GetPlatformInfo(PlatformInfoParameters.Name);
            Vendor = GetPlatformInfo(PlatformInfoParameters.Vendor);
            Profile = GetPlatformInfo(PlatformInfoParameters.Profile);
            Extensions = GetPlatformInfo(PlatformInfoParameters.Extensions).Split(' ');


            var error = DeviceCalls.clGetDeviceIDs(platformId, DeviceType.All, 10, null, out var numDevices);
            var deviceIds = new IntPtr[numDevices];
            error = DeviceCalls.clGetDeviceIDs(platformId, DeviceType.All, numDevices, deviceIds, out _);

            Devices = deviceIds.Select(deviceId => new Device(deviceId)).ToList().AsReadOnly();
        }


        private string GetPlatformInfo(PlatformInfoParameters parameter)
        {
            var error = PlatformCalls.clGetPlatformInfo(new IntPtr(Id), (uint)parameter, UIntPtr.Zero, null, out var infoSize);
            var info = new byte[infoSize.ToUInt32()];
            error = PlatformCalls.clGetPlatformInfo(new IntPtr(Id), (uint)parameter, infoSize, info, out _);
            return Encoding.Default.GetString(info);
        }
    }
}
