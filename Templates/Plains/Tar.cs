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
    internal class Tar : PickableTemplate
    {
        public Tar()
        {
            // Disappears
            Matcher = new InstanceNameRegex("Pickable_Tar(Big)?" + Constants.DigitsBracedOptional + Constants.Clone);
            FirstBiome = Biome.PLAINS;
            SingleLabel = "Tar";
            MultipleLabel = "{0} Tar";
        }
    }
}
