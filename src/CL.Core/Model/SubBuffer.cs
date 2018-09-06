using System;
using CL.Core.API;

namespace CL.Core.Model
{
    public class SubBuffer<T> : BufferBase
    where T : unmanaged
    {
        internal SubBuffer(IOpenClApi api, Buffer<T> parentBuffer)
            : base(api, parentBuffer.Context)
        {
            throw new NotImplementedException();
        }
    }
}