﻿using CL.Core.API;
using CL.Core.Fakes.OpenCL;
using System;
using System.Collections.Generic;

namespace CL.Core.Fakes
{
    public class FakeKernelApi : IKernelApi
    {
        public IDictionary<IntPtr, FakeKernel> FakeKernels { get; }

        public FakeKernelApi()
        {
            FakeKernels = new Dictionary<IntPtr, FakeKernel>();
        }

        public OpenClErrorCode? clCreateKernelErrorCode { get; set; }
        public IntPtr? clCreateKernelReturn { get; set; }
        public IntPtr clCreateKernel(IntPtr program, string kernelName, out OpenClErrorCode errorCodeRet)
        {
            errorCodeRet = clCreateKernelErrorCode ?? OpenClErrorCode.Success;

            if (errorCodeRet != OpenClErrorCode.Success)
                return IntPtr.Zero;

            var id = clCreateKernelReturn ?? new IntPtr(1);
            FakeKernels[id] = new FakeKernel(program, kernelName);

            return id;

        }

        public OpenClErrorCode? clReleaseProgramReturn { get; set; }
        public OpenClErrorCode clReleaseKernel(IntPtr kernel)
        {
            return clReleaseProgramReturn ?? OpenClErrorCode.Success;
        }


        public OpenClErrorCode? clSetKernelArgReturn { get; set; }
        public OpenClErrorCode clSetKernelArg(IntPtr kernel, uint argIndex, ulong argSize, IntPtr argValue)
        {
            return clSetKernelArgReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clGetKernelInfoReturn { get; set; }
        public OpenClErrorCode clGetKernelInfo(IntPtr kernel, KernelInfoParameter paramName, uint paramValueSize, IntPtr paramValue,
            out uint paramValueSizeReturned)
        {
            var errorCode = clGetKernelInfoReturn ?? OpenClErrorCode.Success;
            return FakeKernels[kernel].GetInfo(paramName, paramValueSize, paramValue, out paramValueSizeReturned, errorCode);
        }

        public OpenClErrorCode? clEnqueueNDRangeKernelError { get; set; }
        public OpenClErrorCode clEnqueueNDRangeKernel(IntPtr commandQueue, IntPtr kernel, uint workDim, UIntPtr globalWorkOffset,
            UIntPtr globalWorkSize, UIntPtr localWorkSize, uint numEventsInWaitList, IntPtr eventWaitList, out IntPtr evt)
        {
            evt = IntPtr.Zero;
            return clEnqueueNDRangeKernelError ?? OpenClErrorCode.Success;
        }
    }
}