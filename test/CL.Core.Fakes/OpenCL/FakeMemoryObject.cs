using System;
using CL.Core.API;

namespace CL.Core.Fakes.OpenCL
{
    public class FakeMemoryObject : IInfoProvider<MemoryObjectInfoParameter>
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