using AutoMapPins.Model;
using UnityEngine;

namespace AutoMapPins.Patches;

internal static class CommonPatchLogic
{
    private const int MaxHeight = 4000;

    internal static void PatchName(string internalOriginalName, GameObject gameObject)
    {
        if (gameObject.transform.position.y <= MaxHeight)
        {
            string internalName = PinConfig.ParseInternalName(internalOriginalName);
            PinComponent pin = gameObject.AddComponent<PinComponent>();
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
                Registry.PINS_NO_CONFIG.Add(pin);
            }
        }
        else
        {
            AutoMapPinsPlugin.LOGGER.LogDebug(
                $"skipping object '{internalOriginalName}', since above max height {MaxHeight}");
        }
    }
}