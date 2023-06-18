using AutoMapPins.Model;

namespace AutoMapPins.Templates.Meadows
{
    [Template]
    internal class RedMushroom : MushroomTemplate
    {
        public RedMushroom()
        {
            // Stays
            Matcher = new InstanceNameRegex("Pickable_Mushroom" + Constants.DigitsBracedOptional + Constants.Clone);
            FirstBiome = Biome.MEADOWS;
            SingleLabel = "Mushroom";
            MultipleLabel = "{0} Mushrooms";
        }
    }
}