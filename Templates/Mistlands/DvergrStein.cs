using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mistlands
{
    [Template]
    internal class DvergrStein : PickableTemplate
    {
        public DvergrStein()
        {
            Matcher = new InstanceNameRegex("Pickable_DvergrStein" + Constants.Clone);
            FirstBiome = Biome.MISTLANDS;
            SingleLabel = "Dvergr Stein";
            MultipleLabel = "{0} Dvergr Stein";
        }
    }
}