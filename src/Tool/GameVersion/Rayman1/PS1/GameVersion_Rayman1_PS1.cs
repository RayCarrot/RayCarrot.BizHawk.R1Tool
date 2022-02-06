using System;
using System.Collections.Generic;

namespace RayCarrot.BizHawk.R1Tool;

public abstract class GameVersion_Rayman1_PS1 : GameVersion_Rayman1
{
    protected GameVersion_Rayman1_PS1(Action<string> addLogAction, IBizHawkAPI api, Dictionary<Rayman1Pointer, long> pointerTable, Rayman1EngineVersion engineVersion) : base(addLogAction, api, pointerTable, engineVersion) { }

    public override Platform Platform => Platform.PS1;
}