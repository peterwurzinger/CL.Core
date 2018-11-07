using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Core.Model
{
    public class Platform : IHasId, IEquatable<Platform>
    {
        public IntPtr Id { get; }
        public string Name { get; }
        public string Vendor { get; }
        public string Profile { get; }
        public string Version { get; }
        public IReadOnlyCollection<string> Extensions { get; }
        public IReadOnlyCollection<Device> Devices { get; }

        private readonly IOpenClApi _openClApi;

        internal Platform(IntPtr platformId, IOpenClApi openClApi)
        {
            _openClApi = openClApi ?? throw new ArgumentNullException(nameof(openClApi));

            Id = platformId;
            var platformInfoHelper = new InfoHelper<PlatformInfoParameter>(this, openClApi.PlatformApi.clGetPlatformInfo);

            Profile = platformInfoHelper.GetStringValue(PlatformInfoParameter.Profile);
            Version = platformInfoHelper.GetStringValue(PlatformInfoParameter.Version);
            Name = platformInfoHelper.GetStringValue(PlatformInfoParameter.Name);
            Vendor = platformInfoHelper.GetStringValue(PlatformInfoParameter.Vendor);
            Extensions = platformInfoHelper.GetStringValue(PlatformInfoParameter.Extensions).Split(' ');

            var errorCode = openClApi.DeviceApi.clGetDeviceIDs(platformId, DeviceType.All, 0, null, out var numDevices);
            errorCode.ThrowOnError();

            var deviceIds = new IntPtr[numDevices];
            errorCode = openClApi.DeviceApi.clGetDeviceIDs(platformId, DeviceType.All, numDevices, deviceIds, out _);
            errorCode.ThrowOnError();

            Devices = deviceIds.Select(deviceId => new Device(this, deviceId, openClApi.DeviceApi)).ToList().AsReadOnly();
        }

        public Context CreateContext(IReadOnlyCollection<Device> devices)
        {
            return new Context(_openClApi, devices);
        }

        public bool Equals(Platform other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Platform)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
