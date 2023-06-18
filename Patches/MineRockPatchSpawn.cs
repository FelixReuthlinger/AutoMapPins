using System;
using System.Linq;
using AutoMapPins.Model;
using AutoMapPins.Registry;
using AutoMapPins.Templates;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(MineRock), "Start")]
    class MineRockPatchSpawn
    {
        private static readonly PinTemplate[] Templates = { };

        [UsedImplicitly]
        private static void Postfix(MineRock instance)
        {
            MineRock obj = instance;
            var template = Templates.FirstOrDefault(t => t.IsMatch(obj)) ?? AutoMapPinsPlugin.TEMPLATE_REGISTRY.Find(instance);

            if (template == null)
            {
                AutoMapPinsPlugin.LogUnmatchedName(typeof(MineRock), obj.name);
                AutoMapPinsPlugin.LogUnmatchedHover(typeof(MineRock), obj.GetComponent<HoverText>()?.m_text);
            }
            else if (!String.IsNullOrWhiteSpace(template.Label))
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