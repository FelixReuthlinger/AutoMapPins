using AutoMapPins.Patches;
using AutoMapPins.Registry;
using UnityEngine;

namespace AutoMapPins.Model
{
    internal class PinnedObject : MonoBehaviour
    {
        private Minimap.PinData? _pin;

        public PinnedObject(Minimap.PinData? pin, bool visible, PinTemplate template, PinnedObjectGroup? group)
        {
            _pin = pin;
            _visible = visible;
            Template = template;
            Group = group;
        }

        public PinnedObject(PinTemplate template)
        {
            Template = template;

            if (Template.IsGrouped)
            {
                Group = PinnedObjectGroup.FindOrCreateFor(this);
                Group?.Add(this);
            }

            if (Template.IsEnabled())
            {
                if (Template.IsGrouped)
                {
                    Group?.UpdatePinVisibility();
                }
                else
                {
                    ShowPin();
                }
            }

            ObjectRegistry.AddPinnedObject(this);
            var position = transform.position;
            AutoMapPinsPlugin.LOGGER.LogDebug(
                $"Tracking: {Template.Label} at {position.x} {position.y} {position.z}");
        }

        public PinTemplate Template { get; private set; }
        private PinnedObjectGroup? Group { get; set; }

        public void Init(PinTemplate template)
        {
            Template = template;

            if (Template.IsGrouped)
            {
                Group = PinnedObjectGroup.FindOrCreateFor(this);
                Group.Add(this);
            }

            if (Template.IsEnabled())
            {
                if (Template.IsGrouped)
                {
                    Group.UpdatePinVisibility();
                }
                else
                {
                    ShowPin();
                }
            }

            ObjectRegistry.AddPinnedObject(this);
            var position = transform.position;
            AutoMapPinsPlugin.LOGGER.LogDebug(
                $"Tracking: {Template.Label} at {position.x} {position.y} {position.z}");
        }

        private void ShowPin()
        {
            if (Template.IsPersistent)
            {
                var existing = Minimap.instance.FindSimilarPin(transform.position, Template.Label);

                if (existing != null)
                {
                    _pin = existing;
                    _pin.m_name = AutoMapPinsPlugin.Wrap(Template.Label);
                }
                else
                {
                    _pin = Minimap.instance.AddPin(transform.position, Minimap.PinType.Icon3,
                        AutoMapPinsPlugin.Wrap(Template.Label), true, false);
                }

                if (Template.Icon != null)
                {
                    _pin.m_icon = Template.Icon;
                }
            }
            else
            {
                _pin = Minimap.instance.AddPin(transform.position, Minimap.PinType.Icon3,
                    AutoMapPinsPlugin.Wrap(Template.Label), false, false);
                if (Template.Icon != null)
                {
                    _pin.m_icon = Template.Icon;
                }
            }

            _visible = true;
        }

        private void HidePin()
        {
            if (_pin != null && Minimap.instance != null && !Template.IsPersistent)
            {
                Minimap.instance.RemovePin(_pin);
            }

            _visible = false;
        }

        public void UpdatePinVisibility()
        {
            var toShow = Template.IsEnabled();
            if (toShow != _visible)
            {
                IsVisible = toShow;
            }

            if (Template.IsGrouped)
            {
                Group.UpdatePinVisibility();
            }
        }

        void OnDestroy()
        {
            if (!Template.IsPersistent)
            {
                HidePin();
            }

            if (Group != null)
            {
                Group.Remove(this);
            }
            
            var position = transform.position;
            AutoMapPinsPlugin.LOGGER.LogDebug(string.Format("Removing: {0} at {1} {2} {3}", _pin?.m_name,
                position.x, position.y, position.z));
            ObjectRegistry.RemovePinnedObject(this);
        }

        private bool _visible;

        public bool IsVisible
        {
            get => _visible;
            set
            {
                if (Template.IsGrouped)
                {
                    Group.UpdatePinVisibility();
                }
                else
                {
                    if (value)
                    {
                        ShowPin();
                    }
                    else
                    {
                        HidePin();
                    }
                }

                _visible = value;
            }
        }
    }
}