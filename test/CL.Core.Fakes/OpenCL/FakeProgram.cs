using CL.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Core.Fakes.OpenCL
{
    public class FakeProgram : IInfoProvider<ProgramInfoParameter>
    {
        public IntPtr ContextId { get; }
        public IReadOnlyCollection<string> Sources { get; }
        public IDictionary<IntPtr, byte[]> Binaries { get; }
        public bool Released { get; internal set; }

        public FakeProgram(IntPtr contextId, IReadOnlyCollection<string> sources)
        {
            ContextId = contextId;
            Sources = sources;
            Released = false;

            Binaries = new Dictionary<IntPtr, byte[]>();
            Infos = new InfoLookup<ProgramInfoParameter>
            {
                {ProgramInfoParameter.Context, contextId},
                {ProgramInfoParameter.ReferenceCount, (uint)1},
                {ProgramInfoParameter.NumDevices, (uint)1},
                {ProgramInfoParameter.Devices, new[]{new IntPtr(1)}},
                {ProgramInfoParameter.Source, sources },
                {ProgramInfoParameter.BinarySizes, new[]{(uint)0} },
                {ProgramInfoParameter.Binaries, new []{new IntPtr(1), } }
            };
        }

        public FakeProgram(IntPtr contextId, IEnumerable<Tuple<IntPtr, byte[]>> deviceBinaries)
        {
            ContextId = contextId;
            Released = false;
            Binaries = deviceBinaries.ToDictionary(key => key.Item1, value => value.Item2);

            Infos = new InfoLookup<ProgramInfoParameter>
            {
                {ProgramInfoParameter.Context, contextId},
                {ProgramInfoParameter.ReferenceCount, (uint)1},
                {ProgramInfoParameter.NumDevices, (uint)Binaries.Count},
                {ProgramInfoParameter.Devices, Binaries.Keys.ToArray()},
                {ProgramInfoParameter.Source, new byte[0] },
                {ProgramInfoParameter.BinarySizes, Binaries.Values.Select(v => v.Length).ToArray() },

                //Screw this, seriously
                {ProgramInfoParameter.Binaries, new []{new IntPtr(1), } }
            };
        }

        public InfoLookup<ProgramInfoParameter> Infos { get; }
    }
}