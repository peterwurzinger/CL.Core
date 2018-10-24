using CL.Core.API;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CL.Core.Model
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    public sealed class Event : IHasId
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        public IntPtr Id { get; }
        public EventCommandExecutionStatus Status => _infoHelper.GetValue<EventCommandExecutionStatus>(EventInfoParameter.CommandExecutionStatus);

        private delegate void EventCallback(IntPtr evt, EventCommandExecutionStatus eventCommandExecStatus, IntPtr userData);

        private readonly IOpenClApi _api;
        private readonly TaskCompletionSource<bool> _taskCompletionSource;
        private readonly InfoHelper<EventInfoParameter> _infoHelper;

        private GCHandle _handle;

        public Event(IOpenClApi api, IntPtr evt)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            Id = evt;

            _infoHelper = new InfoHelper<EventInfoParameter>(this, api.EventApi.clGetEventInfo);

            api.EventApi.clRetainEvent(evt).ThrowOnError();

            _taskCompletionSource = new TaskCompletionSource<bool>();

            var callbackDelegate = (EventCallback)Callback;
            _handle = GCHandle.Alloc(callbackDelegate);
            var fp = Marshal.GetFunctionPointerForDelegate(callbackDelegate);

            api.EventApi.clSetEventCallback(evt, EventCommandExecutionStatus.Complete, fp, IntPtr.Zero).ThrowOnError();
        }

        public void WaitComplete()
        {
            ValidateWait();
            _taskCompletionSource.Task.Wait();
        }

        public Task WaitCompleteAsync()
        {
            ValidateWait();
            return _taskCompletionSource.Task;
        }

        private void ValidateWait()
        {
            //TODO: See if CommandQueue is Flushed
            if (false)
                throw new ClCoreException("");
        }

        private void Callback(IntPtr evt, EventCommandExecutionStatus eventCommandExecStatus, IntPtr userData)
        {
            _taskCompletionSource.SetResult(true);
            _handle.Free();
            _api.EventApi.clReleaseEvent(evt).ThrowOnError();
        }
    }
}
