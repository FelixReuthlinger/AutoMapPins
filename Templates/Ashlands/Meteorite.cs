using AutoMapPins.Model;

namespace AutoMapPins.Templates.Ashlands
{
    [Template]
    internal class Meteorite : MineTemplate
    {
        public Meteorite()
        {
            InGameType = typeof(MineRock);
            Matcher = new InstanceNameRegex("MineRock_Meteorite" + Constants.Clone);
            FirstBiome = Biome.ASHLANDS;
            SingleLabel = "Glowing Metal";
            MultipleLabel = "{0} Glowing Metals";
        }
    }
}