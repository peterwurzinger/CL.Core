using CL.Core.API;
using System;

namespace CL.Core.Fakes.OpenCL
{
    public class FakeCommandQueue : IInfoProvider<CommandQueueInfoParameter>
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

            Infos = new InfoLookup<CommandQueueInfoParameter>
            {
                {CommandQueueInfoParameter.Context, contextId },
                {CommandQueueInfoParameter.Device, deviceId },
                {CommandQueueInfoParameter.Properties, Properties},
                {CommandQueueInfoParameter.ReferenceCount, (uint)1}
            };
        }

        public InfoLookup<CommandQueueInfoParameter> Infos { get; }
    }
}
