using System;
using System.Collections.Generic;

namespace RayCarrot.BizHawk.R1Tool;

public abstract class GameVersion_Rayman1_GBA : GameVersion_Rayman1_DOS
{
    protected GameVersion_Rayman1_GBA(Action<string> addLogAction, IBizHawkAPI api, Dictionary<Rayman1Pointer, long> pointerTable, Rayman1EngineVersion engineVersion) : base(addLogAction, api, pointerTable, engineVersion) { }

    public override List<KeyValuePair<string, int>> Worlds
    {
        get
        {
            var w = base.Worlds;
            w.Add(new KeyValuePair<string, int>("Multiplayer", 6)); // GBA has extra multiplayer world
            return w;
        }
    }
    public override Platform Platform => Platform.GBA;
}