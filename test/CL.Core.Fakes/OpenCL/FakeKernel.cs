using System;
using CL.Core.API;

namespace CL.Core.Fakes.OpenCL
{
    public class FakeKernel : IInfoProvider<KernelInfoParameter>
    {
        public IntPtr ProgramId { get; set; }

        public bool Released { get; internal set; }

        public FakeKernel(IntPtr programId, string kernelName)
        {
            ProgramId = programId;
            Released = false;

            Infos = new InfoLookup<KernelInfoParameter>
            {
                {KernelInfoParameter.FunctionName, kernelName },
                {KernelInfoParameter.NumberOfArguments, 1 },
                {KernelInfoParameter.ReferenceCount, 1 },
                {KernelInfoParameter.Context, new IntPtr(1) },
                {KernelInfoParameter.Program, programId }
            };
        }

        public InfoLookup<KernelInfoParameter> Infos { get; }
    }
}