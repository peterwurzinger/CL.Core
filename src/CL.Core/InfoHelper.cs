using CL.Core.API;
using CL.Core.Model;
using System;
using System.Text;

namespace CL.Core
{
    internal sealed class InfoHelper<TParameter>
        where TParameter : Enum
    {
        internal delegate OpenClErrorCode GetEntityInfo(IntPtr handle, TParameter parameterName, uint valueSize,
            IntPtr paramValue, out uint parameterValueSizeReturn);

        private delegate OpenClErrorCode GetParamSizeFunc(TParameter parameterName, out uint parameterValueSizeReturn);
        private unsafe delegate OpenClErrorCode GetInfoFunc(TParameter parameterName, uint paramSize, void* mem);

        private readonly GetInfoFunc _entityInfoFunc;
        private readonly GetParamSizeFunc _paramSizeFunc;
        private readonly Encoding _encoding;

        internal unsafe InfoHelper(IHasId entity, GetEntityInfo entityInfo, Encoding encoding)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entityInfo == null) throw new ArgumentNullException(nameof(entityInfo));
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));

            _entityInfoFunc = (parameterName, paramSize, mem) =>
                entityInfo(entity.Id, parameterName, paramSize, (IntPtr) mem, out _);

            _paramSizeFunc = (TParameter name, out uint parameterValueSizeReturn) => 
                entityInfo(entity.Id, name, 0, IntPtr.Zero, out parameterValueSizeReturn);
        }

        internal InfoHelper(IHasId entity, GetEntityInfo entityInfo)
            : this(entity, entityInfo, Encoding.UTF8) { }

        public unsafe TValue GetValue<TValue>(TParameter parameterName)
            where TValue : unmanaged
        {
            _paramSizeFunc(parameterName, out var paramSize).ThrowOnError();

            var value = default(TValue);

            _entityInfoFunc(parameterName, paramSize, &value).ThrowOnError();

            return value;
        }

        public unsafe TValue[] GetValues<TValue>(TParameter parameterName)
            where TValue : unmanaged
        {
            _paramSizeFunc(parameterName, out var paramSize).ThrowOnError();

            //Explicitly allocating an array. Stackalloc doesn't make sense, since it would get allocated on heap eventually on return
            var memory = new TValue[(int)paramSize / sizeof(TValue)];

            fixed (void* ptr = memory)
            {
                _entityInfoFunc(parameterName, paramSize, ptr).ThrowOnError();
            }

            return memory;
        }

        public unsafe string GetStringValue(TParameter parameterName)
        {
            _paramSizeFunc(parameterName, out var paramSize).ThrowOnError();

            var stackMemory = stackalloc byte[(int)paramSize];
            _entityInfoFunc(parameterName, paramSize, stackMemory).ThrowOnError();

            return _encoding.GetString(stackMemory, (int)paramSize)?.TrimEnd((char)0);
        }
    }
}
