using System;
using AutoMapPins.Common;
using UnityEngine;

namespace AutoMapPins.Model;

internal readonly struct PositionedObject : IComparable
{
    internal readonly string ObjectName;
    internal readonly Vector3 ObjectPosition;

    public PositionedObject(string objectName, Vector3 objectPosition)
    {
        ObjectName = objectName;
        ObjectPosition = objectPosition;
    }

    internal static PositionedObject FromGameObject(GameObject pinnedObject)
    {
        return new PositionedObject(Constants.ParseInternalName(pinnedObject.name), pinnedObject.transform.position);
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            PositionedObject pinnedPosition => Equals(pinnedPosition),
            _ => base.Equals(obj)
        };
    }

    private bool Equals(PositionedObject other)
    {
        return ObjectName == other.ObjectName && ObjectPosition.Equals(other.ObjectPosition);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (ObjectName.GetHashCode() * 397) ^ ObjectPosition.GetHashCode();
        }
    }

    public int CompareTo(object obj)
    {
        return obj switch
        {
            PositionedObject pinnedPosition => CompareTo(pinnedPosition),
            _ => throw new NotImplementedException()
        };
    }

    private int CompareTo(PositionedObject other)
    {
        int stringCompare = String.Compare(ObjectName, other.ObjectName, StringComparison.Ordinal);
        return stringCompare == 0 ? ObjectPosition.x.CompareTo(other.ObjectPosition.x) : stringCompare;
    }

    public override string ToString()
    {
        return $"'{ObjectName}' at '{ObjectPosition}'";
    }
}