using CL.Core.API;
using System;

namespace CL.Core.Model
{
    public class SubBuffer<T> : MemoryObject
    where T : unmanaged
    {
        internal SubBuffer(IOpenClApi api, Buffer<T> parentBuffer)
            : base(api, parentBuffer.Context, IntPtr.Zero)
        {
            throw new NotImplementedException();
        }
    }
}