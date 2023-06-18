using System;
using AutoMapPins.Icons;
using AutoMapPins.Model;

namespace AutoMapPins.Templates
{
    internal class TemplateAttribute : Attribute
    {
    }

    internal abstract class PickableTemplate : PinTemplate
    {
        public PickableTemplate()
        {
            InGameType = typeof(Pickable);
            IsGrouped = true;
            GroupingDistance = Constants.DistanceMushrooms;
            NormalIcon = Assets.HAND_SPRITE;
            SmallerIcon = Assets.HAND_SPRITE48;
        }
    }

    internal abstract class BerryTemplate : PinTemplate
    {
        public BerryTemplate()
        {
            InGameType = typeof(Pickable);
            IsGrouped = true;
            GroupingDistance = Constants.DistanceMushrooms;
            NormalIcon = Assets.BERRY_SPRITE;
            SmallerIcon = Assets.BERRY_SPRITE48;
        }
    }

    internal abstract class FlowerTemplate : PinTemplate
    {
        public FlowerTemplate()
        {
            InGameType = typeof(Pickable);
            IsGrouped = true;
            GroupingDistance = Constants.DistanceMushrooms;
            NormalIcon = Assets.FLOWER_SPRITE;
            SmallerIcon = Assets.FLOWER_SPRITE48;
        }
    }

    internal abstract class RemainsTemplate : PinTemplate
    {
        public RemainsTemplate()
        {
            InGameType = typeof(Pickable);
            IsGrouped = true;
            GroupingDistance = Constants.DistanceMushrooms;
            NormalIcon = Assets.HAND_SPRITE;
            SmallerIcon = Assets.HAND_SPRITE48;
        }
    }

    internal abstract class MushroomTemplate : PinTemplate
    {
        public MushroomTemplate()
        {
            InGameType = typeof(Pickable);
            IsGrouped = true;
            GroupingDistance = Constants.DistanceMushrooms;
            NormalIcon = Assets.MUSHROOM_SPRITE;
            SmallerIcon = Assets.MUSHROOM_SPRITE48;
        }
    }

    internal abstract class SeedTemplate : PinTemplate
    {
        public SeedTemplate()
        {
            InGameType = typeof(Destructible);
            IsGrouped = true;
            GroupingDistance = Constants.DistanceMushrooms;
            NormalIcon = Assets.SEED_SPRITE;
            SmallerIcon = Assets.SEED_SPRITE48;
        }
    }

    internal abstract class MineTemplate : PinTemplate
    {
        public MineTemplate()
        {
            InGameType = typeof(Destructible);
            IsGrouped = true;
            GroupingDistance = Constants.DistanceMushrooms;
            NormalIcon = Assets.MINE_SPRITE;
            SmallerIcon = Assets.MINE_SPRITE48;
        }
    }

    internal abstract class AxeTemplate : PinTemplate
    {
        public AxeTemplate()
        {
            InGameType = typeof(Destructible);
            IsGrouped = true;
            GroupingDistance = Constants.DistanceMushrooms;
            NormalIcon = Assets.AXE_SPRITE;
            SmallerIcon = Assets.AXE_SPRITE48;
        }
    }

    internal abstract class DungeonTemplate : PinTemplate
    {
        public DungeonTemplate()
        {
            InGameType = typeof(Location);
            NormalIcon = Assets.DUNGEON_SPRITE;
            SmallerIcon = Assets.DUNGEON_SPRITE48;
            IsPersistent = true;
        }
    }
}