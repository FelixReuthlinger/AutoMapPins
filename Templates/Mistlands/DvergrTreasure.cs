using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mistlands
{
    [Template]
    internal class DvergrTreasure : PickableTemplate
    {
        public DvergrTreasure()
        {
            Matcher = new InstanceNameRegex("Pickable_DvergrMineTreasure" + Constants.DigitsBracedOptional + Constants.Clone);
            FirstBiome = Biome.MISTLANDS;
            SingleLabel = "Dvergr Treasure";
            MultipleLabel = "{0} Dvergr Treasures";
        }
    }
}