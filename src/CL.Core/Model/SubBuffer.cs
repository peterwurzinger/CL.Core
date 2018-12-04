using CL.Core.API;
using System;

namespace CL.Core.Model
{
    public class SubBuffer<T> : BufferBase<T>
    where T : unmanaged
    {
        public Buffer<T> Parent { get; }

        // ReSharper disable once SuggestBaseTypeForParameter
        internal SubBuffer(IOpenClApi api, Buffer<T> parent, IntPtr id)
            : base(api, parent?.Context, id)
        {
            Parent = parent;
        }
    }
}