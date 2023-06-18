using AutoMapPins.Model;

namespace AutoMapPins.Templates.BlackForest
{
    [Template]
    internal class CarrotSeeds : SeedTemplate
    {
        public CarrotSeeds()
        {
            // Disappears
            // Overlaps with Pickable
            Matcher = new InstanceNameRegex("Pickable_SeedCarrot" + Constants.Clone);
            FirstBiome = Biome.BLACK_FORESTS;
            SingleLabel = "Carrot Seeds";
            MultipleLabel = "{0} Carrot Seeds";
        }
    }
}