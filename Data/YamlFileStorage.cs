using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapPins.Common;
using BepInEx;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AutoMapPins.Data;

internal class YamlFileStorage : HasLogger
{
    internal YamlFileStorage(string modGuid)
    {
        _modGuid = modGuid;
        FindConfigFiles();
    }

    private readonly string _modGuid;
    private readonly List<string> _yamlFiles = new();

    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    internal static readonly ISerializer Serializer = new SerializerBuilder()
        .DisableAliases()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    private string GetModFilePattern(string fileInfix = "categories")
    {
        return $"{_modGuid}.{fileInfix}.*.yaml";
    }

    internal string GetSingleFile(string fileInfix)
    {
        return Path.Combine(Paths.ConfigPath, $"{_modGuid}.{fileInfix}.yaml");
    }

    internal void WriteFile(string file, Dictionary<string, Config.Category> objects)
    {
        var yamlContent = Serializer.Serialize(objects);
        File.WriteAllText(file, yamlContent);
        Log.LogInfo($"wrote yaml content to file '{file}'");
    }

    private Dictionary<string, Config.Category> DeserializeFile(string fileName, string fileContent)
    {
        try
        {
            return Deserializer.Deserialize<Dictionary<string, Config.Category>>(fileContent);
        }
        catch (Exception e)
        {
            Log.LogWarning(
                $"Unable to parse config file '{fileName}' due to '{e.Message}' " +
                $"because of '{e.GetBaseException().Message}', \n{e.StackTrace}");
        }

        return new Dictionary<string, Config.Category>();
    }

    internal Dictionary<string, string> ReadConfigFiles()
    {
        return _yamlFiles.ToDictionary(fileName => fileName, File.ReadAllText);
    }

    private void FindConfigFiles(string fileInfix = "categories")
    {
        foreach (string fileString in Directory.GetFiles(Paths.ConfigPath, GetModFilePattern(fileInfix),
                     SearchOption.AllDirectories))
        {
            Log.LogInfo($"found category and pin config file '{fileString}'");
            _yamlFiles.Add(fileString);
            FileSystemWatcher watcher = new(Path.GetDirectoryName(fileString)!, Path.GetFileName(fileString));
            watcher.Changed += AutoMapPinsPlugin.ReadYamlFileContent;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }
    }

    internal Dictionary<string, Config.Category> DeserializeAndMergeFileData(
        Dictionary<string, string> configFileContents)
    {
        return configFileContents
            .Select(kv => DeserializeFile(fileName: kv.Key, fileContent: kv.Value))
            .SelectMany(x => x)
            .GroupBy(kv => kv.Key)
            .ToDictionary(group => group.Key, group =>
            {
                bool active = group.All(cat => cat.Value.IsActive);
                bool isPermanent = group.All(cat => cat.Value.IsPermanent);
                bool groupable = group.All(cat => cat.Value.Groupable);
                float groupingDistance = group.Min(cat => cat.Value.GroupingDistance);
                string? iconName = group.First().Value.IconName;
                string? name = group.First().Value.Name;
                Config.PinColor? color = group.First().Value.IconColorRGBA;
                Dictionary<string, Config.Pin> individualPins = new();
                List<string> categoryPins = new();
                foreach (Config.Category categoryConfig in group.Select(category => category.Value))
                {
                    if (categoryConfig.IndividualConfiguredObjects != null)
                        individualPins = individualPins.Concat(categoryConfig.IndividualConfiguredObjects)
                            .GroupBy(pin => pin.Key)
                            .ToDictionary(pinGroup => pinGroup.Key,
                                pinGroup => pinGroup.First().Value
                            );
                    if (categoryConfig.CategoryConfiguredObjects != null)
                        categoryPins = categoryPins.Concat(categoryConfig.CategoryConfiguredObjects).ToList();
                }

                return new Config.Category
                {
                    Name = name,
                    IsActive = active,
                    IsPermanent = isPermanent,
                    Groupable = groupable,
                    GroupingDistance = groupingDistance,
                    IconName = iconName,
                    IconColorRGBA = color,
                    IndividualConfiguredObjects = individualPins,
                    CategoryConfiguredObjects = categoryPins
                };
            });
    }
}