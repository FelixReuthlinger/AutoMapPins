using System.Reflection;
using AutoMapPins.Model;
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
        internal const string ModName = "AutoMapPins";
        internal const string ModVersion = "1.0.0";
        private const string ModAuthor = "FixItFelix";
        internal const string ModGuid = ModAuthor + "." + ModName;

        private readonly Harmony _harmony = new(ModGuid);

        [UsedImplicitly] public static readonly ManualLogSource LOGGER =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGuid)
            { DisplayName = ModName, CurrentVersion = ModVersion };

        private static ConfigEntry<bool> _configLocked = null!;

        private void Awake()
        {
            _configLocked = CreateConfig("1 - General", "Lock Configuration", true,
                "If 'true' and playing on a server, config can only be changed on server-side configuration, " +
                "clients cannot override");
            ConfigSync.AddLockingConfigEntry(_configLocked);
            
            Registry.InitializeRegistry();

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
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

        private ConfigEntry<T> CreateConfig<T>(string group, string parameterName, T value, string description,
            bool synchronizedSetting = true) => CreateConfig(group, parameterName, value,
            new ConfigDescription(description), synchronizedSetting);
    }
}