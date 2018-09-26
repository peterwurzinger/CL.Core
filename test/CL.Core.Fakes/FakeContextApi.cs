using CL.Core.API;
using System;
using System.Collections.Generic;

namespace CL.Core.Fakes
{
    public class FakeContextApi : IContextApi
    {
        public IDictionary<IntPtr, FakeOpenClContext> FakeContexts { get; }

        public FakeContextApi()
        {
            FakeContexts = new Dictionary<IntPtr, FakeOpenClContext>();
        }

        public IntPtr? clCreateContextResult { get; set; }
        public OpenClErrorCode? ClCreateContextErrorCode { get; set; }

        public IntPtr clCreateContext(IntPtr properties, uint numDevices, IntPtr[] deviceIds, IntPtr pfnNotify,
            IntPtr userData,
            out OpenClErrorCode errorCode)
        {
            errorCode = ClCreateContextErrorCode ?? OpenClErrorCode.Success;
            var id = clCreateContextResult ?? new IntPtr(1);

            if (errorCode == OpenClErrorCode.Success)
                FakeContexts[id] = new FakeOpenClContext(properties, deviceIds, pfnNotify);

            return id;
        }

        public OpenClErrorCode? clReleaseContextResult { get; set; }

        public OpenClErrorCode clReleaseContext(IntPtr contextId)
        {
            var result = clReleaseContextResult ?? OpenClErrorCode.Success;

            if (result == OpenClErrorCode.Success)
                FakeContexts[contextId].Released = true;

            return result;
        }
    }
}
