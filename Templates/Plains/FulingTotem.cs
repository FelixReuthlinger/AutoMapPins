using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapPins.Model;
using UnityEngine;

namespace AutoMapPins.Templates.Plains
{
    [Template]
    internal class FulingTotem : PickableTemplate
    {
        public FulingTotem()
        {
            // Stays
            Matcher = new InstanceNameRegex("goblin_totempole" + Constants.Clone);
            FirstBiome = Biome.PLAINS;
            SingleLabel = "Fuling Totem";
            MultipleLabel = "{0} Fuling Totems";
        }
    }
}
