using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Core.Model
{
    public static class BuildInfoExtensions
    {

        public static IReadOnlyDictionary<Device, ReadOnlyMemory<byte>> AsBinariesDictionary(this IReadOnlyDictionary<Device, BuildInfo> builds)
        {
            return builds.Select(build => new KeyValuePair<Device, ReadOnlyMemory<byte>>(build.Key, build.Value.Binaries))
                .ToDictionary(k => k.Key, v => v.Value);
        }

    }
}
