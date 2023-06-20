using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapPins.FileIO;

namespace AutoMapPins.Model;

public abstract class Registry
{
    internal static Dictionary<string, CategoryConfig> ConfiguredCategories = null!;
    internal static Dictionary<string, PinConfig> ConfiguredPins = null!;
    internal static readonly List<PinComponent> MANAGED_PINS = new();

    internal static void InitializeRegistry()
    {
        var fileIO = new YamlFileStorage<CategoryConfig>(AutoMapPinsPlugin.ModGuid);
        var file = fileIO.GetSingleFile("categories");
        try
        {
            ConfiguredCategories = fileIO.ReadFile(file);
            AutoMapPinsPlugin.LOGGER.LogInfo($"initialized pin categorization from file '{file}'");
        }
        catch (FileNotFoundException)
        {
            AutoMapPinsPlugin.LOGGER.LogInfo($"no config loaded, since {file} was not present");
            ConfiguredCategories = new Dictionary<string, CategoryConfig>();
        }
        catch (Exception e)
        {
            AutoMapPinsPlugin.LOGGER.LogWarning(
                $"could not initialize categories from config file '{file}' due to {e.Message}\n{e.StackTrace}");
            ConfiguredCategories = new Dictionary<string, CategoryConfig>();
        }

        ConfiguredPins = ConfiguredCategories
            .Select(categories => categories.Value.Pins)
            .SelectMany(x => x)
            .GroupBy(kv => kv.Key)
            .ToDictionary(kv => kv.Key, group => group.First().Value);

        AutoMapPinsPlugin.LOGGER.LogInfo(
            $"loaded {ConfiguredCategories.Count} categories and {ConfiguredPins.Count} pins from configuration");
    }

    internal static Dictionary<string, CategoryConfig> GetConfigurationFromManagedPins()
    {
        AutoMapPinsPlugin.LOGGER.LogInfo($"creating config data from {MANAGED_PINS.Count} pins");
        return CategoryConfig.FromPins(MANAGED_PINS.Select(pinObject => pinObject.Config).ToList());
    }
}