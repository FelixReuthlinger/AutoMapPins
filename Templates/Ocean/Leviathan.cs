using AutoMapPins.Model;

namespace AutoMapPins.Templates.Ocean
{
    [Template]
    internal class Leviathan : MineTemplate
    {
        public Leviathan()
        {
            // Stays
            InGameType = typeof(MineRock);
            Matcher = new InstanceNameRegex("Leviathan" + Constants.Clone);
            FirstBiome = Biome.OCEANS;
            SingleLabel = "Leviathan";
            MultipleLabel = "{0} Leviathans";
        }
    }
}