namespace RayCarrot.BizHawk.R1Tool
{
    public class Pointer32 : Pointer
    {
        public override long Address
        {
            get => Address_32;
            set => Address_32 = (uint)value;
        }

        public uint Address_32 { get; set; }

        public override void Serialize(BinarySerializer s)
        {
            Address_32 = s.Serialize(Address_32, name: nameof(Address_32));
        }
    }
}