using System;
using System.Runtime.InteropServices;

namespace CL.Core.API
{
    [Obsolete]
    public static class InfoHelper
    {
        public delegate OpenClErrorCode GetInfoFunc<in TParameterName>(IntPtr handle, TParameterName parameterName, uint valueSize,
            byte[] parameterValue, out uint parameterValueSizeReturn)
            where TParameterName : Enum;

        [Obsolete]
        public static byte[] GetInfo<TParameterName>(GetInfoFunc<TParameterName> getInfoFunc, IntPtr handle,
            TParameterName parameterName)
            where TParameterName : Enum
        {
            var error = getInfoFunc(handle, parameterName, 0, null, out var paramSize);
            error.ThrowOnError();

            var info = new byte[paramSize];

            error = getInfoFunc(handle, parameterName, paramSize, info, out _);
            error.ThrowOnError();

            return info;
        }
    }

    public class InfoHelper<TParameter>
    where TParameter : Enum
    {
        public delegate OpenClErrorCode GetInfoFunc(IntPtr handle, TParameter parameterName, uint valueSize,
            IntPtr paramValue, out uint parameterValueSizeReturn);

        private readonly GetInfoFunc _infoFunc;

        public InfoHelper(GetInfoFunc infoFunc)
        {
            _infoFunc = infoFunc ?? throw new ArgumentNullException(nameof(infoFunc));
        }

        public TValue GetValue<TValue>(IntPtr handle, TParameter parameterName)
            where TValue : struct
        {
            var bytes = GetBytes(handle, parameterName);
            return MemoryMarshal.Read<TValue>(bytes);
        }

        public ReadOnlySpan<TValue> GetValues<TValue>(IntPtr handle, TParameter parameterName)
            where TValue : struct
        {
            var bytes = GetBytes(handle, parameterName);
            return MemoryMarshal.Cast<byte, TValue>(bytes);
        }

        private unsafe ReadOnlySpan<byte> GetBytes(IntPtr handle, TParameter parameterName)
        {
            var error = _infoFunc(handle, parameterName, 0, IntPtr.Zero, out var paramSize);
            error.ThrowOnError();


            var memory = new ReadOnlyMemory<byte>(new byte[paramSize]);
            var memoryHandle = memory.Pin();

            try
            {
                error = _infoFunc(handle, parameterName, paramSize, new IntPtr(memoryHandle.Pointer), out _);
                error.ThrowOnError();

                return memory.Span;
            }
            finally
            {
                memoryHandle.Dispose();
            }
        }
    }
}
