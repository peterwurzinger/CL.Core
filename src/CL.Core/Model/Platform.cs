using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CL.Core.Model
{
    public class Platform : IHasId, IEquatable<Platform>
    {
        public IntPtr Id { get; }
        public string Name { get; }
        public string Vendor { get; }
        public string Profile { get; }
        public IReadOnlyCollection<string> Extensions { get; }
        public IReadOnlyCollection<Device> Devices { get; }

        internal Platform(IntPtr platformId, IOpenClApi openClApi)
        {
            var openClApi1 = openClApi ?? throw new ArgumentNullException(nameof(openClApi));

            //TODO: Check if Id exists
            Id = platformId;
            var platformInfoHelper = new InfoHelper<PlatformInfoParameter>(this, openClApi1.PlatformApi.clGetPlatformInfo);
            var encoding = Encoding.Default;

            Name = platformInfoHelper.GetStringValue(PlatformInfoParameter.Name, encoding);
            Vendor = platformInfoHelper.GetStringValue(PlatformInfoParameter.Vendor, encoding);
            Profile = platformInfoHelper.GetStringValue(PlatformInfoParameter.Profile, encoding);
            Extensions = platformInfoHelper.GetStringValue(PlatformInfoParameter.Extensions, encoding).Split(' ');

            var errorCode = openClApi1.DeviceApi.clGetDeviceIDs(platformId, DeviceType.All, 0, null, out var numDevices);
            errorCode.ThrowOnError();

            var deviceIds = new IntPtr[numDevices];
            errorCode = openClApi1.DeviceApi.clGetDeviceIDs(platformId, DeviceType.All, numDevices, deviceIds, out _);
            errorCode.ThrowOnError();

            Devices = deviceIds.Select(deviceId => new Device(this, deviceId, openClApi1.DeviceApi)).ToList().AsReadOnly();
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
