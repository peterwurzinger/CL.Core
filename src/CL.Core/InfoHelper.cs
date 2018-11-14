using System;
using System.Text;
using CL.Core.API;
using CL.Core.Model;

namespace CL.Core
{
    internal sealed class InfoHelper<TParameter>
        where TParameter : Enum
    {
        internal delegate OpenClErrorCode GetInfoFunc(IntPtr handle, TParameter parameterName, uint valueSize,
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
            _infoFunc(_entity.Id, parameterName, 0, IntPtr.Zero, out var paramSize).ThrowOnError();

            var stackMemory = stackalloc TValue[1];
            _infoFunc(_entity.Id, parameterName, paramSize, (IntPtr)stackMemory, out _).ThrowOnError();

            return stackMemory[0];
        }

        public unsafe ReadOnlySpan<TValue> GetValues<TValue>(TParameter parameterName)
            where TValue : unmanaged
        {
            _infoFunc(_entity.Id, parameterName, 0, IntPtr.Zero, out var paramSize).ThrowOnError();

            //Explicitly allocating an array. Stackalloc doesn't make sense, since it would get allocated on heap eventually on return
            var memory = new TValue[(int)paramSize / sizeof(TValue)];

            fixed (void* ptr = memory)
            {
                _infoFunc(_entity.Id, parameterName, paramSize, (IntPtr)ptr, out _).ThrowOnError();
            }

            return memory;
        }

        public unsafe string GetStringValue(TParameter parameterName)
        {
            _infoFunc(_entity.Id, parameterName, 0, IntPtr.Zero, out var paramSize).ThrowOnError();

            var stackMemory = stackalloc byte[(int)paramSize];
            _infoFunc(_entity.Id, parameterName, paramSize, (IntPtr) stackMemory, out _).ThrowOnError();

            return _encoding.GetString(stackMemory, (int)paramSize)?.TrimEnd((char)0);
        }
    }
}
