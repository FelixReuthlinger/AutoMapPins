using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Data;
using AutoMapPins.Icons;
using AutoMapPins.Model;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Minimap), nameof(Minimap.LoadMapData))]
    class MinimapPatch
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref Minimap __instance)
        {
            List<Minimap.PinData> pins = __instance.m_pins;
            AutoMapPinsPlugin.LOGGER.LogInfo($"Loaded map with {pins.Count} existing pins");
            foreach (var pin in pins)
            {
                PinConfig? config = Registry.ConfiguredPins
                    .Select(config => config.Value)
                    .First(config => config.Name == pin.m_name);
                if (config != null)
                {
                    if (Assets.ICONS.TryGetValue(config.IconName, out Sprite icon))
                    {
                        pin.m_icon = icon;
                    }
                    else AutoMapPinsPlugin.LOGGER.LogWarning($"no icon found for pin '{config.Name} and '");
                }
                else AutoMapPinsPlugin.LOGGER.LogWarning($"no config found for pin '{pin.m_name}'");
            }
        }
    }
}