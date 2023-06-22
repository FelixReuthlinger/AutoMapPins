using System.Text.RegularExpressions;

namespace AutoMapPins.Model
{
    public abstract class Common
    {
        internal const float DefaultGroupingDistance = 15.0f;
        internal const string NoConfig = "n_a";
        internal static readonly Regex CloneRegex = new("\\(Clone\\)");
        internal static readonly Regex ExceptWordRegex = new("[\\W\\d_]*");
    }
}