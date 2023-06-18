using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapPins.Model;
using AutoMapPins.Templates;
using UnityEngine;
using Object = System.Object;

namespace AutoMapPins.Registry
{
    internal class TemplateRegistry
    {
        internal TemplateRegistry()
        {
            var typesToLoad = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.GetCustomAttribute(typeof(TemplateAttribute)) != null)
                .ToArray();

            AutoMapPinsPlugin.LOGGER.LogDebug($"Initializing Pin Template Registry: {typesToLoad.Length} Templates found");
            foreach (var loadingType in typesToLoad)
            {
                AutoMapPinsPlugin.LOGGER.LogDebug($"Constructing template: {loadingType}");

                var ctor = loadingType.GetConstructor(new Type[] { });
                var obj = (PinTemplate)ctor!.Invoke(new Object[] { });
                AutoMapPinsPlugin.LOGGER.LogDebug($"Added template: {obj.SingleLabel}");
                _templates.Add(obj);
                AutoMapPinsPlugin.CONFIG_REGISTRY.Bind(obj);
            }
        }
        
        private readonly List<PinTemplate> _templates = new();

        internal PinTemplate Find(MonoBehaviour obj)
        {
            return _templates.FirstOrDefault(t => t.IsMatchV2(obj))!;
        }

        internal PinTemplate Find(Minimap.PinData pin)
        {
            return _templates.FirstOrDefault(t =>
                t.IsPersistent && pin.m_name.Contains(t.Label.Replace(' ', '\u00A0'))
            )!;
        }
    }
}