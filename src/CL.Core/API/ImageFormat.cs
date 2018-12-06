using System;
using System.Runtime.InteropServices;

namespace CL.Core.API
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageFormat : IEquatable<ImageFormat>
    {
        public ImageFormat(ChannelOrder channelOrder, ChannelType channelDataType)
        {
            ChannelOrder = channelOrder;
            ChannelDataType = channelDataType;
        }

        public ChannelOrder ChannelOrder { get; }
        public ChannelType ChannelDataType { get; }

        public override bool Equals(object obj)
        {
            return obj is ImageFormat format && Equals(format);
        }

        public bool Equals(ImageFormat other)
        {
            return ChannelOrder == other.ChannelOrder &&
                   ChannelDataType == other.ChannelDataType;
        }

        public override int GetHashCode()
        {
            var hashCode = -383231296;
            hashCode = hashCode * -1521134295 + ChannelOrder.GetHashCode();
            hashCode = hashCode * -1521134295 + ChannelDataType.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ImageFormat format1, ImageFormat format2)
        {
            return format1.Equals(format2);
        }

        public static bool operator !=(ImageFormat format1, ImageFormat format2)
        {
            return !(format1 == format2);
        }
    }
}
