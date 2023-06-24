using System.Collections.Generic;

namespace AutoMapPins.Model
{
    // NOTE: class and fields need to be public for YAML serde
    public class CategoryConfig
    {
        public bool CategoryActive;
        public Dictionary<string, PinConfig> Pins = new();
    }
}