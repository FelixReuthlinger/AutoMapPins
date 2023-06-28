using System;
using System.Linq;
using AutoMapPins.Data;
using AutoMapPins.Icons;
using UnityEngine;

namespace AutoMapPins.Model;

internal class PinComponent : MonoBehaviour
{
    private PinComponent()
    {
    }

    internal Minimap.PinData PinObject = null!;
    internal PinConfig Config = null!;
    internal PinComponentGroup? Group;

    internal void OnDestroy()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (Minimap.instance != null && PinObject != null && !Config.IsPermanent)
            Minimap.instance.RemovePin(PinObject);
        Group?.Remove(this);
    }

    internal static void Create(GameObject gameObject)
    {
        PinComponent pinComponent = gameObject.AddComponent<PinComponent>();
        pinComponent.Config = PinConfig.FromGameObject(gameObject);
        if (Registry.ConfiguredCategories.TryGetValue(pinComponent.Config.CategoryName, out CategoryConfig category))
        {
            if (pinComponent.Config.IsActive && category.CategoryActive)
            {
                var position = gameObject.transform.position;
                pinComponent.PinObject = FindSimilarPin(position, pinComponent.Config.Name)
                                         ?? Minimap.instance.AddPin(
                                             pos: position,
                                             type: Minimap.PinType.Icon1,
                                             name: pinComponent.Config.Name,
                                             save: pinComponent.Config.IsPermanent,
                                             isChecked: false
                                         );
                pinComponent.PinObject.m_icon = Assets.GetIcon(pinComponent.Config.IconName);
                if (pinComponent.Config.Groupable && pinComponent.Group == null)
                    PinComponentGroup.GroupPin(pinComponent);
            }
        }
        else Registry.AddMissingConfig(pinComponent.Config);
    }

    private static Minimap.PinData? FindSimilarPin(Vector3 position, string name)
    {
        return Minimap.instance.m_pins.FirstOrDefault(pin =>
            Math.Sqrt(
                Math.Pow(pin.m_pos.x - position.x, 2) +
                Math.Pow(pin.m_pos.y - position.y, 2) +
                Math.Pow(pin.m_pos.z - position.z, 2)
            ) < 1.0f
            && pin.m_name == name
        );
    }
}