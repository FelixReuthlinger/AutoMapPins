using AutoMapPins.Model;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Destructible), nameof(Destructible.Start))]
    class DestructiblePatch
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref Destructible __instance)
        {
            string internalName = PinConfig.ParseInternalName(__instance.name);
            PinComponent pin = __instance.gameObject.AddComponent<PinComponent>();
            if (Registry.ConfiguredPins.TryGetValue(internalName, out PinConfig config))
            {
                AutoMapPinsPlugin.LOGGER.LogDebug($"initializing pin from config for {config.InternalName}");
                pin.InitializeFromConfig(config);
            }
            else
            {
                AutoMapPinsPlugin.LOGGER.LogWarning(
                    $"not configured object found for internal name '{internalName}', initializing from game data");
                pin.InitializeFromConfig(PinConfig.FromGameObject(internalName));
            }

            Registry.MANAGED_PINS.Add(pin);
        }
    }
}