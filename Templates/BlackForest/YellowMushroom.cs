using AutoMapPins.Model;

namespace AutoMapPins.Templates.BlackForest
{
    [Template]
    internal class YellowMushroom : BerryTemplate
    {
        public YellowMushroom()
        {
            // Stays
            Matcher = new InstanceNameRegex("Pickable_Mushroom_yellow" + Constants.DigitsBracedOptional + Constants.Clone);
            FirstBiome = Biome.BLACK_FORESTS;
            SingleLabel = "Yellow Mushroom";
            MultipleLabel = "{0} Yellow Mushrooms";
        }
    }
}