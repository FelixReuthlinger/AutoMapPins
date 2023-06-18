using AutoMapPins.Icons;
using UnityEngine;

namespace AutoMapPins.Model
{
    internal class Cat
    {
        public static readonly Cat UNCATEGORIZED = null!;

        public static readonly Cat HARVESTABLES =
            new Cat("Category.Harvestables", Assets.HAND_SPRITE, Assets.HAND_SPRITE48);

        internal string Key { get; private set; }
        internal Sprite Icon => AutoMapPinsPlugin.UseSmallerIcons.Value ? _icon48 : _icon64;
        private readonly Sprite _icon64;
        private readonly Sprite _icon48;

        private Cat(string key, Sprite icon64, Sprite icon48)
        {
            Key = key;
            _icon64 = icon64;
            _icon48 = icon48;
        }
    }
}