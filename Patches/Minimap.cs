using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace AutoMapPins.Patches
{
    internal static class MinimapExtension
    {
        public static Minimap.PinData FindSimilarPin(this Minimap me, Vector3 position, String name)
        {
            return me.m_pins.FirstOrDefault(p =>
                Math.Sqrt(
                    Math.Pow(p.m_pos.x - position.x, 2) +
                    Math.Pow(p.m_pos.y - position.y, 2) +
                    Math.Pow(p.m_pos.z - position.z, 2)
                ) < 1f &&
                p.m_name.Contains(name.Replace(' ', '\u00A0'))
            );
        }

        public static void RegisterExistingPins(this Minimap minimap)
        {
            AutoMapPinsPlugin.LOGGER.LogInfo($"Loaded {minimap.m_pins.Count} pins");

            foreach (var pin in minimap.m_pins)
            {
                var template = AutoMapPinsPlugin.TEMPLATE_REGISTRY.Find(pin);

                if (template != null)
                {
                    pin.m_icon = template.Icon;
                    pin.m_name = AutoMapPinsPlugin.Wrap(template.Label);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Minimap), "LoadMapData")]
    class MinimapLoadMapDataPatch
    {
        private static void Postfix(ref Minimap instance)
        {
            instance.RegisterExistingPins();
        }
    }
}