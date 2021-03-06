﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CL.Core.API;

namespace CL.Core.Fakes.OpenCL
{
    public class FakeContext
    {
        public bool Released { get; internal set; }
        public IReadOnlyCollection<IntPtr> Devices { get; }
        public IntPtr PfnNotify { get; }
        public IntPtr Properties { get; }

        public FakeContext(IntPtr properties, IReadOnlyCollection<IntPtr> devices, IntPtr pfnNotify)
        {
            Properties = properties;
            Devices = devices;
            PfnNotify = pfnNotify;
        }

        public void Notify(string message, IntPtr privateInfo, int cb)
        {
            Marshal.GetDelegateForFunctionPointer<ContextErrorDelegate>(PfnNotify).Invoke(message, privateInfo, cb, IntPtr.Zero);
        }
    }
}