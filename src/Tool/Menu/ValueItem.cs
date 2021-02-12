using System;

namespace RayCarrot.BizHawk.R1Tool
{
    public class ValueItem
    {
        public ValueItem(string name, Func<string> getValueFunc)
        {
            Name = name;
            GetValueFunc = getValueFunc;
        }

        public string Name { get; }
        public Func<string> GetValueFunc { get; }

        public string DisplayText => $"{Name}: {GetValueFunc()}";
    }
}