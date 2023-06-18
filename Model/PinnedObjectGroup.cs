using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoMapPins.Model
{
    internal class PinnedObjectGroup
    {
        private const double GroupDistance = 12; //8 is almost enough, 10 looks good so far
        private Minimap.PinData Pin { get; set; }
        private PinTemplate Template { get; set; }
        private List<PinnedObject> Objects { get; } = new();
        private List<Vector3> KnownLocations { get; } = new();
        private Vector3 Middle { get; set; }
        private bool IsVisible { get; set; }


        private bool Accepts(PinnedObject obj)
        {
            return obj.Template == Template &&
                   Objects.Any(o =>
                   {
                       Vector3 position;
                       var position1 = obj.transform.position;
                       return Math.Sqrt(Math.Pow((position = o.transform.position).x - position1.x, 2) +
                                        Math.Pow(position.z - position1.z, 2)) < GroupDistance;
                   });
        }

        public void Add(PinnedObject obj)
        {
            Objects.Add(obj);
            if (!KnownLocations.Contains(obj.transform.position))
            {
                KnownLocations.Add(obj.transform.position);

                var avgX = KnownLocations.Select(p => p.x).Sum() / KnownLocations.Count;
                var avgY = KnownLocations.Select(p => p.y).Sum() / KnownLocations.Count;
                var avgZ = KnownLocations.Select(p => p.z).Sum() / KnownLocations.Count;

                Middle = new Vector3(avgX, avgY, avgZ);
            }

            UpdatePinVisibility();
        }

        public void Remove(PinnedObject obj)
        {
            Objects.Remove(obj);

            if (Objects.Count == 0)
            {
                AllGroups.Remove(this);
            }

            UpdatePinVisibility();
        }

        public void UpdatePinVisibility()
        {
            bool toShow = Template.IsEnabled() && Objects.Count > 0;

            IsVisible = toShow;
            if (IsVisible)
            {
                if (Pin != null)
                {
                    Minimap.instance.RemovePin(Pin);
                }

                var txt = (KnownLocations.Count > 1 ? (KnownLocations.Count + " ") : "") + Template.Label;

                Pin = Minimap.instance.AddPin(Middle, Minimap.PinType.Icon3, AutoMapPinsPlugin.Wrap(txt), false, false);
                if (Template.Icon != null)
                {
                    Pin.m_icon = Template.Icon;
                }
            }
            else if (Pin != null && Minimap.instance != null)
            {
                Minimap.instance.RemovePin(Pin);
            }
        }


        private static readonly List<PinnedObjectGroup> AllGroups = new List<PinnedObjectGroup>();

        public static PinnedObjectGroup? FindOrCreateFor(PinnedObject obj)
        {
            var group = AllGroups.FirstOrDefault(g => g.Accepts(obj));

            if (group == null)
            {
                group = new PinnedObjectGroup() { Template = obj.Template };
                AllGroups.Add(group);
            }

            return group;
        }
    }
}