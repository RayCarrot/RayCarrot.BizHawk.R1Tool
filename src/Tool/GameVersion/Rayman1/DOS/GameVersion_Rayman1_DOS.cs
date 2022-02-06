using System;
using System.Collections.Generic;

namespace RayCarrot.BizHawk.R1Tool;

public abstract class GameVersion_Rayman1_DOS : GameVersion_Rayman1
{
    protected GameVersion_Rayman1_DOS(Action<string> addLogAction, IBizHawkAPI api, Dictionary<Rayman1Pointer, long> pointerTable, Rayman1EngineVersion engineVersion) : base(addLogAction, api, pointerTable, engineVersion) { }

    public override List<KeyValuePair<string, int>> Worlds
    {
        get
        {
            var w = base.Worlds;
            w[0] = new KeyValuePair<string, int>("Jungle", 22); // PC has extra Breakout map
            return w;
        }
    }
    public override Platform Platform => Platform.DOS;
}