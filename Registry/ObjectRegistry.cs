using System;
using System.Collections.Generic;
using AutoMapPins.Model;

namespace AutoMapPins.Registry
{
    internal abstract class ObjectRegistry
    {
        private static readonly List<PinnedObject> PinnedObjects = new();


        public static void AddPinnedObject(PinnedObject pin)
        {
            PinnedObjects.Add(pin);
        }

        public static void RemovePinnedObject(PinnedObject pin)
        {
            PinnedObjects.Remove(pin);
        }


        internal static void SettingChanged(object sender, EventArgs e)
        {
            AutoMapPinsPlugin.LOGGER.LogDebug($"Setting has changed. Rechecking {PinnedObjects.Count} Pinned Objects");
            foreach (var pin in PinnedObjects)
            {
                pin.UpdatePinVisibility();
            }
        }
    }
}