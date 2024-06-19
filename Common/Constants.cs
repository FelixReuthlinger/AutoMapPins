using System.Text.RegularExpressions;
using AutoMapPins.Data;
using AutoMapPins.Model;

namespace AutoMapPins.Common;

internal static class Constants
{
    internal const float DefaultGroupingDistance = 30.0f;
    internal const string NoConfig = "n_a";
    private static readonly Regex CloneRegex = new(@"\(Clone\)");
    private static readonly Regex DuplicateRegex = new(@"[ ]*\([\d]*\)");
    

    internal static string ParseInternalName(string instanceName)
    {
        string nameWithoutClone = CloneRegex.Replace(instanceName, "");
        string nameWithoutDuplicationNumber = DuplicateRegex.Replace(nameWithoutClone, "");
        return nameWithoutDuplicationNumber;
    }
}