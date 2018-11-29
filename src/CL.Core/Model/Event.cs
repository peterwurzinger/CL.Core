using CL.Core.API;
using System;
using System.Threading.Tasks;

namespace CL.Core.Model
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    public sealed class Event : EventBase<bool>
#pragma warning restore CA1716 // Identifiers should not match keywords
    {

        internal Event(IOpenClApi api, IntPtr evt)
            : base(api, evt, true)
        {
        }

        public void WaitComplete()
        {
            TaskCompletionSource.Task.Wait();
        }

        public Task WaitCompleteAsync()
        {
            return TaskCompletionSource.Task;
        }


    }

#pragma warning disable CA1716 // Identifiers should not match keywords
    public sealed class Event<TResult> : EventBase<TResult>
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        public Event(IOpenClApi api, IntPtr evt, TResult result)
            : base(api, evt, result)
        {
        }

        public TResult WaitComplete()
        {
            return TaskCompletionSource.Task.GetAwaiter().GetResult();
        }

        public Task<TResult> WaitCompleteAsync()
        {
            return TaskCompletionSource.Task;
        }

    }
}
