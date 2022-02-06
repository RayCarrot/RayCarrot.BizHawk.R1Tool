namespace RayCarrot.BizHawk.R1Tool;

public class R1_CommandContext : IBinarySerializable
{
    /// <summary>
    /// The offset where the context was stored, used to remember where to jump back to after execution of the sub-function has finished
    /// </summary>
    public ushort CmdOffset { get; set; }

    /// <summary>
    /// The amount of times the execution should repeat before continuing, used for loops
    /// </summary>
    public ushort Count { get; set; }

    public void Serialize(BinarySerializer s)
    {
        CmdOffset = s.Serialize<ushort>(CmdOffset, name: nameof(CmdOffset));
        Count = s.Serialize<ushort>(Count, name: nameof(Count));
    }
}