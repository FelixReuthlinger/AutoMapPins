using AutoMapPins.Model;

namespace AutoMapPins.Templates.BlackForest
{
    [Template]
    internal class TrollCave : DungeonTemplate
    {
        public TrollCave()
        {
            Matcher = new InstanceNameRegex("TrollCave" + Constants.Digits + Constants.Clone);
            FirstBiome = Biome.BLACK_FORESTS;
            SingleLabel = "Troll Cave";
            MultipleLabel = "{0} Troll Caves";
        }
    }
}