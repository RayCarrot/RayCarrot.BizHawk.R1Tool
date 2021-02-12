namespace RayCarrot.BizHawk.R1Tool
{
    public class R1_AnimationDescriptor : IBinarySerializable
    {
        public Pointer32 Pointer_AnimLayers { get; set; }
        public Pointer32 Pointer_AnimFrames { get; set; }

        public ushort LayersPerFrameSerialized { get; set; }
        public byte LayersPerFrame => (byte)(LayersPerFrameSerialized & 0xFF);

        public ushort FrameCountSerialized { get; set; }
        public byte FrameCount => (byte)(FrameCountSerialized & 0xFF);

        public R1_AnimationLayer[] Layers { get; set; }
        public R1_AnimationFrame[] Frames { get; set; }

        public void Serialize(BinarySerializer s)
        {
            // Serialize pointers
            Pointer_AnimLayers = s.SerializeObject(Pointer_AnimLayers, name: nameof(Pointer_AnimLayers));
            Pointer_AnimFrames = s.SerializeObject(Pointer_AnimFrames, name: nameof(Pointer_AnimFrames));

            // Serialize data
            LayersPerFrameSerialized = s.Serialize<ushort>(LayersPerFrameSerialized, name: nameof(LayersPerFrameSerialized));
            FrameCountSerialized = s.Serialize<ushort>(FrameCountSerialized, name: nameof(FrameCountSerialized));

            // Serialize data from pointers
            s.DoAt(Pointer_AnimLayers, () => Layers = s.SerializeObjectArray(Layers, LayersPerFrame * FrameCount, name: nameof(Layers)));
            s.DoAt(Pointer_AnimFrames, () => Frames = s.SerializeObjectArray(Frames, FrameCount, name: nameof(Frames)));
        }
    }
}