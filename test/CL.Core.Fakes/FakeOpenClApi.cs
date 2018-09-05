using CL.Core.API;

namespace CL.Core.Fakes
{
    public class FakeOpenClApi : IOpenClApi
    {
        public IPlatformApi PlatformApi { get; }
        public FakePlatformApi FakePlatformApi { get; }

        public IDeviceApi DeviceApi { get; }
        public FakeDeviceApi FakeDeviceApi { get; }

        public ICommandQueueApi CommandQueueApi { get; }
        public FakeCommandQueueApi FakeCommandQueueApi { get; }

        public IContextApi ContextApi { get; }
        public FakeContextApi FakeContextApi { get; }

        public IBufferApi BufferApi { get; }
        public FakeBufferApi FakeBufferApi { get; }

        public FakeOpenClApi()
        {
            PlatformApi = FakePlatformApi = new FakePlatformApi();
            DeviceApi = FakeDeviceApi = new FakeDeviceApi();
            CommandQueueApi = FakeCommandQueueApi = new FakeCommandQueueApi();
            ContextApi = FakeContextApi = new FakeContextApi();
            BufferApi = FakeBufferApi = new FakeBufferApi();
        }
    }
}
