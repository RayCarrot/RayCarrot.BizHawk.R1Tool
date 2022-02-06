using System;

namespace RayCarrot.BizHawk.R1Tool;

public class R1_AnimationLayer : IBinarySerializable
{
    public bool IsFlippedHorizontally
    {
        get => Flags.HasFlag(Common_AnimationLayerFlags.FlippedHorizontally);
        set
        {
            if (value)
                Flags |= Common_AnimationLayerFlags.FlippedHorizontally;
            else
                Flags &= ~Common_AnimationLayerFlags.FlippedHorizontally;
        }
    }
    public bool IsFlippedVertically
    {
        get => Flags.HasFlag(Common_AnimationLayerFlags.FlippedVertically);
        set
        {
            if (value)
                Flags |= Common_AnimationLayerFlags.FlippedVertically;
            else
                Flags &= ~Common_AnimationLayerFlags.FlippedVertically;
        }
    }
    public Common_AnimationLayerFlags Flags { get; set; }
    public byte XPosition { get; set; }
    public byte YPosition { get; set; }
    public ushort ImageIndex { get; set; }

    public void Serialize(BinarySerializer s)
    {
        //if (s.GameSettings.EngineVersion == EngineVersion.R2_PS1)
        //{
        //    s.SerializeBitValues<ushort>(bitFunc =>
        //    {
        //        ImageIndex = (ushort)bitFunc(ImageIndex, 12, name: nameof(ImageIndex));
        //        Flags = (Common_AnimationLayerFlags)bitFunc((byte)Flags, 4, name: nameof(Flags));
        //    });

        //    XPosition = s.Serialize<byte>(XPosition, name: nameof(XPosition));
        //    YPosition = s.Serialize<byte>(YPosition, name: nameof(YPosition));
        //}
        //else
        //{
        IsFlippedHorizontally = s.Serialize<bool>(IsFlippedHorizontally, name: nameof(IsFlippedHorizontally));
        XPosition = s.Serialize<byte>(XPosition, name: nameof(XPosition));
        YPosition = s.Serialize<byte>(YPosition, name: nameof(YPosition));
        ImageIndex = s.Serialize<byte>((byte)ImageIndex, name: nameof(ImageIndex));
        //}
    }

    [Flags]
    public enum Common_AnimationLayerFlags
    {
        None = 0,
        UnkFlag_0 = 1 << 0,
        UnkFlag_1 = 1 << 1,
        FlippedHorizontally = 1 << 2,
        FlippedVertically = 1 << 3,
    }
}