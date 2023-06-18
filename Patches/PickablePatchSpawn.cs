using System;
using System.Linq;
using AutoMapPins.Model;
using AutoMapPins.Registry;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Pickable), "Awake")]
    internal class PickablePatchSpawn
    {
        private static readonly PinTemplate[] Templates =
        {
            Pin.Name("Pickable_Flint" + Constants.Clone),
            Pin.Name("Pickable_Stone" + Constants.DigitsBracedOptional + Constants.Clone),
            Pin.Name("Pickable_Branch" + Constants.DigitsBracedOptional + Constants.Clone),

            Pin.Name("Pickable_SeedCarrot" + Constants.Clone).Lbl("Carrot Seed").Grp(),
            Pin.Name("Pickable_SeedTurnip" + Constants.Clone).Lbl("Turnip Seed").Grp(),

            Pin.Name("Pickable_SurtlingCoreStand" + Constants.DigitsBracedOptional + Constants.Clone)
                .Lbl("Surtling Core").Grp(), // stays, placed > 5000

            Pin.Name("Pickable_MeatPile" + Constants.DigitsBracedOptional + Constants.Clone) // stays, placed > 5000
                .Lbl("Meat Pile").Nbl(Cat.HARVESTABLES).Grp(),
            Pin.Name("Pickable_MountainCaveCrystal" + Constants.DigitsBracedOptional +
                     Constants.Clone) // stays, placed > 5000
                .Lbl("Cave Crystal").Nbl(Cat.HARVESTABLES).Grp(),

            Pin.Name("Pickable_RoyalJelly" + Constants.Clone).Lbl("Royal Jelly").Nbl(Cat.HARVESTABLES)
                .Grp(), // disappears, placed > 5000
            Pin.Name("Pickable_BlackCoreStand" + Constants.DigitsBracedOptional +
                     Constants.Clone) // stays, placed > 5000
                .Lbl("Black Core").Nbl(Cat.HARVESTABLES).Grp(),

            Pin.Name("Pickable_Fishingrod" + Constants.Clone),
        };

        [UsedImplicitly]
        private static void Postfix(ref Pickable instance)
        {
            Pickable obj = instance;
            var template = Templates.FirstOrDefault(t => t.IsMatch(obj)) ??
                           AutoMapPinsPlugin.TEMPLATE_REGISTRY.Find(instance);

            if (template == null)
            {
                AutoMapPinsPlugin.LogUnmatchedName(typeof(Pickable), obj.name);
                AutoMapPinsPlugin.LogUnmatchedHover(typeof(Pickable), obj.GetComponent<HoverText>()?.m_text);
            }
            else if (!String.IsNullOrWhiteSpace(template.Label))
            {
                var height = instance.gameObject.transform.position.y;

                if (height < Constants.MaxPinHeight)
                {
                    var pin = instance.gameObject.AddComponent<PinnedObject>();
                    pin.Init(template);
                }
            }
        }
    }
}