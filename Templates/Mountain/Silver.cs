using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mountain
{
    [Template]
    internal class Silver : MineTemplate
    {
        public Silver()
        {
            Matcher = new HoverTextRegex("\\$piece_deposit_silver(vein)?");
            FirstBiome = Biome.MOUNTAINS;
            SingleLabel = "Silver";
            MultipleLabel = "{0} Silver";
        }
    }

    [Template]
    internal class PartialSilver : MineTemplate
    {
        public PartialSilver()
        {
            InGameType = typeof(MineRock5);
            Matcher = new InstanceNameRegex("silver(vein)?_frac" + Constants.Clone);
            FirstBiome = Biome.MOUNTAINS;
            SingleLabel = "Partial Silver";
            MultipleLabel = "{0} Partial Silver";
        }
    }
}