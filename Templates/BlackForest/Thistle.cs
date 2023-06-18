using AutoMapPins.Model;

namespace AutoMapPins.Templates.BlackForest
{
    [Template]
    internal class Thistle : FlowerTemplate
    {
        public Thistle()
        {
            // Stays
            Matcher = new InstanceNameRegex("Pickable_Thistle" + Constants.Clone);
            FirstBiome = Biome.BLACK_FORESTS;
            SingleLabel = "Thistle";
            MultipleLabel = "{0} Thistles";
        }
    }
}