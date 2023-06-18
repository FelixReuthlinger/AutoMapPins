using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mountain
{
    [Template]
    internal class DragonEgg : PickableTemplate
    {
        public DragonEgg()
        {
            // Stays
            Matcher = new InstanceNameRegex("Pickable_DragonEgg" + Constants.Clone);
            FirstBiome = Biome.MOUNTAINS;
            SingleLabel = "Dragon Egg";
            MultipleLabel = "{0} Dragon Eggs";
        }
    }
}