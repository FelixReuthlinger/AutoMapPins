using System.Linq;
using AutoMapPins.Model;
using AutoMapPins.Registry;
using AutoMapPins.Templates;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Location), "Awake")]
    class LocationPatchSpawn
    {
        private static readonly PinTemplate[] Templates =
        {
            Pin.Name("StoneHouse" + Constants.Digits).Lbl("Stone House"), // Location with loot
            Pin.Name("StoneTower" + Constants.Digits).Lbl("Stone Tower"), // Location with loot
            Pin.Name("Waymarker" + Constants.Digits).Lbl("Waymarker"), // Point of interest
            //Pin.Make(Pin.Name("" + Mod.DIGITS)),

            Pin.Name("AbandonedLogCabin" + Constants.Digits + Constants.Clone)
                .Lbl("Log Cabin"), // Location with loot
            Pin.Name("Dolmen" + Constants.Digits + Constants.Clone).Lbl("Dolmen"),
            Pin.Name("DrakeNest" + Constants.Digits + Constants.Clone)
                .Lbl("Drake Nest"), // tested overlaps with dragon egg
            Pin.Name("GoblinCamp" + Constants.Digits + Constants.Clone).Lbl("Goblin Camp"), // tested
            Pin.Name("Grave" + Constants.Digits + Constants.Clone)
                .Lbl("Grave"), // Point of interest / Spawner in Swamp
            Pin.Name("Greydwarf_camp" + Constants.Digits + Constants.Clone)
                .Lbl("Greydwarf Nest (Spawner)"), // tested
            Pin.Name("InfestedTree" + Constants.Digits + Constants.Clone)
                .Lbl("Infested Tree"), // tested overlaps with guck sacks
            Pin.Name("MountainGrave" + Constants.Digits + Constants.Clone).Lbl("Mountain Grave"),
            Pin.Name("MountainWell" + Constants.Digits + Constants.Clone)
                .Lbl("Mountain Well"), // Point of interest
            Pin.Name("Mistlands_Excavation" + Constants.Digits + Constants.Clone)
                .Lbl("Excavation"), // Location with loot
            Pin.Name("Mistlands_Giant" + Constants.Digits + Constants.Clone)
                .Lbl("Giant"), // petrified bone
            Pin.Name("Mistlands_GuardTower" + Constants.Digits + "_new" + Constants.Clone)
                .Lbl("Guard Tower"), // Location with loot
            Pin.Name("Mistlands_GuardTower" + Constants.Digits + "_ruined_new\\d*" + Constants.Clone)
                .Lbl("Ruined Guard Tower"), // Point of interest
            Pin.Name("Mistlands_Lighthouse" + Constants.Digits + "_new" + Constants.Clone)
                .Lbl("Lighthouse"), // Location with loot
            Pin.Name("Mistlands_RoadPost" + Constants.Digits + Constants.Clone)
                .Lbl("Road Post"), // Has a "lanternpost"
            Pin.Name("Mistlands_RockSpire" + Constants.Digits + Constants.Clone)
                .Lbl("Rock Spire"), // Point of interest
            Pin.Name("Mistlands_Statue" + Constants.Digits + Constants.Clone)
                .Lbl("Statue"), // Point of interest
            Pin.Name("Mistlands_StatueGroup" + Constants.Digits + Constants.Clone)
                .Lbl("Statue Group"), // Point of interest
            Pin.Name("Mistlands_Swords" + Constants.Digits + Constants.Clone)
                .Lbl("Ancient Armor"), // Point of interest
            Pin.Name("Mistlands_Harbour" + Constants.Digits + Constants.Clone)
                .Lbl("Harbour"), // Location with loot
            Pin.Name("Mistlands_Viaduct" + Constants.Digits + Constants.Clone)
                .Lbl("Viaduct"), // Point of interest
            Pin.Name("Ruin" + Constants.Digits + Constants.Clone)
                .Lbl("Ruin"), // Swamp Tower Spawner / Plains Location with loot
            Pin.Name("ShipSetting" + Constants.Digits + Constants.Clone)
                .Lbl("Ship Stonering"), // Point of interest
            Pin.Name("ShipWreck" + Constants.Digits + Constants.Clone)
                .Lbl("Ship Wreck"), // Location with loot
            Pin.Name("StoneHenge" + Constants.Digits + Constants.Clone)
                .Lbl("Stonehenge"), // Point of interest
            Pin.Name("StoneTowerRuins" + Constants.Digits + Constants.Clone)
                .Lbl("Stone Tower Ruins"), // Location with loot
            Pin.Name("SwampHut" + Constants.Digits + Constants.Clone)
                .Lbl("Swamp Hut"), // Point of interest
            Pin.Name("SwampWell" + Constants.Digits + Constants.Clone)
                .Lbl("Swamp Well"), // Point of interest
            Pin.Name("TarPit" + Constants.Digits + Constants.Clone),
            Pin.Name("WoodHouse" + Constants.Digits + Constants.Clone)
                .Lbl("Wood House"), // Location with loot
            Pin.Name("WoodVillage" + Constants.Digits + Constants.Clone)
                .Lbl("Wood Village"), // Location with loot
            //Pin.Make(Pin.Name("" + Mod.DIGITS + Mod.CLONE)),

            Pin.Name("StartTemple" + Constants.Clone),
            Pin.Name("DrakeLorestone" + Constants.Clone),
            Pin.Name("Eikthyrnir" + Constants.Clone),
            Pin.Name("Bonemass" + Constants.Clone),
            Pin.Name("GDKing" + Constants.Clone),
            Pin.Name("FireHole" + Constants.Clone).Lbl("Surtling Spawner"), // Spawner
            Pin.Name("Meteorite" + Constants.Clone),
            Pin.Name("Runestone_Meadows" + Constants.Clone),
            Pin.Name("Runestone_Mountains" + Constants.Clone),
            Pin.Name("Runestone_Draugr" + Constants.Clone),
            Pin.Name("Runestone_Greydwarfs" + Constants.Clone),
            Pin.Name("Runestone_Swamps" + Constants.Clone),
            Pin.Name("Runestone_Plains" + Constants.Clone),
            Pin.Name("Runestone_BlackForest" + Constants.Clone),
            Pin.Name("Runestone_Boars" + Constants.Clone),
            Pin.Name("Runestone_Mistlands" + Constants.Clone),
            Pin.Name("StoneCircle" + Constants.Clone).Lbl("Stone Circle"),
            //Pin.Make(Pin.Name("" + Mod.CLONE)),
        };


        [UsedImplicitly]
        private static void Postfix(ref Location instance)
        {
            Location obj = instance;
            var template = Templates.FirstOrDefault(t => t.IsMatch(obj)) ??
                           AutoMapPinsPlugin.TEMPLATE_REGISTRY.Find(instance);

            if (template == null)
            {
                AutoMapPinsPlugin.LogUnmatchedName(typeof(Location), obj.name);
                AutoMapPinsPlugin.LogUnmatchedHover(typeof(Location), obj.GetComponent<HoverText>()?.m_text);
            }
            else if (!string.IsNullOrWhiteSpace(template.Label))
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