using CL.Core.Model;
using System;
using Xunit;

namespace CL.Core.Tests.Unit.Model
{
    public class ContextNotificationEventArgsTests
    {

        [Fact]
        public void CtorShouldThrowExceptionIfMessageIsNull()
        {
            Assert.Throws<ArgumentException>(() => new ContextNotificationEventArgs(null, ReadOnlyMemory<byte>.Empty));
        }

        [Fact]
        public void CtorShouldThrowExceptionIfMessageIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new ContextNotificationEventArgs(string.Empty, ReadOnlyMemory<byte>.Empty));
        }

        [Fact]
        public void CtorShouldSetProperties()
        {
            var testData = new byte[1];
            var evtArgs = new ContextNotificationEventArgs("Hi!", testData);

            Assert.Equal("Hi!", evtArgs.Message);
            Assert.Equal(testData, evtArgs.PrivateData);
        }
    }
}
