using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CL.Core.API;

namespace CL.Core.Model
{
    public abstract class EventBase<TResult> : IHasId
    {
        public IntPtr Id { get; }
        public EventCommandExecutionStatus Status => _infoHelper.GetValue<EventCommandExecutionStatus>(EventInfoParameter.CommandExecutionStatus);

        protected IOpenClApi Api { get; }
        protected TaskCompletionSource<TResult> TaskCompletionSource { get; }

        private readonly InfoHelper<EventInfoParameter> _infoHelper;
        private readonly TResult _result;
        private GCHandle _handle;

        protected EventBase(IOpenClApi api, IntPtr evt, TResult result)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            Id = evt;
            _result = result;

            _infoHelper = new InfoHelper<EventInfoParameter>(this, api.EventApi.clGetEventInfo);

            api.EventApi.clRetainEvent(evt).ThrowOnError();

            TaskCompletionSource = new TaskCompletionSource<TResult>();

            var callbackDelegate = new EventCallback(Callback);
            _handle = GCHandle.Alloc(callbackDelegate);

            var fp = Marshal.GetFunctionPointerForDelegate(callbackDelegate);

            api.EventApi.clSetEventCallback(evt, EventCommandExecutionStatus.Complete, fp, IntPtr.Zero).ThrowOnError();
        }

        private void Callback(IntPtr evt, EventCommandExecutionStatus eventCommandExecStatus, IntPtr userData)
        {
            TaskCompletionSource.SetResult(_result);
            _handle.Free();
            Api.EventApi.clReleaseEvent(evt).ThrowOnError();
        }
    }
}