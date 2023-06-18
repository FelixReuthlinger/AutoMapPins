using AutoMapPins.Model;

namespace AutoMapPins.Templates.Plains
{
    [Template]
    internal class Cloudberry : BerryTemplate
    {
        public Cloudberry()
        {
            // Stays
            Matcher = new InstanceNameRegex("CloudberryBush" + Constants.Clone);
            FirstBiome = Biome.PLAINS;
            SingleLabel = "Cloudberry";
            MultipleLabel = "{0} Cloudberrys";
        }
    }
}