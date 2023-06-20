using System;
using System.Linq;
using AutoMapPins.Icons;
using UnityEngine;

namespace AutoMapPins.Model;

internal class PinComponent : MonoBehaviour
{
    private Minimap.PinData _pin = null!;
    private bool _visible = false;
    public PinConfig Config = null!;

    public void InitializeFromConfig(PinConfig config)
    {
        Config = config; // config is always required
        if (Registry.ConfiguredCategories.TryGetValue(config.CategoryName, out CategoryConfig category))
        {
            AutoMapPinsPlugin.LOGGER.LogDebug(
                $"config {config.InternalName} is active '{config.IsActive}' " +
                $"- category {config.CategoryName} is active '{category.CategoryActive}'");
            if (config.IsActive && category.CategoryActive)
            {
                var position = transform.position;
                var existingPin = FindSimilarPin(position, Config.InternalName);
                _pin = existingPin ?? Minimap.instance.AddPin(position, Minimap.PinType.Icon1,
                    Config.Name, save: Config.IsPermanent, isChecked: false);
                _pin.m_icon = GetIcon(Config.IconName);
            }
        }
        else
        {
            AutoMapPinsPlugin.LOGGER.LogWarning(
                $"no configuration found for config {config.InternalName} and category {config.CategoryName}");
        }
    }

    public Sprite GetIcon(string iconName)
    {
        if (Assets.ICONS.TryGetValue(iconName, out Sprite result))
            return result;

        return Assets.DEFAULT_ICON;
    }

    public static Minimap.PinData? FindSimilarPin(Vector3 position, string name)
    {
        return Minimap.instance.m_pins.FirstOrDefault(pin =>
            Math.Sqrt(
                Math.Pow(pin.m_pos.x - position.x, 2) +
                Math.Pow(pin.m_pos.y - position.y, 2) +
                Math.Pow(pin.m_pos.z - position.z, 2)
            ) < 1f &&
            pin.m_name.Contains(name.Replace(' ', '\u00A0'))
        );
    }
}