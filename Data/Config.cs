using System.Collections.Generic;
using AutoMapPins.Common;
using UnityEngine;

// ReSharper disable ConvertToConstant.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnassignedField.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

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
        internal static PinColor White = new() { Red = RGBMax, Green = RGBMax, Blue = RGBMax, Alpha = RGBMax };
        
        private const int RGBMin = 0;
        private const int RGBMax = 255;
        private const float Divider = (float)RGBMax;

        public int Red;
        public int Green;
        public int Blue;
        public int Alpha;

        internal PinColor ClampColor()
        {
            Red = Clamp(Red, RGBMin, RGBMax);
            Green = Clamp(Green, RGBMin, RGBMax);
            Blue = Clamp(Blue, RGBMin, RGBMax);
            Alpha = Clamp(Alpha, RGBMin, RGBMax);
            return this;
        }

        public Color FromConfig()
        {
            float red = Clamp(Red, RGBMin, RGBMax) / Divider;
            float green = Clamp(Green, RGBMin, RGBMax) / Divider;
            float blue = Clamp(Blue, RGBMin, RGBMax) / Divider;
            float alpha = Clamp(Alpha, RGBMin, RGBMax) / Divider;
            return new Color(red, green, blue, alpha);
        }

        private static int Clamp(int value, int min, int max)
        {
            return value < min ? min : value > max ? max : value;
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