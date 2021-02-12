namespace RayCarrot.BizHawk.R1Tool
{
    public class R1_mp : IBinarySerializable
    {
        public short Width { get; set; }
        public short Height { get; set; }
        public int TileMapLength { get; set; }
        public Pointer32 Pointer_TileMap { get; set; }
        public short[] TileMap { get; set; }

        public void Serialize(BinarySerializer s)
        {
            Width = s.Serialize(Width, name: nameof(Width));
            Height = s.Serialize(Height, name: nameof(Height));
            TileMapLength = s.Serialize(TileMapLength, name: nameof(TileMapLength));
            Pointer_TileMap = s.SerializeObject(Pointer_TileMap, name: nameof(Pointer_TileMap));
            TileMap = s.DoAt(Pointer_TileMap, () => s.SerializeArray(TileMap, TileMapLength, name: nameof(TileMap)));
        }
    }
}