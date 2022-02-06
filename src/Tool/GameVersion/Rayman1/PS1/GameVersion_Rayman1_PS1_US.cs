using System;
using System.Collections.Generic;

namespace RayCarrot.BizHawk.R1Tool;

public class GameVersion_Rayman1_PS1_US : GameVersion_Rayman1_PS1
{
    public GameVersion_Rayman1_PS1_US(Action<string> addLogAction, IBizHawkAPI api) : base(addLogAction, api, new Dictionary<Rayman1Pointer, long>()
    {
        [Rayman1Pointer.num_level] = 0x801f9a68,
        [Rayman1Pointer.num_world] = 0x801fa688,
        [Rayman1Pointer.num_level_choice] = 0x801e5a20,
        [Rayman1Pointer.num_world_choice] = 0x801e63e8,
        [Rayman1Pointer.new_level] = 0x801f99f0,
        [Rayman1Pointer.new_world] = 0x801fa5a8,
        [Rayman1Pointer.fin_du_jeu] = 0x801d8b40,
        [Rayman1Pointer.menuEtape] = 0x801f81a0,
        [Rayman1Pointer.ModeDemo] = 0x801f5410,
        [Rayman1Pointer.dead_time] = 0x801f8158,
        [Rayman1Pointer.PROC_EXIT] = 0x801f43e0,

        [Rayman1Pointer.ray_mode] = 0x801e5420,
        [Rayman1Pointer.RAY_MODE_SPEED] = 0x801e4dd8,
        [Rayman1Pointer.RayEvts] = 0x801f43d0,
        [Rayman1Pointer.gele] = 0x801f8120,
        [Rayman1Pointer.h_scroll_speed] = 0x801fa550,
        [Rayman1Pointer.v_scroll_speed] = 0x801fa698,

        [Rayman1Pointer.status_bar] = 0x801e4d50,

        [Rayman1Pointer.actobj] = 0x801e5428,
        [Rayman1Pointer.level_objCount] = 0x801d7ae4,
        [Rayman1Pointer.level_obj] = 0x801d7ae0,

        [Rayman1Pointer.xmap] = 0x801f84b8,
        [Rayman1Pointer.ymap] = 0x801f84c0,
        [Rayman1Pointer.mp] = 0x801f4430,
    }, Rayman1EngineVersion.R1_PS1) { }

    protected override string EXEFileName => "SLUS-000.05";
    protected override uint EXEFileTableLocalAddress => 0x000A0338;
    protected override uint EXEFileTableMemoryAddress => 0x801C4B38;
    protected override uint EXEFileTableLength => 9864;
}