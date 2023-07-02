using AutoMapPins.Model;
using HarmonyLib;
using UnityEngine;

// ReSharper disable All
namespace AutoMapPins.Patches
{
    internal static class CommonPatchLogic
    {
        internal const int MaxHeight = 4000;

        internal static void Patch(GameObject gameObject)
        {
            // 1. we skip any objects inside dungeons (above 4000sm height)
            // 2. if the game object already was pinned by another patch, do not repeat it
            if (gameObject.transform.position.y <= MaxHeight && !gameObject.TryGetComponent(out PinComponent _))
                PinComponent.Create(gameObject);
        }
    }

    [HarmonyPatch(typeof(Destructible), nameof(Destructible.Start))]
    internal class DestructiblePatch
    {
        static void Postfix(ref Destructible __instance)
        {
            // in case an object has the Destructible and Pickable component, we skip patching the Destructible to
            // not patch the object twice, since the pickable patch will handle if object was already picked
            if (!__instance.TryGetComponent(out Pickable _)) CommonPatchLogic.Patch(__instance.gameObject);
        }
    }

    [HarmonyPatch(typeof(MineRock), nameof(MineRock.Start))]
    internal class MineRockPatch
    {
        static void Postfix(ref MineRock __instance) => CommonPatchLogic.Patch(__instance.gameObject);
    }

    [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.Start))]
    internal class MineRock5Patch
    {
        static void Postfix(ref MineRock5 __instance) => CommonPatchLogic.Patch(__instance.gameObject);
    }

    [HarmonyPatch(typeof(Location), nameof(Location.Awake))]
    internal class LocationPatch
    {
        static void Postfix(ref Location __instance) => CommonPatchLogic.Patch(__instance.gameObject);
    }

    [HarmonyPatch(typeof(Leviathan), nameof(Leviathan.Awake))]
    internal class LeviathanPatch
    {
        static void Postfix(ref Leviathan __instance) => CommonPatchLogic.Patch(__instance.gameObject);
    }

    [HarmonyPatch(typeof(TeleportWorld), nameof(TeleportWorld.Awake))]
    internal class TeleportWorldPatch
    {
        static void Postfix(ref TeleportWorld __instance) => CommonPatchLogic.Patch(__instance.gameObject);
    }

    [HarmonyPatch(typeof(PickableItem), nameof(PickableItem.Awake))]
    internal class PickableItemPatch
    {
        static void Postfix(ref PickableItem __instance) => CommonPatchLogic.Patch(__instance.gameObject);
    }

    [HarmonyPatch(typeof(Container), nameof(Container.Awake))]
    internal class ContainerPatch
    {
        static void Postfix(ref Container __instance) => CommonPatchLogic.Patch(__instance.gameObject);
    }
}