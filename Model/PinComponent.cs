using System.Collections;
using AutoMapPins.Patches;
using UnityEngine;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace AutoMapPins.Model;

internal class PinComponent : MonoBehaviour
{
    private PinComponent()
    {
    }

    private Vector3 Position;
    private bool IsVisible;
    private Coroutine Routine = null!;

    internal void Awake()
    {
        Position = gameObject.transform.position;
        IsVisible = Minimap.instance && Minimap.instance.IsExplored(transform.position);
        SetVisiblePin();
    }

    internal void OnDestroy()
    {
        if (Routine != null) StopCoroutine(Routine);
        MinimapPatch.UnpinObject(gameObject);
    }


    private IEnumerator VisibleCheck()
    {
        while (!IsVisible)
        {
            IsVisible = Minimap.instance.IsExplored(Position);
            if (IsVisible)
            {
                SetVisiblePin();
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void SetVisiblePin()
    {
        if (IsVisible) MinimapPatch.UpsertPin(gameObject);
        else
        {
            if (Routine != null) StopCoroutine(Routine);
            Routine = StartCoroutine(VisibleCheck());
        }
    }
}