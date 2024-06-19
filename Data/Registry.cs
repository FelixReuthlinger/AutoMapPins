using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Common;
using AutoMapPins.Icons;
using AutoMapPins.Model;

namespace AutoMapPins.Data;

public abstract class Registry : HasLogger
{
    internal static Dictionary<string, Config.Pin> ConfiguredPins = new();
    private static HashSet<string> ConfiguredObjects = new();
    internal static readonly HashSet<string> MissingConfigs = new();

    internal static void InitializeRegistry(Dictionary<string, Config.Category> newConfiguredCategories)
    {
        Log.LogDebug("initializing registry");
        var updatePins = Map.HasPins();
        MissingConfigs.Clear();
        ConfiguredObjects.Clear();
        ConfiguredPins.Clear();
        RegisterConfiguredObjects(newConfiguredCategories);
        ConfiguredPins = LoadActivePinConfigs(newConfiguredCategories);
        if (updatePins) Map.UpdatePins();
        Log.LogDebug("registry initialization successful");
    }

    private static void RegisterConfiguredObjects(Dictionary<string, Config.Category> newConfiguredCategories)
    {
        List<string> objectNames = newConfiguredCategories
            .SelectMany(categories => categories.Value.Pins)
            .Select(pin => pin.Key).ToList();

        ConfiguredObjects = objectNames.Distinct().ToHashSet();

        Dictionary<string, int> duplicatedConfigs = objectNames
            .GroupBy(name => name)
            .ToDictionary(group => group.Key, group => group.Count())
            .Where(kv => kv.Value > 1)
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        if (duplicatedConfigs.Count <= 0) return;
        Log.LogWarning("found duplicates in configured game objects:");
        foreach (var config in duplicatedConfigs)
            AutoMapPinsPlugin.Log.LogWarning($"name '{config.Key}' has '{config.Value}' entries in config");

        Log.LogWarning(
            "duplicates are not allowed, only first entry will be used, please check and fix the config files");
    }

    private static Dictionary<string, Config.Pin> LoadActivePinConfigs(
        Dictionary<string, Config.Category> newConfiguredCategories)
    {
        Dictionary<string, Config.Pin> activePinConfigs = newConfiguredCategories
            .Where(category => category.Value.CategoryActive)
            .SelectMany(x => x.Value.Pins)
            .Where(pin => pin.Value.IsActive)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        Log.LogInfo($"loaded '{activePinConfigs.Count}' configs for active pins");
        return activePinConfigs;
    }

    internal static void AddMissingConfig(string internalName)
    {
        if (!AutoMapPinsPlugin.PrefabDiscoveryEnabled.Value) return;
        if (ConfiguredObjects.Contains(internalName)) return;
        if (MissingConfigs.Add(internalName))
        {
            if (AutoMapPinsPlugin.SilentDiscoveryEnabled.Value) return;
            Player.m_localPlayer?.Message(MessageHud.MessageType.TopLeft,
                $"discovered object '{internalName}' that was not configured", icon: Assets.GetIcon("amp"));
            Log.LogWarning($"discovered new game object named '{internalName}' that has no configuration");
        }
    }
}