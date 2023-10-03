using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Model;

namespace AutoMapPins.Data;

public static class Registry
{
    internal static Dictionary<string, CategoryConfig> ConfiguredCategories = new();
    internal static Dictionary<string, PinConfig> ConfiguredPins = new();
    internal static readonly List<PinConfig> MissingConfigs = new();
    private static readonly List<PinComponentGroup> AllGroups = new();

    internal static void InitializeRegistry(Dictionary<string, CategoryConfig> configuredCategories)
    {
        ConfiguredCategories = configuredCategories;
        ConfiguredPins = ConfiguredCategories
            .Select(categories => categories.Value.Pins)
            .SelectMany(x => x)
            .GroupBy(kv => kv.Key)
            .ToDictionary(kv => kv.Key, group => group.First().Value);

        AutoMapPinsPlugin.Log.LogInfo(
            $"loaded {ConfiguredCategories.Count} categories " +
            $"and a total of {ConfiguredPins.Count} pins across all categories from configuration");
    }

    internal static PinComponentGroup GetOrCreatePinGroup(PinComponent pin)
    {
        var group = AllGroups.FirstOrDefault(group => group.Accepts(pin));
        if (group == null)
        {
            group = new PinComponentGroup(pin.Config);
            AllGroups.Add(group);
        }

        return group;
    }

    internal static void AddMissingConfig(PinConfig config)
    {
        if (AutoMapPinsPlugin.PrefabDiscoveryEnabled.Value &&
            !MissingConfigs.Exists(missingConfig =>
                missingConfig.CategoryName == config.CategoryName &&
                missingConfig.InternalName == config.InternalName
            )
           )
        {
            MissingConfigs.Add(config);
            if (!AutoMapPinsPlugin.SilentDiscoveryEnabled.Value)
                AutoMapPinsPlugin.Log.LogWarning(
                    $"no configuration found for config {config.InternalName} " +
                    $"and category {config.CategoryName} - run console amp command and add config");
        }
    }
}