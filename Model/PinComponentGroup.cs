using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Data;
using AutoMapPins.Icons;
using UnityEngine;

namespace AutoMapPins.Model;

internal class PinComponentGroup
{
    internal PinComponentGroup(PinConfig config)
    {
        _config = config;
    }
    
    private readonly PinConfig _config;
    private Minimap.PinData? _pin;
    private readonly List<Vector3> _groupedPositions = new();

    internal bool Accepts(PinComponent newPin)
    {
        return _config == newPin.Config && WithinGroupDistance(newPin.PinObject.m_pos);
    }

    internal void Remove(PinComponent pin)
    {
        _groupedPositions.Remove(pin.PinObject.m_pos);
        UpdateCenter();
    }

    private void UpdateCenter()
    {
        if (Minimap.instance != null)
        {
            // always remove before creation, otherwise duplicated - also remove if not creating a new one
            if (_pin != null) Minimap.instance.RemovePin(_pin);
            if (_groupedPositions.Count > 0)
            {
                Vector3 newCenter = new Vector3(
                    _groupedPositions.Average(position => position.x),
                    _groupedPositions.Average(position => position.y),
                    _groupedPositions.Average(position => position.z)
                );
                string groupName = (_groupedPositions.Count > 1 ? _groupedPositions.Count + " " : "") + _config.Name;
                _pin = Minimap.instance.AddPin(newCenter, Minimap.PinType.Icon1, groupName, _config.IsPermanent, false);
                _pin.m_icon = Assets.GetIcon(_config.IconName);
            }
        }
    }

    private bool WithinGroupDistance(Vector3 newPosition)
    {
        float groupingRadius = _config is { GroupingDistance: > 0 }
            ? _config.GroupingDistance
            : AutoMapPinsPlugin.GroupingRadius.Value;
        return _pin != null && Math
            .Sqrt(Math.Pow(_pin.m_pos.x - newPosition.x, 2) +
                  Math.Pow(_pin.m_pos.z - newPosition.z, 2)
            ) < groupingRadius;
    }

    internal static void GroupPin(PinComponent pin)
    {
        var group = Registry.GetOrCreatePinGroup(pin);
        pin.Group = group;
        if (!group._groupedPositions.Contains(pin.PinObject.m_pos))
        {
            group._groupedPositions.Add(pin.PinObject.m_pos);
            // if we group a pin, we should remove the original pin from map, since the grouped pin will replace it
            Minimap.instance.RemovePin(pin.PinObject);
            group.UpdateCenter();
        }
    }
}