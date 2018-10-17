using CL.Core.API;
using System;

namespace CL.Core.Model
{
    public interface IMemoryObject : IHasId, IDisposable
    {
        Context Context { get; }
        ulong Size { get; }
        MemoryFlags Flags { get; }
    }
}