using System.Collections.Generic;
using UnityEngine;
using Resources = AutoMapPins.Properties.Resources;

namespace AutoMapPins.Icons
{
    internal static class Assets
    {
        private static readonly Sprite DefaultIcon = LoadSpriteFromTexture(Resources.Dot);

        private static readonly Dictionary<string, Sprite> Icons = new()
        {
            { "axe", LoadSpriteFromTexture(Resources.Axe) },
            { "axe48", LoadSpriteFromTexture(Resources.Axe48) },
            { "berry", LoadSpriteFromTexture(Resources.Berry) },
            { "berry48", LoadSpriteFromTexture(Resources.Berry48) },
            { "dungeon", LoadSpriteFromTexture(Resources.Dungeon) },
            { "dungeon48", LoadSpriteFromTexture(Resources.Dungeon48) },
            { "flower", LoadSpriteFromTexture(Resources.Flower) },
            { "flower48", LoadSpriteFromTexture(Resources.Flower48) },
            { "hand", LoadSpriteFromTexture(Resources.Hand) },
            { "hand48", LoadSpriteFromTexture(Resources.Hand48) },
            { "mine", LoadSpriteFromTexture(Resources.Mine) },
            { "mine48", LoadSpriteFromTexture(Resources.Mine48) },
            { "mushroom", LoadSpriteFromTexture(Resources.Mushroom) },
            { "mushroom48", LoadSpriteFromTexture(Resources.Mushroom48) },
            { "seed", LoadSpriteFromTexture(Resources.Seed) },
            { "seed48", LoadSpriteFromTexture(Resources.Seed48) },
            { "spawner", LoadSpriteFromTexture(Resources.Spawner) },
            { "spawner48", LoadSpriteFromTexture(Resources.Spawner48) },
            { "rune", LoadSpriteFromTexture(Resources.Rune) },
            { "rune48", LoadSpriteFromTexture(Resources.Rune48) },
            { "dot", LoadSpriteFromTexture(Resources.Dot) },
            { "dot48", LoadSpriteFromTexture(Resources.Dot48) },
            { "herb", LoadSpriteFromTexture(Resources.Herb) },
            { "herb48", LoadSpriteFromTexture(Resources.Herb48) },
            { "island", LoadSpriteFromTexture(Resources.Island) },
            { "island48", LoadSpriteFromTexture(Resources.Island48) },
            { "fire", LoadSpriteFromTexture(Resources.Fire) },
            { "fire48", LoadSpriteFromTexture(Resources.Fire48) },
        };

        internal static Sprite GetIcon(string iconName)
        {
            if (Icons.TryGetValue(iconName, out Sprite result))
                return result;

            return DefaultIcon;
        }

        private static Texture2D LoadTextureFromRaw(byte[] bytes)
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            return tex;
        }

        private static Sprite LoadSpriteFromTexture(Texture2D spriteTexture, float pixelsPerUnit = 100f)
        {
            AutoMapPinsPlugin.Log.LogDebug($"Making Sprite from Texture {spriteTexture}");
            return Sprite.Create(spriteTexture,
                new Rect(0.0f, 0.0f, spriteTexture.width, spriteTexture.height),
                new Vector2(0.0f, 0.0f), pixelsPerUnit);
        }

        private static Sprite LoadSpriteFromTexture(byte[] bytes)
        {
            return LoadSpriteFromTexture(LoadTextureFromRaw(bytes));
        }
    }
}