using System;
using CL.Core.API;

namespace CL.Core.Model
{
    public class BuildInfo
    {
        public BuildStatus Status { get; }
        public string Options { get; }
        public string Log { get; }
        public ReadOnlyMemory<byte> Binaries { get; }

        public BuildInfo(BuildStatus status, string log, string options, ReadOnlyMemory<byte> binaries)
        {
            Status = status;
            Log = log ?? throw new ArgumentNullException(nameof(log));
            Options = options ?? throw new ArgumentNullException(nameof(options));
            Binaries = binaries;
        }
    }
}