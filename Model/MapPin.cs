using System;
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
    private readonly Dictionary<int, Vector3> InstancePositions = new();

    internal MapPin(GameObject objectToPin, Config.Pin config)
    {
        int instanceID = objectToPin.GetInstanceID();
        InternalName = Constants.ParseInternalName(objectToPin.name);
        InstancePositions.Add(instanceID, objectToPin.transform.position);
        m_type = Minimap.PinType.None;
        m_checked = false;
        m_ownerID = 0;
        ApplyConfigUpdate(config);
        Minimap.instance?.m_pins?.Add(this);
    }

    internal void ApplyConfigUpdate(Config.Pin config)
    {
        Config = config;
        if (Config.IconName != null) m_icon = Assets.GetIcon(Config.IconName);
        m_save = Config.IsPermanent;
        m_NamePinData?.DestroyMapMarker();
        m_NamePinData = new Minimap.PinNameData(this);
        UpdatePin();
    }

    private void UpdatePin()
    {
        UpdatePosition();
        UpdatePinText();
        // make sure that (independently of calls inside called methods) we do update the map pins
        if (Minimap.instance) Minimap.instance.m_pinUpdateRequired = true;
    }

    private void UpdatePosition()
    {
        if (InstancePositions.Count > 1)
        {
            Vector3 newCenter = new Vector3(
                InstancePositions.Values.Average(position => position.x),
                InstancePositions.Values.Average(position => position.y),
                InstancePositions.Values.Average(position => position.z)
            );
            m_pos = newCenter;
        }
        else m_pos = InstancePositions.First().Value;
    }
    
    private void UpdatePinText()
    {
        if (InstancePositions.Count > 1)
        {
            var countString = InstancePositions.Count + "";
            m_name = String.IsNullOrEmpty(Config.Name) ? countString :
                countString == "" ? Config.Name : countString + " " + Config.Name;
        }
        else m_name = String.IsNullOrEmpty(Config.Name) ? " " : Config.Name;
        // this is required to be able to re-create the text on textual changes due to grouping
        m_NamePinData.DestroyMapMarker();
        RectTransform root = Minimap.instance.m_mode == Minimap.MapMode.Large
            ? Minimap.instance.m_pinNameRootLarge
            : Minimap.instance.m_pinNameRootSmall;
        Minimap.instance.CreateMapNamePin(this, root);
    }

    internal bool AcceptsObject(GameObject objectToPin)
    {
        return Config.Groupable &&
               InternalName == Constants.ParseInternalName(objectToPin.name) &&
               WithinGroupDistance(objectToPin.transform.position);
    }

    private bool WithinGroupDistance(Vector3 newPosition)
    {
        float groupingRadius = Config is { GroupingDistance: > 0 }
            ? Config.GroupingDistance
            : AutoMapPinsPlugin.GroupingRadius.Value;
        return Map.HorizontalDistance(m_pos, newPosition) < groupingRadius;
    }

    internal void AddObjectToPin(GameObject objectToPin)
    {
        InstancePositions.Add(objectToPin.GetInstanceID(), objectToPin.transform.position);
        UpdatePin();
    }

    internal bool RemoveObjectFromPin(int instanceID)
    {
        if(!Config.IsPermanent) InstancePositions.Remove(instanceID);
        if (InstancePositions.Count <= 0) return false;
        UpdatePin();
        return true;
    }
}