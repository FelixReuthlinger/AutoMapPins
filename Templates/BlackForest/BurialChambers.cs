using AutoMapPins.Model;

namespace AutoMapPins.Templates.BlackForest
{
    [Template]
    internal class BurialChambers : DungeonTemplate
    {
        public BurialChambers()
        {
            Matcher = new InstanceNameRegex("^Crypt" + Constants.Digits + Constants.Clone);
            FirstBiome = Biome.BLACK_FORESTS;
            SingleLabel = "Burial Chambers";
            MultipleLabel = "{0} Burial Chambers";
        }
    }
}