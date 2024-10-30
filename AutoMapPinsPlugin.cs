using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapPins.Data;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ServerSync;

namespace AutoMapPins
{
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    public class AutoMapPinsPlugin : BaseUnityPlugin
    {
        internal const string ModName = "AutoMapPins";
        internal const string ModVersion = "2.2.2";
        private const string ModAuthor = "FixItFelix";
        private const string ModGuid = ModAuthor + "." + ModName;
        private const string ConfigFileName = ModGuid + ".cfg";

        private static AutoMapPinsPlugin _instance = null!;

        private readonly Harmony _harmony = new(ModGuid);

        public static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource(ModGuid);

        internal static readonly YamlFileStorage FileIO = new(ModGuid);

        private static readonly ConfigSync ConfigSync = new(ModGuid)
        {
            DisplayName = ModName,
            CurrentVersion = ModVersion
        };

        private static ConfigEntry<bool> _configLocked = null!;
        private static CustomSyncedValue<Dictionary<string, string>> _categoryPinsConfigFilesContent = null!;
        internal static ConfigEntry<float> GroupingRadius = null!;
        internal static ConfigEntry<float> MaxDetectionHeight = null!;
        internal static ConfigEntry<bool> PrefabDiscoveryEnabled = null!;
        internal static ConfigEntry<bool> SilentDiscoveryEnabled = null!;

        private void Awake()
        {
            _instance = this;

            _configLocked = CreateConfig("1 - General", "Lock Configuration", true,
                "If 'true' and playing on a server, config can only be changed on server-side configuration, " +
                "clients cannot override");
            ConfigSync.AddLockingConfigEntry(_configLocked);

            var generalGroup = "1 - General";
            PrefabDiscoveryEnabled = CreateConfig(generalGroup, "Enable the prefab discovery", false,
                "This option will either enable (true) or disable (false, default) the discovery of new " +
                "prefabs that have not yet been configured. For smoother gameplay you can simply disable it, the mod " +
                "will then not print to console that there are new objects that have not been configured, yet. If you " +
                "want to create new configurations for not configured prefabs, you will need to enable this flag.");

            SilentDiscoveryEnabled = CreateConfig(generalGroup, "Prefab discovery silent mode", true,
                "This option will either enable (true, default) or disable (false) the log messages on " +
                "discovery of new prefabs that have not yet been configured. For smoother gameplay you can simply " +
                "enable it, the mod will then not print to console that there are new objects that have not been " +
                "configured, yet. For finding out if there are prefabs missing that were not configured, you will " +
                "need to disable this flag.");

            MaxDetectionHeight = CreateConfig(generalGroup, "Maximum height to map objects", 4000.0f,
                "This option will set the height over sea level until where game objects will get the auto " +
                "map pins logic applied. The usual default is '4000', since over 4000 meters over ground the dungeons " +
                "are placed in the game scene. BEWARE: if you set this to a lower height, your map has chances to get " +
                "pins added that point to a location inside a dungeon that is just above the height on ground," +
                " you should better not change this.");

            var groupingGroup = "2 - Grouping";
            GroupingRadius = CreateConfig(groupingGroup, "Fallback General Grouping Radius", 15.0f,
                "Grouping radius can be set per configured pin, but if it was set to 0 or no config for a " +
                "pin was provided, this value will be used instead. Radius that will be applied when trying to " +
                "group pins together that have grouping enabled. Default 15.0");

            _categoryPinsConfigFilesContent = new(ConfigSync,
                "CategoryPinsConfigFilesContent", new Dictionary<string, string>());
            _categoryPinsConfigFilesContent.ValueChanged += ReloadRegistry;
            ReadYamlFileContent(null, null);
            SetupFileWatcher(ConfigFileName);

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            LogUsageMode();
        }

        internal static void ReadYamlFileContent(object? _, FileSystemEventArgs? __)
        {
            Log.LogInfo("loading pin configs from yaml files");
            _categoryPinsConfigFilesContent.Value = FileIO.ReadConfigFiles();
        }

        private static void ReloadRegistry()
        {
            Registry.InitializeRegistry(
                newConfiguredCategories: FileIO.DeserializeAndMergeFileData(_categoryPinsConfigFilesContent.Value));
        }

        private void SetupFileWatcher(string fileName)
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, fileName);
            watcher.Changed += ReloadConfig;
            watcher.Created += ReloadConfig;
            watcher.Renamed += ReloadConfig;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReloadConfig(object? _, FileSystemEventArgs? __)
        {
            Log.LogWarning($"config file '{ConfigFileName}' change registered");
            Config.Reload();
            LogUsageMode();
        }

        private static void LogUsageMode()
        {
            if (PrefabDiscoveryEnabled.Value)
                Log.LogWarning("loaded mod with game object discovery enabled, deactivate to increase performance");
            if (!SilentDiscoveryEnabled.Value)
                Log.LogWarning(
                    "silent discovery mode was deactivated, this will print log each time an object without " +
                    "config will be approached");
        }

        public void OnDestroy()
        {
            _harmony.UnpatchSelf();
        }

        private static ConfigEntry<T> CreateConfig<T>(string group, string parameterName, T value,
            ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = _instance.Config.Bind(group, parameterName, value, description);
            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;
            return configEntry;
        }

        private static ConfigEntry<T> CreateConfig<T>(string group, string parameterName, T value, string description,
            bool synchronizedSetting = true) => CreateConfig(group, parameterName, value,
            new ConfigDescription(description), synchronizedSetting);
    }
}