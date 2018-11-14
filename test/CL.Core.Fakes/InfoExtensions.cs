using System;
using CL.Core.API;

namespace CL.Core.Fakes
{
    public static class InfoExtensions
    {
        public static OpenClErrorCode GetInfo<TFake, TInfoParameter>(this TFake fake, TInfoParameter paramName, uint paramValueSize, IntPtr paramValue, out uint paramValueSizeRet, OpenClErrorCode errorCode = OpenClErrorCode.Success)
            where TFake : IInfoProvider<TInfoParameter>
            where TInfoParameter : Enum
        {
            if (errorCode != OpenClErrorCode.Success)
            {
                paramValueSizeRet = 0;
                return errorCode;
            }

            //First call, obtain size
            if (paramValueSize == 0)
            {
                paramValueSizeRet = (uint)fake.Infos[paramName].Length;
                return OpenClErrorCode.Success;
            }

            fake.Infos.CopyTo(paramName, paramValue, (int)paramValueSize);
            paramValueSizeRet = paramValueSize;
            return OpenClErrorCode.Success;
        }
    }
}