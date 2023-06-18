using AutoMapPins.Model;

namespace AutoMapPins.Templates.Plains
{
    [Template]
    internal class Barley : SeedTemplate
    {
        public Barley()
        {
            // Disappears
            // Overlaps with Pickable
            Matcher = new InstanceNameRegex("Pickable_Barley_Wild" + Constants.DigitsBracedOptional + Constants.Clone);
            FirstBiome = Biome.PLAINS;
            SingleLabel = "Barley";
            MultipleLabel = "{0} Barley";
        }
    }
}