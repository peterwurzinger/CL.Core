using CL.Core.API;
using System;

namespace CL.Core.Fakes
{
    public class FakeOpenClCommandQueue
    {
        public IntPtr ContextId { get; }
        public IntPtr DeviceId { get; }
        public CommandQueueProperties Properties { get; }

        public bool Finished { get; internal set; }
        public bool Flushed { get; internal set; }
        public bool Released { get; internal set; }
        public bool Retained { get; internal set; }

        public FakeOpenClCommandQueue(IntPtr contextId, IntPtr deviceId, CommandQueueProperties props)
        {
            ContextId = contextId;
            DeviceId = deviceId;
            Properties = props;
        }

    }
}
