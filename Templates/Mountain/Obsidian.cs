using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mountain
{
    [Template]
    internal class Obsidian : MineTemplate
    {
        public Obsidian()
        {
            GroupingDistance = Constants.DistanceObsidian;
            Matcher = new HoverTextRegex("\\$piece_deposit_obsidian");
            FirstBiome = Biome.MOUNTAINS;
            SingleLabel = "Obsidian";
            MultipleLabel = "{0} Obsidian";
        }
    }
}