using System;
using System.Text.RegularExpressions;
using AutoMapPins.Templates;
using UnityEngine;

namespace AutoMapPins.Model
{
    public static class Pin
    {
        internal static PinTemplate Name(string template)
        {
            return new PinTemplate().Matching(new InstanceNameRegex(template));
        }

        internal static PinTemplate Hovr(string template)
        {
            return new PinTemplate().Matching(new HoverTextRegex(template));
        }
    }

    internal class PinTemplate
    {
        public string Label { get; private set; }
        protected ObjectMatcher Matcher { get; set; }
        public string ConfigurationKey { get; private set; }
        private Cat Category { get; set; }
        public bool IsGrouped { get; protected set; }

        public bool IsMatch(MonoBehaviour obj)
        {
            return Matcher.IsMatch(obj);
        }

        internal bool IsEnabled()
        {
            var regCfg = AutoMapPinsPlugin.CONFIG_REGISTRY.IsEnabled(this);
            if (regCfg.HasValue) return regCfg.Value;
            if (ConfigurationKey == Cat.HARVESTABLES.Key)
            {
                return AutoMapPinsPlugin.ShowHarvestables.Value;
            }

            if (ConfigurationKey == Cat.UNCATEGORIZED.Key)
            {
                return AutoMapPinsPlugin.ShowUncategorized.Value;
            }

            return false;
        }

        protected Type InGameType { get; set; }
        public double GroupingDistance { get; protected set; }

        public String SingleLabel
        {
            get => Label;
            protected set => Label = value;
        }

        public String MultipleLabel { get; protected set; }
        protected Sprite NormalIcon { get; set; }
        protected Sprite SmallerIcon { get; set; }
        public Biome FirstBiome { get; protected set; }
        public bool IsPersistent { get; protected set; }

        public bool IsMatchV2(MonoBehaviour obj)
        {
            return InGameType.IsInstanceOfType(obj) && Matcher.IsMatch(obj);
        }

        public Sprite Icon
        {
            get
            {
                if (NormalIcon != null && SmallerIcon != null)
                    return AutoMapPinsPlugin.UseSmallerIcons.Value ? SmallerIcon : NormalIcon;
                return Category.Icon;
            }
        }

        internal PinTemplate Matching(ObjectMatcher matcher)
        {
            Matcher = matcher;
            return this;
        }

        internal PinTemplate Lbl(string label)
        {
            Label = label;
            return this;
        }

        internal PinTemplate Nbl(Cat category)
        {
            Category = category;
            ConfigurationKey = category.Key;
            return this;
        }

        internal PinTemplate Grp()
        {
            this.IsGrouped = true;
            return this;
        }
    }

    internal abstract class ObjectMatcher
    {
        public abstract bool IsMatch(MonoBehaviour obj);
    }

    internal class InstanceNameRegex : ObjectMatcher
    {
        private readonly Regex _pattern;

        internal InstanceNameRegex(string template)
        {
            _pattern = new Regex(template);
        }

        public override bool IsMatch(MonoBehaviour obj)
        {
            return _pattern.IsMatch(obj.name);
        }
    }

    internal class HoverTextRegex : ObjectMatcher
    {
        private readonly Regex _pattern;

        internal HoverTextRegex(string template)
        {
            _pattern = new Regex(template);
        }

        public override bool IsMatch(MonoBehaviour obj)
        {
            return obj.TryGetComponent<HoverText>(out var hoverTextComponent) &&
                   _pattern.IsMatch(hoverTextComponent.m_text);
        }
    }
}