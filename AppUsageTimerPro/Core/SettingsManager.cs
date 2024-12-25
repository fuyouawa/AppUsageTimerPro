using System;
using System.IO;
using Newtonsoft.Json;

namespace AppUsageTimerPro;

public class Settings
{
    public TimeSpan AutoSaveInterval = TimeSpan.FromSeconds(1);
}

public class SettingsManager : Singleton<SettingsManager>
{
    public Settings Settings { get; private set; } = new();
    public JsonSerializerSettings JsonSerializerSettings { get; } = new();

    public readonly string SavePath;

    SettingsManager()
    {
        JsonConvert.DefaultSettings += () => JsonSerializerSettings;

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