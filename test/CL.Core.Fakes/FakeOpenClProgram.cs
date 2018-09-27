using System;

namespace CL.Core.Fakes
{
    public class FakeOpenClProgram
    {
        public IntPtr ContextId { get; }
        public string[] Sources { get; }
        public bool Released { get; internal set; }

        public FakeOpenClProgram(IntPtr contextId, string[] sources)
        {
            ContextId = contextId;
            Sources = sources;
            Released = false;
        }
    }
}