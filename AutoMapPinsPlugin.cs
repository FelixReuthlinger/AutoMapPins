using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapPins.Model;
using AutoMapPins.Registry;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using ServerSync;

namespace AutoMapPins
{
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    public class AutoMapPinsPlugin : BaseUnityPlugin
    {
        public const string ModName = "AutoMapPins";
        public const string ModVersion = "1.0.0";
        private const string ModAuthor = "FixItFelix";
        private const string ModGuid = ModAuthor + "." + ModName;

        private readonly Harmony _harmony = new(ModGuid);

        [UsedImplicitly] public static readonly ManualLogSource LOGGER =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGuid)
            { DisplayName = ModName, CurrentVersion = ModVersion };

        private static ConfigEntry<bool> _configLocked = null!;

        internal static ConfigEntry<bool> ShowHarvestables = null!;
        internal static ConfigEntry<bool> ShowUncategorized = null!;
        internal static ConfigEntry<bool> UseSmallerIcons = null!;
        private static ConfigEntry<int> _padToWidth = null!;
        private static ConfigEntry<float> _fontFactor = null!;

        internal static readonly TemplateRegistry TEMPLATE_REGISTRY = new();
        internal static readonly ConfigRegistry CONFIG_REGISTRY = new();
        internal static AutoMapPinsPlugin Instance = null!;

        private void Awake()
        {
            Instance = this;
            
            _configLocked = CreateConfig("1 - General", "Lock Configuration", true,
                "If 'true' and playing on a server, config can only be changed on server-side configuration, " +
                "clients cannot override");
            ConfigSync.AddLockingConfigEntry(_configLocked);


            ShowHarvestables = CreateConfig(
                "Categories",
                "Harvestables",
                false,
                "Show harvestables like Berries and Mushrooms");
            ShowUncategorized = CreateConfig(
                "Categories",
                "Uncategorized",
                false,
                "Uncategorized things. WARNING: That's gonna be a lot!");


            UseSmallerIcons = CreateConfig(
                "Display",
                "Smaller Icons",
                false,
                "Will reduce the size of icons by 25% and text by about 50%");

            _padToWidth = CreateConfig(
                "Debug",
                "Pad to Width",
                60,
                "This is the amount of spaces the pin text will be padded to");
            _fontFactor = CreateConfig(
                "Debug",
                "Font Factor",
                1.8f,
                "This is the estimated factor by which normal characters are wider than spaces");

            ShowHarvestables.SettingChanged += ObjectRegistry.SettingChanged;
            ShowUncategorized.SettingChanged += ObjectRegistry.SettingChanged;

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
        }
        
        private static readonly Dictionary<Type, List<string>> UnmatchedNames = new();
        private static readonly Dictionary<Type, List<string>> UnmatchedHovers = new();

        public static void LogUnmatchedName(Type t, string name)
        {
            if (!UnmatchedNames.ContainsKey(t))
            {
                UnmatchedNames.Add(t, new List<string>());
            }

            if (!UnmatchedNames[t].Contains(name))
            {
                UnmatchedNames[t].Add(name);
                LOGGER.LogWarning($"New unmatched {t} discovered with name {name}. #{UnmatchedNames.Count}");
            }
        }

        public static void LogUnmatchedHover(Type t, string hover)
        {
            if (String.IsNullOrWhiteSpace(hover)) return;

            if (!UnmatchedHovers.ContainsKey(t))
            {
                UnmatchedHovers.Add(t, new List<string>());
            }

            if (!UnmatchedHovers[t].Contains(hover))
            {
                UnmatchedHovers[t].Add(hover);

                LOGGER.LogWarning($"New unmatched {t} discovered with hover {hover}. #{UnmatchedHovers.Count}");
            }
        }

        public static string Wrap(string orig)
        {
            if (UseSmallerIcons.Value)
            {
                var n = Math.Max(0, (int)Math.Ceiling((_padToWidth.Value - orig.Length * _fontFactor.Value) / 2.0));
                var pad = new string('\u00A0', n);
                return pad + orig.Replace(' ', '\u00A0') + pad;
            }

            return orig;
        }

        private ConfigEntry<T> CreateConfig<T>(string group, string parameterName, T value,
            ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, parameterName, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        public ConfigEntry<T> CreateConfig<T>(string group, string parameterName, T value, string description,
            bool synchronizedSetting = true) => CreateConfig(group, parameterName, value,
            new ConfigDescription(description), synchronizedSetting);
    }
}