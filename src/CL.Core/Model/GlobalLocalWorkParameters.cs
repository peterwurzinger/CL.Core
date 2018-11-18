using System;

namespace CL.Core.Model
{
    public readonly struct GlobalLocalWorkParameters : IEquatable<GlobalLocalWorkParameters>
    {
        public ulong GlobalWorkOffset { get; }
        public ulong GlobalWorkSize { get; }
        public ulong LocalWorkSize { get; }

        public GlobalLocalWorkParameters(ulong globalWorkSize, ulong localWorkSize)
            : this(globalWorkSize, 0, localWorkSize)
        {
        }

        public GlobalLocalWorkParameters(ulong globalWorkSize, ulong globalWorkOffset, ulong localWorkSize)
        {
            GlobalWorkSize = globalWorkSize;
            GlobalWorkOffset = globalWorkOffset;
            LocalWorkSize = localWorkSize;
        }

        public override bool Equals(object obj)
        {
            return obj is GlobalLocalWorkParameters parameters && Equals(parameters);
        }

        public bool Equals(GlobalLocalWorkParameters other)
        {
            return GlobalWorkOffset == other.GlobalWorkOffset &&
                   GlobalWorkSize == other.GlobalWorkSize &&
                   LocalWorkSize == other.LocalWorkSize;
        }

        public override int GetHashCode()
        {
            var hashCode = -824568562;
            hashCode = hashCode * -1521134295 + GlobalWorkOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + GlobalWorkSize.GetHashCode();
            hashCode = hashCode * -1521134295 + LocalWorkSize.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(GlobalLocalWorkParameters parameters1, GlobalLocalWorkParameters parameters2)
        {
            return parameters1.Equals(parameters2);
        }

        public static bool operator !=(GlobalLocalWorkParameters parameters1, GlobalLocalWorkParameters parameters2)
        {
            return !(parameters1 == parameters2);
        }
    }
}