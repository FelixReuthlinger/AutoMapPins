namespace AutoMapPins.Model
{
    public class PinConfig
    {
        public string CategoryName = Common.NoConfig;
        public string InternalName = Common.NoConfig;
        public string Name = Common.NoConfig;
        public string IconName = Common.NoConfig;
        public bool IsPermanent;
        public bool IsActive;
        public bool Groupable;
        public float GroupingDistance = Common.DefaultGroupingDistance;

        internal static PinConfig FromGameObject(string internalName)
        {
            if (Data.Registry.ConfiguredPins.TryGetValue(internalName, out PinConfig result))
            {
                return result;
            }

            return new PinConfig
            {
                CategoryName = Common.NoConfig,
                InternalName = internalName,
                Name = Common.NoConfig,
                IconName = Common.NoConfig,
                IsPermanent = Common.DefaultFalse,
                IsActive = Common.DefaultFalse,
                Groupable = Common.DefaultFalse
            };
        }

        internal static string ParseInternalName(string instanceName)
        {
            return Common.EXCEPT_WORD_REGEX
                .Replace(Common.CLONE_REGEX
                    .Replace(instanceName, ""), "")
                .ToLowerInvariant();
        }
    }
}