using AutoMapPins.Model;

namespace AutoMapPins.Templates.Mistlands
{
    [Template]
    internal class YggaShoot : AxeTemplate
    {
        public YggaShoot()
        {
            Matcher = new HoverTextRegex("\\$prop_yggashoot");
            FirstBiome = Biome.MISTLANDS;
            SingleLabel = "Yggdrasil Shoot";
            MultipleLabel = "{0} Yggdrasil Shoots";
        }
    }
}