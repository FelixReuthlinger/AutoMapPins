using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Data;
using AutoMapPins.Model;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches;

[HarmonyPatch(typeof(Console), nameof(Console.Awake))]
internal class ConsolePatches
{
    private const string PrintPinsWithMissingConfigs = "print_pins_missing_configs";
    private const string ClearPins = "clear_pins";

    [UsedImplicitly]
    // ReSharper disable once InconsistentNaming
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
                            if (Minimap.instance != null)
                            {
                                AutoMapPinsPlugin.Log.LogWarning($"cleared all pins from map!");
                                Minimap.instance.ClearPins();
                            }

                            break;
                        case PrintPinsWithMissingConfigs:
                            string filePathCategories =
                                AutoMapPinsPlugin.FileIO.GetSingleFile(PrintPinsWithMissingConfigs);
                            Dictionary<string, CategoryConfig> missingConfigs = Registry.MissingConfigs
                                .GroupBy(config => config.CategoryName)
                                .ToDictionary(group => group.Key, group =>
                                    new CategoryConfig
                                    {
                                        CategoryActive = false,
                                        Pins = group
                                            .GroupBy(config => config.InternalName)
                                            .ToDictionary(
                                                configGroup => configGroup.Key,
                                                configGroup => configGroup.First()
                                            )
                                    }
                                );
                            if (missingConfigs.Count > 0)
                                AutoMapPinsPlugin.FileIO.WriteFile(filePathCategories, missingConfigs);
                            else
                                AutoMapPinsPlugin.Log.LogWarning(
                                    "could not print any configs, since no config was recorded during game play.");
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
                        $" {PrintPinsWithMissingConfigs} --> will print all pins not yet configured to yaml file");
                }
            }),
            optionsFetcher: (Terminal.ConsoleOptionsFetcher)OptionFetcher
        );

        __instance.updateCommandList();
    }

    private static List<string> OptionFetcher() => new() { PrintPinsWithMissingConfigs, ClearPins };
}