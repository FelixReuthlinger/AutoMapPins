using AutoMapPins.Model;

namespace AutoMapPins.Templates.BlackForest
{
    [Template]
    internal class Blueberry : BerryTemplate
    {
        public Blueberry()
        {
            // Stays
            Matcher = new InstanceNameRegex("BlueberryBush" + Constants.Clone);
            FirstBiome = Biome.BLACK_FORESTS;
            SingleLabel = "Blueberry";
            MultipleLabel = "{0} Blueberries";
        }
    }
}