﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Common;
using AutoMapPins.Data;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches;

[HarmonyPatch(typeof(Console), nameof(Console.Awake))]
internal class ConsolePatches : HasLogger
{
    private const string WriteMissingConfigsOption = "write_missing_configs_file";
    private const string PrintEffectiveConfig = "print_effective_config";
    private const string DebugLogPins = "debug_log_pins";
    private const string ClearPins = "clear_pins";

    private const string ClearAllMessage = "cleared all pins from map";

    private const string PrintEffectiveConfigRequiredNameMessage =
        "for printing the effective config of an object, please provide the object name as argument to the command";

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
                        case DebugLogPins:
                            if (Minimap.instance)
                                Log.LogInfo("vanilla map registered pins:\n" + MinimapPatch.PrintMapPins());
                            break;
                        case ClearPins:
                            if (Minimap.instance)
                            {
                                Minimap.instance.ClearPins();
                                __instance.Print(ClearAllMessage);
                                Log.LogWarning(ClearAllMessage);
                            }

                            break;
                        case PrintEffectiveConfig:
                            if (consoleEventArgs.Args.Length < 3)
                            {
                                __instance.Print(PrintEffectiveConfigRequiredNameMessage);
                                break;
                            }

                            var objectName = consoleEventArgs.Args[2];
                            if (!String.IsNullOrEmpty(objectName) &&
                                Registry.ConfiguredPins.TryGetValue(objectName, out Config.Pin config))
                            {
                                __instance.Print($"{objectName}:");
                                __instance.Print("  " + config.ToString().Replace("\n", "\n  "));
                            }
                            else __instance.Print($"no config found for object name '{objectName}'");


                            break;
                        case WriteMissingConfigsOption:
                            WriteMissingConfigs();
                            break;
                        default:
                            PrintUsage(__instance);
                            break;
                    }
                }
                else PrintUsage(__instance);
            }),
            optionsFetcher: (Terminal.ConsoleOptionsFetcher)OptionFetcher
        );
    }

    private static void PrintUsage(Console __instance)
    {
        __instance.Print(
            "Auto Map Pins (amp) console commands - use 'amp' followed by one of the following options");
        __instance.Print($" {DebugLogPins} --> print all active pins to log");
        __instance.Print(
            $" {ClearPins} --> will remove all pins from map (use this in case the mod went crazy " +
            $"and created too many pins before ;)");
        __instance.Print(
            $" {PrintEffectiveConfig} <object name> --> replace '<object name>' with the " +
            $"object to print the effective config for, this can be used to debug config " +
            $"issues and config inheritance");
        __instance.Print(
            $" {WriteMissingConfigsOption} --> will write all objects without config to a yaml file");
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
                    {
                        {
                            "example", new Config.Pin
                            {
                                Name = "example name",
                                IconName = "mine",
                                IconColorRGBA = Config.PinColor.White
                            }
                        }
                    },
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

    private static List<string> OptionFetcher() => new()
        { PrintEffectiveConfig, WriteMissingConfigsOption, DebugLogPins, ClearPins };
}