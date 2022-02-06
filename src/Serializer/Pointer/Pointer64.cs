namespace RayCarrot.BizHawk.R1Tool;

public class Pointer64 : Pointer
{
    public override long Address { get; set; }

    public override void Serialize(BinarySerializer s)
    {
        Address = s.Serialize(Address, name: nameof(Address));
    }
}