using System.Linq;
using AutoMapPins.Model;
using AutoMapPins.Registry;
using AutoMapPins.Templates;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(MineRock5), "Start")]
    class MineRock5PatchSpawn
    {
        private static readonly PinTemplate[] Templates =
        {
            Pin.Name("rock\\d+_forest_frac" + Constants.Clone),
            Pin.Name("rock\\d+_mountain_frac" + Constants.Clone),
            Pin.Name("rock\\d+_frac" + Constants.Clone),
            Pin.Name("rock\\d+_coast_frac" + Constants.Clone),
        };

        [UsedImplicitly]
        private static void Postfix(ref MineRock5 instance)
        {
            MineRock5 obj = instance;
            var template = Templates.FirstOrDefault(t => t.IsMatch(obj)) ??
                           AutoMapPinsPlugin.TEMPLATE_REGISTRY.Find(instance);

            if (template == null)
            {
                AutoMapPinsPlugin.LogUnmatchedName(typeof(MineRock5), obj.name);
                AutoMapPinsPlugin.LogUnmatchedHover(typeof(MineRock5), obj.GetComponent<HoverText>()?.m_text);
            }
            else if (!string.IsNullOrWhiteSpace(template.Label))
            {
                var height = instance.gameObject.transform.position.y;

                if (height < Constants.MaxPinHeight)
                {
                    var pin = instance.gameObject.AddComponent<PinnedObject>();
                    pin.Init(template);
                }
            }
        }
    }
}