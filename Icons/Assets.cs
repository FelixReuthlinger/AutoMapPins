using System.Collections.Generic;
using UnityEngine;
using Resources = AutoMapPins.Properties.Resources;

namespace AutoMapPins.Icons
{
    internal static class Assets
    {
        internal static readonly Sprite DEFAULT_ICON = LoadSpriteFromTexture(Resources.AxeIcon);
        internal static readonly Dictionary<string, Sprite> ICONS = new()
        {
            { "axe", LoadSpriteFromTexture(Resources.AxeIcon) },
            { "axe48", LoadSpriteFromTexture(Resources.AxeIcon48) },
            { "berry", LoadSpriteFromTexture(Resources.BerryIcon) },
            { "berry48", LoadSpriteFromTexture(Resources.BerryIcon48) },
            { "dungeon", LoadSpriteFromTexture(Resources.DungeonIcon) },
            { "dungeon48", LoadSpriteFromTexture(Resources.DungeonIcon48) },
            { "flower", LoadSpriteFromTexture(Resources.FlowerIcon) },
            { "flower48", LoadSpriteFromTexture(Resources.FlowerIcon48) },
            { "hand", LoadSpriteFromTexture(Resources.HandIcon) },
            { "hand48", LoadSpriteFromTexture(Resources.HandIcon48) },
            { "mine", LoadSpriteFromTexture(Resources.MineIcon) },
            { "mine48", LoadSpriteFromTexture(Resources.MineIcon48) },
            { "mushroom", LoadSpriteFromTexture(Resources.MushroomIcon) },
            { "mushroom48", LoadSpriteFromTexture(Resources.MushroomIcon48) },
            { "seed", LoadSpriteFromTexture(Resources.SeedIcon) },
            { "seed48", LoadSpriteFromTexture(Resources.SeedIcon48) },
            { "spawner", LoadSpriteFromTexture(Resources.Spawner) },
            { "rune", LoadSpriteFromTexture(Resources.Rune) },
        };
        
        private static Texture2D LoadTextureFromRaw(byte[] bytes)
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            return tex;
        }

        private static Sprite LoadSpriteFromTexture(Texture2D spriteTexture, float pixelsPerUnit = 100f)
        {
            AutoMapPinsPlugin.LOGGER.LogDebug($"Making Sprite from Texture {spriteTexture}");
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