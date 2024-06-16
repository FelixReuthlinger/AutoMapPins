using AutoMapPins.Model;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace AutoMapPins.Patches;

internal static class CommonPatchLogic
{
    internal static void Patch(GameObject gameObject)
    {
        gameObject.AddComponent<PinComponent>();
    }
}

[HarmonyPatch(typeof(Destructible), nameof(Destructible.Start))]
internal class DestructiblePatch
{
    [UsedImplicitly]
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
    [UsedImplicitly]
    static void Postfix(ref MineRock __instance) => CommonPatchLogic.Patch(__instance.gameObject);
}

[HarmonyPatch(typeof(MineRock5), nameof(MineRock5.Awake))]
internal class MineRock5Patch
{
    [UsedImplicitly]
    static void Postfix(ref MineRock5 __instance) => CommonPatchLogic.Patch(__instance.gameObject);
}

[HarmonyPatch(typeof(Location), nameof(Location.Awake))]
internal class LocationPatch
{
    [UsedImplicitly]
    static void Postfix(ref Location __instance) => CommonPatchLogic.Patch(__instance.gameObject);
}

[HarmonyPatch(typeof(Leviathan), nameof(Leviathan.Awake))]
internal class LeviathanPatch
{
    [UsedImplicitly]
    static void Postfix(ref Leviathan __instance) => CommonPatchLogic.Patch(__instance.gameObject);
}

[HarmonyPatch(typeof(TeleportWorld), nameof(TeleportWorld.Awake))]
internal class TeleportWorldPatch
{
    [UsedImplicitly]
    static void Postfix(ref TeleportWorld __instance) => CommonPatchLogic.Patch(__instance.gameObject);
}

[HarmonyPatch(typeof(PickableItem), nameof(PickableItem.Awake))]
internal class PickableItemPatch
{
    [UsedImplicitly]
    static void Postfix(ref PickableItem __instance) => CommonPatchLogic.Patch(__instance.gameObject);
}

[HarmonyPatch(typeof(Container), nameof(Container.Awake))]
internal class ContainerPatch
{
    [UsedImplicitly]
    static void Postfix(ref Container __instance) => CommonPatchLogic.Patch(__instance.gameObject);
}