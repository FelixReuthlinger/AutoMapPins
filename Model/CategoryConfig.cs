using System.Collections.Generic;
using System.Linq;

namespace AutoMapPins.Model
{
    public class CategoryConfig
    {
        public bool CategoryActive;
        public Dictionary<string, PinConfig> Pins = null!;

        public static Dictionary<string, CategoryConfig> FromPins(List<PinConfig> pins)
        {
            return pins.GroupBy(pin => pin.CategoryName)
                .ToDictionary(
                    categoryGroup => categoryGroup.Key,
                    categoryGroup =>
                    {
                        var result = new CategoryConfig
                        {
                            CategoryActive = Common.DefaultFalse,
                            Pins = categoryGroup
                                .GroupBy(pin => pin.InternalName)
                                .ToDictionary(group => group.Key, group => group.First()
                                )
                        };
                        return result;
                    }
                );
        }
    }
}