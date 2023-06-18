using System.Collections.Generic;
using AutoMapPins.Model;
using BepInEx.Configuration;

namespace AutoMapPins.Registry
{
    internal class ConfigRegistry
    {
        private readonly Dictionary<string, ConfigEntry<bool>> _enabled = new();

        internal void Bind(PinTemplate template)
        {
            AutoMapPinsPlugin.LOGGER.LogDebug($"Binding Pin Template Config: {template.SingleLabel}");
            if (!_enabled.ContainsKey(template.SingleLabel))
            {
                var cfg = AutoMapPinsPlugin.Instance.CreateConfig(template.FirstBiome.ToString(),
                    template.SingleLabel, false, "Whether to show this on the map or not");
                cfg.SettingChanged += ObjectRegistry.SettingChanged;
                _enabled.Add(template.SingleLabel, cfg);
            }
        }

        internal bool? IsEnabled(PinTemplate template)
        {
            if (!_enabled.ContainsKey(template.SingleLabel)) return null;
            else return _enabled[template.SingleLabel].Value;
        }
    }
}