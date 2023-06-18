using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapPins.Model;
using UnityEngine;

namespace AutoMapPins.Templates.Swamp
{
    [Template]
    internal class TurnipSeeds : SeedTemplate
    {
        public TurnipSeeds()
        {
            // Disappears
            // Overlaps with Pickable
            Matcher = new InstanceNameRegex("Pickable_SeedTurnip" + Constants.Clone);
            FirstBiome = Biome.SWAMPS;
            SingleLabel = "Turnip Seeds";
            MultipleLabel = "{0} Turnip Seeds";
        }
    }
}
