namespace RayCarrot.BizHawk.R1Tool
{
    public class Rayman1Data
    {
        // General
        public short num_level { get; set; }
        public short num_world { get; set; }
        public short num_level_choice { get; set; }
        public short num_world_choice { get; set; }
        public short new_level { get; set; }
        public short new_world { get; set; }
        public bool fin_du_jeu { get; set; }
        public byte menuEtape { get; set; } // 0 = Menu, 1 = Password/MemoryCard, 2 = Password, 3 = MemoryCard, 4 = Options, 5 = Enter Password, 6 = ?
        public bool ModeDemo { get; set; }
        public sbyte dead_time { get; set; }
        public bool PROC_EXIT { get; set; }

        // Map state
        public R1_RayMode ray_mode { get; set; }
        public byte RAY_MODE_SPEED { get; set; }
        public R1_RayEvtsFlags RayEvts { get; set; }
        public byte gele { get; set; } // 0 = None, 1 = MEDAILLON_TOON, 2 = SIGNPOST
        public short h_scroll_speed { get; set; }
        public short v_scroll_speed { get; set; }

        // Save data
        public R1_StatusBar status_bar { get; set; }

        // Objects
        public short[] actobj { get; set; }
        public short level_objCount { get; set; } // Byte on PS1, but aligned to 4
        public Pointer32 Pointer_level_obj { get; set; }
        public R1_Obj[] level_obj { get; set; }

        // Map
        public short xmap { get; set; }
        public short ymap { get; set; }
        public R1_mp mp { get; set; }

        public void Serialize(Rayman1Serializer s)
        {
            num_level = s.DoAt(Rayman1Pointer.num_level, () => s.Serialize(num_level, name: nameof(num_level)));
            num_world = s.DoAt(Rayman1Pointer.num_world, () => s.Serialize(num_world, name: nameof(num_world)));
            num_level_choice = s.DoAt(Rayman1Pointer.num_level_choice, () => s.Serialize(num_level_choice, name: nameof(num_level_choice)));
            num_world_choice = s.DoAt(Rayman1Pointer.num_world_choice, () => s.Serialize(num_world_choice, name: nameof(num_world_choice)));
            new_level = s.DoAt(Rayman1Pointer.new_level, () => s.Serialize(new_level, name: nameof(new_level)));
            new_world = s.DoAt(Rayman1Pointer.new_world, () => s.Serialize(new_world, name: nameof(new_world)));
            fin_du_jeu = s.DoAt(Rayman1Pointer.fin_du_jeu, () => s.Serialize(fin_du_jeu, name: nameof(fin_du_jeu)));
            menuEtape = s.DoAt(Rayman1Pointer.menuEtape, () => s.Serialize(menuEtape, name: nameof(menuEtape)));
            ModeDemo = s.DoAt(Rayman1Pointer.ModeDemo, () => s.Serialize(ModeDemo, name: nameof(ModeDemo)));
            dead_time = s.DoAt(Rayman1Pointer.dead_time, () => s.Serialize(dead_time, name: nameof(dead_time)));
            PROC_EXIT = s.DoAt(Rayman1Pointer.PROC_EXIT, () => s.Serialize(PROC_EXIT, name: nameof(PROC_EXIT)));

            ray_mode = s.DoAt(Rayman1Pointer.ray_mode, () => s.Serialize(ray_mode, name: nameof(ray_mode)));
            RAY_MODE_SPEED = s.DoAt(Rayman1Pointer.RAY_MODE_SPEED, () => s.Serialize(RAY_MODE_SPEED, name: nameof(RAY_MODE_SPEED)));
            RayEvts = s.DoAt(Rayman1Pointer.RayEvts, () => s.Serialize(RayEvts, name: nameof(RayEvts)));
            h_scroll_speed = s.DoAt(Rayman1Pointer.h_scroll_speed, () => s.Serialize(h_scroll_speed, name: nameof(h_scroll_speed)));
            v_scroll_speed = s.DoAt(Rayman1Pointer.v_scroll_speed, () => s.Serialize(v_scroll_speed, name: nameof(v_scroll_speed)));

            status_bar = s.DoAt(Rayman1Pointer.status_bar, () => s.SerializeObject(status_bar, name: nameof(status_bar)));

            actobj = s.DoAt(Rayman1Pointer.actobj, () => s.SerializeArray(actobj, 112, name: nameof(actobj)));
            level_objCount = s.DoAt(Rayman1Pointer.level_objCount, () => s.Serialize(level_objCount, name: nameof(level_objCount)));
            Pointer_level_obj = s.DoAt(Rayman1Pointer.level_obj, () => s.SerializeObject(Pointer_level_obj, name: nameof(Pointer_level_obj)));
            level_obj = s.DoAt(Pointer_level_obj, () => s.SerializeObjectArray(level_obj, level_objCount, x => x.EngineVersion = s.EngineVersion, name: nameof(level_obj)));

            xmap = s.DoAt(Rayman1Pointer.xmap, () => s.Serialize(xmap, name: nameof(xmap)));
            ymap = s.DoAt(Rayman1Pointer.ymap, () => s.Serialize(ymap, name: nameof(ymap)));
            mp = s.DoAt(Rayman1Pointer.mp, () => s.SerializeObject(mp, name: nameof(mp)));
        }

        /*
            Game loops:

            worldMapLoop = (fin_du_jeu == false && -1 < status_bar.LivesCount && new_world != 0)
            mapLoop = (fin_du_jeu == false && new_world == 0 && new_level != 0)
            deadLoop = (fin_du_jeu == false && new_level == 0 && new_world == 0)
            mainLoop = new_world == 0 - break if (ModeDemo == true || dead_time == 0 || new_level != 0)
         */
    }
}