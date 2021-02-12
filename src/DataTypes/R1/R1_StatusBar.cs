namespace RayCarrot.BizHawk.R1Tool
{
    public class R1_StatusBar : IBinarySerializable
    {
        public sbyte LivesCount { get; set; }
        public byte Unk_01 { get; set; } // Related to the graphics index for the lives count
        public byte LivesDigit0 { get; set; }
        public byte LivesDigit1 { get; set; }
        public byte Unk_04 { get; set; }
        public byte Unk_05 { get; set; }
        public byte TingsCount { get; set; }
        public byte TingsDigit0 { get; set; }
        public byte TingsDigit1 { get; set; }
        public byte MaxHealth { get; set; }

        public void Serialize(BinarySerializer s)
        {
            LivesCount = s.Serialize<sbyte>(LivesCount, name: nameof(LivesCount));
            Unk_01 = s.Serialize<byte>(Unk_01, name: nameof(Unk_01));
            LivesDigit0 = s.Serialize<byte>(LivesDigit0, name: nameof(LivesDigit0));
            LivesDigit1 = s.Serialize<byte>(LivesDigit1, name: nameof(LivesDigit1));
            Unk_04 = s.Serialize<byte>(Unk_04, name: nameof(Unk_04));
            Unk_05 = s.Serialize<byte>(Unk_05, name: nameof(Unk_05));
            TingsCount = s.Serialize<byte>(TingsCount, name: nameof(TingsCount));
            TingsDigit0 = s.Serialize<byte>(TingsDigit0, name: nameof(TingsDigit0));
            TingsDigit1 = s.Serialize<byte>(TingsDigit1, name: nameof(TingsDigit1));
            MaxHealth = s.Serialize<byte>(MaxHealth, name: nameof(MaxHealth));
        }
    }
}