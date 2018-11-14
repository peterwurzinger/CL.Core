using System;
using CL.Core.API;

namespace CL.Core.Fakes.OpenCL
{
    public class FakeCommandQueue
    {
        public IntPtr ContextId { get; }
        public IntPtr DeviceId { get; }
        public CommandQueueProperties Properties { get; }

        public bool Finished { get; internal set; }
        public bool Flushed { get; internal set; }
        public bool Released { get; internal set; }
        public bool Retained { get; internal set; }

        public FakeCommandQueue(IntPtr contextId, IntPtr deviceId, CommandQueueProperties props)
        {
            ContextId = contextId;
            DeviceId = deviceId;
            Properties = props;
        }

    }
}
