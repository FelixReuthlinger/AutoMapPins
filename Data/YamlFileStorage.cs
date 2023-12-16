using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapPins.Model;
using BepInEx;
using BepInEx.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AutoMapPins.Data;

internal class YamlFileStorage
{
    internal YamlFileStorage(string modGuid)
    {
        _modGuid = modGuid;
        _logger = Logger.CreateLogSource(modGuid);
        FindConfigFiles();
    }

    private readonly string _modGuid;
    private readonly ManualLogSource _logger;
    private readonly List<string> _yamlFiles = new();

    private readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    private readonly ISerializer _serializer = new SerializerBuilder()
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

    internal void WriteFile(string file, Dictionary<string, CategoryConfig> objects)
    {
        var yamlContent = _serializer.Serialize(objects);
        File.WriteAllText(file, yamlContent);
        _logger.LogInfo($"wrote yaml content to file '{file}'");
    }

    private Dictionary<string, CategoryConfig> DeserializeFile(string fileName, string fileContent)
    {
        try
        {
            return _deserializer.Deserialize<Dictionary<string, CategoryConfig>>(fileContent);
        }
        catch (Exception e)
        {
            _logger.LogWarning(
                $"Unable to parse config file '{fileName}' due to '{e.Message}' " +
                $"because of '{e.GetBaseException().Message}', \n{e.StackTrace}");
        }

        return new Dictionary<string, CategoryConfig>();
    }

    internal Dictionary<string, string> ReadConfigFiles()
    {
        
        return _yamlFiles.ToDictionary(fileName => fileName, File.ReadAllText);
    }

    private void FindConfigFiles(string fileInfix = "categories")
    {
        foreach (string file in Directory.GetFiles(Paths.ConfigPath, GetModFilePattern(fileInfix),
                     SearchOption.AllDirectories))
        {
            _logger.LogInfo($"found category and pin config file '{file}'");
            _yamlFiles.Add(file);
            FileSystemWatcher watcher = new(Paths.ConfigPath, Path.GetFileName(file));
            watcher.Changed += AutoMapPinsPlugin.ReadYamlFileContent;
            watcher.Created += AutoMapPinsPlugin.ReadYamlFileContent;
            watcher.Renamed += AutoMapPinsPlugin.ReadYamlFileContent;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }
    }

    internal Dictionary<string, CategoryConfig> DeserializeAndMergeFileData(Dictionary<string, string> configFileContents)
    {
        return configFileContents
            .Select(kv => DeserializeFile(kv.Key, kv.Value))
            .SelectMany(x => x)
            .GroupBy(kv => kv.Key)
            .ToDictionary(group => group.Key, group =>
            {
                bool active = group.All(cat => cat.Value.CategoryActive);
                Dictionary<string, PinConfig> pins = new();
                foreach (CategoryConfig categoryConfig in group.Select(category => category.Value))
                {
                    pins = pins.Concat(categoryConfig.Pins)
                        .GroupBy(pin => pin.Key)
                        .ToDictionary(pinGroup => pinGroup.Key,
                            pinGroup => pinGroup.First().Value
                        );
                }

                return new CategoryConfig
                {
                    CategoryActive = active,
                    Pins = pins
                };
            });
    }
}