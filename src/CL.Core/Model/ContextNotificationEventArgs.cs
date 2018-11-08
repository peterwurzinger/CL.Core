using System;

namespace CL.Core.Model
{
    public class ContextNotificationEventArgs : EventArgs
    {
        public string Message { get; }
        public ReadOnlyMemory<byte>? PrivateData { get; }

        internal ContextNotificationEventArgs(string message, ReadOnlyMemory<byte>? privateData)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
            Message = message;
            PrivateData = privateData;
        }
    }
}