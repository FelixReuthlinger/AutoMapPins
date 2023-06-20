using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(SapCollector), nameof(SapCollector.Awake))]
    public class SapCollectorPatch
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref SapCollector __instance)
        {
            CommonPatchLogic.PatchName(__instance.name, __instance.gameObject);
        }
    }
}