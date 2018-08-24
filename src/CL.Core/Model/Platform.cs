using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Core.API;

namespace CL.Core.Model
{
    public class Platform : IHasId, IEquatable<Platform>
    {
        private readonly IOpenClApi _openClApi;
        public IntPtr Id { get; }
        public string Name { get; }
        public string Vendor { get; }
        public string Profile { get; }
        public IReadOnlyCollection<string> Extensions { get; }
        public IReadOnlyCollection<Device> Devices { get; }

        internal Platform(IntPtr platformId, IOpenClApi openClApi)
        {
            _openClApi = openClApi ?? throw new ArgumentNullException(nameof(openClApi));

            //TODO: Check if Id exists
            Id = platformId;

            Name = Encoding.Default.GetString(InfoHelper.GetInfo(_openClApi.PlatformApi.clGetPlatformInfo, platformId, PlatformInfoParameter.Name));
            Vendor = Encoding.Default.GetString(InfoHelper.GetInfo(_openClApi.PlatformApi.clGetPlatformInfo, platformId, PlatformInfoParameter.Vendor));
            Profile = Encoding.Default.GetString(InfoHelper.GetInfo(_openClApi.PlatformApi.clGetPlatformInfo, platformId, PlatformInfoParameter.Profile));
            Extensions = Encoding.Default.GetString(InfoHelper.GetInfo(_openClApi.PlatformApi.clGetPlatformInfo, platformId, PlatformInfoParameter.Extensions)).Split(' ');


            var errorCode = _openClApi.DeviceApi.clGetDeviceIDs(platformId, DeviceType.All, 0, null, out var numDevices);
            errorCode.ThrowOnError();

            var deviceIds = new IntPtr[numDevices];
            errorCode = _openClApi.DeviceApi.clGetDeviceIDs(platformId, DeviceType.All, numDevices, deviceIds, out _);
            errorCode.ThrowOnError();

            Devices = deviceIds.Select(deviceId => new Device(this, deviceId, _openClApi.DeviceApi)).ToList().AsReadOnly();
        }

        public bool Equals(Platform other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Platform) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
