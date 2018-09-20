using System;
using System.Runtime.InteropServices;
using System.Text;
using CL.Core.Model;

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

    internal class InfoHelper<TParameter>
    where TParameter : Enum
    {
        public delegate OpenClErrorCode GetInfoFunc(IntPtr handle, TParameter parameterName, uint valueSize,
            IntPtr paramValue, out uint parameterValueSizeReturn);


        private readonly IHasId _entity;
        private readonly GetInfoFunc _infoFunc;

        internal InfoHelper(IHasId entity, GetInfoFunc infoFunc)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
            _infoFunc = infoFunc ?? throw new ArgumentNullException(nameof(infoFunc));
        }

        public TValue GetValue<TValue>(TParameter parameterName)
            where TValue : struct
        {
            var bytes = GetBytes(parameterName);
            return MemoryMarshal.Read<TValue>(bytes);
        }

        public ReadOnlySpan<TValue> GetValues<TValue>(TParameter parameterName)
            where TValue : struct
        {
            var bytes = GetBytes(parameterName);
            return MemoryMarshal.Cast<byte, TValue>(bytes);
        }

        public string GetStringValue(TParameter parameterName, Encoding encoding)
        {
            var bytes = GetBytes(parameterName);
            return encoding.GetString(bytes.ToArray());
        }

        private unsafe ReadOnlySpan<byte> GetBytes(TParameter parameterName)
        {
            var error = _infoFunc(_entity.Id, parameterName, 0, IntPtr.Zero, out var paramSize);
            error.ThrowOnError();


            var memory = new ReadOnlyMemory<byte>(new byte[paramSize]);
            var memoryHandle = memory.Pin();

            try
            {
                error = _infoFunc(_entity.Id, parameterName, paramSize, new IntPtr(memoryHandle.Pointer), out _);
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
