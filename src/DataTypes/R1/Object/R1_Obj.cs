using System;

namespace RayCarrot.BizHawk.R1Tool;

public class R1_Obj : IBinarySerializable
{
    public Rayman1EngineVersion EngineVersion { get; set; } // Set before serializing

    public Pointer32 Pointer_ImageDescriptors { get; set; }
    public Pointer32 Pointer_AnimDescriptors { get; set; }
    public Pointer32 Pointer_ImageBuffer { get; set; }
    public Pointer32 Pointer_ETA { get; set; }
    public Pointer32 Pointer_Commands { get; set; }
    public Pointer32 Pointer_LabelOffsets { get; set; }

    public byte[] PS1Demo_Unk1 { get; set; }
    public uint PS1Demo_Unk2 { get; set; }
    public uint PS1_Unk1 { get; set; }
    public R1_CommandContext[] CMD_Contexts { get; set; }
    // How many of these uints are a part of the CMD context array?
    public uint Uint_1C { get; set; }
    public uint Uint_20 { get; set; }
    public uint IsActive { get; set; } // 0 if inactive, 1 if active - is this a bool or can it be other values too? Game checks if it's 0 to see if always object is inactive.
    public int XPos { get; set; }
    public int YPos { get; set; }
    public short PS1Demo_Unk3 { get; set; }
    public uint Uint_30 { get; set; }
    public short ObjIndex { get; set; }
    public short XPos_Screen { get; set; }
    public short YPos_Screen { get; set; }
    public short Short_3A { get; set; }
    public short Initial_XPos { get; set; }
    public short Initial_YPos { get; set; }
    public bool PS1Demo_IsFlipped { get; set; } // This isn't correct
    public byte PS1Demo_Unk4 { get; set; }
    public short SpeedX { get; set; }
    public short SpeedY { get; set; }
    public ushort ImageDescriptorCount { get; set; }
    public short CMD_CurrentOffset { get; set; }
    public short CMD_Arg0 { get; set; } // This along with CMD_Arg1 might be a more generic temp value, so might have other uses too
    public short Short_4A { get; set; } // For Rayman this holds the index of the object he's standing on. It most likely has different uses for other events based on type. In R2 this is in the type specific data.
    public short Short_4C { get; set; }
    public short Short_4E { get; set; }

    // This value is used for voice lines as a replacement of the normal HitPoints value in order to have a sample index higher than 255. When this is used HitPoints is always EDU_ExtHitPoints % 256.
    public uint EDU_ExtHitPoints { get; set; }

    public short CMD_Arg1 { get; set; }
    public short Short_52 { get; set; } // Linked event index?
    public short Short_54 { get; set; }
    public short Short_56 { get; set; }
    public short Short_58 { get; set; } // Prev collision type for moving platforms
    public short Short_5A { get; set; }
    public ushort TypeZDC { get; set; }
    public short Short_5E { get; set; }
    public R1_ObjType ObjType { get; set; }
    public byte[] CollisionTypes { get; set; }
    public byte Byte_67 { get; set; }
    public byte Offset_BX { get; set; }
    public byte Offset_BY { get; set; }
    public byte AnimIndex { get; set; }
    public byte AnimFrame { get; set; }
    public byte SubEtat { get; set; }
    public byte Etat { get; set; }
    public byte Initial_SubEtat { get; set; }
    public byte Initial_Etat { get; set; }
    public uint CMD_CurrentCommand { get; set; }
    public byte Offset_HY { get; set; }
    public byte FollowSprite { get; set; }

    public uint ActualHitPoints
    {
        get => ObjType == R1_ObjType.EDU_VoiceLine ? EDU_ExtHitPoints : HitPoints;
        set
        {
            if (ObjType == R1_ObjType.EDU_VoiceLine)
                EDU_ExtHitPoints = value;

            HitPoints = (byte)(value % 256);
        }
    }

    public byte HitPoints { get; set; }
    public byte Initial_HitPoints { get; set; }
    public byte DisplayPrio { get; set; }
    public byte HitSprite { get; set; }
    public byte PS1_Unk5 { get; set; }
    public byte Byte_7A { get; set; }
    public byte Byte_7B { get; set; }
    public byte CMD_CurrentContext { get; set; }
    public byte Byte_7D { get; set; }
    public byte PS1Demo_Unk5 { get; set; }
    public byte PS1Demo_Unk6 { get; set; }
    public byte PS1Demo_Unk7 { get; set; }
    public byte PS1Demo_Unk8 { get; set; }
    public byte Initial_DisplayPrio { get; set; }
    public byte Byte_7F { get; set; }
    public byte AnimDescriptorCount { get; set; }
    public PC_EventFlags PC_Flags { get; set; }
    public PS1_EventFlags PS1_RuntimeFlags { get; set; }
    public byte PS1_Flags { get; set; }
    public byte PS1_Unk7 { get; set; }
    public ushort Ushort_82 { get; set; }

    public bool IsPCFormat() => EngineVersion >= Rayman1EngineVersion.R1_PC;
    public bool GetFollowEnabled()
    {
        if (IsPCFormat())
        {
            return PC_Flags.HasFlag(PC_EventFlags.FollowEnabled);
        }
        else
        {
            var offset = EngineVersion == Rayman1EngineVersion.R1_Saturn ? 7 : 0;

            return BitHelpers.ExtractBits(PS1_Flags, 1, offset) == 1;
        }
    }
    public void SetFollowEnabled(bool value)
    {
        if (IsPCFormat())
        {
            if (value)
                PC_Flags |= PC_EventFlags.FollowEnabled;
            else
                PC_Flags &= ~PC_EventFlags.FollowEnabled;
        }
        else
        {
            var offset = EngineVersion == Rayman1EngineVersion.R1_Saturn ? 7 : 0;

            PS1_Flags = (byte)BitHelpers.SetBits(PS1_Flags, value ? 1 : 0, 1, offset);
        }
    }

    public R1_AnimationDescriptor[] AnimDescriptors { get; set; }

    public void Serialize(BinarySerializer s)
    {
        Pointer_ImageDescriptors = s.SerializeObject(Pointer_ImageDescriptors, name: nameof(Pointer_ImageDescriptors));
        Pointer_AnimDescriptors = s.SerializeObject(Pointer_AnimDescriptors, name: nameof(Pointer_AnimDescriptors));
        Pointer_ImageBuffer = s.SerializeObject(Pointer_ImageBuffer, name: nameof(Pointer_ImageBuffer));
        Pointer_ETA = s.SerializeObject(Pointer_ETA, name: nameof(Pointer_ETA));
        Pointer_Commands = s.SerializeObject(Pointer_Commands, name: nameof(Pointer_Commands));

        if (EngineVersion == Rayman1EngineVersion.R1_PS1_JPDemoVol3 || EngineVersion == Rayman1EngineVersion.R1_PS1_JPDemoVol6)
        {
            PS1Demo_Unk1 = s.SerializeArray(PS1Demo_Unk1, 40, name: nameof(PS1Demo_Unk1));

            ObjIndex = s.Serialize(ObjIndex, name: nameof(ObjIndex));

            PS1Demo_Unk2 = s.Serialize(PS1Demo_Unk2, name: nameof(PS1Demo_Unk2));
        }
        else
        {
            Pointer_LabelOffsets = s.SerializeObject(Pointer_LabelOffsets, name: nameof(Pointer_LabelOffsets));

            if (!IsPCFormat())
                PS1_Unk1 = s.Serialize(PS1_Unk1, name: nameof(PS1_Unk1));
        }

        if (IsPCFormat())
        {
            CMD_Contexts = s.SerializeObjectArray(CMD_Contexts, 1, name: nameof(CMD_Contexts));
            Uint_1C = s.Serialize(Uint_1C, name: nameof(Uint_1C));
            Uint_20 = s.Serialize(Uint_20, name: nameof(Uint_20));
            IsActive = s.Serialize(IsActive, name: nameof(IsActive));
        }

        if (IsPCFormat())
        {
            XPos = s.Serialize(XPos, name: nameof(XPos));
            YPos = s.Serialize(YPos, name: nameof(YPos));
        }
        else
        {
            XPos = s.Serialize((short)XPos, name: nameof(XPos));
            YPos = s.Serialize((short)YPos, name: nameof(YPos));
        }

        if (EngineVersion == Rayman1EngineVersion.R1_PS1_JPDemoVol3 || EngineVersion == Rayman1EngineVersion.R1_PS1_JPDemoVol6)
        {
            PS1Demo_Unk3 = s.Serialize(PS1Demo_Unk3, name: nameof(PS1Demo_Unk3));
        }
        else
        {
            if (IsPCFormat())
                Uint_30 = s.Serialize(Uint_30, name: nameof(Uint_30));

            ObjIndex = s.Serialize(ObjIndex, name: nameof(ObjIndex));
            XPos_Screen = s.Serialize(XPos_Screen, name: nameof(XPos_Screen));
            YPos_Screen = s.Serialize(YPos_Screen, name: nameof(YPos_Screen));
            Short_3A = s.Serialize(Short_3A, name: nameof(Short_3A));
        }

        Initial_XPos = s.Serialize(Initial_XPos, name: nameof(Initial_XPos));
        Initial_YPos = s.Serialize(Initial_YPos, name: nameof(Initial_YPos));

        if (EngineVersion == Rayman1EngineVersion.R1_PS1_JPDemoVol3)
        {
            PS1Demo_IsFlipped = s.Serialize(PS1Demo_IsFlipped, name: nameof(PS1Demo_IsFlipped));
            PS1Demo_Unk4 = s.Serialize(PS1Demo_Unk4, name: nameof(PS1Demo_Unk4));
        }

        SpeedX = s.Serialize(SpeedX, name: nameof(SpeedX));
        SpeedY = s.Serialize(SpeedY, name: nameof(SpeedY));

        ImageDescriptorCount = s.Serialize(ImageDescriptorCount, name: nameof(ImageDescriptorCount));

        CMD_CurrentOffset = s.Serialize(CMD_CurrentOffset, name: nameof(CMD_CurrentOffset));
        CMD_Arg0 = s.Serialize(CMD_Arg0, name: nameof(CMD_Arg0));

        Short_4A = s.Serialize(Short_4A, name: nameof(Short_4A));
        Short_4C = s.Serialize(Short_4C, name: nameof(Short_4C));
        Short_4E = s.Serialize(Short_4E, name: nameof(Short_4E));

        if (EngineVersion == Rayman1EngineVersion.R1_PC_Kit ||
            EngineVersion == Rayman1EngineVersion.R1_PC_Edu ||
            EngineVersion == Rayman1EngineVersion.R1_PS1_Edu)
            EDU_ExtHitPoints = s.Serialize(EDU_ExtHitPoints, name: nameof(EDU_ExtHitPoints));

        CMD_Arg1 = s.Serialize(CMD_Arg1, name: nameof(CMD_Arg1));
        Short_52 = s.Serialize(Short_52, name: nameof(Short_52));
        Short_54 = s.Serialize(Short_54, name: nameof(Short_54));
        Short_56 = s.Serialize(Short_56, name: nameof(Short_56));

        Short_58 = s.Serialize(Short_58, name: nameof(Short_58));
        Short_5A = s.Serialize(Short_5A, name: nameof(Short_5A));
        TypeZDC = s.Serialize(TypeZDC, name: nameof(TypeZDC));
        Short_5E = s.Serialize(Short_5E, name: nameof(Short_5E));

        if (IsPCFormat())
            ObjType = s.Serialize(ObjType, name: nameof(ObjType));

        CollisionTypes = s.SerializeArray(CollisionTypes, EngineVersion != Rayman1EngineVersion.R1_PS1_JPDemoVol3 ? 5 : 1, name: nameof(CollisionTypes));
        Byte_67 = s.Serialize(Byte_67, name: nameof(Byte_67));

        Offset_BX = s.Serialize(Offset_BX, name: nameof(Offset_BX));
        Offset_BY = s.Serialize(Offset_BY, name: nameof(Offset_BY));

        AnimIndex = s.Serialize(AnimIndex, name: nameof(AnimIndex));
        AnimFrame = s.Serialize(AnimFrame, name: nameof(AnimFrame));

        if (IsPCFormat())
        {
            SubEtat = s.Serialize(SubEtat, name: nameof(SubEtat));
            Etat = s.Serialize(Etat, name: nameof(Etat));

            Initial_SubEtat = s.Serialize(Initial_SubEtat, name: nameof(Initial_SubEtat));
            Initial_Etat = s.Serialize(Initial_Etat, name: nameof(Initial_Etat));
        }
        else
        {
            Etat = s.Serialize(Etat, name: nameof(Etat));
            Initial_Etat = s.Serialize(Initial_Etat, name: nameof(Initial_Etat));
            SubEtat = s.Serialize(SubEtat, name: nameof(SubEtat));
            Initial_SubEtat = s.Serialize(Initial_SubEtat, name: nameof(Initial_SubEtat));
        }

        CMD_CurrentCommand = s.Serialize(CMD_CurrentCommand, name: nameof(CMD_CurrentCommand));

        Offset_HY = s.Serialize(Offset_HY, name: nameof(Offset_HY));

        if (EngineVersion == Rayman1EngineVersion.R1_PS1_JPDemoVol3 || EngineVersion == Rayman1EngineVersion.R1_PS1_JPDemoVol6)
            PS1_Flags = s.Serialize(PS1_Flags, name: nameof(PS1_Flags));

        FollowSprite = s.Serialize(FollowSprite, name: nameof(FollowSprite));
        HitPoints = s.Serialize(HitPoints, name: nameof(HitPoints));
        Initial_HitPoints = s.Serialize(Initial_HitPoints, name: nameof(Initial_HitPoints));
        DisplayPrio = s.Serialize(DisplayPrio, name: nameof(DisplayPrio));

        if (!IsPCFormat())
            ObjType = (R1_ObjType)s.Serialize((byte)ObjType, name: nameof(ObjType));

        HitSprite = s.Serialize(HitSprite, name: nameof(HitSprite));

        if (!IsPCFormat())
            PS1_Unk5 = s.Serialize(PS1_Unk5, name: nameof(PS1_Unk5));

        Byte_7A = s.Serialize(Byte_7A, name: nameof(Byte_7A));
        Byte_7B = s.Serialize(Byte_7B, name: nameof(Byte_7B));
        CMD_CurrentContext = s.Serialize(CMD_CurrentContext, name: nameof(CMD_CurrentContext));
        Byte_7D = s.Serialize(Byte_7D, name: nameof(Byte_7D));

        if (EngineVersion == Rayman1EngineVersion.R1_PS1_JPDemoVol3 || EngineVersion == Rayman1EngineVersion.R1_PS1_JPDemoVol6)
        {
            PS1Demo_Unk5 = s.Serialize(PS1Demo_Unk5, name: nameof(PS1Demo_Unk5));

            if (EngineVersion == Rayman1EngineVersion.R1_PS1_JPDemoVol3)
            {
                PS1Demo_Unk6 = s.Serialize(PS1Demo_Unk6, name: nameof(PS1Demo_Unk6));
                PS1Demo_Unk7 = s.Serialize(PS1Demo_Unk7, name: nameof(PS1Demo_Unk7));
                PS1Demo_Unk8 = s.Serialize(PS1Demo_Unk8, name: nameof(PS1Demo_Unk8));
            }
        }

        Initial_DisplayPrio = s.Serialize(Initial_DisplayPrio, name: nameof(Initial_DisplayPrio));
        Byte_7F = s.Serialize(Byte_7F, name: nameof(Byte_7F));

        AnimDescriptorCount = s.Serialize(AnimDescriptorCount, name: nameof(AnimDescriptorCount));

        if (IsPCFormat())
        {
            PC_Flags = s.Serialize(PC_Flags, name: nameof(PC_Flags));
            Ushort_82 = s.Serialize(Ushort_82, name: nameof(Ushort_82));
        }
        else
        {
            if (EngineVersion != Rayman1EngineVersion.R1_PS1_JPDemoVol3)
            {
                if (EngineVersion != Rayman1EngineVersion.R1_PS1_JPDemoVol6)
                {
                    PS1_RuntimeFlags = s.Serialize(PS1_RuntimeFlags, name: nameof(PS1_RuntimeFlags));
                    PS1_Flags = s.Serialize(PS1_Flags, name: nameof(PS1_Flags));
                }

                PS1_Unk7 = s.Serialize(PS1_Unk7, name: nameof(PS1_Unk7));
            }
        }

        if (AnimDescriptors == null)
            AnimDescriptors = s.DoAt(Pointer_AnimDescriptors, () => s.SerializeObjectArray(AnimDescriptors, AnimDescriptorCount, name: nameof(AnimDescriptors)));
    }

    [Flags]
    public enum PC_EventFlags : byte
    {
        None = 0,
        UnkFlag_0 = 1 << 0,
        Test = 1 << 1,
        SwitchedOn = 1 << 2,
        IsFlipped = 1 << 3,
        UnkFlag_4 = 1 << 4,
        FollowEnabled = 1 << 5,
        UnkFlag_6 = 1 << 6,
        UnkFlag_7 = 1 << 7,
    }

    [Flags]
    public enum PS1_EventFlags : byte
    {
        None = 0,

        UnkFlag_0 = 1 << 0,
        UnkFlag_1 = 1 << 1,
        UnkFlag_2 = 1 << 2,
        SwitchedOn = 1 << 3,
        UnkFlag_4 = 1 << 4,
        UnkFlag_5 = 1 << 5,
        IsFlipped = 1 << 6,
        UnkFlag_7 = 1 << 7,
    }
}