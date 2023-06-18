using AutoMapPins.Model;

namespace AutoMapPins.Templates.Meadows
{
    [Template]
    internal class Raspberry : BerryTemplate
    {
        public Raspberry()
        {
            // Stays
            Matcher = new InstanceNameRegex("RaspberryBush" + Constants.Clone);
            FirstBiome = Biome.MEADOWS;
            SingleLabel = "Raspberry";
            MultipleLabel = "{0} Raspberries";
        }
    }
}