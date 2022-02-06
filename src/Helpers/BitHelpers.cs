namespace RayCarrot.BizHawk.R1Tool;

public static class BitHelpers
{
    public static int ExtractBits(int value, int count, int offset)
    {
        return (((1 << count) - 1) & (value >> (offset)));
    }

    public static int SetBits(int bits, int value, int count, int offset)
    {
        int mask = ((1 << count) - 1) << offset;
        bits = (bits & ~mask) | (value << offset);
        return bits;
    }

    public static int ReverseBits(int value)
    {
        var result = 0;

        for (int i = 0; i < 32; i++)
            result = SetBits(result, ExtractBits(value, 1, i), 1, 32 - i - 1);

        return result;
    }
}