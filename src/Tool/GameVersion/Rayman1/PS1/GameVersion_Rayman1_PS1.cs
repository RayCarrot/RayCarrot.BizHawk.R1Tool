using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RayCarrot.BizHawk.R1Tool;

public abstract class GameVersion_Rayman1_PS1 : GameVersion_Rayman1
{
    protected GameVersion_Rayman1_PS1(Action<string> addLogAction, IBizHawkAPI api, Dictionary<Rayman1Pointer, long> pointerTable, Rayman1EngineVersion engineVersion) : base(addLogAction, api, pointerTable, engineVersion) { }

    protected abstract string EXEFileName { get; }
    protected abstract uint EXEFileTableLocalAddress { get; }
    protected abstract uint EXEFileTableMemoryAddress { get; }
    protected abstract uint EXEFileTableLength { get; }

    public override Platform Platform => Platform.PS1;

    public override MenuItem[] Menu => base.Menu.Append(new MenuItem
    {
        Title = () => "Reload EXE File Table",
        OnSelected = m =>
        {
            using OpenFileDialog fileDialog = new()
            {
                Title = $"Select the game executable file ({EXEFileName})",
                FileName = EXEFileName,
                CheckFileExists = true,
            };

            // Mute to disable stuttering sound
            bool toggleSound = API.EmuClient.GetSoundOn();

            if (toggleSound)
                API.EmuClient.SetSoundOn(false);

            DialogResult result = fileDialog.ShowDialog();

            if (toggleSound)
                API.EmuClient.SetSoundOn(true);

            if (result != DialogResult.OK)
                return false;

            using Stream exe = fileDialog.OpenFile();

            var fileTable = new byte[EXEFileTableLength];
            exe.Position = EXEFileTableLocalAddress;
            int read = exe.Read(fileTable, 0, fileTable.Length);

            if (read != fileTable.Length)
            {
                AddLogAction("Error reading file table");
                return false;
            }

            API.Mem.WriteByteRange(EXEFileTableMemoryAddress, fileTable.ToList());

            return false;
        },
    }).ToArray();
}