using System.Text.RegularExpressions;
using AutoMapPins.Data;
using AutoMapPins.Model;

namespace AutoMapPins.Common;

internal static class Constants
{
    internal const float DefaultGroupingDistance = 30.0f;
    internal const string NoConfig = "n_a";
    internal static readonly Config.PinColor WhiteColor = new() { Red = 1f, Blue = 1f, Green = 1f, Alpha = 1f };
    private static readonly Regex CloneRegex = new(@"\(Clone\)");
    internal static readonly Regex CountedPinRegex = new(@"[\d ]*");
    

    internal static string ParseInternalName(string instanceName)
    {
        return CloneRegex.Replace(instanceName, "");
    }
}