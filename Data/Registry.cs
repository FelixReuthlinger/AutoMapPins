using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Model;

namespace AutoMapPins.Data;

public static class Registry
{
    internal static Dictionary<string, CategoryConfig> ConfiguredCategories = null!;
    internal static Dictionary<string, PinConfig> ConfiguredPins = null!;
    internal static readonly List<PinComponent> PINS_NO_CONFIG = new();

    public static void InitializeRegistry(Dictionary<string, CategoryConfig> configuredCategories)
    {
        ConfiguredCategories = configuredCategories;
        ConfiguredPins = ConfiguredCategories
            .Select(categories => categories.Value.Pins)
            .SelectMany(x => x)
            .GroupBy(kv => kv.Key)
            .ToDictionary(kv => kv.Key, group => group.First().Value);

        AutoMapPinsPlugin.LOGGER.LogInfo(
            $"loaded {ConfiguredCategories.Count} categories " +
            $"and a total of {ConfiguredPins.Count} pins across all categories from configuration");
    }

    internal static Dictionary<string, CategoryConfig> GetConfigurationFromManagedPins()
    {
        AutoMapPinsPlugin.LOGGER.LogInfo($"creating config data from {PINS_NO_CONFIG.Count} pins");
        return CategoryConfig.FromPins(PINS_NO_CONFIG.Select(pinObject => pinObject.Config).ToList());
    }
}