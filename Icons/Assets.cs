using UnityEngine;
using Resources = AutoMapPins.Properties.Resources;

namespace AutoMapPins.Icons
{
    internal static class Assets
    {
        public static readonly Sprite AXE_SPRITE = LoadSpriteFromTexture(Resources.AxeIcon);
        public static readonly Sprite AXE_SPRITE48 = LoadSpriteFromTexture(Resources.AxeIcon48);
        public static readonly Sprite BERRY_SPRITE = LoadSpriteFromTexture(Resources.BerryIcon);
        public static readonly Sprite BERRY_SPRITE48 = LoadSpriteFromTexture(Resources.BerryIcon48);
        public static readonly Sprite DUNGEON_SPRITE = LoadSpriteFromTexture(Resources.DungeonIcon);
        public static readonly Sprite DUNGEON_SPRITE48 = LoadSpriteFromTexture(Resources.DungeonIcon48);
        public static readonly Sprite FLOWER_SPRITE = LoadSpriteFromTexture(Resources.FlowerIcon);
        public static readonly Sprite FLOWER_SPRITE48 = LoadSpriteFromTexture(Resources.FlowerIcon48);
        public static readonly Sprite HAND_SPRITE = LoadSpriteFromTexture(Resources.HandIcon);
        public static readonly Sprite HAND_SPRITE48 = LoadSpriteFromTexture(Resources.HandIcon48);
        public static readonly Sprite MINE_SPRITE = LoadSpriteFromTexture(Resources.MineIcon);
        public static readonly Sprite MINE_SPRITE48 = LoadSpriteFromTexture(Resources.MineIcon48);
        public static readonly Sprite MUSHROOM_SPRITE = LoadSpriteFromTexture(Resources.MushroomIcon);
        public static readonly Sprite MUSHROOM_SPRITE48 = LoadSpriteFromTexture(Resources.MushroomIcon48);
        public static readonly Sprite SEED_SPRITE = LoadSpriteFromTexture(Resources.SeedIcon);
        public static readonly Sprite SEED_SPRITE48 = LoadSpriteFromTexture(Resources.SeedIcon48);

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