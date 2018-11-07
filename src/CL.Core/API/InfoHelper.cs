using CL.Core.Model;
using System;
using System.Text;

namespace CL.Core.API
{
    internal class InfoHelper<TParameter>
    where TParameter : Enum
    {
        public delegate OpenClErrorCode GetInfoFunc(IntPtr handle, TParameter parameterName, uint valueSize,
            IntPtr paramValue, out uint parameterValueSizeReturn);

        private readonly IHasId _entity;
        private readonly GetInfoFunc _infoFunc;
        private readonly Encoding _encoding;

        internal InfoHelper(IHasId entity, GetInfoFunc infoFunc, Encoding encoding)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
            _infoFunc = infoFunc ?? throw new ArgumentNullException(nameof(infoFunc));
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        internal InfoHelper(IHasId entity, GetInfoFunc infoFunc)
            : this(entity, infoFunc, Encoding.UTF8) { }

        public unsafe TValue GetValue<TValue>(TParameter parameterName)
            where TValue : unmanaged
        {
            var error = _infoFunc(_entity.Id, parameterName, 0, IntPtr.Zero, out var paramSize);
            error.ThrowOnError();

            var stackMemory = stackalloc TValue[1];
            error = _infoFunc(_entity.Id, parameterName, paramSize, (IntPtr)stackMemory, out _);
            error.ThrowOnError();

            return stackMemory[0];
        }

        public unsafe ReadOnlySpan<TValue> GetValues<TValue>(TParameter parameterName)
            where TValue : unmanaged
        {
            var error = _infoFunc(_entity.Id, parameterName, 0, IntPtr.Zero, out var paramSize);
            error.ThrowOnError();

            //Explicitly allocating an array. Stackalloc doesn't make sense, since it would get allocated on heap eventually on return
            var memory = new TValue[(int)paramSize / sizeof(TValue)];

            fixed (void* ptr = memory)
            {
                error = _infoFunc(_entity.Id, parameterName, paramSize, (IntPtr)ptr, out _);
                error.ThrowOnError();
            }

            return memory;
        }

        public unsafe string GetStringValue(TParameter parameterName)
        {
            var error = _infoFunc(_entity.Id, parameterName, 0, IntPtr.Zero, out var paramSize);
            error.ThrowOnError();

            var stackMemory = stackalloc byte[(int)paramSize];
            error = _infoFunc(_entity.Id, parameterName, paramSize, (IntPtr)stackMemory, out _);
            error.ThrowOnError();

            return _encoding.GetString(stackMemory, (int)paramSize)?.TrimEnd((char)0);
        }
    }
}
