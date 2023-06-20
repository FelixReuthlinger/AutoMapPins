using System.Collections.Generic;
using System.Linq;
using AutoMapPins.FileIO;
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
                    AutoMapPinsPlugin.LOGGER.LogInfo(
                        $"called amp with args: '{string.Join(", ", consoleEventArgs.Args.ToList())}'");
                    switch (consoleEventArgs.Args[1])
                    {
                        case PrintPinsWithMissingConfigs:
                            var fileIO = new YamlFileStorage<CategoryConfig>(AutoMapPinsPlugin.ModGuid);
                            string filePathCategories = fileIO.GetSingleFile(PrintPinsWithMissingConfigs);
                            fileIO.WriteFile(filePathCategories, Registry.GetConfigurationFromManagedPins());
                            AutoMapPinsPlugin.LOGGER.LogInfo($"wrote file {filePathCategories}");
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