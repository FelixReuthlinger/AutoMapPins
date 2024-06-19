using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Common;
using AutoMapPins.Data;
using AutoMapPins.Model;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches;

[HarmonyPatch(typeof(Console), nameof(Console.Awake))]
internal class ConsolePatches : HasLogger
{
    private const string WriteMissingConfigsOption = "write_missing_configs_file";
    private const string ClearPins = "clear_pins";

    private const string ClearAllMessage = "cleared all pins from map";

    [UsedImplicitly]
    private static void Postfix(Console __instance)
    {
        _ = new Terminal.ConsoleCommand("amp", description: "auto map pins commands",
            action: (Terminal.ConsoleEvent)(consoleEventArgs =>
            {
                if (consoleEventArgs.Length > 1)
                {
                    AutoMapPinsPlugin.Log.LogInfo(
                        $"called amp with args: '{string.Join(", ", consoleEventArgs.Args.ToList())}'");
                    switch (consoleEventArgs.Args[1])
                    {
                        case ClearPins:
                            if (Minimap.instance)
                            {
                                Map.Clear();
                                Minimap.instance.ClearPins();
                                __instance.Print(ClearAllMessage);
                                Log.LogWarning(ClearAllMessage);
                            }

                            break;
                        case WriteMissingConfigsOption:
                            WriteMissingConfigs();
                            break;
                    }
                }
                else
                {
                    __instance.Print(
                        "Auto Map Pins (amp) console commands - use 'amp' followed by one of the following options");
                    __instance.Print(
                        $" {ClearPins} --> will remove all pins from map (use this in case the mod went crazy " +
                        $"and created too many pins before ;)");
                    __instance.Print(
                        $" {WriteMissingConfigsOption} --> will write all objects without config to a yaml file");
                }
            }),
            optionsFetcher: (Terminal.ConsoleOptionsFetcher)OptionFetcher
        );
    }

    private static void WriteMissingConfigs()
    {
        Dictionary<string, Config.Category> missingConfigs = Registry.MissingConfigs
            .Select(config => new KeyValuePair<string, string>("category", config))
            .GroupBy(kv => kv.Key)
            .ToDictionary(group => group.Key, group =>
                new Config.Category
                {
                    IsActive = false,
                    IndividualConfiguredObjects = new Dictionary<string, Config.Pin>
                        { { "example", new Config.Pin() } },
                    CategoryConfiguredObjects = group
                        .Select(kv => kv.Value)
                        .OrderBy(x => x)
                        .ToList()
                }
            );
        if (missingConfigs.Count > 0)
            AutoMapPinsPlugin.FileIO.WriteFile(AutoMapPinsPlugin.FileIO.GetSingleFile(WriteMissingConfigsOption),
                missingConfigs);
        else
            Log.LogWarning("could not print any configs, since no config was recorded during game play");
    }

    private static List<string> OptionFetcher() => new() { WriteMissingConfigsOption, ClearPins };
}