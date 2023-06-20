using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Pickable), nameof(Pickable.Awake))]
    public class PickablePatch
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref MineRock __instance)
        {
            CommonPatchLogic.PatchName(__instance.name, __instance.gameObject);
        }
    }
}