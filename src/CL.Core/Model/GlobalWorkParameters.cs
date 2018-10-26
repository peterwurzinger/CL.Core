using System;

namespace CL.Core.Model
{
    public readonly struct GlobalWorkParameters : IEquatable<GlobalWorkParameters>
    {
        public uint GlobalWorkOffset { get; }
        public uint GlobalWorkSize { get; }

        public GlobalWorkParameters(uint globalWorkSize)
            : this(globalWorkSize, 0)
        {
        }

        public GlobalWorkParameters(uint globalWorkSize, uint globalWorkOffset)
        {
            GlobalWorkSize = globalWorkSize;
            GlobalWorkOffset = globalWorkOffset;
        }

        public override bool Equals(object obj)
        {
            return obj is GlobalWorkParameters parameters && Equals(parameters);
        }

        public bool Equals(GlobalWorkParameters other)
        {
            return GlobalWorkOffset == other.GlobalWorkOffset &&
                   GlobalWorkSize == other.GlobalWorkSize;
        }

        public override int GetHashCode()
        {
            var hashCode = 448864466;
            hashCode = hashCode * -1521134295 + GlobalWorkOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + GlobalWorkSize.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(GlobalWorkParameters parameters1, GlobalWorkParameters parameters2)
        {
            return parameters1.Equals(parameters2);
        }

        public static bool operator !=(GlobalWorkParameters parameters1, GlobalWorkParameters parameters2)
        {
            return !(parameters1 == parameters2);
        }
    }
}