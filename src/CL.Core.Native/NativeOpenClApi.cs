using CL.Core.API;

namespace CL.Core.Native
{
    public class NativeOpenClApi : IOpenClApi
    {
        public IPlatformApi PlatformApi { get; }
        public IDeviceApi DeviceApi { get; }
        public ICommandQueueApi CommandQueueApi { get; }
        public IContextApi ContextApi { get; }
        public IBufferApi BufferApi { get; }
        public IProgramApi ProgramApi { get; }
        public IKernelApi KernelApi { get; }
        public IEventApi EventApi { get; }

        public NativeOpenClApi()
        {
            PlatformApi = new NativePlatformApi();
            DeviceApi = new NativeDeviceApi();
            CommandQueueApi = new NativeCommandQueueApi();
            ContextApi = new NativeContextApi();
            BufferApi = new NativeBufferApi();
            ProgramApi = new NativeProgramApi();
            KernelApi = new NativeKernelApi();
            EventApi = new NativeEventApi();
        }
    }
}
