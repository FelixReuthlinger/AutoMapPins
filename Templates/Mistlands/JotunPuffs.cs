using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mistlands
{
    [Template]
    internal class JotunPuffs : MushroomTemplate
    {
        public JotunPuffs()
        {
            // Stays
            Matcher = new InstanceNameRegex("Pickable_Mushroom_JotunPuffs" + Constants.Clone);
            FirstBiome = Biome.MISTLANDS;
            SingleLabel = "Jotun Puffs";
            MultipleLabel = "{0} Jotun Puffs";
        }
    }
}