using System;

namespace CL.Core.API
{
    public static class InfoHelper
    {
        public delegate OpenClErrorCode GetInfoFunc<in TParameterName>(IntPtr handle, TParameterName parameterName, uint valueSize,
            byte[] parameterValue, out uint parameterValueSizeReturn);

        public static byte[] GetInfo<TParameterName>(GetInfoFunc<TParameterName> getInfoFunc, IntPtr handle,
            TParameterName parameterName)
            where TParameterName : struct
        {
            var error = getInfoFunc(handle, parameterName, 0, null, out var paramSize);
            error.ThrowOnError();

            var info = new byte[paramSize];

            error = getInfoFunc(handle, parameterName, paramSize, info, out var _);
            error.ThrowOnError();

            return info;
        }
        
    }
}
