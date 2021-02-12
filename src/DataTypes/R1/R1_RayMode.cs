namespace RayCarrot.BizHawk.R1Tool
{
    // Anything which is not 1-4 will toggle PLACE_RAY. The game sets the mode to 0 when exiting a level. When PLACE_RAY is toggled in-game it inverts the value (so 1 would become -1 and then back to 1, preserving the previous mode)
    public enum R1_RayMode : short
    {
        PLACE_RAY = -1,
        None = 0,
        RAYMAN = 1,
        RAY_ON_MS = 2,
        MORT_DE_RAY_0 = 3,
        MORT_DE_RAY_1 = 4,
    }
}