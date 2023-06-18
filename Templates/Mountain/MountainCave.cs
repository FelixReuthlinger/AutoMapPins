using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mountain
{
    [Template]
    internal class MountainCave : DungeonTemplate
    {
        public MountainCave()
        {
            Matcher = new InstanceNameRegex("MountainCave" + Constants.Digits + Constants.Clone);
            FirstBiome = Biome.MOUNTAINS;
            SingleLabel = "Mountain Cave";
            MultipleLabel = "{0} Mountain Caves";
        }
    }
}