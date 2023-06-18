using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mistlands
{
    [Template]
    internal class GiantBrain : MineTemplate
    {
        public GiantBrain()
        {
            Matcher = new HoverTextRegex("\\$piece_giant_brain");
            FirstBiome = Biome.MISTLANDS;
            SingleLabel = "Giant Brain";
            MultipleLabel = "{0} Giant Brains";
        }
    }
}