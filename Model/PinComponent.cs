using System.Collections;
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
        IsVisible = Minimap.instance != null && Minimap.instance.IsExplored(transform.position);
        SetVisiblePin();
    }

    internal void OnDestroy()
    {
        if (Routine != null) StopCoroutine(Routine);
        Map.RemovePin(gameObject);
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
        if (IsVisible) Map.CreatePin(gameObject);
        else
        {
            if (Routine != null) StopCoroutine(Routine);
            Routine = StartCoroutine(VisibleCheck());
        }
    }
}