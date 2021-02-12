using System;

namespace RayCarrot.BizHawk.R1Tool
{
    public class MenuItem
    {
        public Func<string>? Title { get; set; }
        public MenuItem[] Children { get; set; } = new MenuItem[0];
        public string[] Options { get; set; } = new string[0];
        public Func<MenuItem, bool>? OnSelected { get; set; }

        public int SelectedOption { get; set; }
        public string DisplayName => Title?.Invoke() ?? Options[SelectedOption];

        public bool CanIncreaseOptionsIndex => SelectedOption < Options.Length - 1;
        public bool CanDecreaseOptionsIndex => SelectedOption > 0;
    }
}