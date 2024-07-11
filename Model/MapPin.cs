using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Common;
using AutoMapPins.Data;
using AutoMapPins.Icons;
using UnityEngine;

namespace AutoMapPins.Model;

public class MapPin : Minimap.PinData
{
    internal readonly string InternalName;
    internal Config.Pin Config = null!;
    private readonly List<Vector3> InstancePositions = new();

    internal MapPin(PositionedObject positionedObject, Config.Pin config)
    {
        InternalName = positionedObject.ObjectName;
        InstancePositions.Add(positionedObject.ObjectPosition);
        m_type = Minimap.PinType.None;
        m_checked = false;
        m_ownerID = 0;
        ApplyConfigUpdate(config);
    }

    internal MapPin(Minimap.PinData fromVanilla, Config.Pin config)
    {
        InternalName = Constants.ParseInternalName(fromVanilla.m_name);
        InstancePositions.Add(fromVanilla.m_pos);
        m_type = fromVanilla.m_type;
        m_checked = fromVanilla.m_checked;
        m_ownerID = fromVanilla.m_ownerID;
        ApplyConfigUpdate(config);
    }

    internal PositionedObject GetPinnedPosition()
    {
        return new PositionedObject(InternalName, m_pos);
    }

    internal void ApplyConfigUpdate(Config.Pin config)
    {
        Config = config;
        m_name = Config.Name;
        m_save = Config.IsPermanent;
        if (Config.IconName != null) m_icon = Assets.GetIcon(Config.IconName);
        m_NamePinData?.DestroyMapMarker();
        m_NamePinData = new Minimap.PinNameData(this);
        UpdatePin();
    }

    private void UpdatePin()
    {
        UpdatePosition();
        UpdatePinText();
        UpdatePinColor();
        // make sure that (independently of calls inside called methods) we do update the map pins
        if (Minimap.instance) Minimap.instance.m_pinUpdateRequired = true;
    }

    private void UpdatePosition()
    {
        if (InstancePositions.Count > 1)
        {
            Vector3 newCenter = new Vector3(
                InstancePositions.Average(position => position.x),
                InstancePositions.Average(position => position.y),
                InstancePositions.Average(position => position.z)
            );
            m_pos = newCenter;
        }
        else m_pos = InstancePositions.First();
    }

    private void UpdatePinText()
    {
        if (InstancePositions.Count > 1)
        {
            var countString = InstancePositions.Count + "";
            m_name = string.IsNullOrEmpty(Config.Name) ? countString :
                countString == "" ? Config.Name : countString + " " + Config.Name;
        }
        else m_name = string.IsNullOrEmpty(Config.Name) ? " " : Config.Name;

        // this is required to be able to re-create the text on textual changes due to grouping
        m_NamePinData.DestroyMapMarker();
        if (!Minimap.instance) return;
        RectTransform root = Minimap.instance.m_mode == Minimap.MapMode.Large
            ? Minimap.instance.m_pinNameRootLarge
            : Minimap.instance.m_pinNameRootSmall;
        Minimap.instance.CreateMapNamePin(this, root);
    }

    internal void UpdatePinColor()
    {
        if (!m_iconElement) return;
        Color color = Config.IconColorRGBA?.FromConfig() ?? Color.white;
        m_iconElement.color = color;
        m_NamePinData.PinNameText.color = color;
    }

    internal bool AcceptsObject(PositionedObject objectToGroup)
    {
        return Config.Groupable &&
               InternalName == objectToGroup.ObjectName &&
               WithinGroupDistance(objectToGroup.ObjectPosition);
    }

    private bool WithinGroupDistance(Vector3 newPosition)
    {
        float groupingRadius = Config is { GroupingDistance: > 0 }
            ? Config.GroupingDistance
            : AutoMapPinsPlugin.GroupingRadius.Value;
        return Utils.DistanceXZ(m_pos, newPosition) < groupingRadius;
    }

    internal void AddObjectToPin(PositionedObject objectToAdd)
    {
        if (InstancePositions.Contains(objectToAdd.ObjectPosition)) return; // no duplicates
        InstancePositions.Add(objectToAdd.ObjectPosition);
        UpdatePin();
    }

    internal bool RemoveObjectFromPin(PositionedObject positionedObject)
    {
        if (Config.IsPermanent) return false;
        if (!InstancePositions.Remove(positionedObject.ObjectPosition)) return false;
        if (InstancePositions.Count > 0) UpdatePin();
        return true;
    }

    public override string ToString()
    {
        string result =
            $"custom AMP pin: '{m_name}' (game object name '{InternalName}') at position '{m_pos.ToString()}' " +
            $"with icon '{Config.IconName}', permanent '{Config.IsPermanent}'";
        if (InstancePositions.Count <= 1) return result;
        result += "; grouped pin, containing positions:";
        foreach (var position in InstancePositions)
            result += "\n  " + position;
        return result;
    }
}