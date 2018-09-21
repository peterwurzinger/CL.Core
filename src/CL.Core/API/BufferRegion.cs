using System;
using System.Runtime.InteropServices;

namespace CL.Core.API
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferRegion : IEquatable<BufferRegion>
    {
        public ulong Origin { get; }
        public ulong Size { get; }

        public BufferRegion(ulong origin, ulong size)
        {
            Origin = origin;
            Size = size;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Origin.GetHashCode() * 397) ^ Size.GetHashCode();
            }
        }

        public static bool operator ==(BufferRegion left, BufferRegion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BufferRegion left, BufferRegion right)
        {
            return !(left == right);
        }

        public bool Equals(BufferRegion other)
        {
            return Origin == other.Origin && Size == other.Size;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BufferRegion other && Equals(other);
        }
    }
}