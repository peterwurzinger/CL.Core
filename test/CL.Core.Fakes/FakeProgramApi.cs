﻿using CL.Core.API;
using CL.Core.Fakes.OpenCL;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenClErrorCode = CL.Core.API.OpenClErrorCode;

namespace CL.Core.Fakes
{
    public class FakeProgramApi : IProgramApi
    {
        public IDictionary<IntPtr, FakeProgram> FakePrograms { get; }

        public FakeProgramApi()
        {
            FakePrograms = new Dictionary<IntPtr, FakeProgram>();
        }

        public OpenClErrorCode? clCreateProgramWithSourceErrorCode { get; set; }
        public IntPtr? clCreateProgramWithSourceReturn { get; set; }
        public IntPtr clCreateProgramWithSource(IntPtr context, uint count, string[] strings, uint[] lengths,
            out OpenClErrorCode errorCodeRet)
        {
            errorCodeRet = clCreateProgramWithSourceErrorCode ?? OpenClErrorCode.Success;
            IntPtr id;

            if (errorCodeRet == OpenClErrorCode.Success)
            {
                id = clCreateProgramWithSourceReturn ?? new IntPtr(1);
                FakePrograms[id] = new FakeProgram(context, strings);
            }
            else
                id = IntPtr.Zero;

            return id;
        }

        public OpenClErrorCode? clCreateProgramWithBinaryErrorCodeRet { get; set; }
#pragma warning disable CA1819 // Properties should not return arrays
        public OpenClErrorCode[] clCreateProgramWithBinaryBinaryStatus { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
        public IntPtr? clCreateProgramWithBinaryResult { get; set; }

        public IntPtr clCreateProgramWithBinary(IntPtr context, uint numDevices, IntPtr[] deviceList, uint[] lengths,
            IntPtr[] binaries, OpenClErrorCode[] binaryStatus, out OpenClErrorCode errorCodeRet)
        {
            binaryStatus = clCreateProgramWithBinaryBinaryStatus ?? new OpenClErrorCode[deviceList.Length];
            errorCodeRet = clCreateProgramWithBinaryErrorCodeRet ?? OpenClErrorCode.Success;
            var id = clCreateProgramWithBinaryResult ?? new IntPtr(1);

            if (errorCodeRet != OpenClErrorCode.Success)
                return IntPtr.Zero;

            var deviceBinaries = binaries.Zip(deviceList, (binary, device) => new { binary, device })
                                         .Zip(lengths, (binDev, length) => new { binDev.binary, binDev.device, length })
                                         .Zip(binaryStatus, (binDevLength, status) => new { binDevLength.binary, binDevLength.device, binDevLength.length, status })
                                         .Where(f => f.status == OpenClErrorCode.Success)
                                         .Select(b => Tuple.Create(b.device, new byte[b.length]));

            FakePrograms[id] = new FakeProgram(context, deviceBinaries);
            return id;
        }

        public OpenClErrorCode? clBuildProgramReturn { get; set; }
        public OpenClErrorCode clBuildProgram(IntPtr program, uint numDevices, IntPtr[] devices, string options, IntPtr pfnNotify,
            IntPtr userData)
        {
            return clBuildProgramReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clGetProgramBuildInfoReturn { get; set; }
        public uint? clGetProgramBuildInfoParamValueSizeReturned { get; set; }
        public OpenClErrorCode clGetProgramBuildInfo(IntPtr program, IntPtr device, ProgramBuildInfoParameter paramName, uint paramValueSize,
            IntPtr paramValue, out uint paramValueSizeReturned)
        {
            paramValueSizeReturned = clGetProgramBuildInfoParamValueSizeReturned ?? 4;
            return clGetProgramBuildInfoReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clGetProgramInfoReturn { get; set; }
        public OpenClErrorCode clGetProgramInfo(IntPtr program, ProgramInfoParameter paramName, uint paramValueSize, IntPtr paramValue,
            out uint paramValueSizeReturned)
        {
            var errorCode = clGetProgramInfoReturn ?? OpenClErrorCode.Success;
            return FakePrograms[program].GetInfo(paramName, paramValueSize, paramValue, out paramValueSizeReturned, errorCode);
        }


        public OpenClErrorCode? clRetainProgramReturn { get; set; }
        public OpenClErrorCode clRetainProgram(IntPtr program)
        {
            return clRetainProgramReturn ?? OpenClErrorCode.Success;
        }

        public OpenClErrorCode? clReleaseProgramReturn { get; set; }
        public OpenClErrorCode clReleaseProgram(IntPtr program)
        {
            var result = clReleaseProgramReturn ?? OpenClErrorCode.Success;

            if (result == OpenClErrorCode.Success)
                FakePrograms[program].Released = true;

            return result;
        }
    }
}