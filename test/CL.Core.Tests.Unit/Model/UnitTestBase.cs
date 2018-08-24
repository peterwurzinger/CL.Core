using CL.Core.Fakes;

namespace CL.Core.Tests.Unit.Model
{
    public abstract class UnitTestBase
    {
        protected FakeOpenClApi FakeOpenClApi { get; }

        protected UnitTestBase()
        {
            FakeOpenClApi = new FakeOpenClApi();
        }
    }
}