using System;
using System.Collections.Generic;
using BizHawk.Client.Common;

namespace RayCarrot.BizHawk.R1Tool
{
    public class Rayman1Serializer : BizHawkBinarySerializer
    {
        public Rayman1Serializer(IMemoryApi mem, Dictionary<Rayman1Pointer, long> pointerTable, Rayman1EngineVersion engineVersion) : base(mem)
        {
            PointerTable = pointerTable;
            EngineVersion = engineVersion;
        }

        public T DoAt<T>(Rayman1Pointer p, Func<T> func) => DoAt<T>(PointerTable[p], func);

        public Dictionary<Rayman1Pointer, long> PointerTable { get; }
        public Rayman1EngineVersion EngineVersion { get; }
    }
}