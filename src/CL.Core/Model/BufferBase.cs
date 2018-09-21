using CL.Core.API;

namespace CL.Core.Model
{
    public abstract class BufferBase : MemoryObject
    {
        protected BufferBase(IOpenClApi api, Context context)
            : base(api, context)
        {
        }
    }
}