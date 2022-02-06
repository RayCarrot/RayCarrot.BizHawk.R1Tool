using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BizHawk.Client.Common;
using RayCarrot.BizHawk.R1Tool.Properties;

namespace RayCarrot.BizHawk.R1Tool;

public abstract class GameVersion_Rayman1 : IGameVersion
{
    protected GameVersion_Rayman1(Action<string> addLogAction, IBizHawkAPI api, Dictionary<Rayman1Pointer, long> pointerTable, Rayman1EngineVersion EngineVersion)
    {
        // Set properties
        AddLogAction = addLogAction;
        API = api;
        Serializer = new Rayman1Serializer(api.Mem, pointerTable, EngineVersion);
        Data = new Rayman1Data();
    }

    public Rayman1Data Data { get; }
    public Rayman1Serializer Serializer { get; }
    public Action<string> AddLogAction { get; }
    public IBizHawkAPI API { get; }
    public virtual List<KeyValuePair<string, int>> Worlds => new List<KeyValuePair<string, int>>
    {
        new KeyValuePair<string, int>("Jungle", 21),
        new KeyValuePair<string, int>("Music", 18),
        new KeyValuePair<string, int>("Mountain", 13),
        new KeyValuePair<string, int>("Image", 13),
        new KeyValuePair<string, int>("Cave", 12),
        new KeyValuePair<string, int>("Cake", 4),
        new KeyValuePair<string, int>("Menu", 7),
    };
    public abstract Platform Platform { get; }
    public bool PendingEdits { get; set; }
    public bool ShowOverlays { get; set; }

    private R1_RayMode prevRayMode;
    public bool IsPaused
    {
        set
        {
            // Disable game inputs by enabling place ray with speed 0
            Data.RAY_MODE_SPEED = value ? (byte)0x00 : (byte)0x10;

            if (value)
            {
                prevRayMode = Data.ray_mode;
                Data.ray_mode = R1_RayMode.PLACE_RAY;
            }
            else
            {
                Data.ray_mode = prevRayMode;
            }


            PendingEdits = true;
        }
    }

    public virtual MenuItem[] Menu => new MenuItem[]
    {
        // Load level
        new MenuItem()
        {
            Title = () => "Load Level",
            Children = Worlds.Select((x, w) => new MenuItem()
            {
                Options = Enumerable.Range(1, x.Value).Select(l => $"{x.Key} {l}").ToArray(),
                OnSelected = m =>
                {
                    // Get world and level index to change to
                    var world = w + 1;
                    var lvl = (short)(m.SelectedOption + 1);

                    /*
                    // If the game is not running we can't change level
                    if (Data.fin_du_jeu)
                    {
                        AddLogAction($"Can't load a new level from the current state!");
                        return false;
                    }
                    */

                    // Check if it's a menu map
                    if (world == 7)
                    {
                        Data.menuEtape = (byte)m.SelectedOption; // Set the menu we want to load
                        Data.new_world = 1; // Flag that we're loading a new world to exit out of the level
                        Data.fin_du_jeu = true; // Set end of game flag to true to exit out of the game

                        AddLogAction($"Loading menu {m.SelectedOption}");

                        return true;
                    }

                    // Check if the world needs to be changed
                    if (world != Data.num_world)
                    {
                        AddLogAction($"Loading levels from other worlds is currently not supported");
                        return false;
                    }
                        
                    // Change the level
                    Data.num_level_choice = lvl; // Level to change to
                    Data.new_level = 1; // Flag that we want to load a new level

                    AddLogAction($"Loading level {world}-{lvl}");

                    return true;
                }
            }).ToArray()
        },
        // Cheats
        new MenuItem()
        {
            Title = () => "Cheats",
            Children = SubMenu_Cheats
        }
    };
    public MenuItem[] SettingsMenuItems => new MenuItem[]
    {
        new MenuItem()
        {
            Title = () => $"{(ShowOverlays ? "Hide" : "Show")} overlays",
            OnSelected = x =>
            {
                ShowOverlays = !ShowOverlays;
                return false;
            }
        }
    };

    // TODO: Clean up
    public virtual ValueItem[] Values => new ValueItem[]
    {
        new ValueItem("Menu", () => $"{Data.menuEtape}"), 
        new ValueItem("xmap", () => $"{Data.xmap}"), 
        new ValueItem("ymap", () => $"{Data.ymap}"), 
        new ValueItem("Objects", () => $"{Data.level_objCount}"), 
        new ValueItem("Active objects", () => $"{Data.actobj[100]}"), 
        new ValueItem("Objects", () => $"{Data.Pointer_level_obj.Address:X8}"), 
        new ValueItem("PROC_EXIT", () => $"{Data.PROC_EXIT}"), 
        new ValueItem("Dead time", () => $"{Data.dead_time}"), 
        new ValueItem("End of game", () => $"{Data.fin_du_jeu}"), 
        new ValueItem("New level", () => $"{Data.new_level == 1}"), 
        new ValueItem("New world", () => $"{Data.new_world == 1}"), 
        new ValueItem("World", () => $"{Data.num_world}"), 
        new ValueItem("Level", () => $"{Data.num_level}"), 
    };

    public virtual MenuItem[] SubMenu_Cheats => new MenuItem[]
    {
        new MenuItem()
        {
            Title = () => $"{(prevRayMode == R1_RayMode.PLACE_RAY ? "Disable" : "Enable")} PlaceRay",
            OnSelected = m =>
            {
                // TODO: Invert the value here instead like the game does to preserve the previous mode
                Data.ray_mode = prevRayMode = prevRayMode == R1_RayMode.PLACE_RAY ? R1_RayMode.RAYMAN : R1_RayMode.PLACE_RAY;

                AddLogAction($"Toggled PlaceRay");
                return true;
            }
        },
        new MenuItem()
        {
            Title = () => $"{(Data.RayEvts.HasFlag(R1_RayEvtsFlags.SuperHelico) ? "Disable" : "Enable")} Super Helico",
            OnSelected = m =>
            {
                if (!Data.RayEvts.HasFlag(R1_RayEvtsFlags.SuperHelico))
                    Data.RayEvts |= R1_RayEvtsFlags.SuperHelico;
                else
                    Data.RayEvts &= ~R1_RayEvtsFlags.SuperHelico;

                PendingEdits = true;

                AddLogAction($"Toggled super helico");
                AddLogAction($"Powers: {Data.RayEvts}");
                return false;
            }
        },
        new MenuItem()
        {
            Title = () => "99 Lives",
            OnSelected = m =>
            {
                Data.status_bar.LivesCount = 99;
                PendingEdits = true;

                AddLogAction($"Setting lives to 99");
                return false;
            }
        },
        new MenuItem()
        {
            Title = () => "All Powers",
            OnSelected = m =>
            {
                // TODO: Maybe just enable these powers and don't touch the others? This will disable super heico if it's enabled for example.
                Data.RayEvts = R1_RayEvtsFlags.Fist | R1_RayEvtsFlags.Hang | R1_RayEvtsFlags.Helico | R1_RayEvtsFlags.Grab | R1_RayEvtsFlags.Run;
                PendingEdits = true;

                AddLogAction($"Enabled all powers");
                return false;
            }
        },
    };

    public void Update(bool isActive)
    {
        // Don't update if the tool is not active and there are no pending edits, unless showing overlays
        if (!isActive && !PendingEdits && !ShowOverlays)
            return;

        // If there are pending edits we write instead of read
        Serializer.IsWriting = PendingEdits;

        // Serialize the data
        Data.Serialize(Serializer);

        // No more pending edits
        PendingEdits = false;
    }

    public void UpdateUI(bool isActive)
    {
        if (Data.actobj == null || !ShowOverlays)
            return;

        const int overscanLeft = 18; // TODO: Can we get this value from the game?

        // Draw frames
        for (int i = 0; i < Data.actobj[100]; i++)
        {
            var obj = Data.level_obj[Data.actobj[i]];

            var frame = obj.AnimDescriptors[obj.AnimIndex].Frames[obj.AnimFrame];

            var x = obj.XPos_Screen + frame.XPos;
            var y = obj.YPos_Screen + frame.YPos;
            var w = frame.Width;
            var h = frame.Height;
                
            API.Gui.DrawRectangle(x + overscanLeft, y, w, h, line: Color.Red, background: Color.FromArgb(150, Color.Red));
        }

        // TODO: Get exact game resolution (it's different per version) and handle sub-tile positioning, taking into account the horizontal overscan on PS1
        // Draw collision types
        ///*
        var mapIndex = Data.xmap / 16 + Data.ymap / 16 * Data.mp.Width;
        for (int y = 0; y < 240; y++)
        {
            for (int x = 0; x < 320; x++)
            {
                var tileIndex = mapIndex + x + y * Data.mp.Width;

                if (tileIndex >= Data.mp.TileMap.Length)
                    continue;

                var collisionType = Data.mp.TileMap[tileIndex] >> 10;

                var img = Resources.ResourceManager.GetObject($"BTYP_{collisionType}");
                if (img is Image typImg)
                    API.Gui.DrawImage(typImg, x * 16 + overscanLeft - Data.xmap % 16, y * 16 - Data.ymap % 16);
            }
        }
        //*/
    }

    public void LogData()
    {
        // TODO: Implement - log the current Data object as serialized JSON so we can view runtime values without debugging
        AddLogAction("Data logging has not been implemented");
    }
}