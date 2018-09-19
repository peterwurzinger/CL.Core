﻿using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CL.Core.Model
{
    internal class AsyncBuild : IDisposable
    {
        private delegate void AsyncBuildCallbackDelegate(IntPtr program, IntPtr userData);

        private readonly SemaphoreSlim _semaphore;
        private GCHandle _delegateHandle;

        internal Task BuildTask { get; }

        internal AsyncBuild(IProgramApi programApi, Program program, IReadOnlyCollection<Device> devices)
        {
            if (program == null)
                throw new ArgumentNullException(nameof(program));

            if (devices == null)
                throw new ArgumentNullException(nameof(devices));

            _semaphore = new SemaphoreSlim(0, 1);

            var callbackDelegate = (AsyncBuildCallbackDelegate)AsyncBuildCallback;
            _delegateHandle = GCHandle.Alloc(callbackDelegate);
            var fp = Marshal.GetFunctionPointerForDelegate(callbackDelegate);

            var error = programApi.clBuildProgram(program.Id, (uint)devices.Count, devices.Select(d => d.Id).ToArray(), string.Empty, fp, IntPtr.Zero);
            error.ThrowOnError();

            BuildTask = _semaphore.WaitAsync();
        }

        private void AsyncBuildCallback(IntPtr program, IntPtr userData)
        {
            _semaphore.Release();
            _delegateHandle.Free();
        }

        public void Dispose()
        {
            BuildTask.Dispose();
            _semaphore.Dispose();
        }
    }
}
