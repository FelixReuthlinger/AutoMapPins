using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Resources = AutoMapPins.Properties.Resources;

namespace AutoMapPins.Icons;

internal abstract class Assets
{
    private static readonly Sprite DefaultIcon = LoadSpriteFromTexture(Resources.Dot);

    private static readonly Dictionary<string, Sprite> Icons = new()
    {
        { "amp", LoadSpriteFromTexture(Resources.AMP) },
        { "axe", LoadSpriteFromTexture(Resources.Axe) },
        { "axe48", LoadSpriteFromTexture(Resources.Axe48) },
        { "berry", LoadSpriteFromTexture(Resources.Berry) },
        { "berry48", LoadSpriteFromTexture(Resources.Berry48) },
        { "bones", LoadSpriteFromTexture(Resources.Bones) },
        { "bones48", LoadSpriteFromTexture(Resources.Bones48) },
        { "dot", LoadSpriteFromTexture(Resources.Dot) },
        { "dot48", LoadSpriteFromTexture(Resources.Dot48) },
        { "dungeon", LoadSpriteFromTexture(Resources.Dungeon) },
        { "dungeon48", LoadSpriteFromTexture(Resources.Dungeon48) },
        { "fire", LoadSpriteFromTexture(Resources.Fire) },
        { "fire48", LoadSpriteFromTexture(Resources.Fire48) },
        { "flower", LoadSpriteFromTexture(Resources.Flower) },
        { "flower48", LoadSpriteFromTexture(Resources.Flower48) },
        { "hand", LoadSpriteFromTexture(Resources.Hand) },
        { "hand48", LoadSpriteFromTexture(Resources.Hand48) },
        { "hay", LoadSpriteFromTexture(Resources.Hay) },
        { "hay48", LoadSpriteFromTexture(Resources.Hay48) },
        { "herb", LoadSpriteFromTexture(Resources.Herb) },
        { "herb48", LoadSpriteFromTexture(Resources.Herb48) },
        { "island", LoadSpriteFromTexture(Resources.Island) },
        { "island48", LoadSpriteFromTexture(Resources.Island48) },
        { "mine", LoadSpriteFromTexture(Resources.Mine) },
        { "mine48", LoadSpriteFromTexture(Resources.Mine48) },
        { "monument", LoadSpriteFromTexture(Resources.Monument) },
        { "monument48", LoadSpriteFromTexture(Resources.Monument48) },
        { "mushroom", LoadSpriteFromTexture(Resources.Mushroom) },
        { "mushroom48", LoadSpriteFromTexture(Resources.Mushroom48) },
        { "portal", LoadSpriteFromTexture(Resources.Portal) },
        { "portal48", LoadSpriteFromTexture(Resources.Portal48) },
        { "rune", LoadSpriteFromTexture(Resources.Rune) },
        { "rune48", LoadSpriteFromTexture(Resources.Rune48) },
        { "seed", LoadSpriteFromTexture(Resources.Seed) },
        { "seed48", LoadSpriteFromTexture(Resources.Seed48) },
        { "spawner", LoadSpriteFromTexture(Resources.Spawner) },
        { "spawner48", LoadSpriteFromTexture(Resources.Spawner48) },
        { "temple", LoadSpriteFromTexture(Resources.Temple) },
        { "temple48", LoadSpriteFromTexture(Resources.Temple48) },
        { "treasure", LoadSpriteFromTexture(Resources.Treasure) },
        { "treasure48", LoadSpriteFromTexture(Resources.Treasure48) },
        { "tree", LoadSpriteFromTexture(Resources.Tree) },
        { "tree48", LoadSpriteFromTexture(Resources.Tree48) },
        { "village", LoadSpriteFromTexture(Resources.Village) },
        { "village48", LoadSpriteFromTexture(Resources.Village48) },
        { "whale", LoadSpriteFromTexture(Resources.Whale) },
        { "whale48", LoadSpriteFromTexture(Resources.Whale48) },
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