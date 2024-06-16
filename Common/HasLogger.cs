using BepInEx.Logging;

namespace AutoMapPins.Common;

public abstract class HasLogger
{
    protected static ManualLogSource Log => AutoMapPinsPlugin.Log;
}