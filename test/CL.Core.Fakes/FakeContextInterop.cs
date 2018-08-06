using CL.Core.API;
using System;

namespace CL.Core.Fakes
{
    public class FakeContextInterop : IContextInterop
    {
        public IntPtr? clCreateContextResult { get; set; }
        public OpenClErrorCode? ClCreateContextErrorCode { get; set; }
        public IntPtr clCreateContext(IntPtr properties, uint numDevices, IntPtr[] deviceIds, IntPtr pfnNotify, IntPtr userData,
            out OpenClErrorCode errorCode)
        {
            errorCode = ClCreateContextErrorCode ?? OpenClErrorCode.Success;
            return clCreateContextResult ?? IntPtr.Zero;
        }

        public OpenClErrorCode? clReleaseContextResult { get; set; }
        public OpenClErrorCode clReleaseContext(IntPtr contextId)
        {
            return clReleaseContextResult ?? OpenClErrorCode.Success;
        }
    }
}
