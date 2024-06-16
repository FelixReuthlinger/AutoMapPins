using System.Linq;
using AutoMapPins.Common;
using AutoMapPins.Data;
using AutoMapPins.Icons;
using AutoMapPins.Model;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches;

[HarmonyPatch(typeof(Minimap))]
internal class MinimapPatch : HasLogger
{
    [UsedImplicitly]
    [HarmonyPatch(nameof(Minimap.LoadMapData))]
    [HarmonyPostfix]
    internal static void LoadMapDataPostfix(ref Minimap __instance)
    {
        foreach (var pin in __instance.m_pins)
        {
            var config = Registry.ConfiguredPins
                .FirstOrDefault(config => config.Value.Name == pin.m_name)
                .Value;
            if (config == null) continue;
            if (!config.IsActive) __instance.RemovePin(pin);
            else
            {
                pin.m_save = !config.IsPermanent;
                pin.m_icon = Assets.GetIcon(config.IconName);
            }
        }

        Log.LogInfo($"Loaded map with {__instance.m_pins.Count} existing pins");
    }

    [UsedImplicitly]
    [HarmonyPatch(nameof(Minimap.UpdatePins))]
    [HarmonyPostfix]
    internal static void UpdatePinsPostfix(ref Minimap __instance)
    {
        foreach (var pin in __instance.m_pins)
            if (
                pin.m_iconElement != null &&
                pin is MapPin myPin &&
                Registry.ConfiguredPins.TryGetValue(myPin.InternalName, out Config.Pin config)
            )
            {
                var color = config.IconColorRGBA.FromConfig();
                pin.m_iconElement.color = color;
                pin.m_NamePinData.PinNameText.color = color;
            }
    }
}