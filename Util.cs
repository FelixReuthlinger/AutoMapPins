using System;

namespace AutoMapPins;

public static class Util
{
    public static string Wrap(string orig)
    {
        bool UseSmallerIcons = true;
        int _padToWidth = 60;
        float _fontFactor = 1.8f;
        if (UseSmallerIcons)
        {
            var n = Math.Max(0, (int)Math.Ceiling((_padToWidth - orig.Length * _fontFactor) / 2.0));
            var pad = new string('\u00A0', n);
            return pad + orig.Replace(' ', '\u00A0') + pad;
        }

        return orig;
    }
}