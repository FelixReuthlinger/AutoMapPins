using AutoMapPins.Model;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Pickable), nameof(Pickable.Awake))]
    internal class PickablePatch
    {
        [UsedImplicitly]
        [HarmonyPriority(Priority.High)]
        static void Postfix(ref Pickable __instance)
        {
            // only add a pin, if the pickable is not picked already
            if (!__instance.m_picked) CommonPatchLogic.Patch(__instance.gameObject);
        }
    }

    [HarmonyPatch(typeof(Pickable), nameof(Pickable.SetPicked))]
    public class PickableDropPatch
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref Pickable __instance, bool picked)
        {
            GameObject gameObject = __instance.gameObject;
            // we skip any objects inside dungeons (above 4000m height)
            if (gameObject.transform.position.y <= CommonPatchLogic.MaxHeight && picked)
            {
                if (gameObject.TryGetComponent(out PinComponent component))
                {
                    // if the item is picked, let's destroy the pin
                    component.OnDestroy();
                }
            }
        }
    }
}