using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Common;
using AutoMapPins.Data;
using AutoMapPins.Model;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace AutoMapPins.Patches;

[HarmonyPatch(typeof(Minimap))]
internal abstract class MinimapPatch : HasLogger
{
    private readonly static List<MapPin> MapTableTempStorage = new();
    
    [UsedImplicitly]
    [HarmonyPatch(nameof(Minimap.LoadMapData))]
    [HarmonyPostfix]
    internal static void LoadMapDataPostfix(ref Minimap __instance)
    {
        var pinsToRemove = new List<Minimap.PinData>();
        var convertedPins = new List<MapPin>();
        Log.LogInfo($"loading map data with '{__instance.m_pins.Count}' pins");
        foreach (var pin in __instance.m_pins)
        {
            var config = Registry.ConfiguredPins
                .FirstOrDefault(pinConfig => pinConfig.Value.Name == pin.m_name)
                .Value;
            if (config == null) continue;
            pinsToRemove.Add(pin);
            var newMapPin = new MapPin(pin, config);
            convertedPins.Add(newMapPin);
        }

        if (pinsToRemove.Count > 0)
        {
            Log.LogInfo($"removing {pinsToRemove.Count} pins due to config updates and/or replacement by loading " +
                        $"from player save file");
            foreach (var pin in pinsToRemove) __instance.RemovePin(pin);
        }

        if (convertedPins.Count > 0)
        {
            Log.LogInfo($"adding {convertedPins.Count} AMP pins to replace vanilla stored pins");
            foreach (var pin in convertedPins) __instance.m_pins?.Add(pin);
        }

        Log.LogInfo($"Loaded map with {__instance.m_pins?.Count} existing pins");
    }

    [UsedImplicitly]
    [HarmonyPatch(nameof(Minimap.UpdatePins))]
    [HarmonyPostfix]
    internal static void UpdatePinsPostfix(ref Minimap __instance)
    {
        foreach (var pin in __instance.m_pins)
            if (pin is MapPin mapPin && mapPin.m_iconElement)
                mapPin.UpdatePinColor();
    }
    
    [UsedImplicitly]
    [HarmonyPatch(nameof(Minimap.GetSharedMapData))]
    [HarmonyPrefix]
    internal static void GetSharedMapDataPrefix(ref Minimap __instance)
    {
        MapTableTempStorage.Clear();
        MapTableTempStorage.AddRange(__instance.m_pins.OfType<MapPin>());
        foreach (var mapPin in MapTableTempStorage)
        {
            __instance.m_pins.Remove((Minimap.PinData)mapPin);
        }
    }

    [UsedImplicitly]
    [HarmonyPatch(nameof(Minimap.GetSharedMapData))]
    [HarmonyPostfix]
    internal static void GetSharedMapDataPostfix(ref Minimap __instance)
    {
        __instance.m_pins.AddRange(MapTableTempStorage.Select(pin => (Minimap.PinData)pin));
        MapTableTempStorage.Clear();
    }

    internal static void UpsertPin(GameObject objectToPin)
    {
        if (!Minimap.instance && Minimap.instance.m_pins != null)
            return; // run only if minimap is available
        string internalName = Constants.ParseInternalName(objectToPin.name);
        if (!Registry.ConfiguredPins.TryGetValue(internalName, out Config.Pin config) || !config.IsActive)
        {
            Registry.AddMissingConfig(internalName);
            return; // run only for configured active pins
        }

        Minimap.PinData? existingPin = Minimap.instance.m_pins!.FirstOrDefault(pin =>
            pin.m_pos == objectToPin.transform.position && pin.m_name == config.Name);
        if (existingPin == null)
            CreateAndInsertPin(new PositionedObject(internalName, objectToPin.transform.position), config);
        else
        {
            // found existing pin
        }
    }

    private static void CreateAndInsertPin(PositionedObject positionedObject, Config.Pin config)
    {
        if (!ObjectGroupingAvailable(positionedObject, config))
            Minimap.instance.m_pins!.Add(new MapPin(positionedObject, config));
    }

    private static bool ObjectGroupingAvailable(PositionedObject positionedObject, Config.Pin config)
    {
        // minimap availability checked already
        if (!config.Groupable) return false;
        MapPin? viablePinForGrouping = Minimap.instance.m_pins
            .OfType<MapPin>()
            .Where(pin => pin.Config.Groupable)
            .FirstOrDefault(pin => pin.AcceptsObject(positionedObject));
        if (viablePinForGrouping == null) return false;
        viablePinForGrouping.AddObjectToPin(positionedObject);
        return true;
    }

    internal static void UnpinObject(GameObject objectToRemove) =>
        UnpinObject(PositionedObject.FromGameObject(objectToRemove));

    private static void UnpinObject(PositionedObject positionToRemove)
    {
        if (!Minimap.instance || Minimap.instance.m_pins == null) return;
        MapPin? pinFound = Minimap.instance.m_pins.OfType<MapPin>()
            .FirstOrDefault(pin => pin.RemoveObjectFromPin(positionToRemove));
        if (pinFound == null) return;
        Minimap.instance.RemovePin((Minimap.PinData)pinFound);
    }

    internal static void UpdatePins()
    {
        if (!Minimap.instance || Minimap.instance.m_pins == null) return;
        Log.LogInfo("updating pins");
        // TODO maybe add this later
        // HashSet<int>? activeInstanceIds = ZNetScene.instance?.m_instances
        //     .Select(instance => instance.Value.gameObject.GetInstanceID()).ToHashSet();
        var pinsToRemove = new List<PositionedObject>();
        foreach (var pin in Minimap.instance.m_pins.OfType<MapPin>())
        {
            if (Registry.ConfiguredPins.TryGetValue(pin.InternalName, out Config.Pin config))
                pin.ApplyConfigUpdate(config);
            else pinsToRemove.Add(pin.GetPinnedPosition());
        }

        foreach (var pinnedPosition in pinsToRemove) UnpinObject(pinnedPosition);
    }

    internal static string PrintMapPins()
    {
        string result = "";
        if (!Minimap.instance) return result;
        var pins = Minimap.instance.m_pins.ToDictionary(pin =>
            new PositionedObject(pin.m_NamePinData?.PinNameText.text ?? "no name", pin.m_pos), pin => pin);
        foreach (var pin in pins.OrderBy(pair => pair.Key))
        {
            if (pin.Value is MapPin mapPin) result += mapPin + "\n";
            else result += $"vanilla pin: {pin.Key} of type '{pin.Value.m_type.ToString()}'\n";
        }

        return result;
    }
}