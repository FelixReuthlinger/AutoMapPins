using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mistlands
{
    [Template]
    internal class DvergrLantern : PickableTemplate
    {
        public DvergrLantern()
        {
            // Disappears
            Matcher = new InstanceNameRegex("Pickable_DvergrLantern" + Constants.DigitsBracedOptional + Constants.Clone);
            FirstBiome = Biome.MISTLANDS;
            SingleLabel = "Lantern";
            MultipleLabel = "{0} Lanterns";
        }
    }
}