using System;
using System.Runtime.InteropServices;

namespace CL.Core.API
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferRegion
    {
        public ulong Origin;
        public ulong Size;
    }
}