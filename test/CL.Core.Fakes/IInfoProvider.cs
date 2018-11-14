using System;

namespace CL.Core.Fakes
{
    public interface IInfoProvider<TParameter>
        where TParameter : Enum
    {
        InfoLookup<TParameter> Infos { get; }
    }
}