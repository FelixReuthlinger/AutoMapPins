using System.Collections.Generic;
using AutoMapPins.Common;
using UnityEngine;
// ReSharper disable ConvertToConstant.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnassignedField.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace AutoMapPins.Data;

public abstract class Config
{
    public class Category : Pin
    {
        public Dictionary<string, Pin>? IndividualConfiguredObjects = null!;
        public List<string>? CategoryConfiguredObjects = null!;
    }
    
    public class Pin : SerializedToStrings
    {
        public string? Name = null!;
        public string? IconName = null!;
        public PinColor? IconColorRGBA = null!;
        public bool IsPermanent;
        public bool IsActive;
        public bool Groupable;
        public float GroupingDistance = Constants.DefaultGroupingDistance;
    }

    public class PinColor : SerializedToStrings
    {
        public float Red = 1f;
        public float Green = 1f;
        public float Blue = 1f;
        public float Alpha = 1f;

        public Color FromConfig()
        {
            return new Color(Red, Green, Blue, Alpha);
        }
    }

    public abstract class SerializedToStrings
    {
        public override string ToString()
        {
            return YamlFileStorage.Serializer.Serialize(this);
        }
    }
}