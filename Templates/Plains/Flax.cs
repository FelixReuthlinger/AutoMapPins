using AutoMapPins.Model;

namespace AutoMapPins.Templates.Plains
{
    [Template]
    internal class Flax : SeedTemplate
    {
        public Flax()
        {
            // Disappears
            // Overlaps with Pickable
            Matcher = new InstanceNameRegex("Pickable_Flax_Wild" + Constants.DigitsBracedOptional + Constants.Clone);
            FirstBiome = Biome.PLAINS;
            SingleLabel = "Flax";
            MultipleLabel = "{0} Flax";
        }
    }
}