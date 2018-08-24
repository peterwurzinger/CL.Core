using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class PlatformTests : UnitTestBase
    {

        [Fact]
        public void CtorShouldThrowExceptionIfOpenClApiNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Platform(IntPtr.Zero, null));
        }

        //TODO: To be continued

    }
}
