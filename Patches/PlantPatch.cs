using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Plant), nameof(Plant.Awake))]
    public class PlantPatch
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref Plant __instance)
        {
            CommonPatchLogic.PatchName(__instance.name, __instance.gameObject);
        }
    }
}