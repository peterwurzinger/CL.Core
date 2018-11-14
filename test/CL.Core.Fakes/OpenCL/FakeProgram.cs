using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Core.Fakes.OpenCL
{
    public class FakeProgram
    {
        public IntPtr ContextId { get; }
        public IReadOnlyCollection<string> Sources { get; }
        public IDictionary<IntPtr, byte[]> Binaries { get; }
        public bool Released { get; internal set; }

        public FakeProgram(IntPtr contextId, IReadOnlyCollection<string> sources)
        {
            ContextId = contextId;
            Sources = sources;
            Released = false;

            Binaries = new Dictionary<IntPtr, byte[]>();
        }

        public FakeProgram(IntPtr contextId, IEnumerable<Tuple<IntPtr, byte[]>> deviceBinaries)
        {
            ContextId = contextId;
            Released = false;
            Binaries = deviceBinaries.ToDictionary(key => key.Item1, value => value.Item2);
        }
    }
}