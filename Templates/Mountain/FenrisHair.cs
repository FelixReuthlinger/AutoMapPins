using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mountain
{
    [Template]
    internal class FenrisHair : PickableTemplate
    {
        public FenrisHair()
        {
            // Stays
            Matcher = new InstanceNameRegex("hanging_hairstrands" + Constants.DigitsBracedOptional + Constants.Clone);
            FirstBiome = Biome.MOUNTAINS;
            SingleLabel = "Fenris Hairstrand";
            MultipleLabel = "{0} Fenris Hairstrands";
        }
    }
}