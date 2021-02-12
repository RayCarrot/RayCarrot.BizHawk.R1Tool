namespace RayCarrot.BizHawk.R1Tool
{
    public interface IGameVersion
    {
        Platform Platform { get; }
        MenuItem[] Menu { get; }
        MenuItem[] SettingsMenuItems { get; }
        ValueItem[] Values { get; }
        bool IsPaused { set; }

        void Update(bool isActive);
        void UpdateUI(bool isActive);
        void LogData();
    }
}