using System.Text.RegularExpressions;

namespace AutoMapPins.Model
{
    public abstract class Common
    {
        internal const bool DefaultFalse = false;
        internal const float DefaultGroupingDistance = 10.0f;
        internal const string NoConfig = "n_a";
        internal static readonly Regex CLONE_REGEX = new("\\(Clone\\)");
        internal static readonly Regex EXCEPT_WORD_REGEX = new("[\\W]*");
    }
}