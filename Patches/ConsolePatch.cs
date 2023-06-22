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

    [UsedImplicitly]
    // ReSharper disable once InconsistentNaming
    private static void Postfix(Console __instance)
    {
        _ = new Terminal.ConsoleCommand("amp", "auto map pins commands",
            consoleEventArgs =>
            {
                if (consoleEventArgs.Length > 1)
                {
                    AutoMapPinsPlugin.Log.LogInfo(
                        $"called amp with args: '{string.Join(", ", consoleEventArgs.Args.ToList())}'");
                    switch (consoleEventArgs.Args[1])
                    {
                        case PrintPinsWithMissingConfigs:
                            string filePathCategories =
                                AutoMapPinsPlugin.FileIO.GetSingleFile(PrintPinsWithMissingConfigs);
                            AutoMapPinsPlugin.FileIO.WriteFile(filePathCategories,
                                Registry.MissingConfigs
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
                                    )
                            );
                            break;
                    }
                }
                else
                {
                    __instance.Print(
                        "Auto Map Pins (amp) console commands - use 'amp' followed by one of the following options");
                    __instance.Print(
                        $" {PrintPinsWithMissingConfigs} --> will print all pins not yet configured to yaml file");
                }
            }, optionsFetcher: OptionFetcher);

        __instance.updateCommandList();
    }

    private static List<string> OptionFetcher() => new() { PrintPinsWithMissingConfigs };
}