using System;
using System.Collections.Generic;
using System.IO;
using EasyFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppUsageTimerPro;

public class Settings
{
    public BindableProperty<TimeSpan> AutoSaveInterval = new(TimeSpan.FromSeconds(5));
    public BindableProperty<TimeSpan> UpdateDeltaTime = new(TimeSpan.FromMilliseconds(16));
}

public class SettingsConverter : JsonConverter<Settings>
{
    public override void WriteJson(JsonWriter writer, Settings? value, JsonSerializer serializer)
    {
        if (value == null)
            return;

        writer.WriteStartObject();

        writer.WritePropertyName("AutoSaveInterval");
        serializer.Serialize(writer, value.AutoSaveInterval.Value);

        writer.WritePropertyName("UpdateDeltaTime");
        serializer.Serialize(writer, value.UpdateDeltaTime.Value);

        writer.WriteEndObject();
    }

    public override Settings? ReadJson(JsonReader reader, Type objectType, Settings? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);
        if (!obj.HasValues) return null;

        var autoSaveInterval = obj["AutoSaveInterval"]!.ToObject<TimeSpan>(serializer);
        var updateDeltaTime = obj["UpdateDeltaTime"]!.ToObject<TimeSpan>(serializer);

        var res = new Settings();
        res.AutoSaveInterval.SetValueWithoutEvent(autoSaveInterval);
        res.UpdateDeltaTime.SetValueWithoutEvent(updateDeltaTime);

        return res;
    }
}

public class SettingsManager : Singleton<SettingsManager>
{
    public Settings Settings { get; private set; } = new();

    public readonly string SavePath;

    SettingsManager()
    {
        JsonConvert.DefaultSettings += () => new JsonSerializerSettings()
            { Converters = new List<JsonConverter>() { new SettingsConverter() } };

        SavePath = Path.Combine(DataManager.Instance.ConfigSaveDir, "Settings.json");

        if (!File.Exists(SavePath))
        {
            Save();
        }
        Load();
    }

    public void Load()
    {
        var json = FileHelper.ReadAllTextWithHash(SavePath);
        Settings = JsonConvert.DeserializeObject<Settings>(json) ?? new Settings();
    }

    public void Save()
    {
        FileHelper.WriteAllTextWithHash(SavePath, JsonConvert.SerializeObject(Settings));
    }
}