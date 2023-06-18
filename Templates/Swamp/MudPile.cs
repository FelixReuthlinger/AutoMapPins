using AutoMapPins.Model;

namespace AutoMapPins.Templates.Swamp
{
    [Template]
    internal class MudPile : MineTemplate
    {
        public MudPile()
        {
            Matcher = new HoverTextRegex("\\$piece_mudpile");
            FirstBiome = Biome.SWAMPS;
            SingleLabel = "Iron";
            MultipleLabel = "{0} Iron";
        }
    }

    [Template]
    internal class PartialMudPile : MineTemplate
    {
        public PartialMudPile()
        {
            InGameType = typeof(MineRock5);
            Matcher = new InstanceNameRegex("mudpile_frac" + Constants.Clone);
            FirstBiome = Biome.SWAMPS;
            SingleLabel = "Partial Iron";
            MultipleLabel = "{0} Partial Iron";
        }
    }
}