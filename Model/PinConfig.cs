using System.Diagnostics.CodeAnalysis;
using AutoMapPins.Data;
using UnityEngine;

namespace AutoMapPins.Model
{
    [SuppressMessage("ReSharper", "UnassignedField.Global")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    public class PinConfig
    {
        public string CategoryName = Common.NoConfig;
        public string InternalName = Common.NoConfig;
        public string Name = Common.NoConfig;
        public string IconName = Common.NoConfig;
        public bool IsPermanent;
        public bool IsActive;
        public bool Groupable;
        public float GroupingDistance = Common.DefaultGroupingDistance;

        internal static PinConfig FromGameObject(GameObject gameObject)
        {
            string parsedName = ParseInternalName(gameObject.name);
            return Registry.ConfiguredPins.TryGetValue(parsedName, out PinConfig result)
                ? result
                : new PinConfig { InternalName = parsedName };
        }

        private static string ParseInternalName(string instanceName)
        {
            return Common.ExceptWordRegex
                .Replace(Common.CloneRegex
                    .Replace(instanceName, ""), "")
                .ToLowerInvariant();
        }
    }
}