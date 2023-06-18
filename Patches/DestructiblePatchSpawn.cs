using System.Linq;
using AutoMapPins.Model;
using AutoMapPins.Registry;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Destructible), "Start")]
    class DestructiblePatchSpawn
    {
        private static readonly PinTemplate[] Templates =
        {
            Pin.Hovr("\\$prop_beech"),
            Pin.Hovr("\\$prop_treestump"), //Possibly same as Beech_Stub(Clone)
            Pin.Hovr("\\$prop_fir"),
            Pin.Hovr("\\$prop_treelog"),
            Pin.Hovr("\\$enemy_greydwarfspawner"),
            Pin.Hovr("\\$enemy_skeletonspawner"),
            Pin.Hovr("\\$enemy_draugrspawner"),

            Pin.Name("barrell" + Constants.Clone),
            Pin.Name("blackmarble_altar_crystal" + Constants.Clone),
            Pin.Name("blackmarble_post" + Constants.Digits + Constants.DigitsBracedOptional +
                     Constants.Clone),
            Pin.Name("BlueberryBush" + Constants.Clone),
            Pin.Name("Bush" + Constants.Digits + Constants.Clone),
            Pin.Name("Bush" + Constants.Digits + "_en" + Constants.Clone),
            Pin.Name("Bush" + Constants.Digits + "_heath" + Constants.Clone),
            Pin.Name("CastleKit_metal_groundtorch_unlit" + Constants.Clone),
            Pin.Name("caverock_ice_pillar_wall" + Constants.Clone),
            Pin.Name("caverock_ice_stalagmite" + Constants.Clone),
            Pin.Name("caverock_ice_stalagtite" + Constants.Clone),
            Pin.Name("cliff_mistlands" + Constants.Digits + Constants.DigitsBracedOptional +
                     Constants.Clone),
            Pin.Name("cloth_hanging_door" + Constants.Clone),
            Pin.Name("cloth_hanging_door_double" + Constants.Clone),
            Pin.Name("cloth_hanging_long" + Constants.Clone),
            Pin.Name("CloudberryBush" + Constants.Clone),
            Pin.Name("CreepProp_hanging" + Constants.Digits + Constants.DigitsBracedOptional +
                     Constants.Clone),
            Pin.Name("CreepProp_egg_hanging" + Constants.Digits + Constants.DigitsBracedOptional +
                     Constants.Clone),
            Pin.Name("Crow" + Constants.DigitsBracedOptional + Constants.Clone),
            Pin.Name("dvergrprops_barrel" + Constants.Clone),
            Pin.Name("dvergrprops_pickaxe" + Constants.DigitsBracedOptional + Constants.Clone),
            Pin.Name("dvergrtown_arch" + Constants.DigitsBracedOptional + Constants.Clone),
            Pin.Name("dvergrtown_creep_door" + Constants.Clone),
            Pin.Name("dvergrtown_wood_beam" + Constants.DigitsBracedOptional + Constants.Clone),
            Pin.Name("dvergrtown_wood_support" + Constants.DigitsBracedOptional + Constants.Clone),
            Pin.Name("dvergrtown_wood_pole" + Constants.DigitsBracedOptional + Constants.Clone),
            Pin.Name("dvergrtown_wood_wall" + Constants.Digits + Constants.Clone),
            Pin.Name("fenrirhide_hanging" + Constants.Clone),
            Pin.Name("Greydwarf_Root" + Constants.DigitsBracedOptional + Constants.Clone),
            Pin.Name("hanging_hairstrands" + Constants.Clone),
            Pin.Name("HeathRockPillar" + Constants.Clone),
            Pin.Name("highstone" + Constants.DigitsBracedOptional + Constants.Clone),
            Pin.Name("ice" + Constants.Digits + Constants.Clone),
            Pin.Name("ice_rock" + Constants.Digits + Constants.Clone),
            Pin.Name("marker" + Constants.Digits + Constants.Clone),
            Pin.Name("MountainGraveStone" + Constants.Digits + Constants.Clone),
            Pin.Name("MountainKit_brazier" + Constants.Clone),
            Pin.Name("mountainkit_chair" + Constants.Clone),
            Pin.Name("mountainkit_table" + Constants.Clone),
            Pin.Name("MountainKit_wood_gate" + Constants.Clone),
            Pin.Name("Pickable_MountainCaveCrystal" + Constants.Clone),
            Pin.Name("Pickable_RoyalJelly" + Constants.Clone),
            Pin.Name("RaspberryBush" + Constants.Clone),
            Pin.Name("Rock_" + Constants.Digits + Constants.DigitsBracedOptional +
                     Constants.Clone),
            Pin.Name("Rock_" + Constants.Digits + "_plains" + Constants.Clone),
            Pin.Name("rock" + Constants.Digits + "_forest" + Constants.Clone),
            Pin.Name("rock" + Constants.Digits + "_heath" + Constants.Clone),
            Pin.Name("rock" + Constants.Digits + "_mountain" + Constants.Clone),
            Pin.Name("rock" + Constants.Digits + "_coast" + Constants.Clone),
            Pin.Name("rock_mistlands" + Constants.Digits + Constants.Clone),
            Pin.Name("RockDolmen_" + Constants.Digits + Constants.Clone),
            Pin.Name("root" + Constants.Digits + Constants.DigitsBracedOptional +
                     Constants.Clone),
            Pin.Name("Seagal" + Constants.Clone),
            Pin.Name("SeekerEgg" + Constants.DigitsBracedOptional + Constants.Clone),
            Pin.Name("shrub_" + Constants.Digits + Constants.Clone),
            Pin.Name("shrub_" + Constants.Digits + "_heath" + Constants.Clone),
            Pin.Name("stubbe" + Constants.Clone),
            Pin.Name("trader_wagon_destructable" + Constants.Clone),
            Pin.Name("widestone" + Constants.Clone),
        };

        [UsedImplicitly]
        private static void Postfix(ref Destructible instance)
        {
            Destructible obj = instance;
            var template = Templates.FirstOrDefault(t => t.IsMatch(obj)) ??
                           AutoMapPinsPlugin.TEMPLATE_REGISTRY.Find(instance);

            if (template == null)
            {
                AutoMapPinsPlugin.LogUnmatchedName(typeof(Destructible), obj.name);
                AutoMapPinsPlugin.LogUnmatchedHover(typeof(Destructible), obj.GetComponent<HoverText>()?.m_text);
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