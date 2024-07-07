using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Common;
using AutoMapPins.Data;
using UnityEngine;

namespace AutoMapPins.Model;

internal abstract class Map : HasLogger
{
    private static readonly Dictionary<int, MapPin> AllPins = new();

    internal static void CreatePin(GameObject objectToPin)
    {
        int instanceID = objectToPin.GetInstanceID();
        if (AllPins.ContainsKey(instanceID)) return;
        string internalName = Constants.ParseInternalName(objectToPin.name);
        if (Registry.ConfiguredPins.TryGetValue(internalName, out Config.Pin config))
        {
            if (!config.IsActive) return;
            if (config.Groupable)
            {
                MapPin? viablePinForGrouping = AllPins
                    .FirstOrDefault(pin => pin.Value.AcceptsObject(objectToPin)).Value;
                if (viablePinForGrouping != null)
                {
                    viablePinForGrouping.AddObjectToPin(objectToPin);
                    AllPins.Add(instanceID, viablePinForGrouping);
                    return;
                }
            }

            AllPins.Add(instanceID, new MapPin(objectToPin, config));
        }
        else Registry.AddMissingConfig(internalName);
    }

    internal static void RemovePin(GameObject objectToRemove) => RemovePin(objectToRemove.GetInstanceID());

    private static void RemovePin(int instanceID)
    {
        if (!AllPins.TryGetValue(instanceID, out MapPin pin)) return; // pin not known
        if (!pin.RemoveObjectFromPin(instanceID))
        {
            if (!pin.Config.IsPermanent) Minimap.instance?.RemovePin(pin);
        }

        AllPins.Remove(instanceID);
    }

    internal static void UpdatePins()
    {
        Log.LogInfo("updating pins");
        // TODO maybe add this later
        // HashSet<int>? activeInstanceIds = ZNetScene.instance?.m_instances
        //     .Select(instance => instance.Value.gameObject.GetInstanceID()).ToHashSet();
        var pinsToRemove = new List<int>();
        foreach (var pin in AllPins)
        {
            if (Registry.ConfiguredPins.TryGetValue(pin.Value.InternalName, out Config.Pin config))
                pin.Value.ApplyConfigUpdate(config);
            else pinsToRemove.Add(pin.Key);
        }

        foreach (var instanceId in pinsToRemove) RemovePin(instanceId);
    }

    internal static bool HasPins()
    {
        return AllPins.Count > 0;
    }

    internal static void Clear()
    {
        AllPins.Clear();
    }

    internal static float HorizontalDistance(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
    }
}