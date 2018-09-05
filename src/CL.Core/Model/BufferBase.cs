using CL.Core.API;
using System;

namespace CL.Core.Model
{
    public abstract class BufferBase : MemoryObject
    {
        protected BufferBase(IOpenClApi api, Context context)
            : base(api, context)
        {
        }

        public void Write()
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }
    }
}