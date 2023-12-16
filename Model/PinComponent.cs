using System;
using System.Collections;
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
    private bool IsVisible;
    private Coroutine Routine = null!;

    private IEnumerator VisibleCheck()
    {
        var position = gameObject.transform.position;
        while (!IsVisible)
        {
            IsVisible = Minimap.instance.IsExplored(position);
            if (IsVisible)
            {
                SetVisiblePin(position);
                yield break;
            }

            //yield return new WaitForSeconds(5);
            yield return new WaitForFixedUpdate();
        }
    }

    internal void OnDestroy()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (Routine != null) StopCoroutine(Routine);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (Minimap.instance != null && PinObject != null && !Config.IsPermanent)
        {
            Minimap.instance.RemovePin(PinObject);
            Group?.Remove(this);
        }
    }

    internal static void Create(GameObject gameObject)
    {
        Vector3 position = gameObject.transform.position;
        PinComponent pinComponent = gameObject.AddComponent<PinComponent>();
        pinComponent.IsVisible = Minimap.instance.IsExplored(position);
        pinComponent.Config = PinConfig.FromGameObject(gameObject);
        pinComponent.SetVisiblePin(position);
    }

    private void SetVisiblePin(Vector3 position)
    {
        if (IsVisible)
        {
            if (Registry.ConfiguredCategories.TryGetValue(Config.CategoryName, out CategoryConfig category))
            {
                if (Config.IsActive && category.CategoryActive)
                {
                    PinObject = FindSimilarPin(position, Config.Name) ?? Minimap.instance.AddPin(
                        pos: position,
                        type: Minimap.PinType.Icon1,
                        name: Config.Name,
                        save: Config.IsPermanent,
                        isChecked: false
                    );
                    PinObject.m_icon = Assets.GetIcon(Config.IconName);
                    if (Config.Groupable && Group == null) PinComponentGroup.GroupPin(this);
                }
            }
            else Registry.AddMissingConfig(Config);
        }
        else
        {
            if (Routine != null)
            {
                AutoMapPinsPlugin.Log.LogInfo($"stopping existing coroutine on creation");
                StopCoroutine(Routine);
            }

            Routine = StartCoroutine(VisibleCheck());
        }
    }

    private static Minimap.PinData? FindSimilarPin(Vector3 position, string name)
    {
        return Minimap.instance.m_pins.FirstOrDefault(pin =>
            InDistance(pin.m_pos, position, 1.0f) && pin.m_name == name);
    }

    private static bool InDistance(Vector3 pinPosition, Vector3 referencePosition, float distance)
    {
        return Math.Sqrt(
            Math.Pow(pinPosition.x - referencePosition.x, 2) +
            Math.Pow(pinPosition.y - referencePosition.y, 2) +
            Math.Pow(pinPosition.z - referencePosition.z, 2)
        ) < distance;
    }
}