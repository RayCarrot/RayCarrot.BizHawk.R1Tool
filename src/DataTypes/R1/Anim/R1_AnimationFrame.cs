namespace RayCarrot.BizHawk.R1Tool
{
    public class R1_AnimationFrame : IBinarySerializable
    {
        public byte XPos { get; set; }
        public byte YPos { get; set; }
        public byte Width { get; set; }
        public byte Height { get; set; }

        public void Serialize(BinarySerializer s)
        {
            XPos = s.Serialize<byte>(XPos, name: nameof(XPos));
            YPos = s.Serialize<byte>(YPos, name: nameof(YPos));
            Width = s.Serialize<byte>(Width, name: nameof(Width));
            Height = s.Serialize<byte>(Height, name: nameof(Height));
        }
    }
}