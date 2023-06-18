using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mistlands
{
    [Template]
    internal class Magecap : MushroomTemplate
    {
        public Magecap()
        {
            // Stays
            Matcher = new InstanceNameRegex("Pickable_Mushroom_Magecap" +Constants.Clone);
            FirstBiome = Biome.MISTLANDS;
            SingleLabel = "Magecap";
            MultipleLabel = "{0} Magecaps";
        }
    }
}