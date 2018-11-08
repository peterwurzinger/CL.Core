using CL.Core.API;
using System;

namespace CL.Core.Fakes
{
    public class FakeMemoryObject
    {
        public ulong Size { get; }
        public MemoryFlags Flags { get; }
        public IntPtr HostPointer { get; }
        public bool Released { get; internal set; }

        public InfoLookup<MemoryObjectInfoParameter> Infos { get; }

        public FakeMemoryObject(ulong size, MemoryFlags flags, IntPtr hostPointer)
        {
            Size = size;
            Flags = flags;
            HostPointer = hostPointer;
            Released = false;

            Infos = new InfoLookup<MemoryObjectInfoParameter>
            {
                {MemoryObjectInfoParameter.Flags, Flags},
                {MemoryObjectInfoParameter.HostPointer, HostPointer},
                {MemoryObjectInfoParameter.Size, Size}
            };
        }
    }
}