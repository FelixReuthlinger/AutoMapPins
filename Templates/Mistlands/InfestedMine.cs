using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mistlands
{
    [Template]
    internal class InfestedMine : DungeonTemplate
    {
        public InfestedMine()
        {
            Matcher = new InstanceNameRegex("Mistlands_DvergrTownEntrance" + Constants.Digits + Constants.Clone);
            FirstBiome = Biome.MISTLANDS;
            SingleLabel = "Infested Mine";
            MultipleLabel = "{0} Infested Mines";
        }
    }
}