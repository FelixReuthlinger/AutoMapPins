using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mountain
{
    [Template]
    internal class MountainRemains : RemainsTemplate
    {
        public MountainRemains()
        {
            Matcher = new InstanceNameRegex("Pickable_MountainRemains" + Constants.Digits + "_buried" + Constants.Clone);
            FirstBiome = Biome.MOUNTAINS;
            SingleLabel = "M. Remains";
            MultipleLabel = "{0} M. Remains";
        }
    }
}