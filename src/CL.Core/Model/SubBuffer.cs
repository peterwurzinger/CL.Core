using System;
using CL.Core.API;

namespace CL.Core.Model
{
    public class SubBuffer : BufferBase
    {
        internal SubBuffer(IOpenClApi api, Buffer parentBuffer)
            : base(api, parentBuffer.Context)
        {
            throw new NotImplementedException();
        }
    }
}