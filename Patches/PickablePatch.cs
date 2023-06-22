using AutoMapPins.Model;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Pickable), nameof(Pickable.Drop))]
    public class PickableDropPatch
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref Pickable __instance)
        {
            GameObject gameObject = __instance.gameObject;
            // we skip any objects inside dungeons (above 4000m height)
            if (gameObject.transform.position.y <= CommonPatchLogic.MaxHeight)
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