using System;

namespace CL.Core.Fakes
{
    public class FakeOpenClKernel
    {
        public IntPtr ProgramId { get; set; }

        public bool Released { get; internal set; }

        public FakeOpenClKernel(IntPtr programId)
        {
            ProgramId = programId;
            Released = false;
        }
    }
}