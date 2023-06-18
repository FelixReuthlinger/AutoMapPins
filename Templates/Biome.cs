namespace AutoMapPins.Templates
{
    internal class Biome
    {
        public static readonly Biome OCEANS = new Biome { Name = "Oceans", Order = 0 };
        public static readonly Biome MEADOWS = new Biome { Name = "Meadows", Order = 1 };
        public static readonly Biome BLACK_FORESTS = new Biome { Name = "Black Forests", Order = 2 };
        public static readonly Biome SWAMPS = new Biome { Name = "Swamps", Order = 3 };
        public static readonly Biome MOUNTAINS = new Biome { Name = "Mountains", Order = 4 };
        public static readonly Biome PLAINS = new Biome { Name = "Plains", Order = 5 };
        public static readonly Biome MISTLANDS = new Biome { Name = "Mistlands", Order = 6 };
        public static readonly Biome ASHLANDS = new Biome { Name = "Ashlands", Order = 7 };

        public string Name { get; private set; }
        public int Order { get; private set; }

        public override string ToString()
        {
            return $"{Order} {Name}";
        }
    }
}