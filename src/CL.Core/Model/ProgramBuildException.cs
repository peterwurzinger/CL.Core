using System;
using CL.Core.API;

namespace CL.Core.Model
{
    public class ProgramBuildException : ClCoreException
    {
        public IntPtr ProgramHandle { get; }
        public IntPtr DeviceHandle { get; }
        public string Log { get; }

        internal ProgramBuildException(Program program, Device device, string log) : base(log)
        {
            if (program == null) throw new ArgumentNullException(nameof(program));
            if (device == null) throw new ArgumentNullException(nameof(device));

            ProgramHandle = program.Id;
            DeviceHandle = device.Id;
            Log = log;
        }
    }
}