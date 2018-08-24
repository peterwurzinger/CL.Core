using CL.Core.API;

namespace CL.Core.Native
{
    public class NativeOpenClApi : IOpenClApi
    {
        public IPlatformApi PlatformApi { get; }
        public IDeviceApi DeviceApi { get; }
        public ICommandQueueApi CommandQueueApi { get; }
        public IContextApi ContextApi { get; }

        public NativeOpenClApi()
        {
            PlatformApi = new NativePlatformApi();
            DeviceApi = new NativeDeviceApi();
            CommandQueueApi = new NativeCommandQueueApi();
            ContextApi = new NativeContextApi();
        }
    }
}
