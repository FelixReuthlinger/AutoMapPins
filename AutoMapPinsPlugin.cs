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
        internal const string ModVersion = "1.2.2";
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
        internal static ConfigEntry<bool> PrefabDiscoveryEnabled = null!;
        internal static ConfigEntry<bool> SilentDiscoveryEnabled = null!;

        private void Awake()
        {
            _instance = this;

            _configLocked = CreateConfig("1 - General", "Lock Configuration", true,
                "If 'true' and playing on a server, config can only be changed on server-side configuration, " +
                "clients cannot override");
            ConfigSync.AddLockingConfigEntry(_configLocked);

            PrefabDiscoveryEnabled = CreateConfig("1 - General", "Enable the prefab discovery", false,
                "This option will either enable (true) or disable (false, default) the discovery of new " +
                "prefabs that have not yet been configured. For smoother gameplay you can simply disable it, the mod " +
                "will then not print to console that there are new objects that have not been configured, yet. If you " +
                "want to create new configurations for not configured prefabs, you will need to enable this flag.");

            SilentDiscoveryEnabled = CreateConfig("1 - General", "Prefab discovery silent mode", true,
                "This option will either enable (true, default) or disable (false) the log messages on " +
                "discovery of new prefabs that have not yet been configured. For smoother gameplay you can simply " +
                "enable it, the mod will then not print to console that there are new objects that have not been " +
                "configured, yet. For finding out if there are prefabs missing that were not configured, you will " +
                "need to disable this flag.");

            GroupingRadius = CreateConfig("2 - Grouping", "Fallback General Grouping Radius", 15.0f,
                "Grouping radius can be set per configured pin, but if it was set to 0 or no config for a " +
                "pin was provided, this value will be used instead. Radius that will be applied when trying to " +
                "group pins together that have grouping enabled. Default 15.0");

            _categoryPinsConfigFilesContent = new(ConfigSync,
                "CategoryPinsConfigFilesContent",
                FileIO.ReadConfigFiles());
            Registry.InitializeRegistry(
                FileIO.DeserializeAndMergeFileData(_categoryPinsConfigFilesContent.Value)
            );
            SetupFileWatcher(ConfigFileName);

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            if (PrefabDiscoveryEnabled.Value)
                Log.LogInfo(
                    "loaded mod with configuration discovery enabled, this will create log messages " +
                    "for each not configured prefab");
        }

        internal static void ReadSyncAndLoadRegistry()
        {
            Log.LogInfo("reloading pin configs from files");
            _categoryPinsConfigFilesContent.AssignLocalValue(FileIO.ReadConfigFiles());
            Registry.InitializeRegistry(
                FileIO.DeserializeAndMergeFileData(_categoryPinsConfigFilesContent.Value)
            );
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
            Config.Reload();
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