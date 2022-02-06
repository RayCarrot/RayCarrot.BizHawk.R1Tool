using BizHawk.Client.Common;

namespace RayCarrot.BizHawk.R1Tool;

public interface IBizHawkAPI
{
    IMemoryApi Mem { get; }
    IGuiApi Gui { get; }
    IEmulationApi Emu { get; }
    IEmuClientApi EmuClient { get; }
    IGameInfoApi GameInfo { get; }
    IInputApi Input { get; }
    IUserDataApi Data { get; }
    IMemoryEventsApi MemEvents { get; }
}