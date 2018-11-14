using System;

namespace CL.Core.Fakes.OpenCL
{
    public class FakeKernel
    {
        public IntPtr ProgramId { get; set; }

        public bool Released { get; internal set; }

        public FakeKernel(IntPtr programId)
        {
            ProgramId = programId;
            Released = false;
        }
    }
}