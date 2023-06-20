using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using JetBrains.Annotations;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AutoMapPins.FileIO;

public class YamlFileStorage<T>
{
    public YamlFileStorage(string modGuid)
    {
        _modGuid = modGuid;
        _logger = Logger.CreateLogSource(modGuid);
    }

    private readonly string _modGuid;
    private readonly ManualLogSource _logger;

    private readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    private readonly ISerializer _serializer = new SerializerBuilder()
        .DisableAliases()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    private string GetModDefaultFile()
    {
        return Path.Combine(Paths.ConfigPath, $"{_modGuid}.defaults.yaml");
    }

    private string GetModFilePattern(string fileInfix = "custom")
    {
        return $"{_modGuid}.{fileInfix}.*.yaml";
    }

    public string GetSingleFile(string fileInfix)
    {
        return Path.Combine(Paths.ConfigPath, $"{_modGuid}.{fileInfix}.yaml");
    }

    [UsedImplicitly]
    public void WriteDefaultsFile(Dictionary<string, T> objects)
    {
        WriteFile(GetModDefaultFile(), objects);
    }

    public void WriteFile(string file, Dictionary<string, T> objects)
    {
        var yamlContent = _serializer.Serialize(objects);
        File.WriteAllText(file, yamlContent);
        _logger.LogInfo($"wrote yaml content to file '{file}'");
    }

    public Dictionary<string, T> ReadFile(string file)
    {
        try
        {
            return _deserializer.Deserialize<Dictionary<string, T>>(File.ReadAllText(file));
        }
        catch (Exception e)
        {
            _logger.LogWarning(
                $"Unable to parse config file '{file}' due to '{e.Message}' " +
                $"because of '{e.GetBaseException().Message}', \n{e.StackTrace}");
        }

        return new Dictionary<string, T>();
    }

    [UsedImplicitly]
    public Dictionary<string, T> ReadAllData()
    {
        return Directory.GetFiles(Paths.ConfigPath, GetModFilePattern(), SearchOption.AllDirectories).ToList()
            .Select(ReadFile).SelectMany(x => x).GroupBy(kv => kv.Key)
            .ToDictionary(kv => kv.Key, kv =>
            {
                if (kv.Count(_ => true) > 1)
                {
                    _logger.LogWarning(
                        $"found multiple values for prefab {kv.Key}, " +
                        $"using first result {kv.First().Value}, dropping others");
                }

                return kv.First().Value;
            });
    }
}