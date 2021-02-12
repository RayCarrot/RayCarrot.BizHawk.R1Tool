namespace RayCarrot.BizHawk.R1Tool
{
    public abstract class Pointer : IBinarySerializable
    {
        public abstract long Address { get; set; }
        public abstract void Serialize(BinarySerializer s);

        public static implicit operator Pointer(long addr)
        {
            return new Pointer64()
            {
                Address = addr
            };
        }
    }
}