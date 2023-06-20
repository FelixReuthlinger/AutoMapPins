using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(RuneStone), nameof(RuneStone.GetHoverName))]
    public class RuneStonePatch
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref RuneStone __instance)
        {
            CommonPatchLogic.PatchName(__instance.name, __instance.gameObject);
        }
    }
}