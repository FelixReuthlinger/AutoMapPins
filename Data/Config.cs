using System.Collections.Generic;
using AutoMapPins.Common;
using UnityEngine;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnassignedField.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace AutoMapPins.Data;

public abstract class Config
{
    public class Category
    {
        public bool CategoryActive;
        public Dictionary<string, Pin> Pins = new();

        public override string ToString()
        {
            return YamlFileStorage.Serializer.Serialize(this);
        }
    }

    public class Pin
    {
        public string Name = null!;
        public string IconName = Constants.NoConfig;
        public PinColor IconColorRGBA = Constants.WhiteColor;
        public bool IsPermanent;
        public bool IsActive;
        public bool Groupable;
        public float GroupingDistance = Constants.DefaultGroupingDistance;

        public override string ToString()
        {
            return YamlFileStorage.Serializer.Serialize(this);
        }
    }

    public class PinColor
    {
        public float Red = 0f;
        public float Green = 0f;
        public float Blue = 0f;
        public float Alpha = 1f;

        public Color FromConfig()
        {
            return new Color(Red, Green, Blue, Alpha);
        }
    }
}