using AutoMapPins.Model;

namespace AutoMapPins.Templates.Meadows
{
    [Template]
    internal class Dandelion : FlowerTemplate
    {
        public Dandelion()
        {
            // Stays
            Matcher = new InstanceNameRegex("Pickable_Dandelion" + Constants.DigitsBracedOptional + Constants.Clone);
            FirstBiome = Biome.MEADOWS;
            SingleLabel = "Dandelion";
            MultipleLabel = "{0} Dandelions";
        }
    }
}