using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Location), nameof(Location.Awake))]
    class LocationPatchSpawn
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref Location __instance)
        {
            CommonPatchLogic.PatchName(__instance.name, __instance.gameObject);
        }
    }
}