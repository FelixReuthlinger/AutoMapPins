using AutoMapPins.Model;

namespace AutoMapPins.Templates.Meadows
{
    [Template]
    internal class ForestRemains : RemainsTemplate
    {
        public ForestRemains()
        {
            // Stays
            Matcher = new InstanceNameRegex("Pickable_ForestCryptRemains" + Constants.Digits + Constants.DigitsBracedOptional + Constants.Clone);
            FirstBiome = Biome.MEADOWS;
            SingleLabel = "F. Remains";
            MultipleLabel = "{0} F. Remains";
        }
    }
}